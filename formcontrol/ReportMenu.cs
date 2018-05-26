using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Printing;
using CSharpOskaAPI.Printer;

namespace HaoProgram
{
    public partial class ReportMenu : UserControl, IUserAction<ActionEventArgs>
    {
        static string[] SortByList = { "Invoice ID", "Sales No", "Vehicle No", "Create Date" };
        public static string Prefix = "RM";

        public DataTable SDataTable { get; set; }
        public DataView SDataView { get; set; }

        public DataTable IDataTable { get; set; }
        public DataView IDataView { get; set; }

        public DataTable CDataTable { get; set; }
        public DataView CDataView { get; set; }

        public DataTable PDataTable { get; set; }
        public DataView PDataView { get; set; }

        public ReportMenu()
        {
            InitializeComponent();
            DoubleBuffered = true;

            tabControl1.ItemSize = new Size(tabControl1.Width / tabControl1.TabCount, 0);

            foreach (var str in SortByList)
            {
                SortBy.Items.Add(str);
            }
            SortBy.SelectedIndex = 0;
            InitializeView();
            YearBox.Text = DateTime.Now.Year.ToString();
            MonthBox.Text = DateTime.Now.Month >= 10 ? DateTime.Now.Month.ToString() : $"0{DateTime.Now.Month}";
            AgentYear.Text = DateTime.Now.Year.ToString();
            AgentMonth.Text = DateTime.Now.Month >= 10 ? DateTime.Now.Month.ToString() : $"0{DateTime.Now.Month}";
            UpdateAgent();
            DataManager.InitializeSKey(this, Prefix);
        }

        public void UpdateAgent()
        {
            Agent.Items.Clear();
            AgentSale.Items.Clear();
            Agent.Items.Add("");
            AgentSale.Items.Add("");
            foreach (var agent in new Agent().GetList())
            {
                Agent.Items.Add(agent.Name);
                AgentSale.Items.Add(agent.Name);
            }
        }


        public void InitializeView()
        {
            PDataTable = new DataTable();
            PDataTable.Columns.Add("Customer");
            PDataTable.Columns.Add("Contact");
            PDataTable.Columns.Add("Total");
            PDataTable.Columns.Add("Paid");
            PDataTable.Columns.Add("Due");
            PDataTable.Columns.Add("NRIC / SSM");
            PDataView = PDataTable.DefaultView;
            PaymentDGV.DataSource = PDataView;
            foreach (var cmr in new Customer().GetList())
            {
                SAccount sac = new SAccount(cmr.Name, cmr.NRIC);
                PDataTable.Rows.Add(
                    cmr.Name,
                    cmr.Contact,
                    sac.getTotal(),
                    sac.getPaid(),
                    sac.getOwe(),
                    cmr.NRIC);
            }

            CDataTable = new DataTable();
            CDataTable.Columns.Add("Agent");
            CDataTable.Columns.Add("Sales No");
            CDataTable.Columns.Add("Customer Name");
            CDataTable.Columns.Add("Date");
            CDataTable.Columns.Add("Commission");
            CDataView = CDataTable.DefaultView;
            AgentComDGV.DataSource = CDataView;
            foreach (var agent in new Agent().GetList())
            {
                foreach (var tuple in agent.ComissionGained)
                {
                    var sales = new Sales().LoadJson(tuple.Key);
                    CDataTable.Rows.Add(
                        agent.Name,
                        tuple.Key,
                        sales.Customer,
                        sales.CreateDate.ToString("yyyy/MM/dd"),
                        tuple.Value.ToString("0.00"));
                }
            }

            SDataTable = new DataTable();
            SDataTable.Columns.Add("Sales No");
            SDataTable.Columns.Add("Vehicle No");
            SDataTable.Columns.Add("Customer Name");
            SDataTable.Columns.Add("Create Date");
            SDataTable.Columns.Add("Agent");
            SDataTable.Columns.Add("Total");
            SDataTable.Columns.Add("Paid");
            SDataTable.Columns.Add("Owe");
            SDataView = SDataTable.DefaultView;
            SalesDataDGV.DataSource = SDataView;
            foreach (var sales in new Sales().GetList())
            {
                var customer = new Customer().LoadJson(sales.Customer);
                SDataTable.Rows.Add(
                       sales.ConvertedSalesID,
                       customer.Vehicle,
                       customer.Name,
                       sales.CreateDate.ToString("yyyy/MM/dd"),
                       sales.Agent,
                       sales.GetNeedToPay(),
                       sales.GetNeedToPay() - sales.GetUnpaid(),
                       sales.GetUnpaid());
            }
            SalesDataDGV.Sort(SalesDataDGV.Columns[0], ListSortDirection.Ascending);

            IDataTable = new DataTable();
            IDataTable.Columns.Add("Invoice ID");
            IDataTable.Columns.Add("Sales No");
            IDataTable.Columns.Add("Vehicle No");
            IDataTable.Columns.Add("Customer Name");
            IDataTable.Columns.Add("Create Date");
            IDataTable.Columns.Add("Agent");
            IDataTable.Columns.Add("Total");
            IDataView = IDataTable.DefaultView;
            InvoiceDGV.DataSource = IDataView;
            foreach (var invoice in new Invoice().GetList())
            {
                var sales = new Sales().LoadJson(invoice.Sales);
                var customer = new Customer().LoadJson(sales.Customer);
                IDataTable.Rows.Add(
                    invoice.ConvertedInvoiceID,
                    sales.ConvertedSalesID,
                    customer.Vehicle,
                    customer.Name,
                    sales.CreateDate,
                    sales.Agent,
                    sales.GetNeedToPay().ToString("0.00"));
            }
            InvoiceDGV.Sort(InvoiceDGV.Columns[0], ListSortDirection.Ascending);
        }

