using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HAS2TrainOS
{
    public partial class MainForm
    {
        public bool SCNt32()    //아이템박스 배터리팩 추가
        {
            Console.WriteLine("SCNp32");
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
    }
}
