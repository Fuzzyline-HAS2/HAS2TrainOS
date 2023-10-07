using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Aspose.Cells;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using static HAS2TrainOS.MainForm;
using static HAS2TrainOS.MainForm.SCNProcessor;


namespace HAS2TrainOS
{
    public partial class MainForm : Form
    {
        Mp3Player PlayerSpk = new Mp3Player();
        Mp3Player TaggerSpk = new Mp3Player();
        Mp3Player CommonSpk = new Mp3Player();
        SCNProcessor PlayerSCNProcessor = new SCNProcessor();
        SCNProcessor TaggerSCNProcessor = new SCNProcessor();

        public static MainForm mainform;
        public MainForm()
        {
            mainform = this;
            InitializeComponent();

            PlayerSpk.nDeviceNum = 2;
            TaggerSpk.nDeviceNum = 0;
            CommonSpk.nDeviceNum = 1;
            timerMain = new System.Threading.Timer((Object s) => {BeginInvoke(new TimerEventFiredDelegate_timerPlayerWaitTime(timerMainWork)); });
            
            PlayerSCNProcessor.timerPlayerWaitTime = new System.Threading.Timer((Object s) => { BeginInvoke(new TimerEventFiredDelegate_timerPlayerWaitTime(PlayerSCNProcessor.timerPlayerWaitTimeWork)); });
            PlayerSCNProcessor.timerPlayerSkipTime = new System.Threading.Timer((Object s) => { BeginInvoke(new TimerEventFiredDelegate_timerPlayerSkipTime(PlayerSCNProcessor.timerPlayerSkipTimeWork)); });
            PlayerSCNProcessor.timerForWait = new System.Threading.Timer((Object s) => { BeginInvoke(new TimerEventFiredDelegate_timerPlayerSkipTime(PlayerSCNProcessor.timerForWaitWork)); });
            PlayerSCNProcessor.strNarrDir = strFileDir + @"wavPlayerNarr\";
            PlayerSCNProcessor.lvNarr = lvPlayerNarr;
            PlayerSCNProcessor.lbWaitTimer = lbPlayerWaitTimer;
            PlayerSCNProcessor.lbSkipTimer = lbPlayerSkipTimer;
            PlayerSCNProcessor.SelectedSpk = PlayerSpk;
            PlayerSCNProcessor.CommonSpk = CommonSpk;
            PlayerSCNProcessor.strSelectedNarr = "p";

            TaggerSCNProcessor.timerPlayerWaitTime = new System.Threading.Timer((Object s) => { BeginInvoke(new TimerEventFiredDelegate_timerPlayerWaitTime(TaggerSCNProcessor.timerPlayerWaitTimeWork)); });
            TaggerSCNProcessor.timerPlayerSkipTime = new System.Threading.Timer((Object s) => { BeginInvoke(new TimerEventFiredDelegate_timerPlayerSkipTime(TaggerSCNProcessor.timerPlayerSkipTimeWork)); });
            TaggerSCNProcessor.timerForWait = new System.Threading.Timer((Object s) => { BeginInvoke(new TimerEventFiredDelegate_timerPlayerSkipTime(TaggerSCNProcessor.timerForWaitWork)); });
            TaggerSCNProcessor.strNarrDir = strFileDir + @"wavTaggerNarr\";
            TaggerSCNProcessor.lvNarr = lvTaggerNarr;
            TaggerSCNProcessor.lbWaitTimer = lbTaggerWaitTimer;
            TaggerSCNProcessor.lbSkipTimer = lbTaggerSkipTimer;
            TaggerSCNProcessor.SelectedSpk = TaggerSpk;
            TaggerSCNProcessor.CommonSpk = CommonSpk;
            TaggerSCNProcessor.strSelectedNarr = "t";

            AllDevice.strALLp = new string[] { "EI1", "EI2", "EG", "EE", "ERp", "EVp",  "EA","ED" };
            AllDevice.strALLt = new string[] { "ERt", "EVt", "ET", "EMt", "EA","ED" };
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            SetupForm setupform = new SetupForm(this);
            setupform.Show();
            ExceltoListview();              //Excel에 저장된 값 불러오기
            ComboBoxDeviceAdd();     //Device panel에 있는 콤보박스에 추가하기
            MQTT_Initializtion();         // MQTT 서버 연결
            trbLoad();                        //TrackBar 기존의 엑셀에 저장된 데이터 가져오기
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
        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            ListviewtoExcel();      //현재까지 값 엑셀에 저장+
            try
            {
                client.Disconnect();    //MQTT 연결종료
            }
            catch (Exception ex)
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
            
        }

        private void lvPlayerNarr_DoubleClick(object sender, EventArgs e)
        {
            Console.WriteLine(lvPlayerNarr.SelectedItems[0].Index);
            PlayerSCNProcessor.nCurrentCnt = lvPlayerNarr.SelectedItems[0].Index;
            PlayerSCNProcessor.MainProcessor();
        }

        private void lvTaggerNarr_DoubleClick(object sender, EventArgs e)
        {
            Console.WriteLine(lvTaggerNarr.SelectedItems[0].Index);
            TaggerSCNProcessor.nCurrentCnt = lvTaggerNarr.SelectedItems[0].Index;
            TaggerSCNProcessor.MainProcessor();
        }
    }
}
