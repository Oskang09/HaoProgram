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
    public partial class SelectProduct : Form, IUserAction<ActionEventArgs>
    {
        static string[] SortByList = { "Product Code", "Description" };
        public SelectProduct()
        {
            InitializeComponent();
            foreach (var sorby in SortByList)
            {
                SortBy.Items.Add(sorby);
            }
            SortBy.SelectedIndex = 0;
            InitializeView();
            ProductDataDGV.Sort(ProductDataDGV.Columns[0], ListSortDirection.Ascending);
        }

        public DataView DataView { get; set; }
        public DataTable DataTable { get; set; }
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
        
        public event EventHandler<ActionEventArgs> ActionEvent;

        private void AcceptBtn_Click(object sender, EventArgs e)
        {
            if (ProductDataDGV.SelectedRows.Count == 1)
            { 
                var pcode = ProductDataDGV.SelectedRows[0].Cells[0];
                Product pdt = new Product().LoadJson(pcode.Value.ToString());
                Product obj = new Product();
                obj.Code = pdt.Code;
                obj.Description = pdt.Description;
                obj.UnitPrice = pdt.UnitPrice;

                obj.Quantity = Int32.Parse(Quantity.Value.ToString());
                obj.Discount = Discount.Value;
                ActionEvent(sender, new ActionEventArgs()
                {
                    ActionString = "Accept",
                    ActionObject = obj
                });
            }
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
