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
        public struct structALL
        {
            public String[] strALLp;
            public String[] strALLt;
            public String[] strAGp;
            public String[] strAGt;
            public String[] StringSelecetor(String input)
            {
                if (input == "ALLp")
                    return strALLp;
                else if (input == "ALLt")
                    return strALLt;
                else if (input == "AGp")
                    return strAGp;
                else if (input == "AGt")
                    return strAGt;
                return strALLp;
            }
        }

        public struct structMAC
        {
            public String strDeviceName;
            public String strDeviceMAC;
            public void SaveMAC(string device, String mac)
            {
                this.strDeviceName = device;
                this.strDeviceMAC = mac;
            }
        }

        public structALL AllDevice;

        enum enumGlove{P1 = 0,P2, P3, P4, P5, P6, P7, P8};
        enum enumDevice { EI1 = 0, EI2, ER1,ER2,EVp,EVt,EG,ED,EE,ET,EM, ALL , ITEMBOX, REVIVAL, VENT};
        enum listviewDevice { Name = 0, State, LCBP };
        enum  listviewGlove { Name = 0, Role, State, LC, BP };

        const int nMaxLifeChip = 3;
        const int nMaxBatteryPack = 4;
    }
}

