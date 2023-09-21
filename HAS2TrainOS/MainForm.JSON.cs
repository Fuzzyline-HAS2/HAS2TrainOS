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

            MQTT_Publish("GLOVE", gloveData.ToString());//글러브의 TOPIC은 GLOVE를 통해서 전체 장치에 전달
        }
        public void GloveJSONPublish_MAC(String DN, String role = "", String state = "", String LC = "", String BP = "")
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
            foreach (structMAC m in MACs)
            {
                if (m.strDeviceName == DN)
                {
                    MQTT_Publish(m.strDeviceMAC, gloveData.ToString());//글러브의 TOPIC은 GLOVE를 통해서 전체 장치에 전달
                }
            }
        }
        public void DeviceJSONPublish(String DN, String state = "", String LCBP = "") //DN: 장치이름. State: 장치 상태, LCBP: 생명/배터리 개수
        {
            JObject DeviceData = new JObject(new JProperty("DN", DN));
            if (state != "")
                DeviceData.Add(new JProperty("DS", state));
            if (LCBP != "")
                DeviceData.Add(new JProperty("LCBP", LCBP));
            foreach (structMAC m in MACs)
            {
                if (m.strDeviceName == DN)
                {
                    MQTT_Publish(m.strDeviceMAC, DeviceData.ToString());
                }
            }
        }
        public void DeviceJSONPublishALL(String state, String LCBP = "") //DN: 장치이름. State: 장치 상태, LCBP: 생명/배터리 개수
        {
            JObject DeviceData = new JObject(new JProperty("DS", state));
            if (LCBP != "")
                DeviceData.Add(new JProperty("LCBP", LCBP));
            //MQTT_Publish("ALL", DeviceData.ToString());
        }
        public void SituationJSONPublish(String Device, String Situation, String DN = "")   //Device: 글러브 이름, Situation: 명령어, DN: 글러브 이름(선택적)
        {
            JObject SituationData = new JObject(new JProperty("Situation", Situation));
            if (DN != "")
                SituationData.Add(new JProperty("DN", DN));
            foreach (structMAC m in MACs)
            {
                if(m.strDeviceName == Device)
                {
                    MQTT_Publish(m.strDeviceMAC, SituationData.ToString());
                }
            }
        }
        public void SCNJSONPublish(String Device, String SCN)   //DeviceMAC:보내는 장치 MAC 주소,SCN: 시나리오 번호
            {
                JObject SituationData = new JObject(new JProperty("DS", "scenario"));
                SituationData.Add(new JProperty("SCN", SCN));

                if (Device.Contains("ALLp") || Device.Contains("ALLt") || Device.Contains("AGp") || Device.Contains("AGt"))   
                {
                    foreach (string strAllDevice in AllDevice.StringSelecetor(Device))
                    {
                        foreach (structMAC m in MACs)
                        {
                            if (m.strDeviceName == strAllDevice)
                            {
                                client.Publish(m.strDeviceMAC, Encoding.UTF8.GetBytes(SituationData.ToString()), 0, true);
                                break;
                            }
                        }
                    }
                }
                else
                {
                    foreach (structMAC m in MACs)
                    {
                        if (m.strDeviceName == Device)
                        {
                            client.Publish(m.strDeviceMAC, Encoding.UTF8.GetBytes(SituationData.ToString()), 0, true);
                            break;
                        }
                    }
                }
            }
    }
}
