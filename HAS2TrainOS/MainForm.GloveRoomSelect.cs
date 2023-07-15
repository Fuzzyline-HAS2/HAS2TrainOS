using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HAS2TrainOS
{
    public partial class MainForm
    {
        private void ColorChange(String strTrackBarName, int nTempTrackValue)
        {
            String strLabelName  = "lb" + strTrackBarName.Substring(3); //매개변수로 받아온 트랙바 이름에서 글러브 번호만 추출후 "lb" 붙여서 라벨 이름 만듦
            //Console.WriteLine(strLabelName);
            switch(nTempTrackValue)
            {
                case 0: //Player 
                    this.Controls.Find(strTrackBarName, true).FirstOrDefault().BackColor = Color.GreenYellow;
                    this.Controls.Find(strLabelName, true).FirstOrDefault().BackColor = Color.GreenYellow;
                    break;
                case 1: //None
                    this.Controls.Find(strTrackBarName, true).FirstOrDefault().BackColor = SystemColors.Control;
                    this.Controls.Find(strLabelName, true).FirstOrDefault().BackColor = SystemColors.Control;
                    break;
                case 2: //Killer
                    this.Controls.Find(strTrackBarName, true).FirstOrDefault().BackColor = Color.BlueViolet;
                    this.Controls.Find(strLabelName, true).FirstOrDefault().BackColor = Color.BlueViolet;
                    break;
                default: //Exception
                    this.Controls.Find(strTrackBarName, true).FirstOrDefault().BackColor = SystemColors.Control;
                    this.Controls.Find(strLabelName, true).FirstOrDefault().BackColor = SystemColors.Control;
                    break;
            }
        }
        private void btnGlovePShift_Click(object sender, EventArgs e)
        {
            trbP1.Value = 0;
            trbP2.Value = 0;
            trbP3.Value = 0;
            trbP4.Value = 0;
            trbP5.Value = 0;
            trbP6.Value = 0;
            trbP7.Value = 0;
            trbP8.Value = 0;
        }

        private void btnGloveNShift_Click(object sender, EventArgs e)
        {
            trbP1.Value = 1;
            trbP2.Value = 1;
            trbP3.Value = 1;
            trbP4.Value = 1;
            trbP5.Value = 1;
            trbP6.Value = 1;
            trbP7.Value = 1;
            trbP8.Value = 1;
        }

        private void BtnGloveKShift_Click(object sender, EventArgs e)
        {
            trbP1.Value = 2;
            trbP2.Value = 2;
            trbP3.Value = 2;
            trbP4.Value = 2;
            trbP5.Value = 2;
            trbP6.Value = 2;
            trbP7.Value = 2;
            trbP8.Value = 2;
        }
        private void btnGloveToggle_Click(object sender, EventArgs e)
        {
            if (trbP1.Value == 0)   trbP1.Value = 2;
            else if (trbP1.Value == 2)  trbP1.Value = 0;
            else trbP1.Value = 1;

            if (trbP2.Value == 0) trbP2.Value = 2;
            else if (trbP2.Value == 2) trbP2.Value = 0;
            else trbP2.Value = 1;

            if (trbP3.Value == 0) trbP3.Value = 2;
            else if (trbP3.Value == 2) trbP3.Value = 0;
            else trbP3.Value = 1;

            if (trbP4.Value == 0) trbP4.Value = 2;
            else if (trbP4.Value == 2) trbP4.Value = 0;
            else trbP4.Value = 1;

            if (trbP5.Value == 0) trbP5.Value = 2;
            else if (trbP5.Value == 2) trbP5.Value = 0;
            else trbP5.Value = 1;

            if (trbP6.Value == 0) trbP6.Value = 2;
            else if (trbP6.Value == 2) trbP6.Value = 0;
            else trbP6.Value = 1;

            if (trbP7.Value == 0) trbP7.Value = 2;
            else if (trbP7.Value == 2) trbP7.Value = 0;
            else trbP7.Value = 1;

            if (trbP8.Value == 0) trbP8.Value = 2;
            else if (trbP8.Value == 2) trbP8.Value = 0;
            else trbP8.Value = 1;

        }
        private void trbP1_ValueChanged(object sender, EventArgs e)
        {
            ColorChange(trbP1.Name, trbP1.Value);
        }
        private void trbP2_ValueChanged(object sender, EventArgs e)
        {
            ColorChange(trbP2.Name, trbP2.Value);
        }

        private void trbP3_ValueChanged(object sender, EventArgs e)
        {
            ColorChange(trbP3.Name, trbP3.Value);
        }

        private void trbP4_ValueChanged(object sender, EventArgs e)
        {
            ColorChange(trbP4.Name, trbP4.Value);
        }

        private void trbP5_ValueChanged(object sender, EventArgs e)
        {
            ColorChange(trbP5.Name, trbP5.Value);
        }

        private void trbP6_ValueChanged(object sender, EventArgs e)
        {
            ColorChange(trbP6.Name, trbP6.Value);
        }

        private void trbP7_ValueChanged(object sender, EventArgs e)
        {
            ColorChange(trbP7.Name, trbP7.Value);
        }

        private void trbP8_ValueChanged(object sender, EventArgs e)
        {
            ColorChange(trbP8.Name, trbP8.Value);
        }
    }
}
