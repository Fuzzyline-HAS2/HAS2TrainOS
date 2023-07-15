using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
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
            PlayerSpk.nDeviceNum = 0;
            KillerrSpk.nDeviceNum = 2;
            CommonSpk.nDeviceNum = 2;

        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            ExceltoListview();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PlayerSpk.nDeviceVolume = 25;
            PlayerSpk.PlayMp3("test");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            PlayerSpk.PlayMp3("test2");
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
