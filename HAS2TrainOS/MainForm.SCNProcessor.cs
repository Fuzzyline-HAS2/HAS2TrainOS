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
using System.Drawing;

namespace HAS2TrainOS
{
    public partial class MainForm
    {
        public void FindSCN(string strCurSCN)
        {
            this.GetType().GetMethod(strCurSCN).Invoke(this, null);
        }
        public uint MainTimeSend()
        {
            return nMainTime;
        }

        public void PlayerMainProcessor()
        {
            uint nCurMainTime = nMainTime;
            if (lvPlayerNarr.Items[PlayerSCNProcessor.nCurrentCnt].BackColor == SystemColors.Window)
            {
                PlayerSCNProcessor.MainProcessor();
            }
            else
            {
                string strNarrSkipMinMax = lvPlayerNarr.Items[PlayerSCNProcessor.nCurrentCnt].SubItems[6].Text;
                string[] strSplit = strNarrSkipMinMax.Split(new char[] { ':' });
                uint nMinMaxTime = UInt32.Parse(strSplit[0]) * 60 + UInt32.Parse(strSplit[1]);

                string strNarrWaitTime = lvPlayerNarr.Items[PlayerSCNProcessor.nCurrentCnt].SubItems[5].Text;
                uint nCompareTime = nCurMainTime + UInt32.Parse(strNarrWaitTime);

                Console.WriteLine("현재+나레이션 시간: " + nCompareTime + "  ???  최대/소 시간: " + nMinMaxTime);
                if (lvPlayerNarr.Items[PlayerSCNProcessor.nCurrentCnt].BackColor == Color.LemonChiffon)  // 스킵 할까?
                {
                    if (nCompareTime < nMinMaxTime) // 현재시간 + 나레이션 시간 < 최대 시작 시간 시 실행
                    {
                        PlayerSCNProcessor.MainProcessor();
                    }
                    else
                    {
                        PlayerSCNProcessor.nCurrentCnt++;
                        PlayerMainProcessor();
                    }
                }
                else if (lvPlayerNarr.Items[PlayerSCNProcessor.nCurrentCnt].BackColor == Color.PaleGreen)    // 추가 할까?
                {
                    if (nCompareTime < nMinMaxTime)    // 현재시간 + 나레이션 시간 < 최소 시작 시간 시 실행
                    {
                        PlayerSCNProcessor.MainProcessor();
                    }
                    else
                    {
                        PlayerSCNProcessor.nCurrentCnt++;
                        PlayerMainProcessor();
                    }
                }
            }
        }
        public class SCNProcessor
        {
            public ListView lvNarr;
            public Label lbWaitTimer;
            public Label lbSkipTimer;

            public Mp3Player SelectedSpk;
            public Mp3Player CommonSpk;

            String strCurSCN;
            public string strSelectedNarr;
            public String strNarrDir = "";  //wav 파일이 있는 폴더 위치
            public int nCurrentCnt = 0;     //현재 진행중인 나레이션 번호
            public String strTagTo = "";    
            public String[] strTagDevice;
            public int nTagCnt = 0;
            public int nTagMaxCnt = 0;
            public bool bAccessNext = false;   //다음 방에 입장 가능한지 확인하는 변수 
            String strWavWait;          // 상대방 끝나길 기다린다는 나레이션 경로

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