        protected override CreateParams CreateParams
        {
            get
            {
                var parms = base.CreateParams;
                parms.Style &= ~0x02000000;  // Turn off WS_CLIPCHILDREN
                return parms;
            }
        }

        public event EventHandler<ActionEventArgs> ActionEvent;

        private void CreateInvoice_Click(object sender, EventArgs e)
        {
            CreateInvoice ci = new CreateInvoice(null);
            ci.ActionEvent += (sender_a, at) =>
            {
                if (at.ActionString == "Accept")
                {
                    Invoice inv = at.ActionObject as Invoice;
                    if (DataManager.ID_Tracker.RemovedInvoiceID.Contains(inv.InvoiceID))
                    {
                        DataManager.ID_Tracker.RemovedInvoiceID.Remove(inv.InvoiceID);
                    }
                    else
                    {
                        while (inv.InvoiceID > DataManager.ID_Tracker.InvoiceID)
                        {
                            DataManager.ID_Tracker.RemovedInvoiceID.Add(DataManager.ID_Tracker.InvoiceID++);
                        }
                        DataManager.ID_Tracker.InvoiceID++;
                    }
                    inv.SaveJson(inv.ConvertedInvoiceID);

                    var sales = new Sales().LoadJson(inv.Sales);
                    var customer = new Customer().LoadJson(sales.Customer);
                    IDataTable.Rows.Add(
                        inv.ConvertedInvoiceID,
                        sales.ConvertedSalesID,
                        customer.Vehicle,
                        customer.Name,
                        sales.CreateDate,
                        sales.Agent,
                        sales.GetNeedToPay());
                }
            };
            ci.ShowDialog();
        }

        private void PrintInvoice_Click(object sender, EventArgs e)
        {
            if (InvoiceDGV.SelectedRows.Count == 1)
            {
                var invoiceid = InvoiceDGV.SelectedRows[0].Cells[0];
                var invoice = new Invoice().LoadJson(invoiceid.Value.ToString());
                PrintDocument pd = new PrintDocument();
                pd.PrintPage += (obj, ppe) =>
                {
                    buildPrintPage(invoice, ppe);
                };
                pd.DefaultPageSettings.PaperSize = new PaperSize("Invoice", Convert.ToInt32(216 * 3.936026936026936), Convert.ToInt32(279 * 3.936026936026936));
                PrintPreviewDialog pvd = new PrintPreviewDialog();
                pvd.Document = pd;
                ((Form)pvd).WindowState = FormWindowState.Maximized;
                pvd.PrintPreviewControl.Zoom = 1;
                pvd.ShowDialog();
            }
        }

