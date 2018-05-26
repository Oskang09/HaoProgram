using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaoProgram
{
    public class Appointment : IJsonObject<Appointment>
    {
        public int Id { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Customer { get; set; }
        public DateTime Reminder { get; set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public List<string> Checked { get; set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string[] Remarks { get; set; }

        public DateTime RemindDate { get; set; }

        public static int getCurrentId(bool remove)
        {
            if (DataManager.ID_Tracker.RemovedAptID.Count >  0)
            {
                int id = DataManager.ID_Tracker.RemovedAptID[0];
                if (remove)
                {
                    DataManager.ID_Tracker.RemovedAptID.Remove(id);
                }
                return id;
            }
            else
            {
                if (remove)
                {
                    int id = DataManager.ID_Tracker.AptID++;
                    return id;
                }
                else
                {
                    return DataManager.ID_Tracker.AptID;
                }
            }
        }

        public Appointment() : base(AppDomain.CurrentDomain.BaseDirectory + "/data/appointment")
        {
            
        }
    }
}
