using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NAudio.CoreAudioApi;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HAS2TrainOS
{
    public partial class MainForm : Form
    {
        public void GloveJSONPublish(String DN, String role = "", String state = "", String LC = "", String BP = "")
        {
            JObject gloveData = new JObject(new JProperty("DN", DN));
            if (role != "")
                gloveData.Add(new JProperty("role", role));
            if (state != "")
                gloveData.Add(new JProperty("state", state));
            if (LC != "")
                gloveData.Add(new JProperty("lc", LC));
            if (role != "")
                gloveData.Add(new JProperty("bp", BP));
            MQTT_Publish(DN, gloveData.ToString());
        }
        public void DeviceJSONPublish(String DN, String state = "", String LCBP = "")
        {
            JObject DeviceData = new JObject(new JProperty("dn", DN));
            if (state != "")
                DeviceData.Add(new JProperty("state", state));
            if (LCBP != "")
                DeviceData.Add(new JProperty("lcbp", LCBP));
            MQTT_Publish(DN, DeviceData.ToString());
        }
    }
}
