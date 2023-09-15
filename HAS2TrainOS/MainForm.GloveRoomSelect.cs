using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using NAudio.Dsp;

namespace HAS2TrainOS
{
    public partial class MainForm
    {
        public bool GloveListViewChange(ListViewItem lvSelectedGlove, String Role = "", String State = "", String strLC = "", String strBP = "")
        {
            if(Role != "")
            {
                lvSelectedGlove.SubItems[(int)listviewGlove.Role].Text = Role;   //글러브 Role 데이터 치환
            }
            if (State != "")
            {
                lvSelectedGlove.SubItems[(int)listviewGlove.State].Text = State;   //글러브 Role 데이터 치환
            }
            if (strLC != "")
            {
                int nCurLC = Int32.Parse(lvSelectedGlove.SubItems[(int)listviewGlove.LC].Text); // 현재 선택된 글러브의 배터리팩 개수
                int nLC = 0;    // 현재 배터리팩에서 추가하려는 배터리팩 개수
                String strApplyLC = "";
                try
                {
                    nLC = Int32.Parse(strLC);   //들어온 LC값이 정수로 변환 가능한지 확인
                }
                catch
                { 
                    MessageBox.Show(lvSelectedGlove.SubItems[(int)listviewGlove.Name].Text + "허용되지 않는 숫자의 생명칩이 입력되었습니다\r\nLC: " + strLC);
                    goto GloveBP;    // LC값이 정수로 변환이 안될때 다음 IF문인 BP섹션으로 이동
                }
                if (strLC.StartsWith("+") || strLC.StartsWith("-")) //글러브 배터리팩 +또는 -인지 확인 (게임처럼 동착하기 위함)
                {
                    int nSumLC = nCurLC + nLC;
                    if ((nSumLC <= nMaxLifeChip) && (nSumLC >= 0))   //글러브가 소지 할 수 있는 배터리팩의 범위 안의 경우
                    {
                        strApplyLC = nSumLC.ToString();
                    }
                    else  //글러브가 소지 할 수 있는 배터리팩의 범위를 벗어나는 경우
                    {
                        MessageBox.Show(lvSelectedGlove.SubItems[(int)listviewGlove.Name].Text + "의 배터리팩 범위를 벗어났습니다\r\n현재LC: " + nCurLC.ToString() + " 추가하려는 LC: " + strLC);
                        goto GloveBP;   //범위를 벗어났기 때문에 오류박스 show  후 함수 종료
                    }
                }
                else //글러브 배터리팩 +또는 - 없으면 배터리 최대소지개수에 상관없이 들어온 정수값 그대로 강제변환
                {
                    //MessageBox.Show(lvSelectedGlove.SubItems[(int)listviewGlove.Name].Text + "의 배터리팩을 강제로 바꿉니다.\r\n현재LC: " + "Change LC: " + strBP);
                    strApplyLC = strLC;
                }
                lvSelectedGlove.SubItems[(int)listviewGlove.LC].Text = strApplyLC;   //lvGlove에 적용 

                if (strApplyLC == "0")   //생명칩이 0으로 바뀔때 ghost로 적용
                {
                    if (lvSelectedGlove.SubItems[(int)listviewGlove.Role].Text != "tagger" )  //role이 플레이어 일때만 ghost로
                    { 
                        lvSelectedGlove.SubItems[(int)listviewGlove.Role].Text = "ghost";   //lvGlove에 적용
                        lvSelectedGlove.BackColor = Color.LightBlue;
                    }
                }
                else if(lvSelectedGlove.SubItems[(int)listviewGlove.Role].Text == "ghost")
                {
                    if(strApplyLC == "1")
                    {
                        lvSelectedGlove.SubItems[(int)listviewGlove.Role].Text = "revival";   //lvGlove에 적용 
                        lvSelectedGlove.BackColor = Color.LightYellow;
                    }
                }
            }

            GloveBP:
            if (strBP != "")
            {
                int nCurBP = Int32.Parse(lvSelectedGlove.SubItems[(int)listviewGlove.BP].Text); // 현재 선택된 글러브의 배터리팩 개수
                int nBP = 0;    // 현재 배터리팩에서 추가하려는 배터리팩 개수
                String strApplyBP = "";
                try
                {
                    nBP = Int32.Parse(strBP);   //들어온 BP값이 정수로 변환 가능한지 확인
                }
                catch
                {
                    MessageBox.Show(lvSelectedGlove.SubItems[(int)listviewGlove.Name].Text + "허용되지 않는 숫자의 배터리팩이 입력되었습니다\r\nBP: " + strBP);
                    goto GlovePublish;    // BP값이 정수로 변환이 안될때 함수 종료
                }
                if (strBP.StartsWith("+") || strBP.StartsWith("-")) //글러브 배터리팩 +또는 -인지 확인 (게임처럼 동착하기 위함)
                {
                    int nSumBP = nCurBP + nBP;
                    if((nSumBP <= nMaxBatteryPack) && (nSumBP >= 0))   //글러브가 소지 할 수 있는 배터리팩의 범위 안의 경우
                    { 
                        strApplyBP = nSumBP.ToString();
                    }
                    else  //글러브가 소지 할 수 있는 배터리팩의 범위를 벗어나는 경우
                    {
                        MessageBox.Show(lvSelectedGlove.SubItems[(int)listviewGlove.Name].Text + "의 배터리팩 범위를 벗어났습니다\r\n현재BP: " + nCurBP.ToString() + " 추가하려는 BP: " + strBP);
                        goto GlovePublish;   //범위를 벗어났기 때문에 오류박스 show  후 함수 종료
                    }
                }
                else //글러브 배터리팩 +또는 - 없으면 배터리 최대소지개수에 상관없이 들어온 정수값 그대로 강제변환
                {
                    //MessageBox.Show(lvSelectedGlove.SubItems[(int)listviewGlove.Name].Text + "의 배터리팩을 강제로 바꿉니다.\r\n현재BP: " + "Change BP: " + strBP);
                    strApplyBP = strBP;
                }
                lvSelectedGlove.SubItems[(int)listviewGlove.BP].Text = strApplyBP;   //lvGlove에 적용 
            }

        GlovePublish:
            GloveJSONPublish(lvSelectedGlove.SubItems[(int)listviewGlove.Name].Text,
                                                                lvSelectedGlove.SubItems[(int)listviewGlove.Role].Text,
                                                                lvSelectedGlove.SubItems[(int)listviewGlove.State].Text,
                                                                lvSelectedGlove.SubItems[(int)listviewGlove.LC].Text,
                                                                lvSelectedGlove.SubItems[(int)listviewGlove.BP].Text);
            return false;
        }
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
                        switch (trb.Value)
                        {
                            case 0:
                                lvGlove.Items[i].ForeColor = Color.Black;    //none 일때만 글자색 회색
                                GloveListViewChange(lvGlove.Items[i], Role: "player", State: "ready", strLC: "1", strBP: "0");         //TrackBar에 맞춘 Role 전송
                                lvGlove.Items[i].BackColor = Color.YellowGreen;
                                break;
                            case 1:
                                lvGlove.Items[i].ForeColor = Color.Gray;    //none 일때만 글자색 회색
                                GloveListViewChange(lvGlove.Items[i], Role: "none",State:"ready", strLC: "1", strBP: "0");         //TrackBar에 맞춘 Role 전송
                                lvGlove.Items[i].BackColor = Color.LightGray;
                                break; 
                            case 2:
                                lvGlove.Items[i].ForeColor = Color.Black;    //none 일때만 글자색 회색
                                GloveListViewChange(lvGlove.Items[i], Role: "tagger", State: "ready", strLC: "0", strBP: "0");         //TrackBar에 맞춘 Role 전송
                                lvGlove.Items[i].BackColor = Color.BlueViolet;
                                break;
                            default:
                                lvGlove.Items[i].ForeColor = Color.Black;    //none 일때만 글자색 회색
                                GloveListViewChange(lvGlove.Items[i], Role: "error", State: "ready", strLC: "1", strBP: "0");         //TrackBar에 맞춘 Role 전송
                                lvGlove.Items[i].BackColor = SystemColors.Control;
                                break;
                        }
                    }
                }
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
                case 2: //Tagger
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
