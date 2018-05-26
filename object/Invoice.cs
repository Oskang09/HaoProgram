using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaoProgram
{
    public class Invoice : IJsonObject<Invoice>
    {
        public int InvoiceID { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Sales { get; set; }

        [JsonIgnore]
        public string ConvertedInvoiceID => Format + InvoiceID.ToString();
        public static string Format => DataManager.Formatting["Invoice"];

        public Invoice() : base(AppDomain.CurrentDomain.BaseDirectory + "/data/invoice")
        {

        }

        public static int getCurrentId(bool remove)
        {
            if (DataManager.ID_Tracker.RemovedInvoiceID.Count > 0)
            {
                int id = DataManager.ID_Tracker.RemovedInvoiceID[0];
                if (remove)
                {
                    DataManager.ID_Tracker.RemovedInvoiceID.Remove(id);
                }
                return id;
            }
            else
            {
                if (remove)
                {
                    int id = DataManager.ID_Tracker.InvoiceID++;
                    return id;
                }
                else
                {
                    return DataManager.ID_Tracker.InvoiceID;
                }
            }
        }
    }
}
