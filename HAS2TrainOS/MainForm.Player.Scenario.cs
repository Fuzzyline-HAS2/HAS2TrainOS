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
        public bool SCNp32()
        {
            Console.WriteLine("SCNp32");
            int cnt = 0;
            foreach(ListViewItem lvPlayerGlove in lvGlove.Items) 
            {
                if (lvPlayerGlove.BackColor == Color.YellowGreen)   //생존자 방 글러브들 색은 초록색으로 지정해두었기 때문에 사용
                {
                    lvPlayerGlove.SubItems[(int)listviewGlove.BP].Text = (Int32.Parse(lvPlayerGlove.SubItems[(int)listviewGlove.BP].Text) +2).ToString();   // 생명칩을 주는 글러브 LC 데이터 '-1' 처리
                    int nSelectedIndex = lvPlayerGlove.Index;
                    GloveJSONPublish(lvGlove.Items[nSelectedIndex].SubItems[(int)listviewGlove.Name].Text,
                        lvGlove.Items[nSelectedIndex].SubItems[(int)listviewGlove.Role].Text,
                        lvGlove.Items[nSelectedIndex].SubItems[(int)listviewGlove.State].Text,
                        lvGlove.Items[nSelectedIndex].SubItems[(int)listviewGlove.LC].Text,
                        lvGlove.Items[nSelectedIndex].SubItems[(int)listviewGlove.BP].Text);
                    cnt++;
                }
                if(cnt >= 2)    //플레이어 두명한테 배터리팩 전달 완료 하면 종료
                {
                    return false;
                }
            }
            return false;
        }
    }
}
