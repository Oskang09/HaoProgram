using CSharpOskaAPI.Parse;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HaoProgram
{
    public partial class SelectCustomer : Form, IUserAction<ActionEventArgs>
    {
        static string[] sortbyList = { "Vehicle No", "Name", "NRIC / SSM" };
        public DataTable DataTable { get; set; }
        public DataView DataView { get; set; }
        public SelectCustomer()
        {
            InitializeComponent();
            foreach (var str in sortbyList)
            {
                SortBy.Items.Add(str);
            }
            SortBy.SelectedIndex = 0;

            InitializeView();
        }

        public event EventHandler<ActionEventArgs> ActionEvent;

        private void OnSort(object sender, EventArgs e)
        {
            DataView.RowFilter = $"[{SortBy.Text}] LIKE '*{SortBox.Text}*'";
        }

        private void AcceptBtn_Click(object sender, EventArgs e)
        {
            if (CustomerDataDGV.SelectedRows.Count == 1)
            {
                var pcode = CustomerDataDGV.SelectedRows[0].Cells[0];
                Customer cmr = new Customer().LoadJson(pcode.Value.ToString());
                ActionEvent(sender, new ActionEventArgs()
                {
                    ActionString = "Accept",
                    ActionObject = cmr
                });
            }
            Close();
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            Close();
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
            CustomerDataDGV.DataSource = DataView;
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
    }
}
