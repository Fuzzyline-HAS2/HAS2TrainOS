using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace HAS2TrainOS
{
    public partial class MainForm : Form
    {

        private void btnReady_Click(object sender, EventArgs e)
        {
            if (btnReady.Text == "READY")  //수동모드 시작하면
            {
                //client.Publish("GLOVE_SCN",new byte[0],0,true);
                GloveSelection();
                lbMainTime.Text = "00:00";
                foreach (ListViewItem lvSelectedDevice in lvDevice.Items)   // 장치들 기본 레디 셋팅 하기 위한 foreach
                {
                    string strTempLCBP = "0";
                    switch (lvSelectedDevice.SubItems[(int)listviewDevice.Name].Text[1])
                    {
                        case 'I':
                            strTempLCBP = "2";
                            break;
                        case 'R':
                            strTempLCBP = "1";
                            break;
                        default:
                            strTempLCBP = "0";
                            break;
                    }
                    DeviceListViewChange(lvSelectedDevice, State: "ready", strLCBP: strTempLCBP);
                }
                int nPlayerCnt = 0;
                foreach (ListViewItem lvSelectedDevice in lvGlove.Items)    // 훈련소때 탈출장치에 찍을 수 있는 플레이어 인원 3명 이하 인 경우 알려주기 위해 탈장 LCBP:player수 저장해서보냄
                {
                    if (lvSelectedDevice.SubItems[(int)listviewGlove.Role].Text.Contains("player")) //플레이어 명수 확인하기 위함
                        nPlayerCnt++;
                }
                DeviceListViewChange(lvDevice.Items[(int)enumDevice.EE], strLCBP: nPlayerCnt.ToString());   //탈장 LCBP 바꾸는 부분
            }
            else if (btnReady.Text == "!STOP¡")
            {
                timerMain.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite); //메인타이머 종료
                PlayerSCNProcessor.timerPlayerWaitTime.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);     //PlayerWaitTimer 종료
                PlayerSCNProcessor.timerPlayerSkipTime.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);     //PlayerSkipTimer 종료
                PlayerSCNProcessor.timerForWait.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);               //PlayertimerForWait 종료
                TaggerSCNProcessor.timerPlayerWaitTime.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);    //TaggererSkipTimer 종료
                TaggerSCNProcessor.timerPlayerSkipTime.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);    //TaggererSkipTimer 종료
                TaggerSCNProcessor.timerForWait.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);              //TaggertimerForWait 종료
                pnRoomSelect.Enabled = true;
                btnManual.Enabled = true;
                btnStart.Enabled = true;

                btnReady.Text = "READY";
                btnReady.BackColor = Color.Tomato;
                btnReady.ForeColor = Color.Black;

                btnStart.Text = "START";
                btnStart.BackColor = Color.YellowGreen;
            }
        }
        private void btnStart_Click(object sender, EventArgs e)
        {
            if (btnStart.Text == "START"  || btnStart.Text == "RESUME")  //수동모드 시작하면
            {
                if (btnManual.Enabled == true)  //Start를 맨 처음에 눌렀을경우 타이머 초기화 해서 0부터 카운트 되게
                {
                    
                    foreach (ListViewItem lvSelectedDevice in lvDevice.Items)
                    {
                        DeviceListViewChange(lvSelectedDevice, State:"scenario" , strLCBP:lvSelectedDevice.SubItems[(int)listviewDevice.LCBP].Text);
                    }
                    foreach(ListViewItem lvSelectedGlove in lvGlove.Items)
                    {
                        GloveListViewChange(lvSelectedGlove, State: "scenario");
                    }
                    //메인 파트
                    btnReady.Text = "!STOP¡";
                    btnReady.BackColor = Color.Red;
                    btnReady.ForeColor = Color.Yellow;
                    btnStart.BackColor = Color.Yellow;
                    pnRoomSelect.Enabled = false;
                    nMainTime = 0;  //메인 타이머 변수 초기화

                    //나레이션 파트
                    PlayerSCNProcessor.nCurrentCnt  = 0;         //Player 나레이션 0번부터 초기화
                    PlayerSCNProcessor.MainProcessor();

                    TaggerSCNProcessor.nCurrentCnt = 0;         //Player 나레이션 0번부터 초기화
                    TaggerSCNProcessor.MainProcessor();

                    // 글러브 전송
                    foreach (ListViewItem lvItems in lvGlove.Items)
                    {
                        GloveListViewChange(lvItems);
                    }

                }   
                //중간에 PAUSE 했다가 다시하는경우 타이머 변수 초기화 안되게 하기 위함
                //메인 타이머
                btnStart.Text = "PAUSE";
                btnManual.Enabled = false;
                timerMain.Change(0, 1000);  //메인 타이머 시작

                /* 초기화 해야할 변수들*/
                bCommonTaggerTaken = false; //공용공간에서 술래가 생명칩을 한번 뺏으면 더이상 다른 술래들이 생명칩을 뺏을수 없게 막는 변수
            }
            else // 모드 멈출 때
            {
                btnStart.Text = "RESUME";
                btnManual.Enabled = false;
                timerMain.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite); //메인타이머 종료
                PlayerSCNProcessor.timerPlayerWaitTime.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite); //메인타이머 종료
            }
        }

        private void btnManual_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem lvSelectedDevice in lvDevice.Items)
            {
                DeviceListViewChange(lvSelectedDevice, State: "setting");
                DeviceListViewChange(lvSelectedDevice, State: "activate");
            }

            /*
            if(btnManual.Text == "MANUAL")  //수동모드 시작하면
            {
                btnManual.Text = "STOP";
                btnStart.Enabled = false;
                btnReady.Enabled = false;
                nMainTime = 0;  //메인 타이머 변수 초기화
                timerMain.Change(0, 1000);  //메인 타이머 시작
            }
            else //수동 모드 멈출 때
            {
                btnManual.Text = "MANUAL";
                btnStart.Enabled = true;
                btnReady.Enabled = true;
                timerMain.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite); //메인타이머 종료
            }
            */
        }
    }
}
