using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CSharpOskaAPI.Form;
using CSharpOskaAPI.Parse;
using System.Windows.Input;

namespace HaoProgram
{
    public partial class ProductDataDisplay: UserControl, IUserAction<ActionEventArgs>
    {
        static string[] SortByList = { "Product Code", "Description" };
        public static string Prefix = "PDD";
        public DataView DataView { get; set; }
        public DataTable DataTable { get; set; }
        public ProductDataDisplay()
        {
            InitializeComponent();
            DoubleBuffered = true;
            InitializeView();

            foreach (var sorby in SortByList)
            {
                SortBy.Items.Add(sorby);
            }
            SortBy.SelectedIndex = 0;
            DataManager.InitializeSKey(this, Prefix);
            ProductDataDGV.Sort(ProductDataDGV.Columns[0], ListSortDirection.Ascending);
        }

        public void InitializeView()
        {
            DataTable = new DataTable();
            DataTable.Columns.Add("Product Code");
            DataTable.Columns.Add("Description");
            DataTable.Columns.Add("Unit Price");
            DataView = DataTable.DefaultView;
            ProductDataDGV.DataSource = DataView;
            foreach (var pdt in new Product().GetList())
            {
                DataTable.Rows.Add(
                    ObjectParse.ObjectParseString(pdt.ConvertedProductCode),
                    ObjectParse.ObjectParseString(pdt.Description),
                    "RM " + ObjectParse.ObjectParseString(pdt.UnitPrice));
            }
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

        private void AddBtn_Click(object sender, EventArgs e)
        {
            ActionEvent(sender, new ActionEventArgs()
            {
                ActionString = "OpenRequest"
            });
        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            if (ProductDataDGV.SelectedRows.Count != 0 && ProductDataDGV.SelectedRows.Count > 0)
            {
                var productcode = ProductDataDGV.SelectedRows[0].Cells[0];
                Product product = new Product().LoadJson(productcode.Value.ToString());
                var temp = product;

                if (product.ConvertedProductCode != ProductCode.Text)
                {
                    int code = int.Parse(ProductCode.Text.Replace(Product.Format, ""));
                    if (DataManager.ID_Tracker.RemovedProductID.Contains(code))
                    {
                        DataManager.ID_Tracker.RemovedProductID.Remove(code);
                    }
                    DataManager.ID_Tracker.RemovedProductID.Add(product.Code);
                }
                product = new Product()
                {
                    Code = int.Parse(ProductCode.Text.Replace(Product.Format, "")),
                    Description = Description.Text,
                    UnitPrice = UnitPrice.Text != "" ? decimal.Parse(UnitPrice.Text) : 0
                };
                DataManager.UpdateData(temp, product);
                product.SaveJson(product.ConvertedProductCode);
                if (product.ConvertedProductCode != temp.ConvertedProductCode)
                {
                    new Product().DeleteJson(temp.ConvertedProductCode);
                }

                DataRow[] dr = DataTable.Select($"[Product Code]='{temp.ConvertedProductCode}'");
                dr[0].Delete();

                DataTable.Rows.Add(
                    ObjectParse.ObjectParseString(product.ConvertedProductCode),
                    ObjectParse.ObjectParseString(product.Description),
                    "RM " + ObjectParse.ObjectParseString(product.UnitPrice));
            }
        }


        private void DeleteBtn_Click(object sender, EventArgs e)
        {
            if (ProductDataDGV.SelectedRows.Count == 1)
            {
                if (MessageBox.Show("Did you want to delete selected product?\n* Warning : After delete cant be recover without restore from backup", "Delete Product", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    var productcode = ProductDataDGV.SelectedRows[0].Cells[0];
                    Product product = new Product().LoadJson(productcode.Value.ToString());
                    DataManager.ID_Tracker.RemovedProductID.Add(product.Code);
                    DataManager.UpdateData(product, null);
                    DataRow[] dr = DataTable.Select($"[Product Code]='{product.ConvertedProductCode}'");
                    dr[0].Delete();
                    product.DeleteJson(product.ConvertedProductCode);
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

        private void DGVSelect(object sender, EventArgs e)
        {
            foreach (var ctl in FunctionUtil.GetAllChildren(InputLayout))
            {
                if (ctl is TextBox)
                    ((TextBox)ctl).ResetText();
                if (ctl is NumericUpDown)
                    ((NumericUpDown)ctl).Value = 0;
            }

            var rowsCount = ProductDataDGV.SelectedRows.Count;
            if (rowsCount == 0 || rowsCount > 1) return;

            var productcode = ProductDataDGV.SelectedRows[0].Cells[0];
            var product = new Product().LoadJson(productcode.Value.ToString());
            if (product != null)
            {
                ProductCode.Text = ObjectParse.ObjectParseString(product.ConvertedProductCode);
                Description.Text = ObjectParse.ObjectParseString(product.Description);
                UnitPrice.Text = product.UnitPrice.ToString("0.00");
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
            }
        }

        private void ProductCode_TextChanged(object sender, EventArgs e)
        {
            if (!ProductCode.Text.StartsWith(Product.Format))
            {
                ProductCode.Text = ProductDataDGV.SelectedRows.Count == 1 ? ProductDataDGV.SelectedRows[0].Cells[0].Value.ToString() : Product.Format + Product.getCurrentId(false);
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

        private void UnitPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }
    }
}
