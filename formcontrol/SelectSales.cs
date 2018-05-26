
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
    public partial class SelectSales : Form, IUserAction<ActionEventArgs>
    {
        static string[] SortByList = { "Sales No", "Vehicle No" };
        public DataView DataView { get; set; }
        public DataTable DataTable { get; set; }
        public SelectSales(bool isReplacedMethod)
        {
            InitializeComponent();
            if (!isReplacedMethod)
            {
                InitializeView();

                foreach (var str in SortByList)
                {
                    SortBy.Items.Add(str);
                }
                SortBy.SelectedIndex = 0;
            }
            SalesDataDGV.Sort(SalesDataDGV.Columns[0], ListSortDirection.Ascending);
        }

        public void InitializeView()
        {
            DataTable = new DataTable();
            DataTable.Columns.Add("Sales No");
            DataTable.Columns.Add("Vehicle No");
            DataTable.Columns.Add("Customer Name");
            DataTable.Columns.Add("Create Date");
            DataTable.Columns.Add("Agent");
            DataView = DataTable.DefaultView;
            SalesDataDGV.DataSource = DataView;
            foreach (var sales in new Sales().GetList())
            {
                var customer = new Customer().LoadJson(sales.Customer);
                DataTable.Rows.Add(
                       sales.ConvertedSalesID,
                       customer.Vehicle,
                       customer.Name,
                       sales.CreateDate.ToString("yyyy/MM/dd"),
                       sales.Agent);
            }
        }

        public event EventHandler<ActionEventArgs> ActionEvent;

        private void AcceptBtn_Click(object sender, EventArgs e)
        {
            if (SalesDataDGV.SelectedRows.Count == 1)
            {
                var cell = SalesDataDGV.SelectedRows[0].Cells[0];
                Sales sales = new Sales().LoadJson(cell.Value.ToString());
                ActionEvent(sender, new ActionEventArgs()
                {
                    ActionString = "Accept",
                    ActionObject = sales
                });
            }
            Close();
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void SortBox_TextChanged(object sender, EventArgs e)
        {
            DataView.RowFilter = $"[{SortBy.Text}] LIKE '*{SortBox.Text}*'";
        }
    }
}
