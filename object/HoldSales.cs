using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaoProgram
{
    public class HoldSales
    {
        public string Customer { get; set; }
        public List<Product> Products { get; set; }
        public DateTime CreateDate { get; set; }
        public string Agent { get; set; }

        public static List<HoldSales> LoadJson()
        {
            string json = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "/data/hold_sales.json");
            if (json.Length > 0)
            {
                return JsonConvert.DeserializeObject<List<HoldSales>>(json);
            }
            else
            {
                return new List<HoldSales>();
            }
        }

        public static void SaveJson(List<HoldSales> sales)
        {
            using (StreamWriter sw = File.CreateText(AppDomain.CurrentDomain.BaseDirectory + "/data/hold_sales.json"))
            {
                JsonSerializer json = new JsonSerializer();
                json.Serialize(sw, sales);
            }
        }
    }
}
