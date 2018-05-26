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
    public partial class ProductDataRequest: Form, IUserAction<ActionEventArgs>
    {
        public ProductDataRequest()
        {
            InitializeComponent();
            DoubleBuffered = true;
            ProductCode.Text = Product.Format + Product.getCurrentId(false);
        }
        public event EventHandler<ActionEventArgs> ActionEvent;

        private void AddBtn_Click(object sender, EventArgs e)
        {
            if (new Product().Exists(ProductCode.Text))
            {
                if (MessageBox.Show("Did you want to view the existing product?", "Repeated Product Code!", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    ActionEvent(sender, new ActionEventArgs()
                    {
                        ActionString = "ExistingProduct",
                        ActionObject = new Product()
                        {
                            Code = int.Parse(ProductCode.Text.Replace(Product.Format, ""))
                        }
                    });
                }
            }
            else
            {
                if (ProductCode.Text == "")
                {
                    MessageBox.Show("Product code cannot be empty!");
                }
                else
                {
                    ActionEvent(sender, new ActionEventArgs()
                    {
                        ActionString = "NewProduct",
                        ActionObject = new Product()
                        {
                            Code = int.Parse(ProductCode.Text.Replace(Product.Format, "")),
                            Description = ObjectParse.ObjectParseString(Description.Text),
                            UnitPrice = UnitPrice.Value
                        }
                    });
                }
            }
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            ActionEvent(sender, new ActionEventArgs
            {
                ActionString = "ExitRequest"
            });
        }

        private void ProductCode_TextChanged(object sender, EventArgs e)
        {
            if (!ProductCode.Text.StartsWith(Product.Format))
            {
                ProductCode.Text = Product.Format + Product.getCurrentId(false);
                ProductCode.SelectionStart = ProductCode.Text.Length;
            }
        }

        private void ProductCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }
    }
}
