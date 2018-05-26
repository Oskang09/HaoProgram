using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CSharpOskaAPI.Parse;

namespace HaoProgram
{
    public partial class POSRequest : UserControl, IUserAction<ActionEventArgs>
    {
        public List<Product> content = new List<Product>();
        public Customer Customer { get; set; }
        public int? PendingNum { get; set; }
        public string Mode { get; set; }
        public int? SalesID { get; set; }
        public bool isRepeat { get; set; }
        public static string Prefix = "POS";

        protected override CreateParams CreateParams
        {
            get
            {
                var parms = base.CreateParams;
                parms.Style &= ~0x02000000;  // Turn off WS_CLIPCHILDREN
                return parms;
            }
        }

        public void UpdateAgent()
        {
            Agent.Items.Clear();
            foreach (var agent in new Agent().GetList())
            {
                Agent.Items.Add(agent.Name);
            }
        }
        public POSRequest()
        {
            InitializeComponent();
            DoubleBuffered = true;
            Mode = "Empty";
            UpdateAgent();
            ID.Text = Sales.Format + Sales.getCurrentId(false);
            DataManager.InitializeSKey(this, Prefix);
        }

        public event EventHandler<ActionEventArgs> ActionEvent;

        private void EditCustomer_Click(object sender, EventArgs e)
        {
            SelectCustomer sc = new SelectCustomer();
            sc.ActionEvent += (sender_a, at) =>
            {
                if (at.ActionString == "Accept")
                {
                    Customer = at.ActionObject as Customer;
                    RefreshUI();
                }
            };
            sc.ShowDialog();
        }

        private void AddProduct_Click(object sender, EventArgs e)
        {
            SelectProduct sp = new SelectProduct();
            sp.ActionEvent += (sender_a, at) =>
            {
                if (at.ActionString == "Accept")
                {
                    content.Add(at.ActionObject as Product);
                    RefreshUI();
                }
            };
            sp.ShowDialog();
        }

        private void RemoveBtn_Click(object sender, EventArgs e)
        {
            if (POSProductDGV.SelectedRows.Count == 1)
            {
                var count = POSProductDGV.SelectedRows[0].Cells[0];
                content.RemoveAt(int.Parse(count.Value.ToString()) - 1);

                List<Product> temp = new List<Product>();
                foreach (var pdt in content)
                {
                    temp.Add(pdt);
                }

                content = temp;
                RefreshUI();
            }
        }

        public void RefreshUI()
        {
            while (POSProductDGV.CurrentRow != null)
            {
                POSProductDGV.Rows.Remove(POSProductDGV.CurrentRow);
            }
            int count = 1;
            decimal total = 0;
            foreach (var pdt in content)
            {
                POSProductDGV.Rows.Add(
                    count++,
                    pdt.Description,
                    pdt.Quantity,
                    "RM " + pdt.UnitPrice,
                    "RM " + pdt.Discount,
                    "RM " + ((pdt.UnitPrice - pdt.Discount) * pdt.Quantity).ToString("0.##"));
                total += (pdt.UnitPrice - pdt.Discount) * pdt.Quantity;
            }
            POSProductDGV.Rows.Add();
            POSProductDGV.Rows.Add("", "", "", "", "Total Price :", "RM " + total.ToString("0.00"));

            VehicleNumberBox.ResetText();
            ICorSSM.ResetText();
            Namebox.ResetText();
            Phone_Number.ResetText();
            AddressBox.ResetText();
            Agent.SelectedIndex = -1;

            if (Customer != null)
            {
                VehicleNumberBox.Text = ObjectParse.ObjectParseString(Customer.Vehicle);
                ICorSSM.Text = ObjectParse.ObjectParseString(Customer.NRIC);
                Namebox.Text = ObjectParse.ObjectParseString(Customer.Name);
                Phone_Number.Text = ObjectParse.ObjectParseString(Customer.Contact);
                AddressBox.Lines = ObjectParse.ArrayParseStringArray(Customer.Address.ToArray());
                if (SalesID.HasValue)
                {
                    Agent.Text = new Sales().LoadJson(Sales.Format + SalesID).Agent;
                }
                else
                {
                    Agent.Text = ObjectParse.ObjectParseString(Customer.Agent);
                }
            }
        }