        public void buildPrintPage(Invoice inv, PrintPageEventArgs e)
        {
            var sales = new Sales().LoadJson(inv.Sales);
            var customer = new Customer().LoadJson(sales.Customer);
            // Font List
            Font ab11 = new Font("Arial Black", 11, FontStyle.Bold);
            Font an10 = new Font("Arial Narrow", 10, FontStyle.Regular);
            Font ab10 = new Font("Arial Black", 10, FontStyle.Bold);
            Font ab9 = new Font("Arial Black", 9, FontStyle.Regular);
            Font a9 = new Font("Arial", 9, FontStyle.Bold);
            Font an9 = new Font("Arial Narrow", 9, FontStyle.Regular);
            Font anb9 = new Font("Arial Narrow", 9, FontStyle.Bold);
            // Brush ( Black aja )
            Brush brush = Brushes.Black;
            // Pen
            Pen pen = new Pen(brush);
            // Drawing
            e.Graphics.DrawString($"{DataManager.UserInfo.CompanyName}", ab11, brush, new Point(20, 30));
            e.Graphics.DrawString($"{DataManager.UserInfo.SSM}", an10, brush, new Point(20, 50));
            e.Graphics.DrawString($"{DataManager.UserInfo.Address}", an10, brush, new Point(20, 70));
            e.Graphics.DrawString($"Tel No : {DataManager.UserInfo.TelNo}", an10, brush, new Point(20, 100));
            e.Graphics.DrawString($"GST Reg No : {DataManager.UserInfo.GSTReg}", ab11, brush, new Point(20, 130));
            e.Graphics.DrawString("Sold to : ", ab10, brush, new Point(20, 160));
            e.Graphics.DrawString(customer.Name, a9, brush, new Point(20, 180));

            e.Graphics.DrawString("TAX INVOICE", ab11, brush, new Point(530, 150));
            e.Graphics.DrawString($"Invoice No : {inv.ConvertedInvoiceID}", ab9, brush, new Point(545, 185));
            e.Graphics.DrawString($"Date : {DateTime.Now.ToString("dd / MM / yyyy")}", ab9, brush, new Point(545, 205));

            e.Graphics.DrawLine(pen, new Point(520, 180), new Point(520, 280));
            e.Graphics.DrawString(customer.Vehicle, ab9, brush, new Point(20, 265));
            e.Graphics.DrawLine(pen, new Point(20, 285), new Point(800, 285));

            e.Graphics.DrawString("Description", ab9, brush, new Point(150, 285));
            e.Graphics.DrawString("Qty", ab9, brush, new Point(520, 285));

            e.Graphics.DrawString("U/Price", ab9, brush, new Point(580, 285));
            e.Graphics.DrawString("RM", ab9, brush, new Point(595, 305));

            e.Graphics.DrawString("Discount", ab9, brush, new Point(650, 285));
            e.Graphics.DrawString("RM", ab9, brush, new Point(665, 305));

            e.Graphics.DrawString("Amount", ab9, brush, new Point(730, 285));
            e.Graphics.DrawString("RM", ab9, brush, new Point(735, 305));

            e.Graphics.DrawLine(pen, new Point(20, 325), new Point(800, 325));

            int count = 1;
            foreach (var product in sales.Products)
            {
                if (product.UnitPrice - product.Discount != 0)
                {
                    e.Graphics.DrawString($"{count}      {product.Description}", an9, brush, new Point(40, 310 + (20 * count)));
                    e.Graphics.DrawString($"{product.Quantity}", an9, brush, new Point(530, 310 + (20 * count)));

                    var uprice = product.UnitPrice.ToString("#.00");
                    e.Graphics.DrawString($"{uprice}", an9, brush, new Point(600 - (uprice.Split('.')[0].Length * 5), 310 + (20 * count)));
                    var discount = product.Discount.ToString("#.00");
                    e.Graphics.DrawString($"{discount}", an9, brush, new Point(675 - (discount.Split('.')[0].Length * 5), 310 + (20 * count)));
                    var amount = ((product.UnitPrice - product.Discount) * product.Quantity).ToString("#.00");
                    e.Graphics.DrawString($"{amount}", an9, brush, new Point(750 - (amount.Split('.')[0].Length * 5), 310 + (20 * count)));
                    count++;
                }
            }
            e.Graphics.DrawLine(pen, new Point(20, 315 + (20 * count)), new Point(800, 315 + (20 * count)));

            e.Graphics.DrawString("Total Payable", ab9, brush, new Point(400, 320 + (20 * count)));
            var ntp = sales.GetNeedToPay().ToString("0.00");
            e.Graphics.DrawString($"{ntp}", an9, brush, new Point(750 - (ntp.Split('.')[0].Length * 5), 320 + (20 * count)));

            foreach (var spaid in sales.Paid)
            {
                if (spaid.Key.StartsWith("CHEQUE"))
                {
                    var cheque = spaid.Key.Split('|')[1];
                    e.Graphics.DrawString($"CHEQUE ( {cheque} )", ab9, brush, new Point(400, 340 + (20 * count)));
                }
                if (spaid.Key.StartsWith("CASH"))
                {
                    e.Graphics.DrawString("CASH", ab9, brush, new Point(400, 340 + (20 * count)));
                }
                var cash = sales.Paid[spaid.Key].ToString("0.00");
                e.Graphics.DrawString($"{cash}", an9, brush, new Point(750 - (cash.Split('.')[0].Length * 5), 340 + (20 * count)));
                count++;
            }
            count--;
            e.Graphics.DrawLine(pen, new Point(400, 365 + (20 * count)), new Point(800, 365 + (20 * count)));

            e.Graphics.DrawString("Total Amount Due", ab9, brush, new Point(400, 370 + (20 * count)));
            var due = sales.GetUnpaid().ToString("0.00");
            e.Graphics.DrawString($"{due}", an9, brush, new Point(750 - (due.Split('.')[0].Length * 5), 370 + (20 * count)));

            int footercounter = 1;
            foreach (var footer in DataManager.UserInfo.InvoiceFooter)
            {
                e.Graphics.DrawString($"{footer}", anb9, brush, new Point(30, 990 + (20 * footercounter++)));
            }

            e.Graphics.DrawString($"For {DataManager.UserInfo.CompanyName}", anb9, brush, new Point(600, 970));
            e.Graphics.DrawLine(pen, new Point(600, 1050), new Point(800, 1050));
        }

