using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Aspose.Cells;
using System.Windows.Forms;

namespace HAS2TrainOS
{

    public partial class MainForm : Form
    {
        Workbook wbMain = new Workbook(@"C:\Users\user\Desktop\bbangjun\HAS2_Train\wbMain.xlsx");
        Workbook wbMac = new Workbook(@"C:\Users\user\Desktop\bbangjun\HAS2_Train\wbMac.xlsx");
        Workbook wbDevice = new Workbook(@"C:\Users\user\Desktop\bbangjun\HAS2_Train\wbDevice.xlsx");
        Workbook wbGlove = new Workbook(@"C:\Users\user\Desktop\bbangjun\HAS2_Train\wbGlove.xlsx");
        private void ExceltoListview()
        {
            Worksheet wsPlayer = wbMain.Worksheets[0];    //Player 시트
            Worksheet wsKiller= wbMain.Worksheets[1];      //Killer 시트
            Worksheet wsDevice = wbDevice.Worksheets[0]; //Device 시트
            Worksheet wsGlove = wbGlove.Worksheets[0];  //Glove 시트
            

            int rowsPlayer = wsPlayer.Cells.MaxDataRow;
            for (int i = 1; i < rowsPlayer; i++)
            {
                ListViewItem items = new ListViewItem();
                items.Text = "";    //Ip삽입
                items.SubItems.Add((wsPlayer.Cells[i, 1].Value).ToString());    //port삽입
                items.SubItems.Add((wsPlayer.Cells[i, 5].Value).ToString());    //product삽입
                listView1.Items.Add(items);    //실제 추가
            }

            int rowsKiller = wsKiller.Cells.MaxDataRow;
            for (int i = 1; i < rowsKiller; i++)
            {
                ListViewItem items = new ListViewItem();
                items.Text = "";    //Ip삽입
                items.SubItems.Add((wsKiller.Cells[i, 1].Value).ToString());    //port삽입
                items.SubItems.Add((wsKiller.Cells[i, 5].Value).ToString());    //product삽입
                listView3.Items.Add(items);    //실제 추가
            }

            int rowsDevice = wsDevice.Cells.MaxDataRow;
            int colsDevice = wsDevice.Cells.MaxDataColumn;
            for (int i = 0; i < rowsDevice; i++)
            {
                //Console.WriteLine(wsGlove.Cells[i, 0].Value.ToString());
                for (int j = 1; j <= colsDevice; j++)
                {
                    //Console.WriteLine((wsGlove.Cells[i, j + 1].Value).ToString());
                    //Console.WriteLine("i:" + i.ToString() + "j:" + j.ToString());
                    lvDevice.Items[i].SubItems[j].Text = (wsDevice.Cells[i + 1, j].Value).ToString();
                }
            }

            int rowsGlove = wsGlove.Cells.MaxDataRow;
            int colsGlove = wsGlove.Cells.MaxDataColumn;
            for (int i = 0; i < rowsGlove; i++)
            {
                //Console.WriteLine(wsGlove.Cells[i, 0].Value.ToString());
                for(int j = 1; j <= colsGlove; j++) 
                {
                    //Console.WriteLine((wsGlove.Cells[i, j + 1].Value).ToString());
                    //Console.WriteLine("i:" + i.ToString() + "j:" + j.ToString());
                    lvGlove.Items[i].SubItems[j].Text = (wsGlove.Cells[i + 1, j].Value).ToString();
                }
            }

            /* aspose.cell 라이선스 문제로 저장할때마다 시트가 추가되는 버그를 해결하기 위해 매번 켤때마다 시트를 삭제함*/
            try
            {
                wbDevice.Worksheets.RemoveAt(1);
            }
            catch
            {
                Console.WriteLine("no wbDevice more sheet to erase");
            }

            try
            {
                wbGlove.Worksheets.RemoveAt(1);
            }
            catch
            {
                Console.WriteLine("no wbGlove more sheet to erase");
            }
        }
        private void ListviewtoExcel()
        {
            Worksheet wsDevice = wbDevice.Worksheets[0]; //Device 시트
            Worksheet wsGlove = wbGlove.Worksheets[0];  //Glove 시트

            int rowsDevice = wsDevice.Cells.MaxDataRow;
            int colsDevice = wsDevice.Cells.MaxDataColumn;
            for (int i = 0; i < rowsDevice; i++)
            {
                for (int j = 1; j <= colsDevice; j++)
                {
                    wsDevice.Cells[i + 1, j].Value = lvDevice.Items[i].SubItems[j].Text;
                }
            }
            wbDevice.Save("C:\\Users\\user\\Desktop\\bbangjun\\HAS2_Train\\wbDevice.xlsx");

            int rowsGlove = wsGlove.Cells.MaxDataRow;
            int colsGlove = wsGlove.Cells.MaxDataColumn;
            for (int i = 0; i < rowsGlove; i++)
            {
                for (int j = 1; j <= colsGlove; j++)
                {
                    wsGlove.Cells[i + 1, j].Value = lvGlove.Items[i].SubItems[j].Text;
                }
            }
            wbGlove.Save("C:\\Users\\user\\Desktop\\bbangjun\\HAS2_Train\\wbGlove.xlsx");
        }
    }
}