        private void POSProductDGV_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            var cell = POSProductDGV.Rows[e.RowIndex].Cells[e.ColumnIndex];
            var rows = POSProductDGV.Rows[e.RowIndex];
            if (e.RowIndex < POSProductDGV.Rows.Count - 2)
            {
                Product pdt = content[e.RowIndex];
                string formatvalue = e.FormattedValue.ToString().Replace(" ", "");
                if (e.FormattedValue.ToString().StartsWith("RM"))
                {
                    formatvalue = formatvalue.Substring(2);
                }
                switch (e.ColumnIndex)
                {
                    case 2:
                        if (!int.TryParse(formatvalue, out int result1))
                            e.Cancel = true;
                        else
                        {
                            content[e.RowIndex].Quantity = result1;
                            BeginInvoke(new MethodInvoker(RefreshUI));
                        }
                        break;
                    case 3:
                        if (!decimal.TryParse(formatvalue, out decimal result2))
                            e.Cancel = true;
                        else
                        {
                            content[e.RowIndex].UnitPrice = result2;
                            BeginInvoke(new MethodInvoker(RefreshUI));
                        }
                        break;
                    case 4:
                        if (!decimal.TryParse(formatvalue, out decimal result3))
                            e.Cancel = true;
                        else
                        {
                            content[e.RowIndex].Discount = result3;
                            BeginInvoke(new MethodInvoker(RefreshUI));
                        }
                        break;
                }
            }
        }

        private void EditSales_Click(object sender, EventArgs e)
        {
            SelectSales ss = new SelectSales(false);
            ss.ActionEvent += (sender_a, at) =>
            {
                if (at.ActionString == "Accept")
                {
                    Sales sales = at.ActionObject as Sales;
                    ResetData();
                    content = sales.Products;
                    Customer = sales.Customer != null ? new Customer().LoadJson(sales.Customer) : null;
                    PendingNum = null;
                    ID.Text = sales.ConvertedSalesID;
                    SalesID = sales.SalesId;
                    Mode = "Sale";
                    RefreshUI();
                }
            };
            ss.ShowDialog();
        }

        private void PendingSales_Click(object sender, EventArgs e)
        {
            SelectPendingSales sps = new SelectPendingSales();
            sps.ActionEvent += (sender_a, at) =>
            {
                if (at.ActionString.StartsWith("Accept"))
                {
                    HoldSales sales = at.ActionObject as HoldSales;
                    ResetData();
                    content = sales.Products;
                    Customer = sales.Customer != "" ? new Customer().LoadJson(sales.Customer) : null;
                    PendingNum = int.Parse(at.ActionString.Split('|')[1]);
                    Mode = "Pending";
                    ID.Text = Sales.Format + Sales.getCurrentId(false);
                    RefreshUI();
                }
            };
            sps.ShowDialog();
        }

        private void HoldBtn_Click(object sender, EventArgs e)
        {
            if (content.Count > 0 && (Mode == "Pending" || Mode == "Empty"))
            {
                if (PendingNum == null)
                {
                    List<HoldSales> hs = HoldSales.LoadJson();
                    hs.Add(new HoldSales()
                    {
                        Customer = Customer != null ? Customer.Vehicle : "",
                        Products = content,
                        CreateDate = DateTime.Now,
                        Agent = Agent.Text
                    });
                    HoldSales.SaveJson(hs);

                    Customer = null;
                    content = new List<Product>();
                    Mode = "Empty";
                    RefreshUI();
                }
                else
                {
                    List<HoldSales> hs = HoldSales.LoadJson();
                    hs[PendingNum.Value] =
                        new HoldSales()
                        {
                            Customer = Customer != null ? Customer.Vehicle : "",
                            Products = content,
                            CreateDate = DateTime.Now,
                            Agent = Agent.Text
                        };
                    HoldSales.SaveJson(hs);
                    Customer = null;
                    PendingNum = null;
                    content = new List<Product>();
                    Mode = "Empty";
                    RefreshUI();
                }
            }
        }

