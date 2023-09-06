using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Aspose.Cells;
using System.Windows.Forms;
using System.Globalization;

namespace HAS2TrainOS
{

    public partial class MainForm : Form
    {
        Workbook wbMain = new Workbook(@"C:\Users\user\Desktop\bbangjun\TrainRoom_excel\wbMain.xlsx");        // 나레이션 엑셀
        Workbook wbMAC = new Workbook(@"C:\Users\user\Desktop\bbangjun\TrainRoom_excel\wbMac.xlsx");         //MQTT 구독하기 위해
        Workbook wbDevice = new Workbook(@"C:\Users\user\Desktop\bbangjun\TrainRoom_excel\wbDevice.xlsx");  //장치 데이터 저장용
        Workbook wbGlove = new Workbook(@"C:\Users\user\Desktop\bbangjun\TrainRoom_excel\wbGlove.xlsx");     // 글러브 데이터 저장용
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
                items.Text = "";
                /*items.SubItems.Add((wsPlayer.Cells[i, 1].Value).ToString());    //port삽입
                items.SubItems.Add((wsPlayer.Cells[i, 5].Value).ToString());    //product삽입*/
                items.SubItems.Add(GetCell(wsPlayer, i, 1));    //번호
                items.SubItems.Add(GetCell(wsPlayer, i, 5));    //나레이션
                items.SubItems.Add(GetCell(wsPlayer, i, 6));    //사용장치
                items.SubItems.Add(GetCell(wsPlayer, i, 7));    //스킵조건
                String strNarrTime = GetCell(wsPlayer, i, 15);   //나레이션 길이                            // 나레이션 시간 타임 가져오기
                String strWaitTime = GetCell(wsPlayer, i, 16);   //대기시간                                   // 대기 시간 타임 가져오기
                Console.WriteLine(strNarrTime + " + " + strWaitTime);
                strNarrTime = strNarrTime.Substring(strNarrTime.Length - 5, 5);     //1899-12-31 AM 12:00:00  포멧에서 뒤에 mm:ss만 남기고 자르기
                strWaitTime = strWaitTime.Substring(strWaitTime.Length - 5, 5);     // 1899-12-31 AM 12:00:00  포멧에서 뒤에 mm:ss만 남기고 자르기
                String validformats = "mm:ss";
                DateTime dtNarrTime = DateTime.ParseExact(strNarrTime, validformats, null);
                DateTime dtWaitTime = DateTime.ParseExact(strWaitTime, validformats, null);
                items.SubItems.Add((dtNarrTime.Minute + dtWaitTime.Minute).ToString());
                lvPlayerNarr.Items.Add(items);    //실제 추가
            }

            int rowsKiller = wsKiller.Cells.MaxDataRow;
            for (int i = 1; i < rowsKiller; i++)
            {
                ListViewItem items = new ListViewItem();
                items.Text = "";    //Ip삽입
                /*try
                {
                    items.SubItems.Add((wsKiller.Cells[i, 1].Value).ToString());    //port삽입
                }
                catch
                {
                    items.SubItems.Add((wsKiller.Cells[i, 1].Value).ToString());    //port삽입
                    //MessageBox.Show((String msg) "Hello Mablang World!");
                }*/
                items.SubItems.Add(GetCell(wsKiller, i, 1));    //port삽입
                items.SubItems.Add(GetCell(wsKiller, i, 5));    //product삽입
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
            /*aspose.cell 라이선스 문제로 저장할때마다 시트가 추가되는 버그를 해결하기 위해 매번 켤때마다 시트를 삭제함*/
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
            wbDevice.Save(@"C:\Users\user\Desktop\bbangjun\TrainRoom_excel\wbDevice.xlsx");

            int rowsGlove = wsGlove.Cells.MaxDataRow;
            int colsGlove = wsGlove.Cells.MaxDataColumn;
            for (int i = 0; i < rowsGlove; i++)
            {
                for (int j = 1; j <= colsGlove; j++)
                {
                    wsGlove.Cells[i + 1, j].Value = lvGlove.Items[i].SubItems[j].Text;
                }
            }
            wbGlove.Save(@"C:\Users\user\Desktop\bbangjun\TrainRoom_excel\wbGlove.xlsx");
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
