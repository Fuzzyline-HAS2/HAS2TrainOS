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
            int nMaxCnt  = 0;
            for (int i = (int)enumDevice.EI1; i <= (int)enumDevice.EI2; i++)        //열리지 않은 아이템박스의 개수를 확인하기 위함
            {
                if(lvDevice.Items[i].SubItems[(int)listviewDevice.LCBP].Text != "0")
                {
                    nMaxCnt++;
                }
            }
            foreach (ListViewItem lvPlayerGlove in lvGlove.Items)
            {
                if (lvPlayerGlove.BackColor == Color.YellowGreen)   //생존자 방 글러브들 색은 초록색으로 지정해두었기 때문에 사용
                {
                    GloveListViewChange(lvPlayerGlove, strBP: "+2");
                    cnt++;
                }
                if (cnt >= nMaxCnt)    //플레이어 두명한테 배터리팩 전달 완료 하면 종료
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
                if (lvPlayerGlove.BackColor == Color.YellowGreen)   //생존자 방 글러브들 색은 초록색으로 지정해두었기 때문에 사용
                {
                    if (lvPlayerGlove.SubItems[(int)listviewGlove.BP].Text != "0")
                    {
                        GloveListViewChange(lvPlayerGlove, strBP: "0");
                    }
                }
            }
            DeviceListViewChange(lvDevice.Items[(int)enumDevice.EG], strLCBP: "4"); //발전기 배터리팩 다채워줌
            return false;
        }
        public bool SCNp61()    //술래 퇴장 까지 기다림.
        {
            TaggerSCNProcessor.bAccessNext = true;
            Console.WriteLine("SCNp61 func initiate");
            Console.WriteLine("Wait for Tagger Exit.....");
            return false;
        }

        public bool SCNp69()    //플레이어 글러브 중 Role:player 인 경우 생명칩 전달을 하기 위해 LC +1 함
        {
            Console.WriteLine("SCNp69 func initiate");
            foreach (ListViewItem lvPlayerGlove in lvGlove.Items)   // 모든 글러브 배터리팩 0으로 만듦
            {
                if (lvPlayerGlove.BackColor == Color.YellowGreen)   //생존자 방 글러브들 색은 초록색으로 지정해두었기 때문에 사용
                {
                    if (lvPlayerGlove.SubItems[(int)listviewGlove.Role].Text == "player")
                    {
                        GloveListViewChange(lvPlayerGlove, strLC: "+1");
                        return false;
                    }
                }
            }
            return false;
        }
        public bool SCNp76()    //부활, 플레이어 글러브 중 Role:ghost -> Role:player 부활
        {
            Console.WriteLine("SCNp76 func initiate");
            foreach (ListViewItem lvPlayerGlove in lvGlove.Items)   // 모든 글러브 배터리팩 0으로 만듦
            {
                if (lvPlayerGlove.BackColor == Color.YellowGreen)   //생존자 방 글러브들 색은 초록색으로 지정해두었기 때문에 사용
                {
                    if (lvPlayerGlove.SubItems[(int)listviewGlove.Role].Text == "ghost")
                    {
                        GloveListViewChange(lvPlayerGlove, Role: "player", strLC: "1");
                        return false;
                    }
                }
            }
            return false;
        }
        public bool SCNp97()    //술래 퇴장 까지 기다림.
        {
            TaggerSCNProcessor.bAccessNext = true;
            Console.WriteLine("SCNp97 func initiate");
            Console.WriteLine("Wait for Tagger....");
            return false;
        }
        public bool SCNp98()    //술래 퇴장 까지 기다림.
        {
            TaggerSCNProcessor.bAccessNext = true;
            Console.WriteLine("SCNp98 func initiate");
            Console.WriteLine("Wait for Tagger....");
            return false;
        }
        public bool SCNp101()    //술래 퇴장 까지 기다림.
        {
            Console.WriteLine("SCNp101 func initiate");
            Console.WriteLine("Wait for Tagger....");
            return false;
        }
        public bool SCNp107()    //술래 퇴장 까지 기다림.
        {
            btnReady.PerformClick();
            MessageBox.Show("생존자 훈련소가 종료되었습니다.");
            foreach (ListViewItem lvPlayerGlove in lvGlove.Items)   // 모든 글러브 배터리팩 0으로 만듦
            {  
                GloveListViewChange(lvPlayerGlove, State: "end");         
            }
            return false;
        }
    }
}
