using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaoProgram
{
    public class Agent : IJsonObject<Agent>
    {
        public string Name { get; set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string NRIC { get; set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Contact { get; set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public List<string> Address { get; set; }
        public DateTime DateOfJoin { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string AgentCommission { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, decimal> ComissionGained { get; set; }

        public Agent() : base(AppDomain.CurrentDomain.BaseDirectory + "/data/agent")
        {

        }
    }
}