        private void RemoveInvoice_Click(object sender, EventArgs e)
        {
            if (InvoiceDGV.SelectedRows.Count == 1)
            {
                if (MessageBox.Show("Did you want to remove selected invoice?\n* Warning : After remove cant be recover without restore from backup", "Remove Invoice", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    var invoiceid = InvoiceDGV.SelectedRows[0].Cells[0];
                    Invoice inv = new Invoice().LoadJson(invoiceid.Value.ToString());
                    DataManager.ID_Tracker.RemovedInvoiceID.Add(inv.InvoiceID);
                    inv.DeleteJson(inv.ConvertedInvoiceID);

                    DataRow[] dr = IDataTable.Select($"[Invoice ID]='{inv.ConvertedInvoiceID}'");
                    dr[0].Delete();
                }
            }
        }

        private void ExitBtn_Click(object sender, EventArgs e)
        {
            ActionEvent(sender, new ActionEventArgs()
            {
                ActionString = "CloseCurrent"
            });
        }

        private void SortBox_TextChanged(object sender, EventArgs e)
        {
            IDataView.RowFilter = $"[{SortBy.Text}] LIKE '*{SortBox.Text}*'";
        }

        private void EditBtn_Click(object sender, EventArgs e)
        {
            if (InvoiceDGV.SelectedRows.Count == 1)
            {
                var invoiceid = InvoiceDGV.SelectedRows[0].Cells[0];
                var invoice = new Invoice().LoadJson(invoiceid.Value.ToString());
                CreateInvoice ci = new CreateInvoice(invoice);
                ci.ActionEvent += (sender_a, at) =>
                {
                    if (at.ActionString == "Accept")
                    {
                        invoice.DeleteJson(invoice.ConvertedInvoiceID);
                        DataRow[] dr = IDataTable.Select($"[Invoice ID]='{invoice.InvoiceID}'");
                        dr[0].Delete();

                        if (invoice.InvoiceID != (at.ActionObject as Invoice).InvoiceID)
                        {
                            if (DataManager.ID_Tracker.RemovedInvoiceID.Contains((at.ActionObject as Invoice).InvoiceID))
                            {
                                DataManager.ID_Tracker.RemovedInvoiceID.Remove((at.ActionObject as Invoice).InvoiceID);
                            }
                            DataManager.ID_Tracker.RemovedInvoiceID.Add(invoice.InvoiceID);
                        }
                        Invoice inv = at.ActionObject as Invoice;
                        var sales = new Sales().LoadJson(inv.ConvertedInvoiceID);
                        var customer = new Customer().LoadJson(sales.ConvertedSalesID);
                        IDataTable.Rows.Add(
                            inv.ConvertedInvoiceID,
                            sales.ConvertedSalesID,
                            customer.Vehicle,
                            customer.Name,
                            sales.CreateDate,
                            sales.Agent);
                        inv.SaveJson(inv.ConvertedInvoiceID);
                    }
                };
                ci.ShowDialog();
            }
        }

        private void SortText2_TextChanged(object sender, EventArgs e)
        {
            SDataView.RowFilter = $"[{SortBy.Text}] LIKE '*{SortBox.Text}*'";
        }

        private void AgentSale_SelectedIndexChanged(object sender, EventArgs e)
        {
            SDataView.RowFilter = $"[Create Date] LIKE '{YearBox.Text}/{MonthBox.Text}*' AND [Agent]='{AgentSale.Text}'";
        }

        private void YearBox_TextChanged(object sender, EventArgs e)
        {
            SDataView.RowFilter = $"[Create Date] LIKE '{YearBox.Text}/{MonthBox.Text}*' AND [Agent]='{AgentSale.Text}'";
        }

        private void MonthBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            SDataView.RowFilter = $"[Create Date] LIKE '{YearBox.Text}/{MonthBox.Text}*' AND [Agent]='{AgentSale.Text}'";
        }

