using Aspose.Cells.Drawing;
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
        public bool SCNt9()    //술래 글러브 중 맨처음 Role:tagger -> Role:blink 변환, 나머지 Role:tagger -> Role:player 변환
        {
            Console.WriteLine("SCNt9 func initiate");
            int nGloveCnt = 0;
            foreach (ListViewItem lvTaggerGlove in lvGlove.Items)
            {
                nGloveCnt++;
                switch (nGloveCnt)
                {
                    case 1: GloveListViewChange(lvTaggerGlove, Role: "blink"); break;
                    default: GloveListViewChange(lvTaggerGlove, Role: "player"); break;
                }
            }
            return false;
        }
        public bool SCNt15()    //술래 글러브중 Role:tagger -> Role:blink 변환
        {
            Console.WriteLine("SCNt15 func initiate");
            foreach (ListViewItem lvTaggerGlove in lvGlove.Items)
            {
                if (lvTaggerGlove.BackColor == Color.BlueViolet)   //술래 방 글러브들 색은 보라색으로 지정해두었기 때문에 사용
                {
                    if (lvTaggerGlove.SubItems[(int)listviewGlove.Role].Text == "blink")
                        GloveListViewChange(lvTaggerGlove, Role: "tagger");
                    return false;
                }
            }
            return false;
        }
        public bool SCNt18()    //술래 글러브중 Role:tagger -> Role:player로 변환
        {
            Console.WriteLine("SCNt18 func initiate");
            /*int nSkipFirstTagger = 0;
            foreach (ListViewItem lvTaggerGlove in lvGlove.Items)
            {
                if (lvTaggerGlove.BackColor == Color.BlueViolet)   //술래 방 글러브들 색은 보라색으로 지정해두었기 때문에 사용
                {
                    nSkipFirstTagger++;
                    if (nSkipFirstTagger == 2)
                    {
                        GloveListViewChange(lvTaggerGlove, Role: "player", strLC: "1");
                        return false;
                    }
                }
            }*/
            return false;
        }
        public bool SCNt23()    //tagger가 player LC 뺃는 상황, 술래의 글러브 중 Role:player -> Role:ghost로 변환
        {
            Console.WriteLine("SCNt23 func initiate");
            foreach (ListViewItem lvTaggerGlove in lvGlove.Items)
            {
                if (lvTaggerGlove.BackColor == Color.BlueViolet)   //술래 방 글러브들 색은 보라색으로 지정해두었기 때문에 사용
                {
                    if (lvTaggerGlove.SubItems[(int)listviewGlove.Role].Text == "player")
                    {
                        GloveListViewChange(lvTaggerGlove, Role: "ghost", strLC: "0");
                        return false;
                    }
                    else if (lvTaggerGlove.SubItems[(int)listviewGlove.Role].Text == "tagger")
                    {
                        GloveListViewChange(lvTaggerGlove, strLC: "1");
                    }
                }
            }
            return false;
        }
        public bool SCNt25()    //술래글러브 중 Role:ghost -> Role:player 로 변환
        {
            Console.WriteLine("SCNt25 func initiate");
            foreach (ListViewItem lvTaggerGlove in lvGlove.Items)
            {
                if (lvTaggerGlove.BackColor == Color.BlueViolet)   //술래 방 글러브들 색은 보라색으로 지정해두었기 때문에 사용
                {
                    if (lvTaggerGlove.SubItems[(int)listviewGlove.Role].Text == "ghost")
                    {
                        GloveListViewChange(lvTaggerGlove, Role: "player", strLC: "1");
                        strFirstKilledPlayerName = lvTaggerGlove.SubItems[(int)listviewGlove.Role].Text;    //누가 술래한테 죽엇는지 저장하는 변수
                        return false;
                    }
                }
            }
            return false;
        }
        public bool SCNt28()    //술래글러브중 Role:player -> Role:ghost로 변환
        {
            Console.WriteLine("SCNt28 func initiate");
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
        public bool SCNt29()    //술래글러브중 Role:player -> Role:ghost로 변환
        {
            Console.WriteLine("SCNt29 func initiate");
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
        public bool SCNt36()    // 제단 희생, 술래 글러브중 Role:tagger LC = 0
        {
            Console.WriteLine("SCNt36 func initiate");
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
        public bool SCNt38()    //술래 글러브중 Role:tagger -> Role:blink 변환
        {
            Console.WriteLine("SCNt15 func initiate");
            foreach (ListViewItem lvTaggerGlove in lvGlove.Items)
            {
                if (lvTaggerGlove.BackColor == Color.BlueViolet)   //술래 방 글러브들 색은 보라색으로 지정해두었기 때문에 사용
                {
                    if (lvTaggerGlove.SubItems[(int)listviewGlove.Role].Text == "player" && lvTaggerGlove.SubItems[(int)listviewGlove.Role].Text != strFirstKilledPlayerName)
                        GloveListViewChange(lvTaggerGlove, Role: "ghost");  //보라색에 player이면서 이미 처음에 죽은애랑 일치 하지 않으면  실행
                    return false;
                }
            }   // 위에서 탈출하지 못하는 경우 아래로 진행: 경우1: 술래공간 2명일때. 경우2: strFirstKilledPlayerName변수에 저장된게 없을때
            foreach (ListViewItem lvTaggerGlove in lvGlove.Items)
            {
                if (lvTaggerGlove.BackColor == Color.BlueViolet)   //술래 방 글러브들 색은 보라색으로 지정해두었기 때문에 사용
                {
                    if (lvTaggerGlove.SubItems[(int)listviewGlove.Role].Text == "player")
                        GloveListViewChange(lvTaggerGlove, Role: "ghost");  //보라색에 player이면서 이미 처음에 죽은애랑 일치 하지 않으면  실행
                    return false;
                }
            }
            return false;
        }
        public bool SCNt45()    //모든 술래글러브 Role: tagger로 변경 및 teaken_LC = 0
        {
            Console.WriteLine("SCNt45 func initiate");
            foreach (ListViewItem lvTaggerGlove in lvGlove.Items)
            {
                if (lvTaggerGlove.BackColor == Color.BlueViolet)   //술래 방 글러브들 색은 보라색으로 지정해두었기 때문에 사용
                {
                    GloveListViewChange(lvTaggerGlove, Role: "tagger", strLC: "0");
                }
            }
            return false;
        }
        public bool SCNt58()    //모든 술래글러브 Role: tagger로 변경 및 teaken_LC = 0
        {
            Console.WriteLine("SCNt58 func initiate");
            foreach (ListViewItem lvTaggerGlove in lvGlove.Items)
            {
                if (lvTaggerGlove.BackColor == Color.BlueViolet)   //술래 방 글러브들 색은 보라색으로 지정해두었기 때문에 사용
                {
                    GloveListViewChange(lvTaggerGlove, Role: "tagger", strLC: "1");
                    break;
                }
            }
            foreach (ListViewItem lvPlayrGlove in lvGlove.Items)
            {
                if (lvPlayrGlove.BackColor == Color.YellowGreen)   //술래 방 글러브들 색은 보라색으로 지정해두었기 때문에 사용
                {
                    GloveListViewChange(lvPlayrGlove, Role: "ghost", strLC: "0");
                    break;
                }
            }
            return false;
        }
        public bool SCNt66()    //모든 술래글러브 Role: tagger로 변경 및 teaken_LC = 0
        {
            Console.WriteLine("SCNt66 func initiate");
            PlayerSCNProcessor.nCurrentCnt = 63;
            PlayerSCNProcessor.NarrPlayJudge();
            return false;
        }
        public bool SCNt84()    //플레이어 상태 기다리는 용
        {
            Console.WriteLine("SCNt84 func initiate");
            if (PlayerSCNProcessor.nCurrentCnt == 88)
            {
                PlayerSCNProcessor.nCurrentCnt = 92;
                PlayerSCNProcessor.NarrPlayJudge();
            }
            return false;
        }
        public bool SCNt92()    //덕트킬 실행, 첫번째 술래글러브 LC +1
        {
            Console.WriteLine("SCNt92 func initiate");
            foreach (ListViewItem lvTaggerGlove in lvGlove.Items)
            {
                if (lvTaggerGlove.BackColor == Color.BlueViolet)   //술래 방 글러브들 색은 보라색으로 지정해두었기 때문에 사용
                {
                    GloveListViewChange(lvTaggerGlove, strLC: "1");
                    return false;
                }
            }
            return false;
        }
        public bool SCNt95()    //술래 퇴장 까지 기다림.
        {
            PlayerSCNProcessor.bAccessNext = true;
            Console.WriteLine("SCNt95 func initiate");
            Console.WriteLine("Wait for Player.....");
            return false;
        }
        public bool SCNt99()    //생존자 술래 피하지 못함으로 넘기기
        {
            Console.WriteLine("SCNt99 func initiate");
            PlayerSCNProcessor.timerPlayerWaitTime.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite); //메인타이머 종료
            PlayerSCNProcessor.timerPlayerSkipTime.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite); //PlayerSkipTimer 종료
   
            PlayerSCNProcessor.nCurrentCnt = 105;   //생존자 #103 딜레이 탈출 하기위해
            PlayerSCNProcessor.NarrPlayJudge();

            return false;
        }
        public bool SCNt100()    //술래 퇴장 까지 기다림.
        {
            TaggerSCNProcessor.timerPlayerWaitTime.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite); //메인타이머 종료
            TaggerSCNProcessor.timerPlayerSkipTime.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite); //PlayerSkipTimer 종료

            return false;
        }
        public bool SCNt101()    //술래 퇴장 까지 기다림.
        {
            //MessageBox.Show("술래 훈련소가 종료되었습니다.");
            PlayerSCNProcessor.nCurrentCnt = 105;   //생존자 #103 딜레이 탈출 하기위해
            PlayerSCNProcessor.NarrPlayJudge();
            return false;
        }
    }
}
