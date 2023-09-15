using NAudio.CoreAudioApi;
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
        public bool DeviceListViewChange(ListViewItem lvSelectedDevice, String State = "", String strLCBP = "")
        {
            if (State != "")
            {
                lvSelectedDevice.SubItems[(int)listviewGlove.State].Text = State;   //글러브 Role 데이터 치환
            }
            if(strLCBP != "")
            {
                int nCurLCBP = Int32.Parse(lvSelectedDevice.SubItems[(int)listviewGlove.LC].Text); // 현재 선택된 글러브의 배터리팩 개수
                int nLCBP = 0;    // 현재 배터리팩에서 추가하려는 배터리팩 개수
                String strApplyLCBP = "";
                try
                {
                    nLCBP = Int32.Parse(strLCBP);   //들어온 LC값이 정수로 변환 가능한지 확인
                }
                catch
                {
                    MessageBox.Show(lvSelectedDevice.SubItems[(int)listviewGlove.Name].Text + "허용되지 않는 숫자의 생명칩이 입력되었습니다\r\nLC: " + strLCBP);
                    goto DevicePublish;    // LC값이 정수로 변환이 안될때 다음 IF문인 BP섹션으로 이동
                }
                if (strLCBP.StartsWith("+") || strLCBP.StartsWith("-")) //글러브 배터리팩 +또는 -인지 확인 (게임처럼 동착하기 위함)
                {
                    int nSumLCBP = nCurLCBP + nLCBP;
                    if ((nSumLCBP <= nMaxBatteryPack) && (nSumLCBP >= 0))   //글러브가 소지 할 수 있는 배터리팩의 범위 안의 경우
                    {
                        strApplyLCBP = nSumLCBP.ToString();
                    }
                    else  //글러브가 소지 할 수 있는 배터리팩의 범위를 벗어나는 경우
                    {
                        MessageBox.Show(lvSelectedDevice.SubItems[(int)listviewGlove.Name].Text + "의 배터리팩 범위를 벗어났습니다\r\n현재BP: " + nCurLCBP.ToString() + " 추가하려는 BP: " + strLCBP);
                        goto DevicePublish;   //범위를 벗어났기 때문에 오류박스 show  후 함수 종료
                    }
                }
                else //글러브 배터리팩 +또는 - 없으면 배터리 최대소지개수에 상관없이 들어온 정수값 그대로 강제변환
                {
                    MessageBox.Show(lvSelectedDevice.SubItems[(int)listviewGlove.Name].Text + "의 배터리팩을 강제로 바꿉니다.\r\n현재BP: " + "Change BP: " + strLCBP);
                    strApplyLCBP = strLCBP;
                }
                lvSelectedDevice.SubItems[(int)listviewGlove.BP].Text = strApplyLCBP;   //lvGlove에 적용 
            }

            DevicePublish:
            DeviceJSONPublish(lvSelectedDevice.SubItems[(int)listviewDevice.Name].Text,
                                            lvSelectedDevice.SubItems[(int)listviewDevice.State].Text,
                                            lvSelectedDevice.SubItems[(int)listviewDevice.LCBP].Text);
            return false;
        }
        /* DEVICE ListView Code */
        private void lvDevice_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvDevice.SelectedItems.Count > 0)    //listview에서 선택이 되었을때만 실행되기 위해 조건
            {
                int nSelectedIndex = lvDevice.FocusedItem.Index; //현재 선택된 인데스 번호 전자용
                //Console.WriteLine(nSelectedIndex);
                tbDeviceName.Text = lvDevice.Items[nSelectedIndex].SubItems[0].Text; ;
                cbDeviceState.Text = lvDevice.Items[nSelectedIndex].SubItems[1].Text;
                tbDeviceLCBP.Text = lvDevice.Items[nSelectedIndex].SubItems[2].Text;
                //tbDeviceSCN.Text = lvDevice.Items[nSelectedIndex].SubItems[3].Text;
            }
        }

        private void btnDeviceApply_Click(object sender, EventArgs e)
        {
            if (lvDevice.SelectedItems.Count > 0)    //listview에서 선택이 되었을때만 실행되기 위해 조건
            {
                int nSelectedIndex = lvDevice.FocusedItem.Index; //현재 선택된 인데스 번호 전자용
                lvDevice.Items[nSelectedIndex].SubItems[(int)listviewDevice.State].Text = cbDeviceState.Text;
                lvDevice.Items[nSelectedIndex].SubItems[(int)listviewDevice.LCBP].Text = tbDeviceLCBP.Text;
                DeviceJSONPublish(lvDevice.Items[nSelectedIndex].SubItems[(int)listviewDevice.Name].Text,
                    lvDevice.Items[nSelectedIndex].SubItems[(int)listviewDevice.State].Text,
                    lvDevice.Items[nSelectedIndex].SubItems[(int)listviewDevice.LCBP].Text);

            }
        }
        private void ComboBoxDeviceAdd()
        {
            for(int i =0; i < lvDevice.Items.Count; i++)
            {
                cbDeviceName.Items.Add(lvDevice.Items[i].SubItems[0].Text);
            }
            cbDeviceName.Items.Add("ALL");
            cbDeviceName.Items.Add("ALL ITEMBOX");
            cbDeviceName.Items.Add("ALL REVIVAL");
            cbDeviceName.Items.Add("ALL VENT");
            cbDeviceName.SelectedIndex = lvDevice.Items.Count; // 처음 시작할때 ComboBox 'ALL'로 선택하기위해서
        }
        private void btnDeviceSetting_Click(object sender, EventArgs e)
        {
            //SituationJSONPublish("OS","TAG");
            //SituationJSONPublish("MAINOS", "TAG", "G1P1");
            switch (cbDeviceName.SelectedIndex)
            {
                case (int)enumDevice.ALL:
                    for (int i = 0; i < lvDevice.Items.Count; i++)
                    {
                        lvDevice.Items[i].SubItems[(int)listviewDevice.State].Text = "setting";
                        DeviceJSONPublish(lvDevice.Items[i].SubItems[(int)listviewDevice.Name].Text, 
                            lvDevice.Items[i].SubItems[(int)listviewDevice.State].Text);
                    }
                    break;
                case (int)enumDevice.ITEMBOX:
                    lvDevice.Items[(int)enumDevice.EI1].SubItems[(int)listviewDevice.State].Text = "setting";
                    lvDevice.Items[(int)enumDevice.EI2].SubItems[(int)listviewDevice.State].Text = "setting";
                    DeviceJSONPublish(lvDevice.Items[(int)enumDevice.EI1].SubItems[(int)listviewDevice.Name].Text, 
                        lvDevice.Items[(int)enumDevice.EI1].SubItems[(int)listviewDevice.State].Text);
                    DeviceJSONPublish(lvDevice.Items[(int)enumDevice.EI2].SubItems[(int)listviewDevice.Name].Text,
                        lvDevice.Items[(int)enumDevice.EI2].SubItems[(int)listviewDevice.State].Text);
                    break;
                case (int)enumDevice.REVIVAL:
                    lvDevice.Items[(int)enumDevice.ER1].SubItems[(int)listviewDevice.State].Text = "setting";
                    lvDevice.Items[(int)enumDevice.ER2].SubItems[(int)listviewDevice.State].Text = "setting";
                    DeviceJSONPublish(lvDevice.Items[(int)enumDevice.ER1].SubItems[(int)listviewDevice.Name].Text,
                        lvDevice.Items[(int)enumDevice.ER1].SubItems[(int)listviewDevice.State].Text);
                    DeviceJSONPublish(lvDevice.Items[(int)enumDevice.ER2].SubItems[(int)listviewDevice.Name].Text,
                        lvDevice.Items[(int)enumDevice.ER2].SubItems[(int)listviewDevice.State].Text);

                    break;
                case (int)enumDevice.VENT:
                    lvDevice.Items[(int)enumDevice.EVp].SubItems[(int)listviewDevice.State].Text = "setting";
                    lvDevice.Items[(int)enumDevice.EVt].SubItems[(int)listviewDevice.State].Text = "setting";
                    DeviceJSONPublish(lvDevice.Items[(int)enumDevice.EVp].SubItems[(int)listviewDevice.Name].Text,
                        lvDevice.Items[(int)enumDevice.EVp].SubItems[(int)listviewDevice.State].Text);
                    DeviceJSONPublish(lvDevice.Items[(int)enumDevice.EVt].SubItems[(int)listviewDevice.Name].Text,
                        lvDevice.Items[(int)enumDevice.EVt].SubItems[(int)listviewDevice.State].Text);

                    break;
                default:
                    lvDevice.Items[cbDeviceName.SelectedIndex].SubItems[(int)listviewDevice.State].Text = "setting";
                    DeviceJSONPublish(lvDevice.Items[cbDeviceName.SelectedIndex].SubItems[(int)listviewDevice.Name].Text,
                        lvDevice.Items[cbDeviceName.SelectedIndex].SubItems[(int)listviewDevice.State].Text);
                    break; 
            }
        }

        private void btnDeviceReady_Click(object sender, EventArgs e)
        {
            switch (cbDeviceName.SelectedIndex)
            {
                case (int)enumDevice.ALL:
                    for (int i = 0; i < lvDevice.Items.Count; i++)
                    {
                        lvDevice.Items[i].SubItems[(int)listviewDevice.State].Text = "ready";
                        DeviceJSONPublish(lvDevice.Items[i].SubItems[(int)listviewDevice.Name].Text,
                            lvDevice.Items[i].SubItems[(int)listviewDevice.State].Text);
                    }
                    break;
                case (int)enumDevice.ITEMBOX:
                    lvDevice.Items[(int)enumDevice.EI1].SubItems[(int)listviewDevice.State].Text = "ready";
                    lvDevice.Items[(int)enumDevice.EI2].SubItems[(int)listviewDevice.State].Text = "ready";
                    DeviceJSONPublish(lvDevice.Items[(int)enumDevice.EI1].SubItems[(int)listviewDevice.Name].Text,
                        lvDevice.Items[(int)enumDevice.EI1].SubItems[(int)listviewDevice.State].Text);
                    DeviceJSONPublish(lvDevice.Items[(int)enumDevice.EI2].SubItems[(int)listviewDevice.Name].Text,
                        lvDevice.Items[(int)enumDevice.EI2].SubItems[(int)listviewDevice.State].Text);
                    break;
                case (int)enumDevice.REVIVAL:
                    lvDevice.Items[(int)enumDevice.ER1].SubItems[(int)listviewDevice.State].Text = "ready";
                    lvDevice.Items[(int)enumDevice.ER2].SubItems[(int)listviewDevice.State].Text = "ready";
                    DeviceJSONPublish(lvDevice.Items[(int)enumDevice.ER1].SubItems[(int)listviewDevice.Name].Text,
                        lvDevice.Items[(int)enumDevice.ER1].SubItems[(int)listviewDevice.State].Text);
                    DeviceJSONPublish(lvDevice.Items[(int)enumDevice.ER2].SubItems[(int)listviewDevice.Name].Text,
                        lvDevice.Items[(int)enumDevice.ER2].SubItems[(int)listviewDevice.State].Text);

                    break;
                case (int)enumDevice.VENT:
                    lvDevice.Items[(int)enumDevice.EVp].SubItems[(int)listviewDevice.State].Text = "ready";
                    lvDevice.Items[(int)enumDevice.EVt].SubItems[(int)listviewDevice.State].Text = "ready";
                    DeviceJSONPublish(lvDevice.Items[(int)enumDevice.EVp].SubItems[(int)listviewDevice.Name].Text,
                        lvDevice.Items[(int)enumDevice.EVp].SubItems[(int)listviewDevice.State].Text);
                    DeviceJSONPublish(lvDevice.Items[(int)enumDevice.EVt].SubItems[(int)listviewDevice.Name].Text,
                        lvDevice.Items[(int)enumDevice.EVt].SubItems[(int)listviewDevice.State].Text);

                    break;
                default:
                    lvDevice.Items[cbDeviceName.SelectedIndex].SubItems[(int)listviewDevice.State].Text = "ready";
                    DeviceJSONPublish(lvDevice.Items[cbDeviceName.SelectedIndex].SubItems[(int)listviewDevice.Name].Text,
                        lvDevice.Items[cbDeviceName.SelectedIndex].SubItems[(int)listviewDevice.State].Text);
                    break;
            }
        }

        private void btnDeviceActivate_Click(object sender, EventArgs e)
        {
            switch (cbDeviceName.SelectedIndex)
            {
                case (int)enumDevice.ALL:
                    for (int i = 0; i < lvDevice.Items.Count; i++)
                    {
                        lvDevice.Items[i].SubItems[(int)listviewDevice.State].Text = "activate";
                        DeviceJSONPublish(lvDevice.Items[i].SubItems[(int)listviewDevice.Name].Text,
                            lvDevice.Items[i].SubItems[(int)listviewDevice.State].Text);
                    }
                    break;
                case (int)enumDevice.ITEMBOX:
                    lvDevice.Items[(int)enumDevice.EI1].SubItems[(int)listviewDevice.State].Text = "activate";
                    lvDevice.Items[(int)enumDevice.EI2].SubItems[(int)listviewDevice.State].Text = "activate";
                    DeviceJSONPublish(lvDevice.Items[(int)enumDevice.EI1].SubItems[(int)listviewDevice.Name].Text,
                        lvDevice.Items[(int)enumDevice.EI1].SubItems[(int)listviewDevice.State].Text);
                    DeviceJSONPublish(lvDevice.Items[(int)enumDevice.EI2].SubItems[(int)listviewDevice.Name].Text,
                        lvDevice.Items[(int)enumDevice.EI2].SubItems[(int)listviewDevice.State].Text);
                    break;
                case (int)enumDevice.REVIVAL:
                    lvDevice.Items[(int)enumDevice.ER1].SubItems[(int)listviewDevice.State].Text = "activate";
                    lvDevice.Items[(int)enumDevice.ER2].SubItems[(int)listviewDevice.State].Text = "activate";
                    DeviceJSONPublish(lvDevice.Items[(int)enumDevice.ER1].SubItems[(int)listviewDevice.Name].Text,
                        lvDevice.Items[(int)enumDevice.ER1].SubItems[(int)listviewDevice.State].Text);
                    DeviceJSONPublish(lvDevice.Items[(int)enumDevice.ER2].SubItems[(int)listviewDevice.Name].Text,
                        lvDevice.Items[(int)enumDevice.ER2].SubItems[(int)listviewDevice.State].Text);

                    break;
                case (int)enumDevice.VENT:
                    lvDevice.Items[(int)enumDevice.EVp].SubItems[(int)listviewDevice.State].Text = "activate";
                    lvDevice.Items[(int)enumDevice.EVt].SubItems[(int)listviewDevice.State].Text = "activate";
                    DeviceJSONPublish(lvDevice.Items[(int)enumDevice.EVp].SubItems[(int)listviewDevice.Name].Text,
                        lvDevice.Items[(int)enumDevice.EVp].SubItems[(int)listviewDevice.State].Text);
                    DeviceJSONPublish(lvDevice.Items[(int)enumDevice.EVt].SubItems[(int)listviewDevice.Name].Text,
                        lvDevice.Items[(int)enumDevice.EVt].SubItems[(int)listviewDevice.State].Text);

                    break;
                default:
                    lvDevice.Items[cbDeviceName.SelectedIndex].SubItems[(int)listviewDevice.State].Text = "activate";
                    DeviceJSONPublish(lvDevice.Items[cbDeviceName.SelectedIndex].SubItems[(int)listviewDevice.Name].Text,
                        lvDevice.Items[cbDeviceName.SelectedIndex].SubItems[(int)listviewDevice.State].Text);
                    break;
            }
        }

        /* GLOVE ListView Code */
        private void lvGlove_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvGlove.SelectedItems.Count > 0)    //listview에서 선택이 되었을때만 실행되기 위해 조건
            {
                int nSelectedIndex = lvGlove.FocusedItem.Index; //현재 선택된 인데스 번호 전자용
                //Console.WriteLine(nSelectedIndex);
                tbGloveName.Text = lvGlove.Items[nSelectedIndex].SubItems[0].Text; ;
                cbGloveRole.Text = lvGlove.Items[nSelectedIndex].SubItems[1].Text;
                cbGloveState.Text = lvGlove.Items[nSelectedIndex].SubItems[2].Text;
                tbGloveLifeChip.Text = lvGlove.Items[nSelectedIndex].SubItems[3].Text;
                tbGloveBattery.Text = lvGlove.Items[nSelectedIndex].SubItems[4].Text;
                //tbGloveSCN.Text = lvGlove.Items[nSelectedIndex].SubItems[5].Text;
            }
        }

        private void btnGloveApply_Click(object sender, EventArgs e)
        {
            if (lvGlove.SelectedItems.Count > 0)    //listview에서 선택이 되었을때만 실행되기 위해 조건
            {
                GloveListViewChange(lvGlove.FocusedItem, cbGloveRole.Text, cbGloveState.Text, tbGloveLifeChip.Text, tbGloveBattery.Text);
            }
        }

    }
}