        private void AgentYear_TextChanged(object sender, EventArgs e)
        {
            CDataView.RowFilter = $"[Agent]='{Agent.Text}' AND [Date] LIKE '{AgentYear.Text}/{AgentMonth.Text}*'";
        }

        private void AgentMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            CDataView.RowFilter = $"[Agent]='{Agent.Text}' AND [Date] LIKE '{AgentYear.Text}/{AgentMonth.Text}*'";
        }

        private void Agent_SelectedIndexChanged(object sender, EventArgs e)
        {
            CDataView.RowFilter = $"[Agent]='{Agent.Text}' AND [Date] LIKE '{AgentYear.Text}/{AgentMonth.Text}*'";
        }

        private void PrintAgent_Click(object sender, EventArgs e)
        {
            if (Agent.Text != "" && AgentYear.Text != "" && AgentMonth.Text != "")
            {
                var agent = new Agent().LoadJson(Agent.Text);
                PrintDocument pd = new PrintDocument();
                pd.PrintPage += (obj, ppe) =>
                {
                    buildAgentPrinting(agent, ppe);
                };
                PageSetupDialog psd = new PageSetupDialog();
                psd.Document = pd;
                psd.AllowMargins = false;
                if (psd.ShowDialog() == DialogResult.OK)
                {
                    pd.PrinterSettings = psd.PrinterSettings;
                    PrintPreviewDialog pvd = new PrintPreviewDialog();
                    pvd.Document = pd;
                    ((Form)pvd).WindowState = FormWindowState.Maximized;
                    pvd.PrintPreviewControl.Zoom = 1;
                    pvd.ShowDialog();
                }
            }
        }

