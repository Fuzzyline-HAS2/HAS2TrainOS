using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HAS2TrainOS
{
    public partial class MainForm : Form
    {
        public bool SCNp32()    //아이템박스 배터리팩 추가
        {
            Console.WriteLine("SCNp32 func initiate");
            int cnt = 0;
            foreach (ListViewItem lvPlayerGlove in lvGlove.Items)
            {
                if (lvPlayerGlove.BackColor == Color.YellowGreen)   //생존자 방 글러브들 색은 초록색으로 지정해두었기 때문에 사용
                {
                    GloveListViewChange(lvPlayerGlove, strBP: "+2");
                    cnt++;
                }
                if (cnt >= 2)    //플레이어 두명한테 배터리팩 전달 완료 하면 종료
                {
                    return false;
                }
            }
            return false;
        }
        public bool SCNp40()    //발전기 배터리팩 회수
        {
            Console.WriteLine("SCNp40 func initiate");
            foreach (ListViewItem lvPlayerGlove in lvGlove.Items)   // 모든 글러브 배터리팩 0으로 만듦
            {
                if (lvPlayerGlove.SubItems[(int)listviewGlove.BP].Text != "0")
                {
                    GloveListViewChange(lvPlayerGlove, strBP: "0");
                }
            }
            DeviceListViewChange(lvDevice.Items[(int)enumDevice.EG], strLCBP: "4"); //발전기 배터리팩 다채워줌
            return false;
        }

        public bool SCNp69()    //플레이어 글러브 중 Role:player 인 경우 생명칩 전달을 하기 위해 LC +1 함
        {
            foreach (ListViewItem lvPlayerGlove in lvGlove.Items)   // 모든 글러브 배터리팩 0으로 만듦
            {
                if (lvPlayerGlove.SubItems[(int)listviewGlove.Role].Text == "player")
                {
                    GloveListViewChange(lvPlayerGlove, strLC: "+1");
                    return false;
                }
            }
            return false;
        }
        public bool SCNp76()    //부활, 플레이어 글러브 중 Role:ghost -> Role:player 부활
        {
            foreach (ListViewItem lvPlayerGlove in lvGlove.Items)   // 모든 글러브 배터리팩 0으로 만듦
            {
                if (lvPlayerGlove.SubItems[(int)listviewGlove.Role].Text == "ghost")
                {
                    GloveListViewChange(lvPlayerGlove, Role:"player", strLC: "1");
                    return false;
                }
            }
            return false;
        }
    }
}
