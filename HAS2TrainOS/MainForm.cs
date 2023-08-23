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

namespace HAS2TrainOS
{
    public partial class MainForm : Form
    {
        Mp3Player PlayerSpk = new Mp3Player();
        Mp3Player KillerrSpk = new Mp3Player();
        Mp3Player CommonSpk = new Mp3Player();
       
        public MainForm()
        {
            InitializeComponent();

            structGlove[] glove = new structGlove[8];   //글러브 데이터 저장용 구조체 배열
            
            timerMain = new System.Threading.Timer(timerMain_CallBack);
            timerPlayerWaitTime = new System.Threading.Timer(timerPlayerWaitTime_CallBack);
            timerPlayerSkipTime = new System.Threading.Timer(timerPlayerSkipTime_CallBack);
            timerTaggerWaitTime = new System.Threading.Timer(timerTaggerWaitTime_CallBack);
            timerTaggerSkipTime = new System.Threading.Timer(timerTaggerSkipTime_CallBack);
            

            PlayerSpk.nDeviceNum = 0;
            KillerrSpk.nDeviceNum = 2;
            CommonSpk.nDeviceNum = 2;

        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            SetupForm setupform = new SetupForm(this);
            setupform.Show();
            ExceltoListview();          //Excel에 저장된 값 불러오기
            ComboBoxDeviceAdd();    //Device panel에 있는 콤보박스에 추가하기
            MQTT_Initializtion();
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
            nPlayerCur = lvPlayerNarr.SelectedItems[0].Index;
            PlayerNarr();
        }

        private void lvPlayerNarr_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }
        public void PlayerNarr()
        {
            // WAV 나레이션 플레이 부분
            String strWavName = lvPlayerNarr.Items[nPlayerCur].SubItems[1].Text;
            strWavName = strWavName.Replace("#", "");
            Console.WriteLine("플레이어 나레이션 플레이: SCN" + strWavName.ToString());
            String strNarrNum = strWavName.Substring(strWavName.Length - 1, 1);
            //Console.WriteLine(strWavName);
            strWavName = "C:\\Users\\user\\Desktop\\bbangjun\\HAS2_Train\\wavPlayerNarr\\SCN" + strWavName + ".wav";
            FileInfo fileTmp = new FileInfo(strWavName);
            if (fileTmp.Exists)  //FileInfo.Exists로 파일 존재유무 확인 "
            {
                PlayerSpk.PlayMp3(strWavName);
            }
            else
            {
                Console.WriteLine("플레이어 나레이션 존재 x");
            }

            // PlayerWaitTimer 부분
            timerPlayerWaitTime.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite); //메인타이머 종료
            nPlayerWaitTime = 0; //Player Wait Timer 변수 초기화
            timerPlayerWaitTime.Change(0, 1000);
            foreach (ListViewItem listitem in lvPlayerNarr.Items)
            {
                listitem.Text = "";
            }
            lvPlayerNarr.Items[nPlayerCur].Text = "ᐅ";

            // Device Publish 부분
            if (lvPlayerNarr.Items[nPlayerCur].SubItems[3].Text != "")
            {
                String strDevces = lvPlayerNarr.Items[nPlayerCur].SubItems[3].Text;
                String[] strDevceArr = strDevces.Split(',');
                foreach (String s in strDevceArr)
                {
                    Console.WriteLine(s);
                    foreach (structMAC m in MACs)
                    {
                        if(m.strDeviceName == s)
                        {
                            SCNJSONPublish(m.strDeviceMAC, "t" + lvPlayerNarr.Items[nPlayerCur].SubItems[1].Text.Replace("#","" ));
                        }
                    }
                }
            }

            // Skip Condition 부분
            if(lvPlayerNarr.Items[nPlayerCur].SubItems[4].Text != "")
            {
                String strSkipCon = lvPlayerNarr.Items[nPlayerCur].SubItems[4].Text.Replace(" ", ""); ;
                String[] strSkipConArr = strSkipCon.Split(',');
                foreach(String s in strSkipConArr)
                {
                    //Console.WriteLine(s);   
                    if (s.Contains("TAG"))
                    {
                        String [] strTag = s.Split('_'); //WAIT_시간_다음 나레이션#
                        strTagDevice = strTag[1].Split('/');
                        strTagTo = "#" + strTag[2];
                        nTagMaxCnt = strTagDevice.Count();
                    }
                    if(s.Contains("SKIP"))
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
        }

        private void lvGlove_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            Console.WriteLine("lvGlove Data Changed!");
        }
    }
}
