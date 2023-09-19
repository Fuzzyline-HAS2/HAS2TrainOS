using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Aspose.Cells;
using System.Windows.Forms;
using System.Globalization;
using System.Drawing;

namespace HAS2TrainOS
{

    public partial class MainForm : Form
    {
        Workbook wbMain = new Workbook(@"C:\Users\user\Desktop\bbangjun\TrainRoom_excel\wbMain.xlsx");        // 나레이션 엑셀
        Workbook wbMAC = new Workbook(@"C:\Users\user\Desktop\bbangjun\TrainRoom_excel\wbMac.xlsx");         //MQTT 구독하기 위해
        Workbook wbDevice = new Workbook(@"C:\Users\user\Desktop\bbangjun\TrainRoom_excel\wbDevice.xlsx");  //장치 데이터 저장용
        Workbook wbGlove = new Workbook(@"C:\Users\user\Desktop\bbangjun\TrainRoom_excel\wbGlove.xlsx");     // 글러브 데이터 저장용

        private void MainRead(ListView lvNarr, Worksheet wsNarr)
        {
            int rowsPlayer = wsNarr.Cells.MaxDataRow;
            for (int i = 1; i < rowsPlayer; i++)
            {
                ListViewItem items = new ListViewItem();
                items.Text = "";
                items.SubItems.Add(GetCell(wsNarr, i, 1));    //번호
                items.SubItems.Add(GetCell(wsNarr, i, 5));    //나레이션
                items.SubItems.Add(GetCell(wsNarr, i, 6));    //사용장치
                items.SubItems.Add(GetCell(wsNarr, i, 7));    //스킵조건
                String strNarrTime = GetCell(wsNarr, i, 15);   //나레이션 길이                            // 나레이션 시간 타임 가져오기
                String strWaitTime = GetCell(wsNarr, i, 16);   //대기시간                                   // 대기 시간 타임 가져오기
                Console.WriteLine(strNarrTime + " + " + strWaitTime);
                strNarrTime = strNarrTime.Substring(strNarrTime.Length - 5, 5);     //1899-12-31 AM 12:00:00  포멧에서 뒤에 mm:ss만 남기고 자르기
                strWaitTime = strWaitTime.Substring(strWaitTime.Length - 5, 5);     // 1899-12-31 AM 12:00:00  포멧에서 뒤에 mm:ss만 남기고 자르기
                String validformats = "mm:ss";
                DateTime dtNarrTime = DateTime.ParseExact(strNarrTime, validformats, null);
                DateTime dtWaitTime = DateTime.ParseExact(strWaitTime, validformats, null);
                items.SubItems.Add((dtNarrTime.Minute + dtWaitTime.Minute).ToString());
                lvNarr.Items.Add(items);    //실제 추가
            }
        }
        private void DeviceRead(ListView lvDV, Worksheet wsDV)
        {
            int rowsDevice = wsDV.Cells.MaxDataRow;
            int colsDevice = wsDV.Cells.MaxDataColumn;
            for (int i = 0; i < rowsDevice; i++)
            {
                //Console.WriteLine(wsGlove.Cells[i, 0].Value.ToString());
                for (int j = 0; j <= colsDevice; j++)
                {
                    //Console.WriteLine((wsGlove.Cells[i, j + 1].Value).ToString());
                    //Console.WriteLine("i:" + i.ToString() + "j:" + j.ToString());
                    lvDV.Items[i].SubItems[j].Text = (wsDV.Cells[i + 1, j].Value).ToString();
                }
            }
        }
        private void AsposeTabEraser(Workbook wbInput)
        {
            try
            {
                wbInput.Worksheets.RemoveAt(1);
            }
            catch
            {
                Console.WriteLine("no wbDevice more sheet to erase");
            }
        }
        private void ExceltoListview()
        {
            /* aspose.cell 이용해 엑셀 데이터 ㅣListView로 불러와줌*/
            MainRead(lvPlayerNarr, wbMain.Worksheets[0]);    //Player 시트
            MainRead(lvTaggerNarr, wbMain.Worksheets[1]);   //Killer 시트
            DeviceRead(lvDevice, wbDevice.Worksheets[0]);   //Device 시트
            DeviceRead(lvGlove, wbGlove.Worksheets[0]);      //Glove 시트

            /* aspose.cell 라이선스 문제로 저장할때마다 시트가 추가되는 버그를 해결하기 위해 매번 켤때마다 시트를 삭제함*/
            AsposeTabEraser(wbDevice);
            AsposeTabEraser(wbGlove);

            /*lvGlove 가져온다음에 item당 BackColor 적용 시키기 위함*/
            foreach (ListViewItem lvG in lvGlove.Items)
            {
                if (lvG.SubItems[(int)listviewGlove.Role].Text == "player")
                    lvG.BackColor = Color.YellowGreen;
                else if (lvG.SubItems[(int)listviewGlove.Role].Text == "tagger")
                    lvG.BackColor = Color.BlueViolet;
                else
                    lvG.BackColor = Color.LightGray;
            }
        }

        private void DevicSave(ListView lvDV, Workbook wbDV, String strXlsx)
        {
            int rowsDevice = wbDV.Worksheets[0].Cells.MaxDataRow;
            int colsDevice = wbDV.Worksheets[0].Cells.MaxDataColumn;
            for (int i = 0; i < rowsDevice; i++)
            {
                for (int j = 0; j <= colsDevice; j++)
                    wbDV.Worksheets[0].Cells[i + 1, j].Value = lvDV.Items[i].SubItems[j].Text;
            }
            wbDV.Save(@"C:\Users\user\Desktop\bbangjun\TrainRoom_excel\"+ strXlsx+".xlsx");
        }
        private void ListviewtoExcel()
        {
            /* aspose.cell 이용해 ListView 데이터 엑셀에 저장함*/
            DevicSave(lvDevice, wbDevice, "wbDevice");
            DevicSave(lvGlove, wbGlove, "wbGlove");
        }
        
        // excel에서 불러올때 null 이면 예외처리 해주는 함수
        private String GetCell(Worksheet ws, int col, int row)
        {
            String strWSDatea = "";
            DateTime dtTemp;
            Cell cellTemp = ws.Cells[col, row];
            switch (cellTemp.Type)
            {
                case CellValueType.IsString:
                    strWSDatea = cellTemp.StringValue;
                    Console.WriteLine("String Value: " + strWSDatea);
                    break;

               case CellValueType.IsDateTime:
                    dtTemp = cellTemp.DateTimeValue;
                    Console.WriteLine("DateTime Value: " + dtTemp);
                    strWSDatea = dtTemp.ToString();
                    break;

                // Evaluating the unknown data type of the cell data
                case CellValueType.IsUnknown:
                    strWSDatea = cellTemp.StringValue;
                    Console.WriteLine("Unknown Value: " + strWSDatea);
                    break;
                // Terminating the type checking of type of the cell data is null
                case CellValueType.IsNull:
                    strWSDatea = "";
                    break;
                default:
                    strWSDatea = cellTemp.StringValue;
                    break;
            }
            if(strWSDatea == "-")
            {
                strWSDatea = "1899 - 12 - 31 AM 12:00:00";
            }
            return strWSDatea;
        }
        
    }
}
