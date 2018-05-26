using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaoProgram
{
    public class Sales : IJsonObject<Sales>
    {
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int SalesId { get; set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Customer { get; set; }
        public List<Product> Products { get; set; }

        public DateTime CreateDate { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Dictionary<string, decimal> Paid { get; set; }
        public string Agent { get; set; }

        public Sales() : base(AppDomain.CurrentDomain.BaseDirectory + "/data/sales")
        {

        }

        public decimal GetUnpaid()
        {
            decimal paid = 0;
            foreach (var p in Paid)
            {
                paid += p.Value;
            }
            return GetNeedToPay() - paid;
        }
        public decimal GetNeedToPay()
        {
            decimal total = 0;
            foreach (var pdt in Products)
            {
                total += (pdt.UnitPrice - pdt.Discount) * pdt.Quantity;
            }
            return total;
        }

        public string GetCustomerName()
        {
            return new Customer().LoadJson(Customer).Name;
        }

        public string GetCustomerIC()
        {
            return new Customer().LoadJson(Customer).NRIC;
        }

        [JsonIgnore]
        public string ConvertedSalesID => Format + SalesId.ToString();
        public static string Format => DataManager.Formatting["Sales"];

        public static int getCurrentId(bool remove)
        {
            if (DataManager.ID_Tracker.RemovedSalesID.Count > 0)
            {
                int id = DataManager.ID_Tracker.RemovedSalesID[0];
                if (remove)
                {
                    DataManager.ID_Tracker.RemovedSalesID.Remove(id);
                }
                return id;
            }
            else
            {
                if (remove)
                {
                    int id = DataManager.ID_Tracker.SalesID++;
                    return id;
                }
                else
                {
                    return DataManager.ID_Tracker.SalesID;
                }
            }
        }
    }
}
