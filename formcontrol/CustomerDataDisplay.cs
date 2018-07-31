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
using CSharpOskaAPI.Form;
using System.IO;

namespace HaoProgram_Rebuild
{
    public partial class CustomerDataDisplay : DisplayControl, IUserAction<ActionEventArgs>
    {
        static string[] sortbyList = { "Vehicle No", "Name", "NRIC / SSM"};
        public DataView DataView { get; set; }
        public DataTable DataTable { get; set; }
        public static string Prefix = "CDD";

        protected override CreateParams CreateParams
        {
            get
            {
                var parms = base.CreateParams;
                parms.Style &= ~0x02000000;  // Turn off WS_CLIPCHILDREN
                return parms;
            }
        }

        public void InitializeView()
        {
            DataTable = new DataTable();
            DataTable.Columns.Add("Vehicle No");
            DataTable.Columns.Add("Name");
            DataTable.Columns.Add("NRIC / SSM");
            DataTable.Columns.Add("Address");
            DataTable.Columns.Add("Phone No");
            DataTable.Columns.Add("Agent");
            DataView = DataTable.DefaultView;
            Customer_Data_DGV.DataSource = DataView;
            foreach (var customer in new Customer().GetList())
            {
                DataTable.Rows.Add(
                    customer.Vehicle,
                    customer.Name,
                    customer.NRIC,
                    ObjectParse.ArrayParseString(customer.Address.ToArray()),
                    customer.Contact,
                    customer.Agent);
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

        public CustomerDataDisplay()
        {
            InitializeComponent();
            DoubleBuffered = true;
            InitializeView(); 

            foreach (var str in sortbyList)
            {
                SortBy.Items.Add(str);
            }
            SortBy.SelectedIndex = 0;
            UpdateAgent();
            DataManager.InitializeSKey(this, Prefix);
        }

        public event EventHandler<ActionEventArgs> ActionEvent;

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            if (Customer_Data_DGV.SelectedRows.Count != 0 && Customer_Data_DGV.SelectedRows.Count > 0)
            {
                var vehiclenumber = Customer_Data_DGV.SelectedRows[0].Cells[0];
                Customer customer = new Customer().LoadJson(vehiclenumber.Value.ToString());
                var temp = customer;
                customer = new Customer()
                {
                    Vehicle = ObjectParse.ObjectParseString(VehicleNumberBox.Text),
                    NRIC = ObjectParse.ObjectParseString(ICorSSM.Text),
                    Name = ObjectParse.ObjectParseString(Namebox.Text),
                    Address = ObjectParse.ArrayParseStringArray(AddressBox.Lines).ToList(),
                    Contact = ObjectParse.ObjectParseString(Phone_Number.Text),
                    Agent = Agent.SelectedIndex > -1
                        ? Agent.Text
                        : ""
                };
                if (temp.Vehicle != customer.Vehicle)
                {
                    new Customer().DeleteJson(temp.Vehicle);
                    Directory.Move($"{AppDomain.CurrentDomain.BaseDirectory}/data/customer/{temp.Vehicle}", $"{AppDomain.CurrentDomain.BaseDirectory}/data/customer/{customer.Vehicle}");
                }
                customer.SaveJson(customer.Vehicle);
                DataManager.UpdateData(temp, customer);

                DataRow[] dr = DataTable.Select($"[Vehicle No]='{temp.Vehicle}'");
                dr[0].Delete();

                DataTable.Rows.Add(
                    customer.Vehicle,
                    customer.Name,
                    customer.NRIC,
                    ObjectParse.ArrayParseString(customer.Address.ToArray()),
                    customer.Contact,
                    customer.Agent);
            }
        }

        private void DeleteBtn_Click(object sender, EventArgs e)
        {
            if (Customer_Data_DGV.SelectedRows.Count != 0 && Customer_Data_DGV.SelectedRows.Count < 2)
            {
                if ( MessageBox.Show("Did you want to delete selected customer?\n* Warning : After delete cant be recover without restore from backup", "Delete Customer", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    var vehiclenumber = Customer_Data_DGV.SelectedRows[0].Cells[0];
                    Customer customer = new Customer().LoadJson(vehiclenumber.Value.ToString());
                    customer.DeleteJson(customer.Vehicle);
                    Directory.Delete($"{AppDomain.CurrentDomain.BaseDirectory}/data/customer/" + customer.Vehicle + "/", true);
                    DataManager.UpdateData(customer, null);
                    
                    DataRow[] dr = DataTable.Select($"[Vehicle No]='{customer.Vehicle}'");
                    dr[0].Delete();
                }
            }
        }

        private void AddBtn_Click(object sender, EventArgs e)
        {
            ActionEvent(sender, new ActionEventArgs()
            {
                ActionString = "OpenRequest"
            });
        }

        private void ExitBtn_Click(object sender, EventArgs e)
        {
            ActionEvent(sender, new ActionEventArgs()
            {
                ActionString = "CloseCurrent"
            });
        }

        private void DGVSelect(object sender, EventArgs e)
        {
            foreach (var ctl in FunctionUtil.GetAllChildren(InputLayout))
            {
                if (ctl is TextBox)
                    ((TextBox)ctl).ResetText();
                if (ctl is RichTextBox)
                    ((RichTextBox)ctl).ResetText();
                if (ctl is ComboBox)
                    ((ComboBox)ctl).SelectedIndex = -1;
            }
            var rowsCount = Customer_Data_DGV.SelectedRows.Count;
            if (rowsCount == 0 || rowsCount > 1) return;

            var vehiclenumber = Customer_Data_DGV.SelectedRows[0].Cells[0];
            var customer = new Customer().LoadJson(vehiclenumber.Value.ToString());
            if (customer != null)
            {
                VehicleNumberBox.Text = ObjectParse.ObjectParseString(customer.Vehicle);
                ICorSSM.Text = ObjectParse.ObjectParseString(customer.NRIC);
                Namebox.Text = ObjectParse.ObjectParseString(customer.Name);
                Phone_Number.Text = ObjectParse.ObjectParseString(customer.Contact);
                AddressBox.Lines = ObjectParse.ArrayParseStringArray(customer.Address.ToArray());
                Agent.Text = ObjectParse.ObjectParseString(customer.Agent);
            }
        }

        private void OnSortTyping(object sender, EventArgs e)
        {
            DataView.RowFilter = $"[{SortBy.Text}] LIKE '*{SortBox.Text}*'";
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

        private void EditBtn_Click(object sender, EventArgs e)
        {
            foreach (var ctl in FunctionUtil.GetAllChildren(InputLayout))
            {
                if (ctl is TextBox)
                    ((TextBox)ctl).ReadOnly = !((TextBox)ctl).ReadOnly;
                if (ctl is RichTextBox)
                    ((RichTextBox)ctl).ReadOnly = !((RichTextBox)ctl).ReadOnly;
                if (ctl is ComboBox)
                    ((ComboBox)ctl).Enabled = !((ComboBox)ctl).Enabled;
            }
        }

        private void ScanHistoryBtn_Click(object sender, EventArgs e)
        {
            if (Customer_Data_DGV.SelectedRows.Count == 1)
            {
                var vehicle_number = Customer_Data_DGV.SelectedRows[0].Cells[0];
                ActionEvent(sender, new ActionEventArgs()
                {
                    ActionString = "ViewScan",
                    ActionObject = new Customer().LoadJson(vehicle_number.Value.ToString())
                });
            }
        }
    }
}
