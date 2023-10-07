using Newtonsoft.Json.Linq;
using System;
using System.Text;
using System.Windows.Forms;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace HAS2TrainOS
{
    public partial class MainForm : Form
    {
        MqttClient client;
        string clientId;
        public void MQTT_Initializtion()
        {

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
            string[] mqtt_topic = new string[nMaxMACNum + 1];
            byte[] mqtt_qos = new byte[nMaxMACNum + 1];
            int nCntMAC = 0;
            foreach(structMAC m in MACs)
            {
                mqtt_topic[nCntMAC] = m.strDeviceMAC;
                mqtt_qos[nCntMAC] = (byte)0;
                nCntMAC++;
            }
            mqtt_topic[nCntMAC] = "OS";
            mqtt_qos[nCntMAC] = (byte)0;

            client.Subscribe(mqtt_topic, mqtt_qos);
        }
        public void MQTT_Publish(string mqtt_topic, string mqtt_msg)
        {
            client.Publish(mqtt_topic, Encoding.UTF8.GetBytes(mqtt_msg), 0, true);
        }
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
                                if (m.strDeviceName.StartsWith("G")){   //r
                                    lvTemp = lvGlove;
                                    if (!m.strDeviceName[1].Equals(lvGlove.Items[0].SubItems[(int)listviewGlove.Name].Text[1]))
                                    {    //글러브 그룹의 번호가 lvGlove에 있는 글러브 그룹 번호가 동일하지 않다면 해당 글러브 change 전송
                                        SituationJSONPublish(m.strDeviceName, "change"); //현재 훈련소에 들어가있는 번호와 다르면 change 보내서 게임 상태로 전송
                                    }
                                }

                                foreach(ListViewItem lvTempDevice in lvTemp.Items)  //lvTempDevice = 통신 보낸 주체, lvTempGlove = 통신보낸주체에 쓰여 있는 글러브 DN
                                {
                                    if (lvTempDevice.SubItems[(int)listviewDevice.Name].Text == m.strDeviceName)  // 찾은 이름 m. 으로 임시 저장
                                    {
                                        if (jsonInput.ContainsKey("SIT")) // json에 situation 존재할때
                                        {
                                            if (jsonInput["SIT"].ToString() == "tag"|| jsonInput["SIT"].ToString() == "kill")
                                            {
                                                if (jsonInput["DN"].ToString().Contains("G"))
                                                {
                                                    foreach (ListViewItem lvTempGlove in lvGlove.Items) //lvTempDevice = 통신 보낸 주체, lvTempGlove = 통신보낸주체에 쓰여 있는 글러브 DN
                                                    {
                                                        if (jsonInput["DN"].ToString() == lvTempGlove.SubItems[(int)listviewGlove.Name].Text)
                                                        {
                                                            String GloveData;
                                                            switch (m.strDeviceName[1])
                                                            {
                                                                case 'I':
                                                                    if (PlayerSCNProcessor.nCurrentCnt == 29)
                                                                    {
                                                                        GloveData = "+" + lvTempDevice.SubItems[(int)listviewDevice.LCBP].Text;   //글러브 BP 데이터 수정
                                                                        GloveListViewChange(lvTempGlove, strBP: GloveData);
                                                                        DeviceListViewChange(lvTempDevice, strLCBP: "0"); // 아박 배터리팩 데이터 사용완료 '0' 처리
                                                                    }
                                                                    break;
                                                                case 'G':
                                                                    GloveListViewChange(lvTempGlove, strBP: "-1"); //글러브 BP 데이터 '-1' 처리
                                                                    DeviceListViewChange(lvTempDevice, strLCBP: "+1");// 발전기 배터리팩 데이터  '+1' 처리
                                                                    break;
                                                                case 'R':
                                                                    GloveListViewChange(lvTempGlove, strLC: "+1");//글러브 LC 데이터 +1 처리
                                                                    DeviceListViewChange(lvTempDevice, strLCBP: "0"); // 생장 생명칩 데이터 사용완료 '0' 처리
                                                                    break;
                                                                case 'T':
                                                                    Console.WriteLine("cur:" + TaggerSCNProcessor.nCurrentCnt);
                                                                    if (TaggerSCNProcessor.nCurrentCnt != 10)
                                                                    {
                                                                        GloveListViewChange(lvTempGlove, strLC: "-1"); //글러브 LC 데이터 '-1' 처리
                                                                        DeviceListViewChange(lvTempDevice, strLCBP: "+1"); // 제단 생명칩 데이터  '+1' 처리
                                                                    }
                                                                    break;
                                                                case 'V':
                                                                    GloveListViewChange(lvTempGlove, strLC: "-1"); //생존자 글러브 LC 데이터 '-1' 처리
                                                                    foreach (ListViewItem lvTagger in lvGlove.Items)
                                                                    {
                                                                        if (lvTagger.SubItems[(int)listviewGlove.Role].Text == "tagger")
                                                                        {
                                                                            GloveListViewChange(lvTagger, strLC: "-1"); //술래글러브 LC 데이터 '-1' 처리
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
                                                                    if (jsonInput["SIT"].ToString() == "tag")
                                                                    {
                                                                        GloveListViewChange(lvTempGlove, strLC: "-1");// 생명칩을 주는 글러브 LC 데이터 '-1' 처리
                                                                        GloveListViewChange(lvTempDevice, strLC: "+1");// 생명칩을 받는 글러브 LC 데이터  '+1' 처리
                                                                    }
                                                                    else if (jsonInput["SIT"].ToString() == "kill")
                                                                    {
                                                                        GloveListViewChange(lvTempGlove, strLC: "+1");// 술래 글러브 LC 데이터 '+1' 처리
                                                                        GloveListViewChange(lvTempDevice, strLC: "-1");// 생존자 글러브 LC 데이터  '-1' 처리
                                                                    }
                                                                    break;
                                                                default:
                                                                    break;
                                                            }
                                                        }
                                                    }
                                                    if (PlayerSCNProcessor.strTagDevice != null)   //엑셀에서 TAG명령어 들어왔을때만 실행
                                                    {
                                                        Console.WriteLine("PlayerSkip Tag detected");
                                                        foreach (String s in PlayerSCNProcessor.strTagDevice)  //엑셀에 있는 TAG_장치이름_시나리오# 에서 장치 이름 strTagDevice에 저장 후 비교중
                                                        {
                                                            if (s == m.strDeviceName)
                                                            {
                                                                Console.WriteLine(s);
                                                                PlayerSCNProcessor.nTagCnt++;
                                                            }
                                                            else if (s == "SGp")
                                                            {
                                                                if (m.strDeviceName.Contains("P"))
                                                                {
                                                                    Console.WriteLine("TAG_SGp에서 태그된 글러브 인식됨");
                                                                    PlayerSCNProcessor.nTagCnt++;
                                                                }
                                                            }
                                                        }   // 비교 종료.
                                                        Console.WriteLine("PlayererSCNProcessor.nTagCnt: " + PlayerSCNProcessor.nTagCnt.ToString() + " PlayerSCNProcessor.nTagMaxCnt: " + PlayerSCNProcessor.nTagMaxCnt, ToString());
                                                        if (PlayerSCNProcessor.nTagCnt >= PlayerSCNProcessor.nTagMaxCnt)  //엑셀 TAG 명령어 총 개수와 일치하면 다음 나레인 재생 위한 if문
                                                        {
                                                            Console.WriteLine("PlayerSkip MAXTag detected");
                                                            PlayerSCNProcessor.timerPlayerSkipTime.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite); //PlayerSkipTimer 종료
                                                            foreach (ListViewItem listitem in lvPlayerNarr.Items)
                                                            {
                                                                if (listitem.SubItems[1].Text == PlayerSCNProcessor.strTagTo)
                                                                {
                                                                    PlayerSCNProcessor.nCurrentCnt = listitem.Index;
                                                                }
                                                            }
                                                            PlayerSCNProcessor.strTagDevice = null;    //TAG 명령어 초기화
                                                            PlayerSCNProcessor.NarrPlayJudge();
                                                        }
                                                    }
                                                    if (TaggerSCNProcessor.strTagDevice != null)   //엑셀에서 TAG명령어 들어왔을때만 실행
                                                    {
                                                        Console.WriteLine("TaggerSkip Tag detected");
                                                        foreach (String s in TaggerSCNProcessor.strTagDevice)  //엑셀에 있는 TAG_장치이름_시나리오# 에서 장치 이름 strTagDevice에 저장 후 비교중
                                                        {
                                                            if (s == m.strDeviceName)
                                                            {
                                                                Console.WriteLine(s);
                                                                TaggerSCNProcessor.nTagCnt++;
                                                            }
                                                            else if (s == "SGt")
                                                            {
                                                                if (m.strDeviceName.Contains("P"))
                                                                {
                                                                    Console.WriteLine("TAG_SGt에서 태그된 글러브 인식됨");
                                                                    TaggerSCNProcessor.nTagCnt++;
                                                                }
                                                            }
                                                        }   // 비교 종료
                                                        Console.WriteLine("TaggerSCNProcessor.nTagCnt: " + TaggerSCNProcessor.nTagCnt.ToString() + " TaggerSCNProcessor.nTagMaxCnt: " + TaggerSCNProcessor.nTagMaxCnt, ToString());
                                                        if (TaggerSCNProcessor.nTagCnt >= TaggerSCNProcessor.nTagMaxCnt)  //엑셀 TAG 명령어 총 개수와 일치하면 다음 나레인 재생 위한 if문
                                                        {
                                                            Console.WriteLine("TaggerSkip MAXTag detected");
                                                            TaggerSCNProcessor.timerPlayerSkipTime.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite); //PlayerSkipTimer 종료
                                                            foreach (ListViewItem listitem in lvTaggerNarr.Items)
                                                            {
                                                                if (listitem.SubItems[1].Text == TaggerSCNProcessor.strTagTo)
                                                                {
                                                                    TaggerSCNProcessor.nCurrentCnt = listitem.Index;
                                                                }
                                                            }
                                                            TaggerSCNProcessor.strTagDevice = null;    //TAG 명령어 초기화
                                                            TaggerSCNProcessor.NarrPlayJudge();
                                                        }
                                                    }
                                                } // if (jsonInput["DN"].ToString().Contains("G")) ")
                                            } //if (jsonInput["SIT"].ToString() == "tag")
                                            else if (jsonInput["SIT"].ToString() == "start")
                                            {
                                                if (lvTemp == lvGlove)
                                                {
                                                    GloveJSONPublish_MAC(lvTempDevice.SubItems[(int)listviewDevice.Name].Text);
                                                    GloveListViewChange(lvTempDevice);
                                                }
                                                else
                                                {
                                                    DeviceListViewChange(lvTempDevice);
                                                }
                                            }
                                        } //if (jsonInput.ContainsKey("SIT")) 
                                        else if (jsonInput.ContainsKey("EMRG"))
                                        {
                                            if (jsonInput["EMRG"].ToString() == "null")
                                            {
                                                MessageBox.Show("비상탈출 버튼 문제 해결됨");
                                            }
                                            else if (jsonInput["EMRG"].ToString() == "player")
                                            {
                                                MessageBox.Show("!생존자 비상탈출 버튼 문제 발생!");
                                            }
                                            else if (jsonInput["EMRG"].ToString() == "tagger")
                                            {
                                                MessageBox.Show("!생존자 비상탈출 버튼 문제 발생!");
                                            }
                                            else if (jsonInput["EMRG"].ToString() == "all")
                                            {
                                                MessageBox.Show("!양쪽 비상탈출 버튼 문제 발생!");
                                            }
                                            else
                                            {
                                                MessageBox.Show("훈련소 공용공간 문제 발생");
                                            }

                                        }
                                        if (jsonInput.ContainsKey("DS"))
                                        {
                                            if (m.strDeviceName.StartsWith("E"))
                                            {
                                                DeviceListViewChange(lvTempDevice, State: jsonInput["DS"].ToString());
                                            }
                                            else if (m.strDeviceName.StartsWith("G"))
                                            {
                                                GloveListViewChange(lvTempDevice, State: jsonInput["DS"].ToString());
                                            }
                                        }
                                        if (jsonInput.ContainsKey("LCBP"))
                                        {
                                            DeviceListViewChange(lvTempDevice, strLCBP: jsonInput["LCBP"].ToString()); // 아박 배터리팩 데이터 사용완료 '0' 처리
                                        }
                                        if (jsonInput.ContainsKey("ROLE"))
                                        {
                                            GloveListViewChange(lvTempDevice, Role: jsonInput["ROLE"].ToString());
                                        }
                                        if (jsonInput.ContainsKey("LC"))
                                        {
                                            GloveListViewChange(lvTempDevice, strLC: jsonInput["LC"].ToString());
                                        }
                                        if (jsonInput.ContainsKey("BP"))
                                        {
                                            GloveListViewChange(lvTempDevice, strBP: jsonInput["BP"].ToString());
                                        }

                                    }//if (lvTempDevice.SubItems[(int)listviewDevice.Name].Text == m.strDeviceName)
                                } //foreach(ListViewItem lvTempDevice in lvDevice.Items)
                            } //if (m.strDeviceMAC == jsonInput["MAC"].ToString())
                        } //foreach (structMAC m in MACs)
                    } //if (jsonInput.ContainsKey("MAC"))
                }
            }));
            //DO SOMETHING..!
        }
    }
}
