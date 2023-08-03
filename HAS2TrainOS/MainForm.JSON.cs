using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NAudio.CoreAudioApi;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static System.Windows.Forms.AxHost;

namespace HAS2TrainOS
{
    public partial class MainForm : Form
    {
        public void GloveJSONPublish(String DN, String role = "", String state = "", String LC = "", String BP = "")
        {
            JObject gloveData = new JObject(new JProperty("DN", DN));
            if (role != "")
                gloveData.Add(new JProperty("R", role));
            if (state != "")
                gloveData.Add(new JProperty("DS", state));
            if (LC != "")
                gloveData.Add(new JProperty("LC", LC));
            if (role != "")
                gloveData.Add(new JProperty("BP", BP));
            MQTT_Publish("GLOVE", gloveData.ToString());
        }
        public void DeviceJSONPublish(String DN, String state = "", String LCBP = "")
        {
            JObject DeviceData = new JObject(new JProperty("DN", DN));
            if (state != "")
                DeviceData.Add(new JProperty("DS", state));
            if (LCBP != "")
                DeviceData.Add(new JProperty("LCBP", LCBP));
            MQTT_Publish(DN, DeviceData.ToString());
        }
        public void SituationJSONPublish(String Device, String Situation, String DN = "")
        {
            JObject SituationData = new JObject(new JProperty("Situation", Situation));
            SituationData.Add(new JProperty("MAC", "8A:88"));
            if (DN != "")
                SituationData.Add(new JProperty("DN", DN));

            MQTT_Publish(Device, SituationData.ToString());
        }
        public void SCNJSONPublish(String Device, String SCN)
        {
            JObject SituationData = new JObject(new JProperty("DS", "scenario"));
            SituationData.Add(new JProperty("SCN", SCN));
           
            MQTT_Publish(Device, SituationData.ToString());
        }
    }
}