            /* 플레이어 WaitTime 타이머*/
            public System.Threading.Timer timerForWait;
            public delegate void TimerEventFiredDelegate_timerForWait();
            uint nForWait = 0;
            uint nWaitAlarmTime = 10;   //"상대방 기다리는중" 나레이션 주기
            public void MainProcessor()
            {
                // WAV 나레이션 플레이 부분
                String strWavName = lvNarr.Items[nCurrentCnt].SubItems[1].Text;
                strWavName = strNarrDir + "SCN" + strWavName.Replace("#", "") + ".wav";
                Console.WriteLine("나레이션 플레이: SCN" + strWavName.ToString());
                String strNarrNum = strWavName.Substring(strWavName.Length - 1, 1);
                //Console.WriteLine(strWavName);
                FileInfo fileTmp = new FileInfo(strWavName);
                string[] strPlayDevices = lvNarr.Items[nCurrentCnt].SubItems[8].Text.Split(',');
                if (fileTmp.Exists)  //FileInfo.Exists로 파일 존재유무 확인 "
                {
                    foreach(string strSelectedPlayDevice in strPlayDevices){
                        if (strSelectedPlayDevice == strSelectedNarr)
                            SelectedSpk.PlayMp3(strWavName);
                        else
                            CommonSpk.PlayMp3(strWavName);
                    }
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
                    if (listitem.Text == "ᐅᐅᐅ")
                        listitem.Text = "";
                }
                lvNarr.Items[nCurrentCnt].Text = "ᐅᐅᐅ";

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
                            MainForm.mainform.FindSCN(strCurSCN);
                        }
                        else
                        {
                            MainForm.mainform.SCNJSONPublish(strDN, strSelectedNarr + lvNarr.Items[nCurrentCnt].SubItems[1].Text.Replace("#", ""));   //ex. #41 -> p41/t41
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
                            Console.WriteLine("TAG CONDITION");
                            String[] strTag = s.Split('_'); //TAG_장치_다음 나레이션#
                            strTagDevice = strTag[1].Split('/');
                            strTagTo = "#" + strTag[2];
                            nTagMaxCnt = strTagDevice.Count();
                            Console.WriteLine(strTagDevice[0]);
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
                        if (s.Contains("DELAY"))
                        {
                            if (nCurrentCnt == 88)
                            {
                                if (MainForm.mainform.TaggerSCNProcessor.nCurrentCnt < 83)
                                { 
                                    timerPlayerSkipTime.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite); //PlayerSkipTimer 종료
                                    timerForWait.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);         //timerForWait 종료
                                    nForWait = 0;                                                                                                                             //timerForWait 변수 초기화
                                    timerForWait.Change(0, 1000);                                                                                                    //timerForWait 시작
                                }
                                else
                                {
                                    Console.WriteLine("Player SCN83 대기 통과");
                                }
                            }
                            else
                            {
                                timerPlayerSkipTime.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite); //PlayerSkipTimer 종료
                                timerForWait.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);         //timerForWait 종료
                                nForWait = 0;                                                                                                                             //timerForWait 변수 초기화
                                timerForWait.Change(0, 1000);                                                                                                    //timerForWait 시작
                            }                                                                                                //timerForWait 시작
                        }
                    }
                }

                MainForm.mainform.SCNJSONPublish("EM"+ strSelectedNarr, strSelectedNarr + lvNarr.Items[nCurrentCnt].SubItems[1].Text.Replace("#", ""));    //생존자 훈련소 모니터 시나리오 전송하는 부분
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
                    timerPlayerWaitTime.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite); //메인타이머 종료
                    Console.WriteLine(nCurrentCnt.ToString() + " Player Narr Wait Time End!");
                    //lvNarr.Focus();
                    nCurrentCnt++;
                    NarrPlayJudge();
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
                            timerPlayerWaitTime.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite); //메인타이머 종료
                            timerPlayerSkipTime.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite); //PlayerSkipTimer 종료
                            nCurrentCnt = listitem.Index;
                            strTagDevice = null;
                            nTagCnt = 0;
                            NarrPlayJudge();
                            break;
                        }
                    }
                }
            }
            /* ForWait 타이머: 상대방 기다리기 위한 함수*/
            public void timerForWaitWork()
            {
                nForWait += 1;                                                                      //초 마다 타이머 함수 실행되면 -1해 남은시간 줄여줌
                if ((nForWait % nWaitAlarmTime ) == 0)
                {
                    strWavWait = strNarrDir + "Wait.wav";    // 상대방 끝나길 기다린다는 나레이션 경로
                    Console.WriteLine( strWavWait );
                    SelectedSpk.PlayMp3(strWavWait);
                }
                if(bAccessNext == true)
                {
                    timerForWait.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite); //timerForWait 종료
                    bAccessNext = false;
                    nCurrentCnt++;
                    NarrPlayJudge();
                }
            }
            public void NarrPlayJudge()
            {
                uint nCurMainTime = MainForm.mainform.nMainTime;
                if (lvNarr.Items[nCurrentCnt].BackColor == SystemColors.Window)
                {
                    MainProcessor();
                }
                else
                {
                    string strNarrSkipMinMax = lvNarr.Items[nCurrentCnt].SubItems[6].Text;
                    string[] strSplit = strNarrSkipMinMax.Split(new char[] { ':' });
                    uint nMinMaxTime = UInt32.Parse(strSplit[0]) * 60 + UInt32.Parse(strSplit[1]);


                    string strTempNarrSkipMinMax = lvNarr.Items[nCurrentCnt].SubItems[7].Text;
                    string[] strTempSplit = strNarrSkipMinMax.Split(new char[] { ':' });
                    uint nSkipMinMaxTime = UInt32.Parse(strSplit[0]) * 60 + UInt32.Parse(strSplit[1]);

                    string strNarrWaitTime = lvNarr.Items[nCurrentCnt].SubItems[5].Text;
                    uint nCompareTime = nCurMainTime + UInt32.Parse(strNarrWaitTime);

                    Console.WriteLine("현재 시간: " + nCurMainTime + "  ???  최대/소 시간: " + nSkipMinMaxTime);
                    if (lvNarr.Items[nCurrentCnt].BackColor == Color.LemonChiffon)  // 스킵 할까?
                    {
                        if (nCurMainTime < nSkipMinMaxTime) // 현재시간 + 나레이션 시간 < 최대 시작 시간 시 실행
                        {
                            
                            MainProcessor();
                        }
                        else
                        {
                            nCurrentCnt++;
                            Console.WriteLine(nCurrentCnt.ToString() + "스킵");
                            NarrPlayJudge();
                        }
                    }
                    else if (lvNarr.Items[nCurrentCnt].BackColor == Color.PaleGreen)    // 추가 할까?
                    {
                        if (nCurMainTime < nSkipMinMaxTime)    // 현재시간 + 나레이션 시간 < 최소 시작 시간 시 실행
                        {
                            Console.WriteLine(nCurrentCnt.ToString() + "추가");
                            MainProcessor();
                        }
                        else
                        {
                            nCurrentCnt++;
                            NarrPlayJudge();
                        }
                    } //else if 추가 할까?
                } //else
            }// func 종료

        }
    }
}
