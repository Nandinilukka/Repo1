using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Model
{
    public class MqttSettings
    {
        public string BrokerAddress { get; set; }
        public int BrokerPort { get; set; }
        public string ClientId { get; set; }
        public bool CleanSession { get; set; }
        public int KeepAlivePeriod { get; set; }
        public List<string> Topics { get; set; }
    }
}

