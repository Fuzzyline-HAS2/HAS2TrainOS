using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;

namespace HAS2TrainOS
{
    public partial class MainForm
    {
        private void GloveSelection()
        {
            if (rbG1.Checked) TrackBarFindAndChange("G1");
            else if (rbG2.Checked) TrackBarFindAndChange("G2");
            else if (rbG3.Checked) TrackBarFindAndChange("G3");
            else if (rbG4.Checked) TrackBarFindAndChange("G4");
            else MessageBox.Show("그룹을 선택해주세요.");
        }
        private void TrackBarFindAndChange(String gloveGroup)
        {
            for (int i = (int)enumGlove.P1; i <= (int)enumGlove.P8; i++)
            {
                lvGlove.Items[i].SubItems[(int)listviewGlove.Name].Text = gloveGroup +"P" + (i + 1).ToString(); //listview name G1 붙여줌
                foreach (Control tmp in pnRoomSelect.Controls)  //패널 안에서 TrackBar 찾기
                {
                    if (("trbP" + (i + 1).ToString()) == tmp.Name)
                    {
                        TrackBar trb = (TrackBar)tmp;
                        lvGlove.Items[i].SubItems[(int)listviewGlove.Role].Text = RoleReturn(trb.Value);    //TrackBar에 맞춘 Role 전송
                        lvGlove.Items[i].BackColor = ColorReturn(trb.Value);                                            //TrackBar에 맞춘 색 전송
                        if (trb.Value == 1)
                        {
                            lvGlove.Items[i].ForeColor = Color.Gray;    //none 일때만 글자색 회색
                        }
                        else
                        {
                            lvGlove.Items[i].ForeColor = Color.Black;    //none 일때만 글자색 회색
                        }
                    }
                }
                GloveJSONPublish(lvGlove.Items[i].SubItems[(int)listviewGlove.Name].Text, 
                    lvGlove.Items[i].SubItems[(int)listviewGlove.Role].Text, 
                    lvGlove.Items[i].SubItems[(int)listviewGlove.State].Text, 
                    lvGlove.Items[i].SubItems[(int)listviewGlove.LC].Text, 
                    lvGlove.Items[i].SubItems[(int)listviewGlove.BP].Text);
            }
        }
        private String RoleReturn(int value)
        {
            switch (value)
            {
                case 0: return "player";
                case 1: return "none";
                case 2: return "killer";
                default: return "error";
            }
        }
        private Color ColorReturn(int value)
        {
            switch (value)
            {
                case 0: return Color.PaleGreen;
                case 1: return Color.LightGray;
                case 2: return Color.Violet;
                default: return Color.Gray;
            }
        }
        private void ColorChange(String strTrackBarName, int nTempTrackValue)
        {
            String strLabelName  = "lb" + strTrackBarName.Substring(3); //매개변수로 받아온 트랙바 이름에서 글러브 번호만 추출후 "lb" 붙여서 라벨 이름 만듦
            //Console.WriteLine(strLabelName);
            switch(nTempTrackValue)
            {
                case 0: //Player 
                    this.Controls.Find(strTrackBarName, true).FirstOrDefault().BackColor = Color.YellowGreen;
                    this.Controls.Find(strLabelName, true).FirstOrDefault().BackColor = Color.YellowGreen;
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
