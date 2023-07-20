using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using Aspose.Cells;


namespace HAS2TrainOS
{
    public partial class MainForm : Form
    {
        MqttClient client;
        string clientId;

        public void MQTT_Initializtion()
        {
            /*Excel에서 MAC 주소 가져오기*/
            Worksheet wsMac = wbMac.Worksheets[0];  //MAC 시트

            string BrokerAddress = "172.30.1.44";
            client = new MqttClient(BrokerAddress);
            // register a callback-function (we have to implement, see below) which is called by the library when a message was received
            client.MqttMsgPublishReceived += client_MqttMsgPublishReceived; //message 들어왔을때 이벤트 핸들러 실행
            // use a unique id as client id, each time we start the application
            clientId = Guid.NewGuid().ToString();   //mqtt 클라이언트가 갖는 고유 id 생성
            client.Connect(clientId);

            //Subscribe Topic 추가
            //client.Subscribe(new string[] { "test" }, new byte[] { 1 });   // we need arrays as parameters because we can subscribe to different topics with one call
            string[] mqtt_topic = { "MAINOS", "ALL", "EI1", "EI2", "ER1", "ER2", "EV1", "EV2", "ED", "EG", "ET", "EE", "DOOR1", "DOOR2", "EM1", "EM2" };
            //[] mqtt_topic = { "MAINOS" };
            //for(int i = 0; i <)
            byte[] mqtt_qos = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            client.Subscribe(mqtt_topic, mqtt_qos);
        }
        private void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            string ReceivedMessage = Encoding.UTF8.GetString(e.Message);
            string ReceivedTopic = e.Topic;
            Console.WriteLine(ReceivedMessage);
            this.Invoke(new MethodInvoker(delegate ()
            {
                Console.WriteLine(ReceivedTopic + ": " + ReceivedMessage + "\r\n");
            }));
            //DO SOMETHING..!
        }
        public void MQTT_Publish(string mqtt_topic, string mqtt_msg)
        {
            client.Publish(mqtt_topic, Encoding.UTF8.GetBytes(mqtt_msg), 0, true);
        }
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }
    }
}
