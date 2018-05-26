using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaoProgram
{
    public class Product : IJsonObject<Product>
    {
        public int Code { get; set; }
        public string Description { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int Quantity { get; set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public decimal UnitPrice { get; set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public decimal Discount { get; set; }
        
        [JsonIgnore]
        public string DisplayText => ToString();

        public override string ToString()
        {
            return $"{Description} x{Quantity}";
        }

        [JsonIgnore]
        public string ConvertedProductCode => Format + Code.ToString();
        public static string Format => DataManager.Formatting["Product"];

        public Product() : base(AppDomain.CurrentDomain.BaseDirectory + "/data/product")
        {

        }
        public static int getCurrentId(bool remove)
        {
            if (DataManager.ID_Tracker.RemovedProductID.Count > 0)
            {
                int id = DataManager.ID_Tracker.RemovedProductID[0];
                if (remove)
                {
                    DataManager.ID_Tracker.RemovedProductID.Remove(id);
                }
                return id;
            }
            else
            {
                if (remove)
                {
                    int id = DataManager.ID_Tracker.ProductID++;
                    return id;
                }
                else
                {
                    return DataManager.ID_Tracker.ProductID;
                }
            }
        }
    }
}