        public void ResetData()
        {
            if (Mode == "Sale")
            {
                Customer = null;
                content = new List<Product>();
                SalesID = null;
                Mode = "Empty";
            }
            if (Mode == "Pending")
            {
                Customer = null;
                PendingNum = null;
                content = new List<Product>();
                Mode = "Empty";
            }
            if (Mode == "Empty")
            {
                Customer = null;
                content = new List<Product>();
                Mode = "Empty";
            }
            RefreshUI();
        }

        private void PayBtn_Click(object sender, EventArgs e)
        {
            List<Sales> saleslist = new Sales().GetList();
            if (SalesID.HasValue && Sales.Format + SalesID.Value.ToString() != ID.Text)
            {
                if (saleslist.Any((x) => x.ConvertedSalesID == ID.Text))
                {
                    MessageBox.Show($"Sales ID - {Sales.Format}{ID.Text} already exists!");
                    return;
                }
            }
            if (Customer != null && Mode == "Sale")
            {
                int saleid = int.Parse(ID.Text.Replace(Sales.Format, ""));
                if (SalesID != saleid)
                {
                    var sales = new Sales().LoadJson(Sales.Format + SalesID);

                    if (DataManager.ID_Tracker.RemovedSalesID.Contains(SalesID.Value))
                    {
                        DataManager.ID_Tracker.RemovedSalesID.Remove(SalesID.Value);
                    }
                    else
                    {
                        while (SalesID.Value > DataManager.ID_Tracker.SalesID)
                        {
                            DataManager.ID_Tracker.RemovedSalesID.Add(DataManager.ID_Tracker.SalesID++);
                        }
                        SalesID = DataManager.ID_Tracker.SalesID++;
                    }
                    DataManager.ID_Tracker.RemovedSalesID.Add(sales.SalesId);


                    Sales sales2 = new Sales()
                    {
                        Products = content,
                        Customer = Customer.Vehicle,
                        SalesId = saleid,
                        CreateDate = DateTime.Now,
                        Paid = sales.Paid,
                        Agent = Agent.Text
                    };
                    sales.DeleteJson(sales.ConvertedSalesID);
                    sales2.SaveJson(sales2.ConvertedSalesID);
                    DataManager.UpdateData(sales, sales2);
                    PayMoney pm = new PayMoney(sales2);
                    var temp = Customer;

                    if (isRepeat)
                    {
                        pm.FormClosed += delegate
                        {
                            if (MessageBox.Show("Did you want to continue pay owe for current customer?", "Ask for continue", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                            {
                                PayOwe_Click(temp, new EventArgs());
                            }
                        };
                    }

                    Customer = null;
                    content = new List<Product>();
                    SalesID = null;
                    Mode = "Empty";
                    RefreshUI();
                    ID.Text = Sales.Format + Sales.getCurrentId(false);
                    pm.ShowDialog();
                }
                else
                {
                    var sales = new Sales().LoadJson(Sales.Format + SalesID);
                    Sales sales2 = new Sales()
                    {
                        Products = content,
                        Customer = Customer.Vehicle,
                        SalesId = sales.SalesId,
                        CreateDate = DateTime.Now,
                        Paid = sales.Paid,
                        Agent = Agent.Text
                    };
                    sales.DeleteJson(sales.ConvertedSalesID);
                    sales2.SaveJson(sales2.ConvertedSalesID);
                    DataManager.UpdateData(sales, sales2);
                    PayMoney pm = new PayMoney(sales2);

                    var temp = Customer;
                    if (isRepeat)
                    {
                        pm.FormClosed += delegate
                        {
                            if (MessageBox.Show("Did you want to continue pay owe for current customer?", "Ask for continue", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                            {
                                PayOwe_Click(temp, new EventArgs());
                            }
                        };
                    }
                    Customer = null;
                    content = new List<Product>();
                    Mode = "Empty";
                    RefreshUI();
                    ID.Text = Sales.Format + Sales.getCurrentId(false);
                    pm.ShowDialog();
                }
                return;
            }
            if (Customer != null && Mode == "Pending")
            {
                var hs = HoldSales.LoadJson();
                hs.RemoveAt(PendingNum.Value);
                HoldSales.SaveJson(hs);

                int saleid = int.Parse(ID.Text.Replace(Sales.Format, ""));
                if (DataManager.ID_Tracker.RemovedSalesID.Contains(saleid))
                {
                    DataManager.ID_Tracker.RemovedSalesID.Remove(saleid);
                }
                else
                {
                    while (saleid > DataManager.ID_Tracker.SalesID)
                    {
                        DataManager.ID_Tracker.RemovedSalesID.Add(DataManager.ID_Tracker.SalesID++);
                    }
                    saleid = DataManager.ID_Tracker.SalesID++;
                }

                Sales sale = new Sales()
                {
                    Customer = Customer.Vehicle,
                    Paid = new Dictionary<string, decimal>(),
                    Products = content,
                    CreateDate = DateTime.Now,
                    SalesId = saleid,
                    Agent = Agent.Text
                };
                sale.SaveJson(sale.ConvertedSalesID);
                PayMoney pm = new PayMoney(sale);

                Customer = null;
                PendingNum = null;
                content = new List<Product>();
                Mode = "Empty";
                RefreshUI();
                ID.Text = Sales.Format + Sales.getCurrentId(false);
                pm.ShowDialog();

                ActionEvent(sender, new ActionEventArgs()
                {
                    ActionString = "AskInvoice",
                    ActionObject = sale
                });
                return;

            }
            if (Customer != null)
            {
                int saleid = int.Parse(ID.Text.Replace(Sales.Format, ""));
                if (DataManager.ID_Tracker.RemovedSalesID.Contains(saleid))
                {
                    DataManager.ID_Tracker.RemovedSalesID.Remove(saleid);
                }
                else
                {
                    while (saleid > DataManager.ID_Tracker.SalesID)
                    {
                        DataManager.ID_Tracker.RemovedSalesID.Add(DataManager.ID_Tracker.SalesID++);
                    }
                    saleid = DataManager.ID_Tracker.SalesID++;
                }

                Sales sale = new Sales()
                        {
                            Customer = Customer.Vehicle,
                            Paid = new Dictionary<string, decimal>(),
                            Products = content,
                            CreateDate = DateTime.Now,
                            SalesId = saleid,
                            Agent = Agent.Text
                        };
                sale.SaveJson(sale.ConvertedSalesID);
                PayMoney pm = new PayMoney(sale);

                Customer = null;
                content = new List<Product>();
                Mode = "Empty";
                RefreshUI();
                ID.Text = Sales.Format + Sales.getCurrentId(false);
                pm.ShowDialog();

                ActionEvent(sender, new ActionEventArgs()
                {
                    ActionString = "AskInvoice",
                    ActionObject = new Sales().LoadJson(sale.ConvertedSalesID)
                });
                return;
            }
        }

        private void POSProductDGV_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.RowIndex >= POSProductDGV.Rows.Count - 2)
            {
                e.Cancel = true;
            }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (POSProductDGV.SelectedRows.Count == 1)
            { 
                if (keyData.ToString() == DataManager.UserInfo.EditQTY)
                {
                    if (!POSProductDGV.IsCurrentCellInEditMode)
                    {
                        var cell = POSProductDGV.SelectedRows[0].Cells[2];
                        POSProductDGV.CurrentCell = cell;
                        POSProductDGV.BeginEdit(true);
                    }
                }
                if (keyData.ToString() == DataManager.UserInfo.EditUP)
                {
                    if (!POSProductDGV.IsCurrentCellInEditMode)
                    {
                        var cell = POSProductDGV.SelectedRows[0].Cells[3];
                        POSProductDGV.CurrentCell = cell;
                        POSProductDGV.BeginEdit(true);
                    }
                }
                if (keyData.ToString() == DataManager.UserInfo.EditDiscount)
                {
                    if (!POSProductDGV.IsCurrentCellInEditMode)
                    {
                        var cell = POSProductDGV.SelectedRows[0].Cells[4];
                        POSProductDGV.CurrentCell = cell;
                        POSProductDGV.BeginEdit(true);
                    }
                }
            }
            foreach (var sk in DataManager.UserInfo.ShortKeyData.Where((x) => x.Key.StartsWith(Prefix)))
            {
                if (sk.Value == keyData.ToString())
                {
                    (Controls.Find(sk.Key.Split('.')[1], true)[0] as Button).PerformClick();
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void ID_TextChanged(object sender, EventArgs e)
        {
            if (!ID.Text.StartsWith(Sales.Format))
            {
                ID.Text = Sales.Format + Sales.getCurrentId(false);
                ID.SelectionStart = ID.Text.Length;
            }
        }

        private void ID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void PayOwe_Click(object sender, EventArgs e)
        {
            if (sender is Customer)
            {
                Customer customer = sender as Customer;
                SelectSales ss = new SelectSales(true);
                ss.ActionEvent += (sender_b, at2) =>
                {
                    if (at2.ActionString == "Accept")
                    {
                        Sales sales = at2.ActionObject as Sales;

                        ResetData();
                        content = sales.Products;
                        Customer = sales.Customer != null ? new Customer().LoadJson(sales.Customer) : null;
                        PendingNum = null;
                        ID.Text = sales.ConvertedSalesID;
                        SalesID = sales.SalesId;
                        Mode = "Sale";
                        isRepeat = true;

                        RefreshUI();
                    }
                };
                ss.DataTable = new DataTable();
                ss.DataTable.Columns.Add("Sales No");
                ss.DataTable.Columns.Add("Vehicle No");
                ss.DataTable.Columns.Add("Customer Name");
                ss.DataTable.Columns.Add("NRIC");
                ss.DataTable.Columns.Add("Create Date");
                ss.DataTable.Columns.Add("Agent");
                ss.DataView = ss.DataTable.DefaultView;
                ss.SalesDataDGV.DataSource = ss.DataView;
                foreach (var sales in new Sales().GetList().Where((x) => x.GetCustomerIC() == customer.NRIC && x.GetCustomerName() == customer.Name))
                {
                    ss.DataTable.Rows.Add(
                           sales.ConvertedSalesID,
                           customer.Vehicle,
                           customer.Name,
                           customer.NRIC,
                           sales.CreateDate.ToString("yyyy/MM/dd"),
                           sales.Agent);

                }
                ss.ShowDialog();
            }
            else
            {
                SelectOweCustomer soc = new SelectOweCustomer();
                soc.ActionEvent += (sender_a, at) =>
                {
                    if (at.ActionString.StartsWith("Accept"))
                    {
                        Customer customer = at.ActionObject as Customer;
                        
                        SelectSales ss = new SelectSales(true);
                        ss.ActionEvent += (sender_b, at2) =>
                        {
                            if (at2.ActionString == "Accept")
                            {
                                Sales sales = at2.ActionObject as Sales;

                                ResetData();
                                content = sales.Products;
                                Customer = sales.Customer != null ? new Customer().LoadJson(sales.Customer) : null;
                                PendingNum = null;
                                ID.Text = sales.ConvertedSalesID;
                                SalesID = sales.SalesId;
                                Mode = "Sale";
                                isRepeat = true;

                                RefreshUI();
                            }
                        };
                        ss.DataTable = new DataTable();
                        ss.DataTable.Columns.Add("Sales No");
                        ss.DataTable.Columns.Add("Vehicle No");
                        ss.DataTable.Columns.Add("Customer Name");
                        ss.DataTable.Columns.Add("NRIC");
                        ss.DataTable.Columns.Add("Create Date");
                        ss.DataTable.Columns.Add("Agent");
                        ss.DataView = ss.DataTable.DefaultView;
                        ss.SalesDataDGV.DataSource = ss.DataView;
                        foreach (var sales in new Sales().GetList().Where((x) => x.GetCustomerIC() == customer.NRIC && x.GetCustomerName() == customer.Name))
                        {
                            if (sales.GetUnpaid() > 0)
                            {
                                ss.DataTable.Rows.Add(
                                       sales.ConvertedSalesID,
                                       customer.Vehicle,
                                       customer.Name,
                                       customer.NRIC,
                                       sales.CreateDate.ToString("yyyy/MM/dd"),
                                       sales.Agent);
                            }
                        }
                        ss.ShowDialog();
                    }
                };
                soc.ShowDialog();
            }
        }
    }
}
