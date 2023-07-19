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
                
            }
        }

        private void btnDeviceApply_Click(object sender, EventArgs e)
        {
            if (lvDevice.SelectedItems.Count > 0)    //listview에서 선택이 되었을때만 실행되기 위해 조건
            {
                int nSelectedIndex = lvDevice.FocusedItem.Index; //현재 선택된 인데스 번호 전자용
                lvDevice.Items[nSelectedIndex].SubItems[1].Text = cbDeviceState.Text;
                lvDevice.Items[nSelectedIndex].SubItems[2].Text = tbDeviceLCBP.Text;

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
            switch (cbDeviceName.SelectedIndex)
            {
                case (int)enumDevice.ALL:
                    for (int i = 0; i < lvDevice.Items.Count; i++)
                    {
                        lvDevice.Items[i].SubItems[(int)listviewDevice.State].Text = "setting";
                    }
                    break;
                case (int)enumDevice.ITEMBOX:
                    lvDevice.Items[(int)enumDevice.EI1].SubItems[(int)listviewDevice.State].Text = "setting";
                    lvDevice.Items[(int)enumDevice.EI2].SubItems[(int)listviewDevice.State].Text = "setting";
                    break;
                case (int)enumDevice.REVIVAL:
                    lvDevice.Items[(int)enumDevice.ER1].SubItems[(int)listviewDevice.State].Text = "setting";
                    lvDevice.Items[(int)enumDevice.ER2].SubItems[(int)listviewDevice.State].Text = "setting";
                    break;
                case (int)enumDevice.VENT:
                    lvDevice.Items[(int)enumDevice.EV1].SubItems[(int)listviewDevice.State].Text = "setting";
                    lvDevice.Items[(int)enumDevice.EV2].SubItems[(int)listviewDevice.State].Text = "setting";
                    break;
                default:
                    lvDevice.Items[cbDeviceName.SelectedIndex].SubItems[(int)listviewDevice.State].Text = "setting";
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
                    }
                    break;
                case (int)enumDevice.ITEMBOX:
                    lvDevice.Items[(int)enumDevice.EI1].SubItems[(int)listviewDevice.State].Text = "ready";
                    lvDevice.Items[(int)enumDevice.EI2].SubItems[(int)listviewDevice.State].Text = "ready";
                    break;
                case (int)enumDevice.REVIVAL:
                    lvDevice.Items[(int)enumDevice.ER1].SubItems[(int)listviewDevice.State].Text = "ready";
                    lvDevice.Items[(int)enumDevice.ER2].SubItems[(int)listviewDevice.State].Text = "ready";
                    break;
                case (int)enumDevice.VENT:
                    lvDevice.Items[(int)enumDevice.EV1].SubItems[(int)listviewDevice.State].Text = "ready";
                    lvDevice.Items[(int)enumDevice.EV2].SubItems[(int)listviewDevice.State].Text = "ready";
                    break;
                default:
                    lvDevice.Items[cbDeviceName.SelectedIndex].SubItems[(int)listviewDevice.State].Text = "ready";
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
                    }
                    break;
                case (int)enumDevice.ITEMBOX:
                    lvDevice.Items[(int)enumDevice.EI1].SubItems[(int)listviewDevice.State].Text = "activate";
                    lvDevice.Items[(int)enumDevice.EI2].SubItems[(int)listviewDevice.State].Text = "activate";
                    break;
                case (int)enumDevice.REVIVAL:
                    lvDevice.Items[(int)enumDevice.ER1].SubItems[(int)listviewDevice.State].Text = "activate";
                    lvDevice.Items[(int)enumDevice.ER2].SubItems[(int)listviewDevice.State].Text = "activate";
                    break;
                case (int)enumDevice.VENT:
                    lvDevice.Items[(int)enumDevice.EV1].SubItems[(int)listviewDevice.State].Text = "activate";
                    lvDevice.Items[(int)enumDevice.EV2].SubItems[(int)listviewDevice.State].Text = "activate";
                    break;
                default:
                    lvDevice.Items[cbDeviceName.SelectedIndex].SubItems[(int)listviewDevice.State].Text = "activate";
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
            }
        }

        private void btnGloveApply_Click(object sender, EventArgs e)
        {
            if (lvGlove.SelectedItems.Count > 0)    //listview에서 선택이 되었을때만 실행되기 위해 조건
            {
                int nSelectedIndex = lvGlove.FocusedItem.Index; //현재 선택된 인데스 번호 전자용
                lvGlove.Items[nSelectedIndex].SubItems[1].Text = cbGloveRole.Text;
                lvGlove.Items[nSelectedIndex].SubItems[2].Text = cbGloveState.Text;
                lvGlove.Items[nSelectedIndex].SubItems[3].Text = tbGloveLifeChip.Text;
                lvGlove.Items[nSelectedIndex].SubItems[4].Text = tbGloveBattery.Text;
            }
        }

    }
}
