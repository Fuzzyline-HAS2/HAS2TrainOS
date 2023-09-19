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
                GloveSelection();
            }
            else if (btnReady.Text == "!STOP¡")
            {
                timerMain.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite); //메인타이머 종료
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
                    //메인 파트
                    btnReady.Text = "!STOP¡";
                    btnReady.BackColor = Color.Red;
                    btnReady.ForeColor = Color.Yellow;
                    btnStart.BackColor = Color.Yellow;
                    pnRoomSelect.Enabled = false;
                    nMainTime = 0;  //메인 타이머 변수 초기화

                    //나레이션 파트
                    nPlayerCur  = 0;         //Player 나레이션 0번부터 초기화
                    PlayerNarr();

                    // 글러브 전송
                    foreach(ListViewItem lvItems in lvGlove.Items)
                    {
                        GloveListViewChange(lvItems);
                    }

                }   
                //중간에 PAUSE 했다가 다시하는경우 타이머 변수 초기화 안되게 하기 위함
                //메인 타이머
                btnStart.Text = "PAUSE";
                btnManual.Enabled = false;
                timerMain.Change(0, 1000);  //메인 타이머 시작
            }
            else // 모드 멈출 때
            {
                btnStart.Text = "RESUME";
                btnManual.Enabled = false;
                timerMain.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite); //메인타이머 종료
                timerPlayerWaitTime.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite); //메인타이머 종료
            }
        }

        private void btnManual_Click(object sender, EventArgs e)
        {
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
        }
    }
}
