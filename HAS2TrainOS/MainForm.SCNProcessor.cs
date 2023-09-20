using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Diagnostics.Eventing.Reader;

namespace HAS2TrainOS
{
    public partial class MainForm
    {
        public void FindSCN(string strCurSCN)
        {
            this.GetType().GetMethod(strCurSCN).Invoke(this, null);
        }
        public class SCNProcessor
        {
            public ListView lvNarr;
            public Label lbWaitTimer;
            public Label lbSkipTimer;

            public MqttClient classclient;
            public Mp3Player SelectedSpk;


            public structMAC[] classMAC;
            public structALL AllDevice;

            String strCurSCN;
            public string [] strAGp;
            public string [] strAGt;
            public string strSelectedNarr;
            public String strNarrDir = "";  //wav 파일이 있는 폴더 위치
            public int nCurrentCnt = 0;     //현재 진행중인 나레이션 번호
            public String strTagTo = "";    
            public String[] strTagDevice;
            public int nTagCnt = 0;
            public int nTagMaxCnt = 0;

            public void formFindSCN()
            {
                MainForm.mainform.FindSCN(strCurSCN);
            }
            public void MQTT_Init()
            {
                string BrokerAddress = "172.30.1.44";
                classclient = new MqttClient(BrokerAddress);
                // register a callback-function (we have to implement, see below) which is called by the library when a message was received
                                                                                // use a unique id as client id, each time we start the application
                string clientId = Guid.NewGuid().ToString();   //mqtt 클라이언트가 갖는 고유 id 생성
                classclient.Connect(clientId);
                AllDevice.strALLp = new string[] { "EI1", "EI2", "EG", "EE", "ERp", "EVp" , "EMp","EA"};
                AllDevice.strALLt = new string[] { "ERt", "EVt", "ET","EMt", "EA" };

            }
            /* 플레이어 WaitTime 타이머*/
            public System.Threading.Timer timerPlayerWaitTime;
            public delegate void TimerEventFiredDelegate_timerPlayerWaitTime();
            uint nPlayerWaitTime = 0;

            /* 플레이어 SkipTime 타이머*/
            public System.Threading.Timer timerPlayerSkipTime;
            public delegate void TimerEventFiredDelegate_timerPlayerSkipTime();
            uint nPlayerSkipTime = 0;
            uint nPlayerSkipCondition = 0;
            String strPlayerSkipTo = "";

            public void MainProcessor()
            {
                // WAV 나레이션 플레이 부분
                String strWavName = lvNarr.Items[nCurrentCnt].SubItems[1].Text;
                strWavName = strNarrDir + "SCN" + strWavName.Replace("#", "") + ".wav";
                Console.WriteLine("나레이션 플레이: SCN" + strWavName.ToString());
                String strNarrNum = strWavName.Substring(strWavName.Length - 1, 1);
                //Console.WriteLine(strWavName);
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
                        if (strDN.Contains("SG"))   //SGp 명령어 있을때 해당하는 함수 실행함
                        {
                            strCurSCN = "SCN"+ strSelectedNarr + (nCurrentCnt + 1).ToString();  //ex. SCN41 -> SCNp41 로 만드는작업
                            Console.WriteLine(strCurSCN);
                            //MainForm.mainform.GetType().GetMethod(strCurSCN).Invoke(this, null);    //strCurSCNp/t에 해당하는 함수 찾아서 실행
                            //formFindSCN();
                            MainForm.mainform.FindSCN(strCurSCN);

                        }
                        else
                        {
                            SCNJSONPublish(strDN, strSelectedNarr + lvNarr.Items[nCurrentCnt].SubItems[1].Text.Replace("#", ""));   //ex. #41 -> p41/t41
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

                SCNJSONPublish("image", strSelectedNarr + lvNarr.Items[nCurrentCnt].SubItems[1].Text.Replace("#", ""));    //생존자 훈련소 모니터 시나리오 전송하는 부분
                return;
            } //public void PlayerNarr()



            /* 플레이어 WaitTime 타이머*/

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
            public void SCNJSONPublish(String Device, String SCN)   //DeviceMAC:보내는 장치 MAC 주소,SCN: 시나리오 번호
            {
                JObject SituationData = new JObject(new JProperty("DS", "scenario"));
                SituationData.Add(new JProperty("SCN", SCN));

                if (Device.Contains("ALLp") || Device.Contains("ALLt"))   
                {
                    foreach (string strAllDevice in AllDevice.StringSelecetor(Device))
                    {
                        foreach (structMAC m in classMAC)
                        {
                            if (m.strDeviceName == strAllDevice)
                            {
                                classclient.Publish(m.strDeviceMAC, Encoding.UTF8.GetBytes(SituationData.ToString()), 0, true);
                                break;
                            }
                        }
                    }
                }
                else if (Device.Contains("AGp"))
                {
                    foreach (string strAllGlove in strAGp)
                    {
                        foreach (structMAC m in classMAC)
                        {
                            if (m.strDeviceName == strAllGlove)
                            {
                                classclient.Publish(m.strDeviceMAC, Encoding.UTF8.GetBytes(SituationData.ToString()), 0, true);
                                break;
                            }
                        }
                    }
                }
                else if (Device.Contains("AGt"))
                {
                    foreach (string strAllGlove in strAGt)
                    {
                        foreach (structMAC m in classMAC)
                        {
                            if (m.strDeviceName == strAllGlove)
                            {
                                classclient.Publish(m.strDeviceMAC, Encoding.UTF8.GetBytes(SituationData.ToString()), 0, true);
                                break;
                            }
                        }
                    }
                }


                else
                {
                    foreach (structMAC m in classMAC)
                    {
                        if (m.strDeviceName == Device)
                        {
                            classclient.Publish(m.strDeviceMAC, Encoding.UTF8.GetBytes(SituationData.ToString()), 0, true);
                            break;
                        }
                    }
                }
            }
        }
    }
}
