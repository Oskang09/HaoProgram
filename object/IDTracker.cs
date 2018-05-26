using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaoProgram
{
    public class IDTracker
    {
        [JsonIgnore]
        public int SID { get; set; }
        [JsonIgnore]
        public int IID { get; set; }
        [JsonIgnore]
        public int PID { get; set; }
        [JsonIgnore]
        public int AID { get; set; }
        [JsonIgnore]
        public int RN { get; set; }

        public ObservableCollection<int> RemovedAptID { get; set; }
        public ObservableCollection<int> RemovedInvoiceID { get; set; }
        public ObservableCollection<int> RemovedSalesID { get; set; }
        public ObservableCollection<int> RemovedProductID { get; set; }
        
        public int SalesID
        {
            get
            {
                return SID;
            }
            set
            {
                SID = value;
                SaveJson();
            }
        }
        
        public int InvoiceID
        {
            get
            {
                return IID;
            }
            set
            {
                IID = value;
                SaveJson();
            }
        }
        
        public int ProductID
        {
            get
            {
                return PID;
            }
            set
            {
                PID = value;
                SaveJson();
            }
        }
        
        public int AptID
        {
            get
            {
                return AID;
            }
            set
            {
                AID = value;
                SaveJson();
            }
        }
        
        public int ReceiptNumber
        {
            get
            {
                return RN;
            }
            set
            {
                RN = value;
                SaveJson();
            }
        } 

        public IDTracker(bool newdata)
        {
            SalesID = 1;
            InvoiceID = 1;
            ProductID = 1;
            AptID = 1;
            ReceiptNumber = 1;
            RemovedAptID = new ObservableCollection<int>();
            RemovedInvoiceID = new ObservableCollection<int>();
            RemovedSalesID = new ObservableCollection<int>();
            RemovedProductID = new ObservableCollection<int>();
            evtListen();
            SaveJson();
        }

        public void evtListen()
        {
            RemovedAptID.CollectionChanged += SaveWhenVarChange;
            RemovedInvoiceID.CollectionChanged += SaveWhenVarChange;
            RemovedSalesID.CollectionChanged += SaveWhenVarChange;
            RemovedProductID.CollectionChanged += SaveWhenVarChange;
        }

        private void SaveWhenVarChange(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            RemovedAptID = new ObservableCollection<int>(RemovedAptID.OrderBy(x => x));
            RemovedInvoiceID = new ObservableCollection<int>(RemovedInvoiceID.OrderBy(x => x));
            RemovedSalesID = new ObservableCollection<int>(RemovedSalesID.OrderBy(x => x));
            RemovedProductID = new ObservableCollection<int>(RemovedProductID.OrderBy(x => x));
            evtListen();
            SaveJson();
        }

        private void SaveJson()
        {
            using (StreamWriter sw = File.CreateText(AppDomain.CurrentDomain.BaseDirectory + "/data/id.json"))
            {
                JsonSerializer json = new JsonSerializer();
                json.Serialize(sw, this);
            }
        }
    }
}
