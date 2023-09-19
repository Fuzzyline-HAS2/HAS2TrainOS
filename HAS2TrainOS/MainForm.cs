using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Aspose.Cells;

using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using static HAS2TrainOS.MainForm;

namespace HAS2TrainOS
{
    public partial class MainForm : Form
    {
        Mp3Player PlayerSpk = new Mp3Player();
        Mp3Player KillerrSpk = new Mp3Player();
        Mp3Player CommonSpk = new Mp3Player();
        SCNProcessor PlayerSCNProcessor = new SCNProcessor();
        public MainForm()
        {
            InitializeComponent();

            structGlove[] glove = new structGlove[8];   //글러브 데이터 저장용 구조체 배열
            
            timerMain = new System.Threading.Timer(timerMain_CallBack);

            PlayerSCNProcessor.timerPlayerWaitTime = new System.Threading.Timer(PlayerSCNProcessor.timerPlayerWaitTime_CallBack);
            PlayerSCNProcessor.timerPlayerSkipTime = new System.Threading.Timer(PlayerSCNProcessor.timerPlayerSkipTime_CallBack);
            PlayerSCNProcessor.lvNarr = lvPlayerNarr;
            PlayerSCNProcessor.lbWaitTimer = lbPlayerWaitTimer;
            PlayerSCNProcessor.lbSkipTimer = lbPlayerSkipTimer;
            PlayerSCNProcessor.SelectedSpk = PlayerSpk;

            PlayerSpk.nDeviceNum = 0;
            KillerrSpk.nDeviceNum = 2;
            CommonSpk.nDeviceNum = 2;

        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            Aspose.Cells.License license = new Aspose.Cells.License();
            license.SetLicense("Aspose.Cells.lic");
            SetupForm setupform = new SetupForm(this);
            setupform.Show();
            //ExceltoListview();              //Excel에 저장된 값 불러오기
            ComboBoxDeviceAdd();     //Device panel에 있는 콤보박스에 추가하기
            MQTT_Initializtion();         // MQTT 서버 연결
            //trbLoad();                        //TrackBar 기존의 엑셀에 저장된 데이터 가져오기
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            ListviewtoExcel();      //현재까지 값 엑셀에 저장+
            try
            {
                client.Disconnect();    //MQTT 연결종료
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);  
            }
            wbMain.Dispose();
            wbMAC.Dispose();
            wbDevice.Dispose();
            wbGlove.Dispose();  
        }

        private void btnSetup_Click(object sender, EventArgs e)
        {
            SetupForm setupform = new SetupForm(this);
            setupform.Show();
        }

        private void btnPlayerNarrApply_Click(object sender, EventArgs e)
        {
            MQTT_Initializtion();
        }

        private void lvPlayerNarr_DoubleClick(object sender, EventArgs e)
        {
            Console.WriteLine(lvPlayerNarr.SelectedItems[0].Index);
            PlayerSCNProcessor.nCurrentCnt = lvPlayerNarr.SelectedItems[0].Index;
            PlayerSCNProcessor.MainProcessor();
        }

        public class SCNProcessor
        {
            MainForm mainform = new MainForm();

            public ListView lvNarr;
            public Label lbWaitTimer;
            public Label lbSkipTimer;

            public Mp3Player SelectedSpk;

            public int nCurrentCnt = 0;
            public String strTagTo = "";
            public String[] strTagDevice;
            public int nTagCnt = 0;
            public int nTagMaxCnt = 0;

            /* 플레이어 WaitTime 타이머*/
            public System.Threading.Timer timerPlayerWaitTime;
            delegate void TimerEventFiredDelegate_timerPlayerWaitTime();
            uint nPlayerWaitTime = 0;

            /* 플레이어 SkipTime 타이머*/
            public System.Threading.Timer timerPlayerSkipTime;
            delegate void TimerEventFiredDelegate_timerPlayerSkipTime();
            uint nPlayerSkipTime = 0;
            uint nPlayerSkipCondition = 0;
            String strPlayerSkipTo = "";
            public void MainProcessor()
            {
                // WAV 나레이션 플레이 부분
                String strWavName = lvNarr.Items[nCurrentCnt].SubItems[1].Text;
                strWavName = strWavName.Replace("#", "");
                Console.WriteLine("플레이어 나레이션 플레이: SCN" + strWavName.ToString());
                String strNarrNum = strWavName.Substring(strWavName.Length - 1, 1);
                //Console.WriteLine(strWavName);
                strWavName = @"C:\Users\user\Desktop\bbangjun\TrainRoom_excel\wavPlayerNarr\SCN" + strWavName + ".wav";
                FileInfo fileTmp = new FileInfo(strWavName);
                if (fileTmp.Exists)  //FileInfo.Exists로 파일 존재유무 확인 "
                {
                    SelectedSpk.PlayMp3(strWavName);
                }
                else
                {
                    Console.WriteLine("플레이어 나레이션 존재 x");
                }

                // PlayerWaitTimer 부분
                timerPlayerWaitTime.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite); //메인타이머 종료
                nPlayerWaitTime = 0; //Player Wait Timer 변수 초기화
                timerPlayerWaitTime.Change(0, 1000);
                foreach (ListViewItem listitem in lvNarr.Items)
                {
                    if (listitem.Text == "ᐅ")
                        listitem.Text = "";
                }
                lvNarr.Items[nCurrentCnt].Text = "ᐅ";