        public void buildTotalAgent(PrintPageEventArgs e)
        {
            // Font List
            Font ab11 = new Font("Arial Black", 11, FontStyle.Bold);
            Font an10 = new Font("Arial Narrow", 10, FontStyle.Regular);
            Font ab10 = new Font("Arial Black", 10, FontStyle.Bold);
            Font ab9 = new Font("Arial Black", 9, FontStyle.Regular);
            Font an9 = new Font("Arial Narrow", 9, FontStyle.Regular);
            // Brush ( Black aja )
            Brush brush = Brushes.Black;
            // Pen
            Pen pen = new Pen(brush);
            // Drawing
            e.Graphics.DrawString($"{DataManager.UserInfo.CompanyName}", ab11, brush, new Point(20, 30));
            e.Graphics.DrawString($"All Agent Commission", ab11, brush, new Point(350, 30));
            e.Graphics.DrawString($"DateTime : {AgentMonth.Text} / {AgentYear.Text}", ab11, brush, new Point(600, 30));

            e.Graphics.DrawLine(pen, new Point(20, 60), new Point(800, 60));

            e.Graphics.DrawString("Agent", ab10, brush, new Point(80, 60));
            e.Graphics.DrawString("Contact", ab10, brush, new Point(300, 60));
            e.Graphics.DrawString("Commission ( RM )", ab10, brush, new Point(550, 60));

            e.Graphics.DrawLine(pen, new Point(20, 80), new Point(800, 80));
            List<Tuple<string, string, decimal>> agentlist = new List<Tuple<string, string, decimal>>();
            foreach (var agent in new Agent().GetList())
            {
                if (agent.ComissionGained.Count > 0)
                {
                    decimal total = 0;
                    foreach (var acom in agent.ComissionGained)
                    {
                        total += acom.Value;
                    }
                    agentlist.Add(Tuple.Create(agent.Name, agent.Contact, total));
                }
            }
            int count = 1;
            foreach (var tuple in agentlist)
            {
                e.Graphics.DrawString($"{count}", an9, brush, new Point(40, 70 + (20 * count)));
                e.Graphics.DrawString($"{tuple.Item1}", an9, brush, new Point(85, 70 + (20 * count)));
                e.Graphics.DrawString($"{tuple.Item2}", an9, brush, new Point(305, 70 + (20 * count)));
                var amount = tuple.Item3.ToString("0.00");
                e.Graphics.DrawString($"{amount}", an9, brush, new Point(600 - (amount.Split('.')[0].Length * 5), 70 + (20 * count)));
                count++;
            }
        }
        public void buildAgentPrinting(Agent agent, PrintPageEventArgs e)
        {
            // Font List
            Font ab11 = new Font("Arial Black", 11, FontStyle.Bold);
            Font an10 = new Font("Arial Narrow", 10, FontStyle.Regular);
            Font ab10 = new Font("Arial Black", 10, FontStyle.Bold);
            Font ab9 = new Font("Arial Black", 9, FontStyle.Regular);
            Font an9 = new Font("Arial Narrow", 9, FontStyle.Regular);
            // Brush ( Black aja )
            Brush brush = Brushes.Black;
            // Pen
            Pen pen = new Pen(brush);
            // Drawing
            e.Graphics.DrawString($"{DataManager.UserInfo.CompanyName}", ab11, brush, new Point(20, 30));
            e.Graphics.DrawString($"Agent : {agent.Name}", ab11, brush, new Point(450, 30));
            e.Graphics.DrawString($"DateTime : {AgentMonth.Text} / {AgentYear.Text}", ab11, brush, new Point(600, 30));

            e.Graphics.DrawLine(pen, new Point(20, 60), new Point(800, 60));

            e.Graphics.DrawString("Code", ab10, brush, new Point(50, 60));
            e.Graphics.DrawString("Description", ab10, brush, new Point(200, 60));
            e.Graphics.DrawString("Date", ab10, brush, new Point(450, 60));
            e.Graphics.DrawString("Commission ( RM )", ab10, brush, new Point(600, 60));

            e.Graphics.DrawLine(pen, new Point(20, 80), new Point(800, 80));

            int count = 1;
            decimal total = 0;
            List<Tuple<Product, string, decimal>> products = new List<Tuple<Product, string, decimal>>();
            foreach (var acom in agent.ComissionGained)
            {
                var sales = new Sales().LoadJson(acom.Key);
                var agentcom = new AgentCommission().LoadJson(agent.AgentCommission);
                foreach (var product in sales.Products)
                {
                    foreach (var acp in agentcom.Product)
                    {
                        if (acp.Item1.Code == product.Code)
                        {
                            if (acp.Item3 == CommissionType.INTEGER)
                            {
                                products.Add(Tuple.Create(acp.Item1, sales.CreateDate.ToString("yyyy / MM /dd"), acp.Item2));
                            }
                            if (acp.Item3 == CommissionType.PERCENTAGE)
                            {
                                products.Add(Tuple.Create(acp.Item1, sales.CreateDate.ToString("yyyy / MM /dd"), ((product.UnitPrice - product.Discount) * product.Quantity) * (acp.Item2 / 100)));
                            }
                            break;
                        }
                    }
                }
                total += acom.Value;
            }
            foreach (var tuple in products)
            {
                e.Graphics.DrawString($"{tuple.Item1.ConvertedProductCode}", an9, brush, new Point(50, 70 + (20 * count)));
                e.Graphics.DrawString($"{tuple.Item1.Description}", an9, brush, new Point(205, 70 + (20 * count)));
                e.Graphics.DrawString($"{tuple.Item2}", an9, brush, new Point(440, 70 + (20 * count)));
                var amount = tuple.Item3.ToString("0.00");
                e.Graphics.DrawString($"{amount}", an9, brush, new Point(650 - (amount.Split('.')[0].Length * 5), 70 + (20 * count)));
                count++;
            }

            e.Graphics.DrawLine(pen, new Point(20, 80 + (20 * count)), new Point(800, 80 + (20 * count)));
            var totalstring = total.ToString("0.00");
            e.Graphics.DrawString($"Total Commission : RM {totalstring}", ab9, brush, new Point(560 - (totalstring.Split('.')[0].Length * 5), 85 + (20 * count)));
        }

