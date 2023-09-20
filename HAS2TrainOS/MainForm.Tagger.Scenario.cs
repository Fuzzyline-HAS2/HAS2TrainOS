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
        public bool SCNt18()    //아이템박스 배터리팩 추가
        {
            Console.WriteLine("SCNt18");
            foreach (ListViewItem lvTaggerGlove in lvGlove.Items)
            {
                if (lvTaggerGlove.BackColor == Color.BlueViolet)   //술래 방 글러브들 색은 보라색으로 지정해두었기 때문에 사용
                {
                    GloveListViewChange(lvTaggerGlove, Role: "player", strLC: "1");
                    return false;
                }
            }
            return false;
        }
        public bool SCNt23()    //아이템박스 배터리팩 추가
        {
            Console.WriteLine("SCNt23");
            foreach (ListViewItem lvTaggerGlove in lvGlove.Items)
            {
                if (lvTaggerGlove.BackColor == Color.BlueViolet)   //술래 방 글러브들 색은 보라색으로 지정해두었기 때문에 사용
                {
                    if (lvTaggerGlove.SubItems[(int)listviewGlove.Role].Text == "player")
                    {
                        GloveListViewChange(lvTaggerGlove, Role: "ghost", strLC: "0");
                    }
                    else if (lvTaggerGlove.SubItems[(int)listviewGlove.Role].Text == "tagger")
                    {
                        GloveListViewChange(lvTaggerGlove, strLC: "1");
                        return false;
                    }
                }
            }
            return false;
        }
        public bool SCNt25()    //아이템박스 배터리팩 추가
        {
            Console.WriteLine("SCNt25");
            foreach (ListViewItem lvTaggerGlove in lvGlove.Items)
            {
                if (lvTaggerGlove.SubItems[(int)listviewGlove.Role].Text == "player")
                {
                    if (lvTaggerGlove.BackColor == Color.BlueViolet)   //술래 방 글러브들 색은 보라색으로 지정해두었기 때문에 사용
                    {
                        if (lvTaggerGlove.SubItems[(int)listviewGlove.Role].Text == "ghost")
                        {
                            GloveListViewChange(lvTaggerGlove, Role: "player", strLC: "1");
                            return false;
                        }
                    }
                }
            }
            return false;
        }
        public bool SCNt28()    //아이템박스 배터리팩 추가
        {
            Console.WriteLine("SCNt28");
            foreach (ListViewItem lvTaggerGlove in lvGlove.Items)
            {
                if (lvTaggerGlove.BackColor == Color.BlueViolet)   //술래 방 글러브들 색은 보라색으로 지정해두었기 때문에 사용
                {
                    if (lvTaggerGlove.SubItems[(int)listviewGlove.Role].Text == "player")
                    {
                        GloveListViewChange(lvTaggerGlove, Role: "ghost", strLC: "0");
                        return false;
                    }
                }
            }
            return false;
        }
        public bool SCNt29()    //아이템박스 배터리팩 추가
        {
            Console.WriteLine("SCNt29");
            foreach (ListViewItem lvTaggerGlove in lvGlove.Items)
            {
                if (lvTaggerGlove.BackColor == Color.BlueViolet)   //술래 방 글러브들 색은 보라색으로 지정해두었기 때문에 사용
                {
                    if (lvTaggerGlove.SubItems[(int)listviewGlove.Role].Text == "player")
                    {
                        GloveListViewChange(lvTaggerGlove, Role: "ghost", strLC: "0");
                        return false;
                    }
                }
            }
            return false;
        }
        public bool SCNt36()    //아이템박스 배터리팩 추가
        {
            Console.WriteLine("SCNt29");
            foreach (ListViewItem lvTaggerGlove in lvGlove.Items)
            {
                if (lvTaggerGlove.BackColor == Color.BlueViolet)   //술래 방 글러브들 색은 보라색으로 지정해두었기 때문에 사용
                {
                    if (lvTaggerGlove.SubItems[(int)listviewGlove.Role].Text == "tagger")
                    {
                        GloveListViewChange(lvTaggerGlove, strLC: "0");
                        return false;
                    }
                }
            }
            return false;
        }
        public bool SCNt47()    //아이템박스 배터리팩 추가
        {
            Console.WriteLine("SCNt29");
            foreach (ListViewItem lvTaggerGlove in lvGlove.Items)
            {
                if (lvTaggerGlove.BackColor == Color.BlueViolet)   //술래 방 글러브들 색은 보라색으로 지정해두었기 때문에 사용
                {
                    GloveListViewChange(lvTaggerGlove, Role: "tagger", strLC: "0");
                    return false;   
                }
            }
            return false;
        }
    }
}
