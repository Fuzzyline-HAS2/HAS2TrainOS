using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HAS2TrainOS
{
    public partial class MainForm : Form
    {
        /* 나레이션 메인 타이머*/
        System.Threading.Timer timerMain;
        delegate void TimerEventFiredDelegate_timerMain();
        uint nMainTime = 0;
        void timerMain_CallBack(Object state)
        {
            BeginInvoke(new TimerEventFiredDelegate_timerMain(timerMainWork));
        }
        private void timerMainWork()
        { 
            lbMainTime.Text = (nMainTime / 60).ToString("00") + ":" + (nMainTime % 60).ToString("00");    //남은 시간 uint -> String으로 변환하는 작업
            nMainTime += 1;                                                                      //초 마다 타이머 함수 실행되면 -1해 남은시간 줄여줌
        }
    }
}
