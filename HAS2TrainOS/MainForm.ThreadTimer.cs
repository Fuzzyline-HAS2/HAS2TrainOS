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
        System.Threading.Timer PCM_ThreadTimer;
        delegate void TimerEventFiredDelegate_PCM();
        void PCM_timerCallBack(Object state)
        {
            BeginInvoke(new TimerEventFiredDelegate_PCM(PCM_timerWork));
        }
        private void PCM_timerWork()
        {

        }
    }
}