        private void PrintTotal_Click(object sender, EventArgs e)
        {
            PrintDocument pd = new PrintDocument();
            pd.PrintPage += (obj, ppe) =>
            {
                buildTotalAgent(ppe);
            };
            PageSetupDialog psd = new PageSetupDialog();
            psd.Document = pd;
            psd.AllowMargins = false;
            if (psd.ShowDialog() == DialogResult.OK)
            {
                pd.PrinterSettings = psd.PrinterSettings;
                PrintPreviewDialog pvd = new PrintPreviewDialog();
                pvd.Document = pd;
                ((Form)pvd).WindowState = FormWindowState.Maximized;
                pvd.PrintPreviewControl.Zoom = 1;
                pvd.ShowDialog();
            }
        }

        private void OnSortVehicle(object sender, EventArgs e)
        {
            PDataView.RowFilter = $"[Customer] LIKE '*{Customer.Text}*'";
        }

        private void PrintOwe_Click(object sender, EventArgs e)
        {
            SelectCustomer ss = new SelectCustomer();
            ss.ActionEvent += (sender_a, at2) =>
            {
                if (at2.ActionString == "Accept")
                {
                    PrintDocument pd = new PrintDocument();
                    pd.PrintPage += (obj, ppe) =>
                    {
                        buildOwePrinting(at2.ActionObject as Customer, ppe);
                    };
                    pd.EndPrint += delegate
                    {
                        if (pd.PrintController.IsPreview) return;
                        DataManager.ID_Tracker.ReceiptNumber++;
                    };
                    pd.DefaultPageSettings.PaperSize = new PaperSize("Invoice", Convert.ToInt32(216 * 3.936026936026936), Convert.ToInt32(279 * 3.936026936026936));
                    PrintPreviewDialog pvd = new PrintPreviewDialog();
                    pvd.Document = pd;
                    ((Form)pvd).WindowState = FormWindowState.Maximized;
                    pvd.PrintPreviewControl.Zoom = 1;
                    pvd.ShowDialog();
                }
            };
            ss.ShowDialog();
        }

