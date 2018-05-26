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
    public partial class SelectOweCustomer : Form, IUserAction<ActionEventArgs>
    {
        public DataTable PDataTable { get; set; }
        public DataView PDataView { get; set; }

        public static string[] SortByList = { "Customer", "NRIC / SSM" };

        public event EventHandler<ActionEventArgs> ActionEvent;

        public SelectOweCustomer()
        {
            InitializeComponent();
            foreach (var str in SortByList)
            {
                SortBy.Items.Add(str);
            }
            SortBy.SelectedIndex = 0;

            PDataTable = new DataTable();
            PDataTable.Columns.Add("Customer");
            PDataTable.Columns.Add("Contact");
            PDataTable.Columns.Add("Total");
            PDataTable.Columns.Add("Paid");
            PDataTable.Columns.Add("Due");
            PDataTable.Columns.Add("NRIC / SSM");
            PDataView = PDataTable.DefaultView;
            CustomerDataDGV.DataSource = PDataView;
            foreach (var cmr in new Customer().GetList())
            {
                SAccount sac = new SAccount(cmr.Name, cmr.NRIC);
                if (sac.getOwe() > 0)
                {
                    PDataTable.Rows.Add(
                        cmr.Name,
                        cmr.Contact,
                        sac.getTotal(),
                        sac.getPaid(),
                        sac.getOwe(),
                        cmr.NRIC);
                }
            }
        }

        private void SortBox_TextChanged(object sender, EventArgs e)
        {
            PDataView.RowFilter = $"[{SortBy.Text}] LIKE '*{SortBox.Text}*'";
        }

        private void AcceptBtn_Click(object sender, EventArgs e)
        {
            if (CustomerDataDGV.SelectedRows.Count == 1)
            {
                var name = CustomerDataDGV.SelectedRows[0].Cells[0].Value.ToString();
                var ic = CustomerDataDGV.SelectedRows[0].Cells[5].Value.ToString();
                ActionEvent(sender, new ActionEventArgs()
                {
                    ActionString = "Accept",
                    ActionObject = new Customer().GetList()
                        .First((x) => x.Name == name && x.NRIC == ic)
                });
                Close();
            }
        }
    }
}
