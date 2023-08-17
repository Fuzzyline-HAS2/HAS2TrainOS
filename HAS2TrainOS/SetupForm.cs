using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Aspose.Cells;

namespace HAS2TrainOS
{
    public partial class SetupForm : Form
    {
        MainForm mainform;
        public SetupForm(MainForm form)
        {
            InitializeComponent();
            mainform = form;
            
        }

        private void SetupForm_Load(object sender, EventArgs e)
        {
            ExceltoListview();
            Console.WriteLine(dgMAC.Rows[0]);
            try
            {
                wbMAC = new Workbook(@"C:\Users\user\Desktop\bbangjun\HAS2_Train\wbMac.xlsx");
            }
            catch 
            {
                OpenFileDialog pFileDlg = new OpenFileDialog();
                if (pFileDlg.ShowDialog() == DialogResult.OK)
                {
                    wbMAC = new Workbook(pFileDlg.FileName);
                }
            }

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
