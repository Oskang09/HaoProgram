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
using CSharpOskaAPI;

namespace HaoProgram
{
    public partial class AgentComissionDisplay : UserControl, IUserAction<ActionEventArgs>
    {
        static string[] SortByList = { "Product Code", "Description" };
        public static string Prefix = "ACD";
        public AgentComissionDisplay()
        {
            InitializeComponent();
            RefreshComponent();

            foreach (var str in SortByList)
            {
                SortBy.Items.Add(str);
            }
            SortBy.SelectedIndex = 0;
            DataManager.InitializeSKey(this, Prefix);
            ProductComDGV.Sort(ProductComDGV.Columns[0], ListSortDirection.Ascending);
        }

        public void RefreshComponent()
        {
            ComCategory.Items.Clear();
            foreach (var comcat in new AgentCommission().GetList())
            {
                ComCategory.Items.Add(comcat.CategoryName);
            }
        }
        public event EventHandler<ActionEventArgs> ActionEvent;

        private void ComCategorySelected(object sender, EventArgs e)
        {
            if (ComCategory.SelectedIndex > -1)
            {
                RefreshComUI(null);
            }
        }

        public void RefreshComUI(Product selected)
        {
            while (ProductComDGV.CurrentRow != null)
            {
                ProductComDGV.Rows.Remove(ProductComDGV.CurrentRow);
            }
            foreach (var product in new Product().GetList())
            {
                var agentcom = new AgentCommission().LoadJson(ComCategory.Text);
                foreach (var kvp in agentcom.Product)
                {
                    if (kvp.Item1.Code == product.Code)
                    {
                        ProductComDGV.Rows.Add(
                            ObjectParse.ObjectParseString(product.ConvertedProductCode),
                            ObjectParse.ObjectParseString(product.Description),
                            "RM " + ObjectParse.ObjectParseString(product.UnitPrice),
                            kvp.Item2,
                            CommissionTypeExtension.ToString(kvp.Item3));
                        goto NEXTLOOP;
                    }
                }
                ProductComDGV.Rows.Add(
                    ObjectParse.ObjectParseString(product.ConvertedProductCode),
                    ObjectParse.ObjectParseString(product.Description),
                    "RM " + ObjectParse.ObjectParseString(product.UnitPrice));

                NEXTLOOP:
                continue;
            }
        }
        public void RefreshUI2(IEnumerable<Product> queries)
        {
            while (ProductComDGV.CurrentRow != null)
            {
                ProductComDGV.Rows.Remove(ProductComDGV.CurrentRow);
            }
            foreach (var product in queries)
            {
                var agentcom = new AgentCommission().LoadJson(ComCategory.Text);

                foreach (var kvp in agentcom.Product)
                {
                    if (kvp.Item1.Code == product.Code)
                    {
                        ProductComDGV.Rows.Add(
                            ObjectParse.ObjectParseString(product.ConvertedProductCode),
                            ObjectParse.ObjectParseString(product.Description),
                            "RM " + ObjectParse.ObjectParseString(product.UnitPrice),
                            kvp.Item2,
                            CommissionTypeExtension.ToString(kvp.Item3));
                        goto NEXTLOOP;
                    }
                }
                ProductComDGV.Rows.Add(
                    ObjectParse.ObjectParseString(product.ConvertedProductCode),
                    ObjectParse.ObjectParseString(product.Description),
                    "RM " + ObjectParse.ObjectParseString(product.UnitPrice));

                NEXTLOOP:
                    continue;
            }
        }

