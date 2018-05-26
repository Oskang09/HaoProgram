using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HaoProgram
{
    public class Customer : IJsonObject<Customer>
    {
        // Primary Key
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string NRIC { get; set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Vehicle { get; set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Name { get; set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Contact { get; set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public List<string> Address { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore), DefaultValue(null)]
        public string Agent { get; set; }

        public Customer() : base(AppDomain.CurrentDomain.BaseDirectory + "/data/customer")
        {

        }
    }
}
