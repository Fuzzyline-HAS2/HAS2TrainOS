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
        public String strFileDir = @"C:\Users\user\Desktop\bbangjun\TrainRoom_excel\";
        int nMaxMACNum;         //맥주소  최대개수
        structMAC[] MACs;

        Workbook wbMain;        // 나레이션 엑셀
        Workbook wbMAC ;       //MQTT 구독하기 위해
        Workbook wbDevice;     //장치 데이터 저장용
        Workbook wbGlove ;     // 글러브 데이터 저장용

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
                //Console.WriteLine(strNarrTime + " + " + strWaitTime);
                strNarrTime = strNarrTime.Substring(strNarrTime.Length - 5, 5);     //1899-12-31 AM 12:00:00  포멧에서 뒤에 mm:ss만 남기고 자르기
                strWaitTime = strWaitTime.Substring(strWaitTime.Length - 5, 5);     // 1899-12-31 AM 12:00:00  포멧에서 뒤에 mm:ss만 남기고 자르기
                String validformats = "mm:ss";
                DateTime dtNarrTime = DateTime.ParseExact(strNarrTime, validformats, null);
                DateTime dtWaitTime = DateTime.ParseExact(strWaitTime, validformats, null);
                items.SubItems.Add((dtNarrTime.Minute + dtWaitTime.Minute).ToString());
                lvNarr.Items.Add(items);    //실제 추가

                Color NarrAbstract = Color.FromArgb(255, 253, 228, 155);    //시간 오바하면 나레이션 스킵하는 색상
                Color NarrAdd = Color.FromArgb(255, 166, 227, 183);          //시간 빠르면 추가하는 색상
                var style = wsNarr.Cells[i, 5].GetStyle();                              //나레이션에 해당하는 나레이션 색상 추출
                if (style.ForegroundColor == NarrAbstract)                           //스킵조건
                {
                    lvNarr.Items[i - 1].SubItems.Add(GetCellTime(wsNarr, i, 13));
                    lvNarr.Items[i - 1].BackColor = Color.LemonChiffon;
                }
                else if (style.ForegroundColor == NarrAdd)                          //추가조건
                {
                    lvNarr.Items[i - 1].SubItems.Add(GetCellTime(wsNarr, i, 8));
                    lvNarr.Items[i - 1].BackColor = Color.PaleGreen;
                }
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
            wbMain = new Workbook(strFileDir + "wbMain.xlsx");        // 나레이션 엑셀
            wbMAC = new Workbook(strFileDir + "wbMac.xlsx");         //MQTT 구독하기 위해
            wbDevice = new Workbook(strFileDir + "wbDevice.xlsx");  //장치 데이터 저장용
            wbGlove = new Workbook(strFileDir + "wbGlove.xlsx");     // 글러브 데이터 저장용

            /* aspose.cell 이용해 엑셀 데이터 ㅣListView로 불러와줌*/
            MainRead(lvPlayerNarr, wbMain.Worksheets["Player"]);    //Player 시트
            MainRead(lvTaggerNarr, wbMain.Worksheets["Tagger"]);   //Killer 시트
            DeviceRead(lvDevice, wbDevice.Worksheets["Device"]);   //Device 시트
            DeviceRead(lvGlove, wbGlove.Worksheets["Glove"]);      //Glove 시트

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
            Worksheet wsMAC = wbMAC.Worksheets["MAC"];
            nMaxMACNum = wsMAC.Cells.MaxDataRow;
            MACs = new structMAC[nMaxMACNum];
            PlayerSCNProcessor.classMAC = new structMAC[nMaxMACNum];
            TaggerSCNProcessor.classMAC = new structMAC[nMaxMACNum];
            for (int i = 0; i < nMaxMACNum; i++)
            {
                MACs[i].SaveMAC(GetCell(wsMAC, i+1,0), GetCell(wsMAC, i+1, 1));
                PlayerSCNProcessor.classMAC[i].SaveMAC(GetCell(wsMAC, i + 1, 0), GetCell(wsMAC, i + 1, 1));
                TaggerSCNProcessor.classMAC[i].SaveMAC(GetCell(wsMAC, i + 1, 0), GetCell(wsMAC, i + 1, 1));
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
            wbDV.Save(strFileDir+ strXlsx);
        }
        private void ListviewtoExcel()
        {
            /* aspose.cell 이용해 ListView 데이터 엑셀에 저장함*/
            DevicSave(lvDevice, wbDevice, "wbDevice.xlsx");
            DevicSave(lvGlove, wbGlove, "wbGlove.xlsx");
        }

        // excel에서 불러올때 null 이면 예외처리 해주는 함수
        private String GetCell(Worksheet ws, int col, int row)
        {
            String strWSData = "";
            DateTime dtTemp;
            Cell cellTemp = ws.Cells[col, row];
            switch (cellTemp.Type)
            {
                case CellValueType.IsString:
                    strWSData = cellTemp.StringValue;
                    //Console.WriteLine("String Value: " + strWSData);
                    break;

                case CellValueType.IsDateTime:
                    dtTemp = cellTemp.DateTimeValue;
                    //Console.WriteLine("DateTime Value: " + dtTemp);
                    strWSData = dtTemp.ToString();
                    break;

                // Evaluating the unknown data type of the cell data
                case CellValueType.IsUnknown:
                    strWSData = cellTemp.StringValue;
                    //Console.WriteLine("Unknown Value: " + strWSData);
                    break;
                // Terminating the type checking of type of the cell data is null
                case CellValueType.IsNull:
                    strWSData = "";
                    break;
                default:
                    strWSData = cellTemp.StringValue;
                    break;
            }
            if (strWSData == "-")
            {
                strWSData = "1899 - 12 - 31 AM 12:00:00";
            }
            return strWSData;
        }
        String GetCellTime(Worksheet ws, int col, int row)
        {
            String strWSData = "";
            DateTime dtTemp;
            Cell cellTemp = ws.Cells[col, row];
            dtTemp = cellTemp.DateTimeValue;
            strWSData = dtTemp.ToString();
            Console.WriteLine(strWSData);
            strWSData = strWSData.Substring(strWSData.Length - 8, 5);
            Console.WriteLine(strWSData);
            return strWSData;
        }
    }
}
