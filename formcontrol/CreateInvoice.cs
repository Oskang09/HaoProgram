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
    public partial class CreateInvoice : Form, IUserAction<ActionEventArgs>
    {
        public CreateInvoice(Invoice invoice)
        {
            InitializeComponent();
            if (invoice != null)
            {
                InvoiceID.Text = invoice.ConvertedInvoiceID;
                salesID.Text = invoice.Sales;
            }
            else
            {
                InvoiceID.Text = Invoice.Format + Invoice.getCurrentId(false);
            }
        }

        public event EventHandler<ActionEventArgs> ActionEvent;

        private void AcceptBtn_Click(object sender, EventArgs e)
        {
            if (InvoiceID.Text == "")
            {
                MessageBox.Show("Invoice ID cannot be empty!");
                return;
            }
            if (salesID.Text == "")
            {
                MessageBox.Show("Select a sales for create invoice!");
                return;
            }
            if (!new Invoice().Exists(InvoiceID.Text))
            {
                ActionEvent(sender, new ActionEventArgs()
                {
                    ActionString = "Accept",
                    ActionObject = new Invoice()
                    {
                        InvoiceID = Invoice.getCurrentId(false),
                        Sales = salesID.Text
                    }
                });
                Close();
            }
            else
            {
                MessageBox.Show($"Invoice ID - {Invoice.Format}{InvoiceID.Text} already exists!");
            }
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void editSales_Click(object sender, EventArgs e)
        {
            SelectSales ss = new SelectSales(false);
            ss.ActionEvent += (sender_a, at) =>
            {
                if (at.ActionString == "Accept")
                {
                    Sales sales = at.ActionObject as Sales;
                    salesID.Text = sales.ConvertedSalesID;
                }
            };
            ss.ShowDialog();
        }

        private void InvoiceID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void InvoiceID_TextChanged(object sender, EventArgs e)
        {
            if (!InvoiceID.Text.StartsWith(Invoice.Format))
            {
                InvoiceID.Text = Invoice.Format + Invoice.getCurrentId(false);
                InvoiceID.SelectionStart = InvoiceID.Text.Length;
            }
        }
    }
}
