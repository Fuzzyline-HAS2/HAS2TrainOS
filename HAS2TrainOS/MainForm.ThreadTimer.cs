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

        /* 플레이어 WaitTime 타이머*/
        System.Threading.Timer timerPlayerWaitTime;
        delegate void TimerEventFiredDelegate_timerPlayerWaitTime();
        uint nPlayerWaitTime = 0;
        void timerPlayerWaitTime_CallBack(Object state)
        {
            BeginInvoke(new TimerEventFiredDelegate_timerPlayerWaitTime(timerPlayerWaitTimeWork));
        }
        private void timerPlayerWaitTimeWork()
        {
            lbMainTime.Text = (nPlayerWaitTime / 60).ToString("00") + ":" + (nPlayerWaitTime % 60).ToString("00");    //남은 시간 uint -> String으로 변환하는 작업
            nPlayerWaitTime += 1;                                                                      //초 마다 타이머 함수 실행되면 -1해 남은시간 줄여줌
            //Gamesys_TimeAction();
        }
        /* 플레이어 SkipTime 타이머*/
        System.Threading.Timer timerPlayerSkipTime;
        delegate void TimerEventFiredDelegate_timerPlayerSkipTime();
        uint nPlayerSkipTime = 0;
        void timerPlayerSkipTime_CallBack(Object state)
        {
            BeginInvoke(new TimerEventFiredDelegate_timerPlayerSkipTime(timerPlayerSkipTimeWork));
        }
        private void timerPlayerSkipTimeWork()
        {
            lbMainTime.Text = (nPlayerSkipTime / 60).ToString("00") + ":" + (nPlayerSkipTime % 60).ToString("00");    //남은 시간 uint -> String으로 변환하는 작업
            nPlayerSkipTime += 1;                                                                      //초 마다 타이머 함수 실행되면 -1해 남은시간 줄여줌
            //Gamesys_TimeAction();
        }


        /* 술래 WaitTime 타이머*/
        System.Threading.Timer timerTaggerWaitTime;
        delegate void TimerEventFiredDelegate_timerTaggerWaitTime();
        uint nTaggerWaitTime = 0;
        void timerTaggerWaitTime_CallBack(Object state)
        {
            BeginInvoke(new TimerEventFiredDelegate_timerTaggerWaitTime(timerTaggerWaitTimeWork));
        }
        private void timerTaggerWaitTimeWork()
        {
            lbMainTime.Text = (nTaggerWaitTime / 60).ToString("00") + ":" + (nTaggerWaitTime % 60).ToString("00");    //남은 시간 uint -> String으로 변환하는 작업
            nTaggerWaitTime += 1;                                                                      //초 마다 타이머 함수 실행되면 -1해 남은시간 줄여줌
            //Gamesys_TimeAction();
        }

        /* 술래 SkipTime 타이머*/
        System.Threading.Timer timerTaggerSkipTime;
        delegate void TimerEventFiredDelegate_timerTaggerSkipTime();
        uint nTaggerSkipTime = 0;
        void timerTaggerSkipTime_CallBack(Object state)
        {
            BeginInvoke(new TimerEventFiredDelegate_timerTaggerSkipTime(timerTaggerSkipTimeWork));
        }
        private void timerTaggerSkipTimeWork()
        {
            lbMainTime.Text = (nTaggerSkipTime / 60).ToString("00") + ":" + (nTaggerSkipTime % 60).ToString("00");    //남은 시간 uint -> String으로 변환하는 작업
            nTaggerSkipTime += 1;                                                                      //초 마다 타이머 함수 실행되면 -1해 남은시간 줄여줌
            //Gamesys_TimeAction();
        }

    }
}
