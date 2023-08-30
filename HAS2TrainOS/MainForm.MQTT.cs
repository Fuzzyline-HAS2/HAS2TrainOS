using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using Aspose.Cells;
using NAudio.CoreAudioApi;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace HAS2TrainOS
{
    public partial class MainForm : Form
    {
        MqttClient client;
        string clientId;
        structMAC[] MACs;
        public void MQTT_Initializtion()
        {
            /*Excel에서 MAC 주소 가져오기*/
            Worksheet wsMac = wbMAC.Worksheets[0];  //MAC 시트
            int rowsMac = wsMac.Cells.MaxDataRow;

            /*MQTT 서버 연결*/
            string BrokerAddress = "172.30.1.44";
            client = new MqttClient(BrokerAddress);
            // register a callback-function (we have to implement, see below) which is called by the library when a message was received
            client.MqttMsgPublishReceived += client_MqttMsgPublishReceived; //message 들어왔을때 이벤트 핸들러 실행
            // use a unique id as client id, each time we start the application
            clientId = Guid.NewGuid().ToString();   //mqtt 클라이언트가 갖는 고유 id 생성
            client.Connect(clientId);

            //Subscribe Topic 추가
            //client.Subscribe(new string[] { "test" }, new byte[] { 1 });   // we need arrays as parameters because we can subscribe to different topics with one call
            //string[] mqtt_topic = { "MAINOS", "ALL", "EI1", "EI2", "ER1", "ER2", "EV1", "EV2", "ED", "EG", "ET", "EE", "DOOR1", "DOOR2", "EM1", "EM2" };
            string[] mqtt_topic = new string[rowsMac + 1];
            byte[] mqtt_qos = new byte[rowsMac + 1];
            MACs = new structMAC[rowsMac];
            for (int i = 0; i < rowsMac; i++)
            {
                //Console.WriteLine((wsMac.Cells[i + 1, 0].Value).ToString());
                mqtt_topic[i] = (wsMac.Cells[i + 1, 1].Value).ToString();
                mqtt_qos[i] = (byte)0;
                MACs[i].SaveMAC((wsMac.Cells[i + 1, 0].Value).ToString(), (wsMac.Cells[i + 1, 1].Value).ToString());
            }
            mqtt_topic[rowsMac] = "OS";
            mqtt_qos[rowsMac] = (byte)0;

            client.Subscribe(mqtt_topic, mqtt_qos);
        }
        public void MQTT_Publish(string mqtt_topic, string mqtt_msg)
        {
            client.Publish(mqtt_topic, Encoding.UTF8.GetBytes(mqtt_msg), 0, true);
        }

        String strTagTo = "";
        String[] strTagDevice;
        int nTagCnt = 0;
        int nTagMaxCnt = 0;
        
        private void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            string ReceivedMessage = Encoding.UTF8.GetString(e.Message);
            string ReceivedTopic = e.Topic;
            //Console.WriteLine(ReceivedMessage);
            this.Invoke(new MethodInvoker(delegate ()
            {
            //Console.WriteLine(ReceivedTopic + ":" + ReceivedMessage + "\r\n");
            JObject jsonInput = JObject.Parse(ReceivedMessage);
                //JObject jsonInput = JsonConvert.DeserializeObject<Name>(ReceivedMessage);
                Console.WriteLine(ReceivedTopic + ": " + ReceivedMessage) ;

                if (ReceivedTopic == "OS")
                {                
                    if (jsonInput.ContainsKey("MAC"))   // 발신자 MAC 확인
                    {
                        foreach (structMAC m in MACs)   // 발신자 MAC으로 이름 검색
                        {
                            if (m.strDeviceMAC == jsonInput["MAC"].ToString())  // 찾은 이름 m. 으로 임시 저장
                            {
                                ListView lvTemp = lvDevice;
                                if (m.strDeviceName.StartsWith("G")){
                                     lvTemp = lvGlove;
                                }

                                foreach(ListViewItem lvTempDevice in lvTemp.Items)  //lvTempDevice = 통신 보낸 주체, lvTempGlove = 통신보낸주체에 쓰여 있는 글러브 DN
                                {
                                    if (lvTempDevice.SubItems[(int)listviewDevice.Name].Text == m.strDeviceName)  // 찾은 이름 m. 으로 임시 저장
                                    {

                                        if (jsonInput.ContainsKey("SIT")) // json에 situation 존재할때
                                        {
                                            if (jsonInput["SIT"].ToString() == "tag")
                                            {
                                                if (jsonInput["DN"].ToString().Contains("G"))
                                                {
                                                    foreach (ListViewItem lvTempGlove in lvGlove.Items) //lvTempDevice = 통신 보낸 주체, lvTempGlove = 통신보낸주체에 쓰여 있는 글러브 DN
                                                    {
                                                        if (jsonInput["DN"].ToString() == lvTempGlove.SubItems[(int)listviewGlove.Name].Text)
                                                        {
                                                            switch (m.strDeviceName[1])
                                                            {
                                                                case 'I':
                                                                    lvTempGlove.SubItems[(int)listviewGlove.BP].Text 
                                                                        = (Int32.Parse(lvTempGlove.SubItems[(int)listviewGlove.BP].Text) + Int32.Parse(lvTempDevice.SubItems[(int)listviewDevice.LCBP].Text)).ToString();   //글러브 BP 데이터 수정
                                                                    lvTempDevice.SubItems[(int)listviewDevice.LCBP].Text = "0"; // 아박 배터리팩 데이터 사용완료 '0' 처리
                                                                    break;
                                                                case 'G':
                                                                    lvTempGlove.SubItems[(int)listviewGlove.BP].Text = (Int32.Parse(lvTempGlove.SubItems[(int)listviewGlove.BP].Text) - 1).ToString();   //글러브 BP 데이터 '-1' 처리
                                                                    lvTempDevice.SubItems[(int)listviewDevice.LCBP].Text = (Int32.Parse(lvTempDevice.SubItems[(int)listviewDevice.LCBP].Text) + 1).ToString(); ; // 발전기 배터리팩 데이터  '+1' 처리
                                                                    break;
                                                                case 'R':
                                                                    lvTempGlove.SubItems[(int)listviewGlove.LC].Text
                                                                        = (Int32.Parse(lvTempGlove.SubItems[(int)listviewGlove.LC].Text) + Int32.Parse(lvTempDevice.SubItems[(int)listviewDevice.LCBP].Text)).ToString();   //글러브 LC 데이터 수정
                                                                    lvTempDevice.SubItems[(int)listviewDevice.LCBP].Text = "0"; // 생장 생명칩 데이터 사용완료 '0' 처리
                                                                    break;
                                                                case 'T':
                                                                    lvTempGlove.SubItems[(int)listviewGlove.LC].Text = (Int32.Parse(lvTempGlove.SubItems[(int)listviewGlove.LC].Text) - 1).ToString();   //글러브 LC 데이터 '-1' 처리
                                                                    lvTempDevice.SubItems[(int)listviewDevice.LCBP].Text = (Int32.Parse(lvTempDevice.SubItems[(int)listviewDevice.LCBP].Text) + 1).ToString(); ; // 제단 생명칩 데이터  '+1' 처리
                                                                    break;
                                                                case 'V':
                                                                    lvTempGlove.SubItems[(int)listviewGlove.LC].Text = (Int32.Parse(lvTempGlove.SubItems[(int)listviewGlove.LC].Text) - 1).ToString();   //생존자 글러브 LC 데이터 '-1' 처리
                                                                    foreach(ListViewItem lvTagger in lvGlove.Items)
                                                                    {
                                                                        if (lvTagger.SubItems[(int)listviewGlove.Role].Text == "tagger")
                                                                        {
                                                                            lvTagger.SubItems[(int)listviewGlove.LC].Text = (Int32.Parse(lvTempGlove.SubItems[(int)listviewGlove.LC].Text) + 1).ToString();   //술래글러브 LC 데이터 '-1' 처리
                                                                        }
                                                                    }
                                                                    break;
                                                                case '1':   //글러브 인경우
                                                                case '2':
                                                                case '3':
                                                                case '4':
                                                                case '5':
                                                                case '6':
                                                                case '7':
                                                                case '8':
                                                                    lvTempGlove.SubItems[(int)listviewGlove.LC].Text = (Int32.Parse(lvTempGlove.SubItems[(int)listviewGlove.LC].Text) - 1).ToString();   // 생명칩을 주는 글러브 LC 데이터 '-1' 처리
                                                                    lvTempDevice.SubItems[(int)listviewDevice.LCBP].Text = (Int32.Parse(lvTempDevice.SubItems[(int)listviewDevice.LCBP].Text) + 1).ToString(); ; // 생명칩을 받는 글러브 LC 데이터  '+1' 처리
                                                                    break;
                                                                default:
                                                                    break;
                                                            }
                                                        }
                                                    }
                                                    if (strTagDevice != null)   //엑셀에서 TAG명령어 들어왔을때만 실행
                                                    {
                                                        foreach (String s in strTagDevice)  //엑셀에 있는 TAG_장치이름_시나리오# 에서 장치 이름 strTagDevice에 저장 후 비교중
                                                        {
                                                            if (s == m.strDeviceName)
                                                            {
                                                                Console.WriteLine(s);
                                                                nTagCnt++;
                                                                break;
                                                            }
                                                        }   // 비교 종료
                                                        if (nTagCnt >= nTagMaxCnt)  //엑셀 TAG 명령어 총 개수와 일치하면 다음 나레인 재생 위한 if문
                                                        {
                                                            timerPlayerSkipTime.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite); //PlayerSkipTimer 종료
                                                            foreach (ListViewItem listitem in lvPlayerNarr.Items)
                                                            {
                                                                if (listitem.SubItems[1].Text == strTagTo)
                                                                {
                                                                    nPlayerCur = listitem.Index;
                                                                }
                                                            }
                                                            strTagDevice = null;    //TAG 명령어 초기화
                                                            PlayerNarr();
                                                        }
                                                    }
                                                } // if (jsonInput["DN"].ToString().Contains("G")) ")
                                            } //if (jsonInput["SIT"].ToString() == "tag")
                                        } //if (jsonInput.ContainsKey("SIT")) 
                                    } //if (lvTempDevice.SubItems[(int)listviewDevice.Name].Text == m.strDeviceName)
                                } //foreach(ListViewItem lvTempDevice in lvDevice.Items)
                            } //if (m.strDeviceMAC == jsonInput["MAC"].ToString())
                        } //foreach (structMAC m in MACs)
                    } //if (jsonInput.ContainsKey("MAC"))
                    if (jsonInput.ContainsKey("DN"))
                    {
                        if (jsonInput["DN"].ToString().Contains("G"))
                        {
                            foreach(ListViewItem listitem in lvGlove.Items)
                            {
                                if(jsonInput["DN"].ToString() == listitem.SubItems[0].Text)
                                {
                                    if (jsonInput.ContainsKey("LC"))
                                    {

                                    }
                                }
                            }
                        }

                    }
                }
        
                  /*  if (jsonInput.ContainsKey("situation"))
                    {
                        if (jsonInput.ContainsKey("MAC"))
                        {
                            Console.WriteLine("MAC: " + jsonInput["MAC"].ToString());

                        }
                            Console.WriteLine("Situation: "+ jsonInput["situation"].ToString());
                        if (jsonInput.ContainsKey("DN"))
                        {
                            Console.WriteLine("DN: " + jsonInput["DN"].ToString());
                        }
                    }*/
                
            }));
            //DO SOMETHING..!
        }
    }
}
