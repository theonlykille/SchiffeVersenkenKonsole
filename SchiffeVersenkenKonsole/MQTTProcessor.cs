using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace SchiffeVersenkenKonsole
{
    internal class MQTT
    {
        public delegate void MsgRecievedHandler(object source, string topic, string message);

        private MqttClient client;
        List<string> topiclist = new List<string>();
        List<byte> qoslist = new List<byte>();

        public MQTT()
        {
            client = new MqttClient("193.197.230.137");
            client.Connect(Guid.NewGuid().ToString());
            client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;
        }

        public void subscribe(string topic, byte qos)
        {
            topiclist.Add(topic);
            qoslist.Add(qos);
            client.Subscribe(topiclist.ToArray(), qoslist.ToArray());
        }

        public void unsubscribe(string topic)
        {
            int n = topiclist.IndexOf(topic);
            qoslist.RemoveRange(n, 1);
            topiclist.Remove(topic);
            client.Unsubscribe(new string[1] { topic });
        }

        public void subscribe(string[] topic, byte[] qos) => client.Subscribe(topic, qos);
        public void unsubscribe(string[] topic) => client.Unsubscribe(topic);
        public void Senden(string topic, string message)
        {
            client.Publish(topic, Encoding.UTF8.GetBytes(message));
        }

        private void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            String topic = e.Topic;
            byte[] messagebytes = e.Message;
            string message = Encoding.UTF8.GetString(messagebytes);
            OnTopic1Recieved(topic, message);
        }
        public event MsgRecievedHandler MsgRecieved;

        public void OnTopic1Recieved(string topic, string message)
        {
            if (MsgRecieved != null)
            {
                MsgRecieved(this, topic, message);
            }
        }

    }
}
