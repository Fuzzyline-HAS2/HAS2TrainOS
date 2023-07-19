using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HAS2TrainOS
{
    public partial class MainForm : Form
    {
        struct structGlove
        {
            String name;
            String role;
            String state;
            int LC;
            int BP;

            public void SaveAll(string name , String role, String state, int LC, int BP)
            {
                this.name = name; 
                this.role = role;
                this.state = state;
                this.LC = LC;
                this.BP = BP;
            }
        }
        enum enumGlove{P1 = 0,P2, P3, P4, P5, P6, P7, P8};
        enum enumDevice { EI1 = 0, EI2, ER1,ER2,EV1,EV2,EG,ED,EE,ET,EM, ALL , ITEMBOX, REVIVAL, VENT};
        enum listviewDevice { Name = 0, State, LCBP };
    }
}
