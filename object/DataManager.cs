using CSharpOskaAPI.Form;
using CSharpOskaAPI.Parse;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HaoProgram
{
    public class DataManager
    {
        public static UserInfomation UserInfo { get; set; }
        public static Dictionary<string, string> AppointmentTypes { get; set; }
        public static Dictionary<string, string> Formatting { get; set; }
        public static IDTracker ID_Tracker { get; set; }

        public DataManager()
        {
            InitializeFS();
            InitializeFormatting();
            InitializeAppointmentType();
            InitializeUser();
            InitializeID_Tracker();
        }

        public static string BASE_PATH = AppDomain.CurrentDomain.BaseDirectory + "/data";
        public static string[] FILES = new string[] { "/agent", "/appointment", "/commission", "/customer", "/invoice", "/product", "/sales", "/apt_type.config", "/user_info.json", "/id.json", "/formatting.config" , "/hold_sales.json" }; 

        public static string ERROR_TRACKER_PATH = AppDomain.CurrentDomain.BaseDirectory + "/error_tracker.log";
        
        public static void InitializeSKey(Control ctrl, string Prefix)
        {
            foreach (var ctl in FunctionUtil.GetAllChildren(ctrl))
            {
                if (ctl is Button)
                {
                    Button btn = ctl as Button;
                    btn.MouseDown += (sender, e) =>
                    {
                        if (e.Button == MouseButtons.Right)
                        {
                            string empty = string.Empty;
                            if (UserInfo.ShortKeyData.ContainsKey($"{Prefix}.{btn.Name}"))
                            {
                                empty = UserInfo.ShortKeyData[$"{Prefix}.{btn.Name}"];
                            }
                            AssignShorcutKey ask = new AssignShorcutKey(empty);
                            ask.ActionEvent += (obj, ea) =>
                            {
                                if (ea.ActionString == "Assign")
                                {
                                    if (UserInfo.ShortKeyData
                                        .ContainsKey($"{Prefix}.{btn.Name}"))
                                    {
                                        if (ea.ActionObject as string == "")
                                        {
                                            UserInfo.ShortKeyData.Remove($"{Prefix}.{btn.Name}");
                                            UserInfo.SaveJson();
                                        }
                                        else
                                        {
                                            UserInfo.ShortKeyData[$"{Prefix}.{btn.Name}"]
                                                = ea.ActionObject as string;
                                            UserInfo.SaveJson();
                                        }
                                    }
                                    else if (ea.ActionObject as string != "")
                                    {
                                        UserInfo.ShortKeyData
                                            .Add($"{Prefix}.{btn.Name}", ea.ActionObject as string);
                                        UserInfo.SaveJson();
                                    }
                                }
                            };
                            ask.ShowDialog();
                        }
                    };
                }
            }
        }

        public static void InitializeID_Tracker()
        {
            string json = File.ReadAllText(BASE_PATH + "/id.json");
            if (json.Length > 0)
            {
                ID_Tracker = JsonConvert.DeserializeObject<IDTracker>(json);
            }
            else
            {
                ID_Tracker = new IDTracker(true);
            }
        }
        public static void InitializeUser()
        {
            string json = File.ReadAllText(BASE_PATH + "/user_info.json");
            if (json.Length > 0)
            {
                UserInfo = JsonConvert.DeserializeObject<UserInfomation>(File.ReadAllText(BASE_PATH + "/user_info.json"));
            }
            else
            {
                UserInfo = new UserInfomation();
            }
        }

        public static void InitializeFS()
        {
            if (!File.Exists(ERROR_TRACKER_PATH))
            {
                File.Create(ERROR_TRACKER_PATH).Close();
            }
            if (!Directory.Exists(BASE_PATH))
            {
                Directory.CreateDirectory(BASE_PATH);
            }
            foreach (String str in FILES)
            {
                if (str.Contains("."))
                {
                    if (!File.Exists(BASE_PATH + str))
                    {
                        File.Create(BASE_PATH + str).Close();
                        if (str == "/apt_type.config")
                        {
                            File.WriteAllLines(BASE_PATH + str
                                , new string[] { "ROAD_TAX|RT", "INSPECTION|PUS", "INSURANCE|INS" });
                        }
                        if (str == "/formatting.config")
                        {
                            File.WriteAllLines(BASE_PATH + str
                                , new string[] { "Invoice=IV", "Sales=SL", "Product=PD" });
                        }
                    }
                }
                else
                {
                    if (!Directory.Exists(BASE_PATH + str))
                    {
                        Directory.CreateDirectory(BASE_PATH + str);
                    }
                }
            }
        }
        public static void InitializeFormatting()
        {
            Dictionary<string, string> lists = new Dictionary<string, string>();
            foreach (var str in File.ReadAllLines(BASE_PATH + "/formatting.config"))
            {
                var array = str.Split('=');
                lists.Add(array[0], array[1]);
            }
            Formatting = lists;
        }
        public static void InitializeAppointmentType()
        {
            Dictionary<string, string> lists = new Dictionary<string, string>();
            foreach (var str in File.ReadAllLines(BASE_PATH + "/apt_type.config"))
            {
                var array = str.Split('|');
                lists.Add(array[0], array[1]);
            }
            AppointmentTypes = lists;
        }

        // Recursively update data
        public static void UpdateData(object olddata, object newdata)
        {
            if (olddata is Product)
            {
                var newpro = newdata as Product;
                var oldpro = olddata as Product;
                foreach (var comc in new AgentCommission().GetList())
                {
                    if (comc.Product.Any((x) => x.Item1.ConvertedProductCode == oldpro.ConvertedProductCode))
                    {
                        comc.Product.Remove(comc.Product.First((x) => x.Item1.ConvertedProductCode == oldpro.ConvertedProductCode));
                        comc.SaveJson(comc.CategoryName);
                    }
                }
            }
            if (olddata is Sales)
            {
                var newsales = newdata as Sales;
                var oldsales = olddata as Sales;
                List<Invoice> invoice = new Invoice().GetList();
                if (invoice.Any((x) => x.Sales == oldsales.ConvertedSalesID))
                {
                    var recursive = invoice.First(
                        (x) => x.Sales == oldsales.ConvertedSalesID);

                    if (newdata != null)
                    {
                        Invoice inv = invoice.First((x) => x.Sales == oldsales.ConvertedSalesID);
                        inv.Sales = newsales.ConvertedSalesID;
                        inv.SaveJson(inv.ConvertedInvoiceID);
                        UpdateData(recursive, inv);
                    }
                    if (newdata == null)
                    {
                        recursive.DeleteJson(recursive.ConvertedInvoiceID);
                        UpdateData(recursive, null);
                    }
                }

                foreach (var agent in new Agent().GetList())
                {
                    if (agent.Name == oldsales.Agent)
                    {
                        if (agent.ComissionGained.ContainsKey(oldsales.ConvertedSalesID))
                        {
                            if (newdata != null)
                            {
                                var temp = agent.ComissionGained[oldsales.ConvertedSalesID];
                                agent.ComissionGained.Remove(oldsales.ConvertedSalesID);
                                agent.ComissionGained.Add(newsales.ConvertedSalesID, temp);
                            }
                            if (newdata == null)
                            {
                                agent.ComissionGained.Remove(oldsales.ConvertedSalesID);
                            }
                            agent.SaveJson(agent.Name);
                        }
                    }
                }
                MainWindow.getInstance.RM.InitializeView();
            }
            if (olddata is Agent)
            {
                var newagent = newdata as Agent;
                var oldagent = olddata as Agent;
                List<Customer> customers = new Customer().GetList();
                if (customers.Any((x) => x.Agent == oldagent.Name))
                {
                    foreach (var customer in customers)
                    {
                        if (customer.Agent == oldagent.Name)
                        {
                            Customer cmr = customers.First((x) => x.Vehicle == customer.Vehicle);
                            cmr.Agent = newagent != null ? newagent.Name : "";
                            UpdateData(customer, cmr);
                            cmr.SaveJson(cmr.Vehicle);
                        }
                    }
                    MainWindow.getInstance.CDD.UpdateAgent();
                    MainWindow.getInstance.POS.UpdateAgent();
                    MainWindow.getInstance.RM.UpdateAgent();
                    MainWindow.getInstance.CDD.InitializeView();
                }

                List<HoldSales> hs = HoldSales.LoadJson();
                if (hs.Any(x => x.Agent == oldagent.Name))
                {
                    for (int i = 0; i < hs.Count; i++)
                    {
                        if (hs[i].Agent == oldagent.Name)
                        {
                            hs[i].Agent = newagent != null ? newagent.Name : "";
                        }
                    }
                    HoldSales.SaveJson(hs);
                }

                List<Sales> sales = new Sales().GetList();
                foreach (var sale in sales.Where(x => x.Agent == oldagent.Name))
                {
                    if (sale.Agent == oldagent.Name)
                    {
                        sale.Agent = newagent != null ? newagent.Name : "";
                        sale.SaveJson(sale.ConvertedSalesID);
                    }
                }
                MainWindow.getInstance.RM.InitializeView();
            }
            if (olddata is Customer)
            {
                var newcus = newdata as Customer;
                var oldcus = olddata as Customer;
                if (MainWindow.getInstance.POS.Customer != null && MainWindow.getInstance.POS.Customer.Vehicle == oldcus.Vehicle)
                {
                    MainWindow.getInstance.POS.Customer = newcus;
                    MainWindow.getInstance.POS.RefreshUI();
                }

                List<Appointment> apt = new Appointment().GetList();
                if (apt.Any((x) => x.Customer == oldcus.Vehicle))
                {
                    var recursive = apt.FindAll(
                        (x) => x.Customer == oldcus.Vehicle);
                    
                    foreach (Appointment aptobj in recursive)
                    {
                        if (newdata == null)
                        {
                            aptobj.DeleteJson(aptobj.Id.ToString());
                            UpdateData(aptobj, null);
                        }
                        if (newdata != null)
                        {
                            var temp = aptobj;
                            aptobj.Customer = newcus.Vehicle;
                            aptobj.SaveJson(aptobj.Id.ToString());
                            UpdateData(temp, aptobj);
                        }
                    }
                    MainWindow.getInstance.APDD.InitializeView();
                }

                List<HoldSales> hs = HoldSales.LoadJson();
                if (hs.Any((x) => x.Customer == oldcus.Vehicle))
                {
                    for (int i = 0; i < hs.Count; i++)
                    {
                        if (hs[i].Customer == oldcus.Vehicle)
                        {
                            hs[i].Customer = newdata != null ? newcus.Vehicle : "";
                        }
                    }
                    HoldSales.SaveJson(hs);
                }

                List<Sales> sales = new Sales().GetList();
                if (sales.Any((x) => x.Customer == oldcus.Vehicle))
                { 
                    var recursive = sales.First(
                        (x) => x.Customer == oldcus.Vehicle);
                    if (newdata == null)
                    {
                        recursive.Customer = "";
                        UpdateData(recursive, null);
                    }
                    if (newdata != null)
                    {
                        var temp = recursive;
                        recursive.Customer = newcus.Vehicle;
                        recursive.SaveJson(recursive.ConvertedSalesID);
                        UpdateData(temp, recursive);
                    }
                    MainWindow.getInstance.RM.InitializeView();
                }
            }
        }
    }
}
