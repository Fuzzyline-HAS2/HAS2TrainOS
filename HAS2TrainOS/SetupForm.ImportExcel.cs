using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aspose.Cells;
using System.Windows.Forms;
using NAudio.CoreAudioApi;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Security.Cryptography;

namespace HAS2TrainOS
{
    public partial class SetupForm : Form
    {

        Workbook wbMAC = new Workbook();
        private void ExceltoListview()
        {
            Worksheet wsMAC = wbMAC.Worksheets[0];    //Player 시트
            int rowsMAC = wsMAC.Cells.MaxDataRow;
            int colsMAC = wsMAC.Cells.MaxDataColumn;
            for (int i = 1; i < rowsMAC; i++)
            {
                //Console.WriteLine(wsMAC.Cells[i, 0].Value.ToString());
                dgMAC.Rows.Add((wsMAC.Cells[i, 0].Value).ToString());
                for (int j = 1; j <= colsMAC; j++)
                {
                    //items.SubItems.Add((wsMAC.Cells[i, j].Value).ToString());    //port삽입
                    //dgMAC.SelectedRows[i].Insert(j,(wsMAC.Cells[i, j].Value).ToString());    //실제 추가
                    //Console.WriteLine((wsMAC.Cells[i, j + 1].Value).ToString());
                    //Console.WriteLine("i:" + i.ToString() + "j:" + j.ToString());
                }

            }

            /* aspose.cell 라이선스 문제로 저장할때마다 시트가 추가되는 버그를 해결하기 위해 매번 켤때마다 시트를 삭제함*/
            try
            {
                wbMAC.Worksheets.RemoveAt(1);
            }
            catch
            {
                Console.WriteLine("no wbDevice more sheet to erase");
            }
        }
    }

}