        private void OnSortTyping2(object sender, EventArgs e)
        {
            if (ComCategory.SelectedIndex > -1)
            {
                IEnumerable<Product> query = null;
                switch (SortBy.SelectedIndex)
                {
                    case 0:
                        query = from obj in new Product().GetList()
                                where StringExtension.Contains(obj.ConvertedProductCode, SortBox.Text, StringComparison.OrdinalIgnoreCase)
                                select obj;
                        break;
                    case 1:
                        query = from obj in new Product().GetList()
                                where StringExtension.Contains(obj.Description, SortBox.Text, StringComparison.OrdinalIgnoreCase)
                                select obj;
                        break;
                }
                RefreshUI2(query);
            }
        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            if (ComCategory.SelectedIndex > -1 && ProductCode.Text != "")
            {
                Product product = null;
                var agentcom = new AgentCommission().LoadJson(ComCategory.Text);

                if (agentcom.Product.Any((x) => x.Item1.ConvertedProductCode == ProductCode.Text))
                {
                    var product_tuple = agentcom.Product.First((x) =>
                        x.Item1.ConvertedProductCode == ProductCode.Text);
                    agentcom.Product.Remove(product_tuple);
                }
                if (Rateby.SelectedIndex > -1)
                {
                    product = new Product().LoadJson(ProductCode.Text);
                    agentcom.Product.Add(Tuple.Create(product, Rate.Text != "" ? decimal.Parse(Rate.Text) : 0, CommissionTypeExtension.FromString(Rateby.Text)));
                }
                agentcom.SaveJson(agentcom.CategoryName);
                RefreshComUI(product);
            }
        }

        private void AddBtn_Click(object sender, EventArgs e)
        {
            ActionEvent(sender, new ActionEventArgs()
            {
                ActionString = "OpenRequest"
            });
        }

        private void ComSelection(object sender, EventArgs e)
        {
            foreach (var ctl in FunctionUtil.GetAllChildren(InputLayout))
            {
                if (ctl is TextBox)
                    ((TextBox)ctl).ResetText();
                if (ctl is NumericUpDown)
                    ((NumericUpDown)ctl).Value = 0;
                if (ctl is ComboBox)
                    if (ctl != ComCategory)
                    ((ComboBox)ctl).SelectedIndex = -1;
            }
            var rowsCount = ProductComDGV.SelectedRows.Count;
            if (rowsCount == 0 || rowsCount > 1) return;

            var productname = ProductComDGV.SelectedRows[0].Cells[0];
            var product = new Product().LoadJson(productname.Value.ToString());
            if (product != null)
            {
                ProductCode.Text = ObjectParse.ObjectParseString(product.ConvertedProductCode);
                Description.Text = ObjectParse.ObjectParseString(product.Description);
                UnitPrice.Text = "RM " + product.UnitPrice;
                if (ProductComDGV.SelectedRows[0].Cells[3].Value != null && decimal.Parse(ProductComDGV.SelectedRows[0].Cells[3].Value.ToString()) > 0)
                {
                    Rate.Text = ProductComDGV.SelectedRows[0].Cells[3].Value.ToString();
                    Rateby.Text = ProductComDGV.SelectedRows[0].Cells[4].Value.ToString();
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

        private void DeleteBtn_Click(object sender, EventArgs e)
        {
            if (ComCategory.Text != "")
            {
                if (MessageBox.Show("Did you want to delete selected commission category?\n* Warning : After delete cant be recover without restore from backup", "Delete Agent", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    var comname = ComCategory.Text;
                    new AgentCommission().DeleteJson(comname);
                    ComCategory.Items.Remove(comname);
                }
            }
        }

        private void EditBtn_Click(object sender, EventArgs e)
        {
            foreach (var ctl in FunctionUtil.GetAllChildren(InputLayout))
            {
                if (ctl is TextBox)
                    if (ctl != ProductCode && ctl != Description && ctl != UnitPrice)
                        ((TextBox)ctl).ReadOnly = !((TextBox)ctl).ReadOnly;
                if (ctl is ComboBox)
                    if (ctl != ComCategory)
                        ((ComboBox)ctl).Enabled = !ctl.Enabled;
            }
        }

        private void Rate_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                (e.KeyChar != '.'))
            {
                e.Handled = true;
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