                // Device Publish 부분
                if (lvNarr.Items[nCurrentCnt].SubItems[3].Text != "")
                {
                    String strDevces = lvNarr.Items[nCurrentCnt].SubItems[3].Text;
                    String[] strDevceArr = strDevces.Split(',');
                    foreach (String strDN in strDevceArr)
                    {

                        Console.WriteLine(strDN);
                        if (strDN.Contains("SG"))
                        {
                            String strCurSCNp = "SCNp" + (nCurrentCnt + 1).ToString();
                            Console.WriteLine(strCurSCNp);
                            this.GetType().GetMethod(strCurSCNp).Invoke(this, null);
                        }
                        else
                        {
                            mainform.SCNJSONPublish(strDN, "p" + lvNarr.Items[nCurrentCnt].SubItems[1].Text.Replace("#", ""));
                        }
                    }
                }

                // Skip Condition 부분
                if (lvNarr.Items[nCurrentCnt].SubItems[4].Text != "")
                {
                    String strSkipCon = lvNarr.Items[nCurrentCnt].SubItems[4].Text.Replace(" ", ""); ;
                    String[] strSkipConArr = strSkipCon.Split(',');
                    foreach (String s in strSkipConArr)
                    {
                        //Console.WriteLine(s);   
                        if (s.Contains("TAG"))
                        {
                            String[] strTag = s.Split('_'); //WAIT_시간_다음 나레이션#
                            strTagDevice = strTag[1].Split('/');
                            strTagTo = "#" + strTag[2];
                            nTagMaxCnt = strTagDevice.Count();
                        }
                        if (s.Contains("SKIP"))
                        {
                            String[] strWait = s.Split('_'); //WAIT_시간_다음 나레이션#
                            timerPlayerSkipTime.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite); //PlayerSkipTimer 종료
                            nPlayerSkipTime = 0;                                                                                                                     //Player Skip Timer 변수 초기화
                            timerPlayerSkipTime.Change(0, 1000);//Player Skip Timer 시작

                            nPlayerSkipCondition = UInt32.Parse(strWait[1]);    //스킵하는 시간 저장
                            strPlayerSkipTo = "#" + strWait[2];                                 //스킵하고 진행시킬 나레이션 번호 저장
                        }
                    }
                }

                mainform.SCNJSONPublish("image", "p" + lvNarr.Items[nCurrentCnt].SubItems[1].Text.Replace("#", ""));    //생존자 훈련소 모니터 시나리오 전송하는 부분
                                                                                                                    //SCNJSONPublish("4F:0E", "t" + lvNarr.Items[nCurrentCnt].SubItems[1].Text.Replace("#", ""));    //생존자 훈련소 모니터 시나리오 전송하는 부분
            } //public void PlayerNarr()



            /* 플레이어 WaitTime 타이머*/
            public void timerPlayerWaitTime_CallBack(Object state)
            {
                mainform.BeginInvoke(new TimerEventFiredDelegate_timerPlayerWaitTime(timerPlayerWaitTimeWork));
            }
            public void timerPlayerWaitTimeWork()
            {
                lbWaitTimer.Text = (nPlayerWaitTime / 60).ToString("00") + ":" + (nPlayerWaitTime % 60).ToString("00");    //남은 시간 uint -> String으로 변환하는 작업
                if (Int32.Parse(lvNarr.Items[nCurrentCnt].SubItems[5].Text) == 0)
                {
                    Console.WriteLine("0sec detected Wait for skip condition!");
                    nPlayerWaitTime = 0;
                }
                else if (nPlayerWaitTime >= Int32.Parse(lvNarr.Items[nCurrentCnt].SubItems[5].Text))
                {
                    Console.WriteLine(nCurrentCnt.ToString() + " Player Narr Wait Time End!");
                    //lvNarr.Focus();
                    nCurrentCnt++;
                    MainProcessor();
                }
                nPlayerWaitTime += 1;                                                                      //초 마다 타이머 함수 실행되면 -1해 남은시간 줄여줌

            }
            /* 플레이어 SkipTime 타이머*/

            public void timerPlayerSkipTime_CallBack(Object state)
            {
                mainform.BeginInvoke(new TimerEventFiredDelegate_timerPlayerSkipTime(timerPlayerSkipTimeWork));
            }
            public void timerPlayerSkipTimeWork()
            {
                lbSkipTimer.Text = (nPlayerSkipTime / 60).ToString("00") + ":" + (nPlayerSkipTime % 60).ToString("00");    //남은 시간 uint -> String으로 변환하는 작업
                nPlayerSkipTime += 1;                                                                      //초 마다 타이머 함수 실행되면 -1해 남은시간 줄여줌
                if (nPlayerSkipTime >= nPlayerSkipCondition)
                {
                    foreach (ListViewItem listitem in lvNarr.Items)
                    {
                        if (listitem.SubItems[1].Text == strPlayerSkipTo)
                        {
                            timerPlayerSkipTime.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite); //PlayerSkipTimer 종료
                            nCurrentCnt = listitem.Index;
                            strTagDevice = null;
                            MainProcessor();
                            break;
                        }
                    }
                }
            }
        }
    }
}
