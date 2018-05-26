
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
    public partial class SelectPendingSales : Form, IUserAction<ActionEventArgs>
    {
        public SelectPendingSales()
        {
            InitializeComponent();
            RefreshUI(HoldSales.LoadJson());
        }

        public void RefreshUI(IEnumerable<HoldSales> saleslist)
        {
            while (SalesDataDGV.CurrentRow != null)
            {
                SalesDataDGV.Rows.Remove(SalesDataDGV.CurrentRow);
            }
            foreach (var sales in saleslist)
            {
                int? rows = null;
                if (sales.Customer != "")
                {
                    var customer = new Customer().LoadJson(sales.Customer);
                    rows = SalesDataDGV.Rows.Add(
                        customer.Vehicle,
                        customer.Name,
                        sales.CreateDate.ToString("yyyy/MM/dd"),
                        null,
                        customer.Agent);
                }
                else
                {
                    rows = SalesDataDGV.Rows.Add(
                        "",
                        "",
                        sales.CreateDate.ToString("yyyy/MM/dd"),
                        null,
                        "");
                }
                var cell = SalesDataDGV.Rows[rows.Value].Cells[3] as DataGridViewComboBoxCell;
                cell.DataSource = sales.Products;
                cell.DisplayMember = "DisplayText";

            }
        }

        public event EventHandler<ActionEventArgs> ActionEvent;

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void AcceptBtn_Click(object sender, EventArgs e)
        {
            if (SalesDataDGV.SelectedRows.Count == 1)
            {
                HoldSales sales = HoldSales.LoadJson()[SalesDataDGV.CurrentRow.Index];
                ActionEvent(sender, new ActionEventArgs()
                {
                    ActionString = "Accept|" + SalesDataDGV.CurrentRow.Index,
                    ActionObject = sales
                });
            }
            Close();
        }
    }
}
