using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Aspose.Cells;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace HAS2TrainOS
{

    public partial class MainForm : Form
    {
        Workbook wbMain = new Workbook(@"C:\Users\user\Desktop\bbangjun\HAS2_Train\wbMain.xlsx");
        Workbook wbMac = new Workbook(@"C:\Users\user\Desktop\bbangjun\HAS2_Train\wbMac.xlsx");
        private void ExceltoListview()
        {
            Worksheet wsPlayer = wbMain.Worksheets[0]; //Player 시트
            Worksheet wsKiller= wbMain.Worksheets[1]; //Player 시트

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

        }
    }
}
