using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Aspose.Cells;

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
            
            
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            ListviewtoExcel();      //현재까지 값 엑셀에 저장
            client.Disconnect();    //MQTT 연결종료
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
    }
}
