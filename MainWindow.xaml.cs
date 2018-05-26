using CSharpOskaAPI.GoogleApis;
using CSharpOskaAPI.Parse;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HaoProgram
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public POSRequest POS { get; set; }
        public ProductDataDisplay PDD { get; set; }
        public SettingMenu SM { get; set; }
        public ReportMenu RM { get; set; }
        public CustomerDataDisplay CDD { get; set; }
        public AppointmentDataDisplay APDD { get; set; }
        public AgentDataDisplay ADD { get; set; }
        public AgentComissionDisplay ACD { get; set; }

        public static MainWindow getInstance { get; set; }
        public static NotifyIcon getNotify { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            getInstance = this;

            new DataManager();
            InitializeUI();
            
            getNotify = buildTrayIcon();
            WinFormHost.ChildChanged += delegate
            {
                GC.Collect();
            };
            
            List<Appointment> apts = new Appointment().GetList();
            if (apts.Any((x) => x.RemindDate.ToString("dd/MM/yyyy") == DateTime.Now.ToString("dd/MM/yyyy")))
            {
                foreach (var apt in apts.Where((x) => x.RemindDate.ToString("dd/MM/yyyy") == DateTime.Now.ToString("dd/MM/yyyy") && x.RemindDate.ToString("dd/MM/yyyy") != x.Reminder.ToString("dd/MM/yyyy")))
                {
                    apt.RemindDate = apt.RemindDate.AddDays(1);
                    apt.SaveJson(apt.Id.ToString());
                }

                getNotify.BalloonTipText = $"You have appointment in three days coming soon!";
                getNotify.BalloonTipTitle = $"Click for view appointment details.";
                getNotify.Visible = true;
                getNotify.BalloonTipIcon = ToolTipIcon.Info;
                getNotify.BalloonTipClicked += delegate
                {
                    WinFormHost.Child = APDD;
                    APDD.Focus();
                    APDD.tabControl1.SelectedIndex = 1;
                    APDD.YearBox.Text = DateTime.Now.Year.ToString();
                    APDD.MonthBox.Text = DateTime.Now.Month >= 10 ? DateTime.Now.Month.ToString() : $"0{DateTime.Now.Month}";
                };
                getNotify.ShowBalloonTip(1000);
            }
        }

        public void InitializeUI()
        {
            SM = new SettingMenu();

            RM = new ReportMenu();
            RM.ActionEvent += (sender_a, at) =>
            {
                if (at.ActionString == "CloseCurrent")
                {
                    WinFormHost.Child = null;
                }
            };

            POS = new POSRequest();
            POS.ActionEvent += (sender_a, at) =>
            {
                if (at.ActionString == "AskInvoice")
                {
                    if (System.Windows.Forms.MessageBox.Show("Did you want generate invoice for sales?", "Generate Invoice", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                    {
                        Sales sales = at.ActionObject as Sales;
                        Invoice inv = new Invoice() { Sales = sales.ConvertedSalesID };
                        inv.InvoiceID = Invoice.getCurrentId(true);

                        var c = new Customer().LoadJson(sales.Customer);
                        RM.IDataTable.Rows.Add(
                            inv.ConvertedInvoiceID,
                            sales.ConvertedSalesID,
                            c.Vehicle,
                            c.Name,
                            sales.CreateDate,
                            sales.Agent,
                            sales.GetNeedToPay());
                        RM.SortBy.SelectedIndex = 0;
                        RM.SortBox.Text = inv.ConvertedInvoiceID;
                        WinFormHost.Child = RM;
                        RM.Focus();
                        RM.tabControl1.SelectedIndex = 0;
                        inv.SaveJson(inv.ConvertedInvoiceID);
                    }
                }
            };

            ACD = new AgentComissionDisplay();
            ACD.ActionEvent += (sender_a, at) =>
            {
                if (at.ActionString == "OpenRequest")
                {
                    AgentComissionRequest acr = new AgentComissionRequest();
                    acr.ActionEvent += (sender_b, at2) =>
                    {
                        if (at2.ActionString == "ExitRequest")
                        {
                            acr.Close();
                        }
                        else if (at2.ActionString == "ExistingCom")
                        {
                            acr.Close();
                        }
                        else if (at2.ActionString == "NewCom")
                        {
                            AgentCommission ac = at2.ActionObject as AgentCommission;
                            ac.SaveJson(ac.CategoryName);
                            ACD.RefreshComponent();
                            ADD.UpdateAgentCom();
                            acr.Close();
                        }
                    };
                    acr.Show();
                }
                if (at.ActionString == "CloseCurrent")
                {
                    WinFormHost.Child = null;
                }
            };

            PDD = new ProductDataDisplay();
            PDD.ActionEvent += (sender_a, at) =>
            {
                if (at.ActionString == "OpenRequest")
                {
                    ProductDataRequest pdr = new ProductDataRequest();
                    pdr.ActionEvent += (sender_b, at2) =>
                    {
                        if (at2.ActionString == "ExitRequest")
                        {
                            pdr.Close();
                        }
                        else if (at2.ActionString == "ExistingProduct")
                        {
                            pdr.Close();
                            PDD.SortBy.SelectedIndex = 0;
                            PDD.SortBox.Text = $"{(at2.ActionObject as Product).ConvertedProductCode}";
                        }
                        else if (at2.ActionString == "NewProduct")
                        {
                            Product pdt = at2.ActionObject as Product;
                            pdt.SaveJson(pdt.ConvertedProductCode);
                            pdr.Close();

                            if (DataManager.ID_Tracker.RemovedProductID.Contains(pdt.Code))
                            {
                                DataManager.ID_Tracker.RemovedProductID.Remove(pdt.Code);
                            }
                            else
                            {
                                while (pdt.Code > DataManager.ID_Tracker.ProductID)
                                {
                                    DataManager.ID_Tracker.RemovedProductID.Add(DataManager.ID_Tracker.ProductID++);
                                }
                                DataManager.ID_Tracker.ProductID++;
                            }

                            PDD.SortBy.SelectedIndex = 0;
                            PDD.SortBox.Text = $"{pdt.ConvertedProductCode}";
                            PDD.DataTable.Rows.Add(
                                ObjectParse.ObjectParseString(pdt.ConvertedProductCode),
                                ObjectParse.ObjectParseString(pdt.Description),
                                "RM " + ObjectParse.ObjectParseString(pdt.UnitPrice));
                        }
                    };
                    pdr.Show();
                }
                if (at.ActionString == "CloseCurrent")
                {
                    WinFormHost.Child = null;
                }
            };

            APDD = new AppointmentDataDisplay();
            APDD.ActionEvent += (sender_a, at) =>
            {
                if (at.ActionString == "CloseCurrent")
                {
                    WinFormHost.Child = null;
                }
                if (at.ActionString == "OpenRequest")
                {
                    AppointmentDataRequest adr = new AppointmentDataRequest();
                    adr.ActionEvent += (sender_b, at2) =>
                    {
                        if (at2.ActionString == "Save")
                        {
                            Appointment apt = at2.ActionObject as Appointment;
                            adr.Close();

                            List<string> shortname = new List<string>();
                            foreach (var shortn in apt.Checked)
                            {
                                shortname.Add(DataManager.AppointmentTypes[shortn]);
                            }

                            var customer = new Customer().LoadJson(apt.Customer);

                            APDD.DataTable.Rows.Add(
                                apt.Id,
                                apt.Reminder.ToString("yyyy/MM/dd"),
                                apt.Customer,
                                string.Join(" , ", shortname),
                                customer.Name,
                                customer.Contact,
                                ObjectParse.ArrayParseString(apt.Remarks));
                            apt.SaveJson(apt.Id.ToString());
                        }
                        if (at2.ActionString == "StartScan")
                        {
                            Appointment app = at2.ActionObject as Appointment;

                            List<string> shortname = new List<string>();
                            foreach (var shortn in app.Checked)
                            {
                                shortname.Add(DataManager.AppointmentTypes[shortn]);
                            }

                            var customer = new Customer().LoadJson(app.Customer);

                            APDD.DataTable.Rows.Add(
                                app.Id,
                                app.Reminder.ToString("yyyy/MM/dd"),
                                app.Customer,
                                string.Join(" , ", shortname),
                                customer.Name,
                                customer.Contact,
                                ObjectParse.ArrayParseString(app.Remarks));
                            app.SaveJson(app.Id.ToString());

                            AppointmentScanRequest asr = new AppointmentScanRequest(customer);
                            asr.ActionEvent += (sender_c, at3) =>
                            {
                                if (at3.ActionString == "Cancel")
                                {
                                    asr.Close();
                                }
                                if (at3.ActionString == "Accept")
                                {
                                    Customer cmri = new Customer().LoadJson(app.Customer);
                                    cmri = at3.ActionObject as Customer;
                                    asr.Close();
                                    cmri.SaveJson(cmri.Vehicle);
                                }
                            };
                            adr.Close();
                            asr.Show();
                        }
                        if (at2.ActionString == "ExitRequest")
                        {
                            adr.Close();
                        }
                    };
                    adr.Show();
                }
                if (at.ActionString == "ViewScan")
                {
                    Appointment app = at.ActionObject as Appointment;
                    Customer cmr = new Customer().LoadJson(app.Customer);
                    AppointmentScanRequest asr = new AppointmentScanRequest(cmr);
                    asr.ActionEvent += (sender_c, at3) =>
                    {
                        if (at3.ActionString == "Cancel")
                        {
                            asr.Close();
                        }
                        if (at3.ActionString == "Accept")
                        {
                            Customer cmri = new Customer().LoadJson(app.Customer);
                            cmri = at3.ActionObject as Customer;
                            asr.Close();
                            cmri.SaveJson(cmri.Vehicle);
                        }
                    };
                    asr.Show();
                }
            };

            ADD = new AgentDataDisplay();
            ADD.ActionEvent += (sender_a, at) =>
            {
                if (at.ActionString == "OpenRequest")
                {
                    AgentDataRequest adr = new AgentDataRequest();
                    adr.ActionEvent += (sender_b, at2) =>
                    {
                        if (at2.ActionString == "ExitRequest")
                        {
                            adr.Close();
                            ADD.RefreshUI(null);
                        }
                        else if (at2.ActionString == "ExistingAgent")
                        {
                            adr.Close();
                            ADD.RefreshUI(at2.ActionObject as Agent);
                        }
                        else if (at2.ActionString == "NewAgent")
                        {
                            Agent agt = at2.ActionObject as Agent;
                            agt.SaveJson(agt.Name);
                            adr.Close();
                            CDD.UpdateAgent();
                            POS.UpdateAgent();
                            RM.UpdateAgent();
                            ADD.RefreshUI(at2.ActionObject as Agent);
                        }
                    };
                    adr.Show();
                }
                if (at.ActionString == "CloseCurrent")
                {
                    WinFormHost.Child = null;
                }
            };

            CDD = new CustomerDataDisplay();
            CDD.ActionEvent += (sender_a, at) =>
            {
                string action = at.ActionString;
                if (action == "OpenRequest")
                {
                    CustomerDataRequest cdr = new CustomerDataRequest();
                    cdr.ActionEvent += (sender_b, at2) =>
                    {
                        if (at2.ActionString == "ExitRequest")
                        {
                            cdr.Close();
                        }
                        else if (at2.ActionString == "ExistingCustomer")
                        {
                            cdr.Close();
                            CDD.SortBy.SelectedIndex = 0;
                            CDD.SortBox.Text = $"{(at2.ActionObject as Customer).Vehicle}";
                        }
                        else if (at2.ActionString == "NewCustomer")
                        {
                            Customer cmr = at2.ActionObject as Customer;
                            cmr.SaveJson(cmr.Vehicle);
                            cdr.Close();
                            CDD.SortBy.SelectedIndex = 0;
                            CDD.SortBox.Text = $"{cmr.Vehicle}";
                            CDD.DataTable.Rows.Add(
                                cmr.Vehicle,
                                cmr.Name,
                                cmr.NRIC,
                                ObjectParse.ArrayParseString(cmr.Address.ToArray()),
                                cmr.Contact,
                                cmr.Agent);
                        }
                    };
                    cdr.Show();
                }
                if (action == "ViewScan")
                {
                    Customer cmr = at.ActionObject as Customer;
                    AppointmentScanRequest asr = new AppointmentScanRequest(cmr);
                    asr.ActionEvent += (sender_c, at3) =>
                    {
                        if (at3.ActionString == "Cancel")
                        {
                            asr.Close();
                        }
                        if (at3.ActionString == "Accept")
                        {
                            cmr = at3.ActionObject as Customer;
                            cmr.SaveJson(cmr.Vehicle);
                            asr.Close();
                        }
                    };
                    asr.Show();
                }
                if (action == "CloseCurrent")
                {
                    WinFormHost.Child = null;
                }
            };
        }
        #region MainUI Element
        private void HalfFlow(object sender, RoutedEventArgs e)
        {
            SelectCustomer sc = new SelectCustomer();
            sc.ActionEvent += (sender_a, at) =>
            {
                if (at.ActionString == "Accept")
                {
                    var cmr = at.ActionObject as Customer;
                    // Flow 1
                    AppointmentDataRequest adr = new AppointmentDataRequest();
                    adr.VehicleNumber.Text = cmr.Vehicle;
                    adr.ActionEvent += (sender_c, at3) =>
                    {
                        if (at3.ActionString == "Save")
                        {
                            Appointment apt = at3.ActionObject as Appointment;
                            apt.SaveJson(apt.Id.ToString());
                            adr.Close();

                            List<string> shortname = new List<string>();
                            foreach (var shortn in apt.Checked)
                            {
                                shortname.Add(DataManager.AppointmentTypes[shortn]);
                            }

                            var customer = new Customer().LoadJson(apt.Customer);
                            APDD.DataTable.Rows.Add(
                                apt.Id,
                                apt.Reminder.ToString("yyyy/MM/dd"),
                                apt.Customer,
                                string.Join(" , ", shortname),
                                customer.Name,
                                customer.Contact,
                                ObjectParse.ArrayParseString(apt.Remarks));
                            // FLOW 3
                            WinFormHost.Child = POS;
                            POS.Focus();
                            POS.Customer = cmr;
                            POS.RefreshUI();
                        }
                        if (at3.ActionString == "StartScan")
                        {
                            Appointment app = at3.ActionObject as Appointment;
                            app.SaveJson(app.Id.ToString());
                            List<string> shortname = new List<string>();
                            foreach (var shortn in app.Checked)
                            {
                                shortname.Add(DataManager.AppointmentTypes[shortn]);
                            }

                            var customer = new Customer().LoadJson(app.Customer);
                            APDD.DataTable.Rows.Add(
                                app.Id,
                                app.Reminder.ToString("yyyy/MM/dd"),
                                app.Customer,
                                string.Join(" , ", shortname),
                                customer.Name,
                                customer.Contact,
                                ObjectParse.ArrayParseString(app.Remarks));
                            adr.Close();
                            // FLOW 2
                            AppointmentScanRequest asr = new AppointmentScanRequest(customer);
                            asr.ActionEvent += (sender_d, at4) =>
                            {
                                if (at4.ActionString == "Accept")
                                {
                                    customer = at4.ActionObject as Customer;
                                    customer.SaveJson(customer.Vehicle);
                                    asr.Close();
                                    // FLOW 3
                                    WinFormHost.Child = POS;
                                    POS.Focus();
                                    POS.ResetData();
                                    POS.Customer = cmr;
                                    POS.RefreshUI();
                                }
                            };
                            asr.Show();
                        }
                        if (at3.ActionString == "ExitRequest")
                        {
                            adr.Close();
                        }
                    };
                    adr.Show();
                }
            };
            sc.Show();
        }
        private void FullFlow(object sender, RoutedEventArgs e)
        {
            CustomerDataRequest cdr = new CustomerDataRequest();
            cdr.ActionEvent += (sender_b, at2) =>
            {
                if (at2.ActionString == "ExitRequest")
                {
                    cdr.Close();
                }
                else if (at2.ActionString == "ExistingCustomer")
                {
                    cdr.Close();
                    CDD.SortBy.SelectedIndex = 0;
                    CDD.SortBox.Text = $"{(at2.ActionObject as Customer).Vehicle}";
                }
                else if (at2.ActionString == "NewCustomer")
                {
                    Customer cmr = at2.ActionObject as Customer;
                    cmr.SaveJson(cmr.Vehicle);
                    CDD.SortBy.SelectedIndex = 0;
                    CDD.SortBox.Text = $"{cmr.Vehicle}";
                    CDD.DataTable.Rows.Add(
                        cmr.Vehicle,
                        cmr.Name,
                        cmr.NRIC,
                        ObjectParse.ArrayParseString(cmr.Address.ToArray()),
                        cmr.Contact,
                        cmr.Agent);
                    cdr.Close();
                    // FLOW 1
                    AppointmentDataRequest adr = new AppointmentDataRequest();
                    adr.VehicleNumber.Text = cmr.Vehicle;
                    adr.ActionEvent += (sender_c, at3) =>
                    {
                        if (at3.ActionString == "Save")
                        {
                            Appointment apt = at3.ActionObject as Appointment;
                            apt.SaveJson(apt.Id.ToString());
                            adr.Close();

                            List<string> shortname = new List<string>();
                            foreach (var shortn in apt.Checked)
                            {
                                shortname.Add(DataManager.AppointmentTypes[shortn]);
                            }

                            var customer = new Customer().LoadJson(apt.Customer);
                            APDD.DataTable.Rows.Add(
                                apt.Id,
                                apt.Reminder.ToString("yyyy/MM/dd"),
                                apt.Customer,
                                string.Join(" , ", shortname),
                                customer.Name,
                                customer.Contact,
                                ObjectParse.ArrayParseString(apt.Remarks));
                            // FLOW 3
                            WinFormHost.Child = POS;
                            POS.Focus();
                            POS.Customer = cmr;
                            POS.RefreshUI();
                        }
                        if (at3.ActionString == "StartScan")
                        {
                            Appointment app = at3.ActionObject as Appointment;
                            app.SaveJson(app.Id.ToString());

                            List<string> shortname = new List<string>();
                            foreach (var shortn in app.Checked)
                            {
                                shortname.Add(DataManager.AppointmentTypes[shortn]);
                            }

                            var customer = new Customer().LoadJson(app.Customer);
                            APDD.DataTable.Rows.Add(
                                app.Id,
                                app.Reminder.ToString("yyyy/MM/dd"),
                                app.Customer,
                                string.Join(" , ", shortname),
                                customer.Name,
                                customer.Contact,
                                ObjectParse.ArrayParseString(app.Remarks));
                            adr.Close();
                            // FLOW 2
                            AppointmentScanRequest asr = new AppointmentScanRequest(customer);
                            asr.ActionEvent += (sender_d, at4) =>
                            {
                                if (at4.ActionString == "Accept")
                                {
                                    customer = at4.ActionObject as Customer;
                                    customer.SaveJson(customer.Vehicle);
                                    asr.Close();
                                    // FLOW 3
                                    WinFormHost.Child = POS;
                                    POS.Focus();
                                    POS.ResetData();
                                    POS.Customer = cmr;
                                    POS.RefreshUI();
                                }
                            };
                            asr.Show();
                        }
                        if (at3.ActionString == "ExitRequest")
                        {
                            adr.Close();
                        }
                    };
                    adr.Show();
                }
            };
            cdr.Show();
        }
        private void SettingMenu(object sender, RoutedEventArgs e)
        {
            WinFormHost.Child = SM;
            SM.Focus();
        }
        private void ReportMenu(object sender, RoutedEventArgs e)
        {
            WinFormHost.Child = RM;
            RM.Focus();
        }
        private void POSMenu(object sender, RoutedEventArgs e)
        {
            WinFormHost.Child = POS;
            POS.Focus();
        }
        private void AgentComissionMenu(object sender, RoutedEventArgs e)
        {
            WinFormHost.Child = ACD;
            ACD.Focus();
        }
        private void ProductMenu(object sender, RoutedEventArgs e)
        {
            WinFormHost.Child = PDD;
            PDD.Focus();
        }

        private void AppointmentMenu(object sender, RoutedEventArgs e)
        {
            WinFormHost.Child = APDD;
            APDD.Focus();
        }
        private void AgentMenu(object sender, RoutedEventArgs e)
        {
            WinFormHost.Child = ADD;
            ADD.Focus();
        }
        private void CustomerMenu(object sender, RoutedEventArgs e)
        {
            WinFormHost.Child = CDD;
            CDD.Focus();
        }
        #endregion MainUI Element

        #region TrayIcon
        public static NotifyIcon buildTrayIcon()
        {
            NotifyIcon tray = new NotifyIcon();
            tray.Icon = new System.Drawing.Icon("assets/product.ico");
            tray.Visible = true;
            tray.DoubleClick += (s, e) =>
            {
                getInstance.Show();
                getInstance.WindowState = WindowState.Normal;
            };

            ContextMenuStrip cms = new ContextMenuStrip();

            ToolStripMenuItem tsmi = new ToolStripMenuItem("Show")
            {
                DisplayStyle = ToolStripItemDisplayStyle.ImageAndText,
                ShortcutKeys = Keys.Control | Keys.S,
                ShowShortcutKeys = true,
                Image = System.Drawing.Image.FromFile("assets/cfc_logo_small.png")
            };
            tsmi.Click += delegate
            {
                getInstance.Show();
                getInstance.WindowState = WindowState.Normal;
            };
            cms.Items.Add(tsmi);


            tsmi = new ToolStripMenuItem("Minimize")
            {
                DisplayStyle = ToolStripItemDisplayStyle.ImageAndText,
                ShortcutKeys = Keys.Control | Keys.M,
                ShowShortcutKeys = true,
                Image = System.Drawing.Image.FromFile("assets/hold.png")
            };
            tsmi.Click += delegate
            {
                getInstance.WindowState = WindowState.Minimized;
                getInstance.Hide();
            };
            cms.Items.Add(tsmi);

            cms.Items.Add(new ToolStripSeparator());

            tsmi = new ToolStripMenuItem("Exit")
            {
                DisplayStyle = ToolStripItemDisplayStyle.ImageAndText,
                ShortcutKeys = Keys.Control | Keys.X,
                ShowShortcutKeys = true,
                Image = System.Drawing.Image.FromFile("assets/exit.png")
            };
            tsmi.Click += delegate
            {
                getInstance.Close();
            };
            cms.Items.Add(tsmi);

            tray.ContextMenuStrip = cms;
            return tray;
        }
        #endregion TrayIcon
    }
}
