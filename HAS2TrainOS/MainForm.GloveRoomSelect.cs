using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using NAudio.Dsp;
using System.Reflection;

namespace HAS2TrainOS
{
    public partial class MainForm
    {
        public bool GloveListViewChange(ListViewItem lvSelectedGlove, String Name = "", String Role = "", String State = "", String strLC = "", String strBP = "")
        {
            if (Name != "")
            {
                lvSelectedGlove.SubItems[(int)listviewGlove.Name].Text = Name;   //글러브 Role 데이터 치환
            }
            if (Role != "")
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
                    MessageBox.Show(lvSelectedGlove.SubItems[(int)listviewGlove.Name].Text + " 허용되지 않는 숫자의 생명칩이 입력되었습니다\r\nLC: " + strLC);
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
                    if (lvSelectedGlove.SubItems[(int)listviewGlove.Role].Text == "player")  //role이 플레이어 일때만 ghost로
                    {
                        if (Role != "ghost") //이미 glovelistviewchange가 ghost로 들어와서 바꿔준 경우 실행 안하기 위해
                        {
                            lvSelectedGlove.SubItems[(int)listviewGlove.Role].Text = "ghost";   //lvGlove에 적용 
                        }
                    }
                }
                else if(lvSelectedGlove.SubItems[(int)listviewGlove.Role].Text == "ghost")
                {
                    if(strApplyLC == "1")
                    {
                        if (lvSelectedGlove.SubItems[(int)listviewGlove.Role].Text == "ghost")  //role이 플레이어 일때만 ghost로
                        {
                            if (Role != "player") //이미 glovelistviewchange가 player 들어와서 바꿔준 경우 실행 안하기 위해
                            {
                                lvSelectedGlove.SubItems[(int)listviewGlove.Role].Text = "revival";   //lvGlove에 적용 
                            }
                        }
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
            int nIndex = 0;
            String strGoveRole = "none";
            String strLifeChip = "0";
            Color tmpBackColor = SystemColors.Control;
            Color tmpForeColor = Color.Black;
            List<string> listAGp = new List<string>();  //플레이어 글러브를 리스트로 하나씩 저장하기 위한 리스트
            List<string> listAGt = new List<string>();  //술래 글러브를 리스트로 하나씩 저장하기 위한 리스트

            foreach (ListViewItem lvSelectedGlove in lvGlove.Items)
            {
                nIndex++;                                                                       //현재 선택중인 글러브 인덱스 저장용 변수 foreach에 따라 +1씩 증가함
                foreach (Control tmp in pnRoomSelect.Controls)                  //패널 안에서 TrackBar 찾기
                {
                    String strTrackBarName = "trbP" + nIndex.ToString();    //찾아야할 트랙바 이름
                    if (strTrackBarName == tmp.Name)                              //foreach문 이용해서 trackbar 하나씩 비교해서 찾음
                    {
                        TrackBar trb = (TrackBar)tmp;
                        switch (trb.Value)
                        {
                            case 0:
                                strGoveRole = "player";
                                strLifeChip = "1";
                                tmpBackColor = Color.YellowGreen;
                                tmpForeColor = Color.Black;
                                listAGp.Add(gloveGroup + lvSelectedGlove.Text.Substring(2));  //플레이어 글러브 리스트에 추가

                                break;
                            case 1:
                                strGoveRole = "none";
                                strLifeChip = "0";
                                tmpBackColor = Color.LightGray;
                                tmpForeColor = Color.Gray;
                                break;
                            case 2:
                                strGoveRole = "tagger";
                                strLifeChip = "0";
                                tmpBackColor = Color.BlueViolet;
                                tmpForeColor = Color.Black;
                                listAGt.Add(gloveGroup + lvSelectedGlove.Text.Substring(2));  //술래 글러브 리스트에 추가
                                break;
                            default:
                                strGoveRole = "error";
                                strLifeChip = "0";
                                tmpBackColor = Color.Black;
                                tmpForeColor = Color.White;
                                break;
                        }
                    }
                }
                string strGloveName = gloveGroup + "P" + nIndex.ToString();                                 //글러브 이름 만들기
                GloveListViewChange(lvSelectedGlove,Name:strGloveName, 
                                                                        Role: strGoveRole,
                                                                        State: "ready",
                                                                        strLC: strLifeChip,
                                                                        strBP: "0");                                                //TrackBar에 맞춘 Name, Role,LC,BP 전송
                lvSelectedGlove.ForeColor = tmpForeColor;                                                            //TrackBar에 맞춘 글자색 변경
                lvSelectedGlove.BackColor = tmpBackColor;                                                           //TrackBar에 맞춘 바탕색 변경
            }

            AllDevice.strAGp = listAGp.ToArray();
            AllDevice.strAGt = listAGt.ToArray();
            /*
            PlayerSCNProcessor.strAGp = listAGp.ToArray();    //플레이어 리스트를 배열로 변환
            PlayerSCNProcessor.strAGt = listAGt.ToArray();    //플레이어 리스트를 배열로 변환
            TaggerSCNProcessor.strAGp = listAGp.ToArray();    //플레이어 리스트를 배열로 변환
            TaggerSCNProcessor.strAGt = listAGt.ToArray();    //술래 리스트를 배열로 변환
            */
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
        private void trbLoad()
        {
            string strGroupNum = lvGlove.Items[0].Text.Substring(0,2);
            Console.WriteLine("GROUP: "+strGroupNum);
            foreach (Control tmp in pnRoomSelect.Controls)                  //패널 안에서 TrackBar 찾기
            {
                String strTrackBarName = "rb" + strGroupNum;    //찾아야할 트랙바 이름
                if (strTrackBarName == tmp.Name)                              //foreach문 이용해서 trackbar 하나씩 비교해서 찾음
                {
                    RadioButton rb = (RadioButton)tmp;
                    rb.Checked = true;
                }
            }
            int nIndex = 0;
            foreach (ListViewItem lvSelectedGlove in lvGlove.Items)
            {
                nIndex++;
                foreach (Control tmp in pnRoomSelect.Controls)                  //패널 안에서 TrackBar 찾기
                {
                    String strTrackBarName = "trbP" + nIndex.ToString();    //찾아야할 트랙바 이름
                    if (strTrackBarName == tmp.Name)                              //foreach문 이용해서 trackbar 하나씩 비교해서 찾음
                    {
                        TrackBar trb = (TrackBar)tmp;
                        if (lvSelectedGlove.SubItems[(int)listviewGlove.Role].Text == "player")
                            trb.Value = 0;
                        else if (lvSelectedGlove.SubItems[(int)listviewGlove.Role].Text == "tagger")
                            trb.Value = 2;
                        else
                            trb.Value = 1;
                    }
                }
            }
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