        public void buildOwePrinting(Customer customer, PrintPageEventArgs e)
        {
            var sales = new Sales().GetList().Where((x) => x.GetCustomerName() == customer.Name && x.GetCustomerIC() == customer.NRIC);
            // Font List
            Font ab11 = new Font("Arial Black", 11, FontStyle.Bold);
            Font an10 = new Font("Arial Narrow", 10, FontStyle.Regular);
            Font ab10 = new Font("Arial Black", 10, FontStyle.Bold);
            Font ab9 = new Font("Arial Black", 9, FontStyle.Regular);
            Font a9 = new Font("Arial", 9, FontStyle.Bold);
            Font an9 = new Font("Arial Narrow", 9, FontStyle.Regular);
            Font anb9 = new Font("Arial Narrow", 9, FontStyle.Bold);
            // Brush ( Black aja )
            Brush brush = Brushes.Black;
            // Pen
            Pen pen = new Pen(brush);
            // Drawing
            e.Graphics.DrawString($"{DataManager.UserInfo.CompanyName}", ab11, brush, new Point(20, 30));
            e.Graphics.DrawString($"{DataManager.UserInfo.SSM}", an10, brush, new Point(20, 50));
            e.Graphics.DrawString($"{DataManager.UserInfo.Address}", an10, brush, new Point(20, 70));
            e.Graphics.DrawString($"Tel No : {DataManager.UserInfo.TelNo}", an10, brush, new Point(20, 100));
            e.Graphics.DrawString($"GST Reg No : {DataManager.UserInfo.GSTReg}", ab11, brush, new Point(20, 130));
            e.Graphics.DrawString("Sold to : ", ab10, brush, new Point(20, 160));
            e.Graphics.DrawString(customer.Name, a9, brush, new Point(20, 180));

            e.Graphics.DrawString("Official Receipt", ab11, brush, new Point(530, 150));
            e.Graphics.DrawString($"Receipt No : {DataManager.ID_Tracker.ReceiptNumber}", ab9, brush, new Point(545, 185));
            e.Graphics.DrawString($"Date : {DateTime.Now.ToString("dd / MM / yyyy")}", ab9, brush, new Point(545, 205));

            e.Graphics.DrawLine(pen, new Point(520, 180), new Point(520, 280));
            e.Graphics.DrawLine(pen, new Point(20, 285), new Point(800, 285));

            e.Graphics.DrawString("REFERENCE", ab9, brush, new Point(40, 285));
            e.Graphics.DrawString("DATE", ab9, brush, new Point(270, 285));
            e.Graphics.DrawString("AMOUNT ( RM )", ab9, brush, new Point(500, 285));

            e.Graphics.DrawLine(pen, new Point(20, 325), new Point(800, 325));

            int count = 1;
            List<Invoice> invoices = new Invoice().GetList();
            foreach (var sale in sales)
            {
                if (invoices.Any((x) => x.Sales == sale.ConvertedSalesID))
                {
                    var invoice = invoices.First((x) => x.Sales == sale.ConvertedSalesID);
                    e.Graphics.DrawString($"{invoice.ConvertedInvoiceID}", an9, brush, new Point(40, 310 + (20 * count)));
                    e.Graphics.DrawString($"{sale.CreateDate.ToString("dd / MM / yyyy")}", an9, brush, new Point(250, 310 + (20 * count)));
                    var amount = sale.GetNeedToPay().ToString("0.00");
                    e.Graphics.DrawString($"{amount}", an9, brush, new Point(530 - (amount.Split('.')[0].Length * 5), 310 + (20 * count)));
                }
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            foreach (var sk in DataManager.UserInfo.ShortKeyData.Where((x) => x.Key.StartsWith(Prefix)))
            {
                if (sk.Value == keyData.ToString())
                {
                    (Controls.Find(sk.Key.Split('.')[1], true)[0] as Button).PerformClick();
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
