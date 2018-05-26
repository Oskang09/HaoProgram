using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaoProgram
{
    public class UserInfomation
    {
        public string CompanyName { get; set; }
        public string SSM { get; set; }
        public string Address { get; set; }
        public string TelNo { get; set; }
        public string GSTReg { get; set; }

        public string EditQTY { get; set; }
        public string EditUP { get; set; }
        public string EditDiscount { get; set; }
        public string[] InvoiceFooter { get; set; }
        
        public DateTime LastBackup { get; set; }
        
        public Dictionary<string, string> ShortKeyData { get; set; }

        public UserInfomation()
        {
            ShortKeyData = new Dictionary<string, string>();
        }

        public void SaveJson()
        {
            using (StreamWriter sw = File.CreateText(AppDomain.CurrentDomain.BaseDirectory + "/data/user_info.json"))
            {
                JsonSerializer json = new JsonSerializer();
                json.Serialize(sw, this);
            }
        }
    }
}
