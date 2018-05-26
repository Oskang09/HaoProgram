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
    public partial class PayMoney : Form
    {
        public Sales sales { get; set; }
        public PayMoney(Sales sales)
        {
            InitializeComponent();

            var customer = new Customer().LoadJson(sales.Customer);
            if (sales.Agent != "")
            {
                var agent = new Agent().LoadJson(sales.Agent);
                var ac = agent.AgentCommission;
                if (new AgentCommission().Exists(ac))
                {
                    var acobj = new AgentCommission().LoadJson(ac);
                    decimal total = 0;
                    foreach (var pdt in sales.Products)
                    {
                        foreach (var acom in acobj.Product)
                        {
                            if (acom.Item1.Code == pdt.Code)
                            {
                                if (acom.Item3 == CommissionType.INTEGER)
                                {
                                    total += acom.Item2;
                                }
                                if (acom.Item3 == CommissionType.PERCENTAGE)
                                {
                                    total += ((pdt.UnitPrice - pdt.Discount) * pdt.Quantity) * (acom.Item2 / 100);
                                }
                                break;
                            }
                        }
                    }
                    var comgain = agent.ComissionGained;
                    if (comgain.ContainsKey(sales.ConvertedSalesID))
                    {
                        agent.ComissionGained[sales.ConvertedSalesID] = total;
                    }
                    else
                    {
                        agent.ComissionGained.Add(sales.ConvertedSalesID, total);
                    }
                    MainWindow.getInstance.RM.InitializeView();
                }
                agent.SaveJson(agent.Name);
            }
            this.sales = sales;
            RefreshUI();
        }

        public void RefreshUI()
        {
            while (PaymentView.CurrentRow != null)
            {
                PaymentView.Rows.Remove(PaymentView.CurrentRow);
            }
            PaymentView.Rows.Add("Total Payable : ", "RM " + sales.GetNeedToPay());
            if (sales.Paid.Count > 0)
            {
                foreach (var kvp in sales.Paid)
                {
                    if (kvp.Key.StartsWith("CHEQUE"))
                    {
                        string chequenum = kvp.Key.Split('|')[1];
                        PaymentView.Rows.Add($"CHEQUE ( {chequenum } ) :", "RM " + kvp.Value.ToString("0.00"));
                    }
                    if (kvp.Key.StartsWith("CASH"))
                    {
                        PaymentView.Rows.Add("CASH :", "RM " + kvp.Value);
                    }
                }
            }
            PaymentView.Rows.Add("Total Amount Due : ", "RM " + sales.GetUnpaid());
        }

        private void PayBtn_Click(object sender, EventArgs e)
        {
            switch (PayBy.SelectedIndex)
            {
                case 0:
                    if (sales.Paid.ContainsKey(PayBy.Text))
                    {
                        sales.Paid[PayBy.Text] += Paid.Value;
                    }
                    else
                    {
                        sales.Paid.Add(PayBy.Text, Paid.Value);
                    }
                    break;
                case 1:
                    if (CHEQUE.Text != "")
                    {
                        if (sales.Paid.ContainsKey(PayBy.Text + "|" + CHEQUE.Text))
                        {
                            sales.Paid[PayBy.Text + "|" + CHEQUE.Text] += Paid.Value;
                        }
                        else
                        {
                            if (MessageBox.Show($"Did you want to add cheque number ( {CHEQUE.Text} ) ? ", "Verify Cheque Number", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                            {
                                sales.Paid.Add(PayBy.Text + "|" + CHEQUE.Text, Paid.Value);
                            }
                        }
                    }
                    break;
            }
            sales.SaveJson(sales.ConvertedSalesID);
            RefreshUI();
        }

        private void ExitBtn_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void PayBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (PayBy.SelectedIndex == 1)
            {
                CHEQUE.ReadOnly = false;
            }
            else
            {
                CHEQUE.ReadOnly = true;
            }
        }

        private void DeleteBtn_Click(object sender, EventArgs e)
        {
            if (PayBy.SelectedIndex == 1 && CHEQUE.Text != "")
            {
                if (sales.Paid.ContainsKey("CHEQUE|" + CHEQUE.Text))
                {
                    if (MessageBox.Show($"Did you sure want to remove the cheque ( {CHEQUE.Text} )", "Removeing Cheque", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                    {
                        sales.Paid.Remove("CHEQUE|" + CHEQUE.Text);
                    }
                }
            }
            RefreshUI();
        }

        private void PaymentView_SelectionChanged(object sender, EventArgs e)
        {
            Paid.Value = 0;
            CHEQUE.ResetText();
            if (PaymentView.SelectedRows.Count == 1)
            {
                var row = PaymentView.CurrentRow.Index;
                if (row != 0 && row != PaymentView.Rows.Count)
                {
                    var type = PaymentView.SelectedRows[0].Cells[0].Value.ToString();
                    if (type.StartsWith("CASH"))
                    { 
                        Paid.Value = sales.Paid["CASH"];
                        PayBy.Text = "CASH";
                    }
                    if (type.StartsWith("CHEQUE"))
                    {
                        var index1 = type.IndexOf("(");
                        var cheqnum = type.Substring(index1 + 2);
                        var index2 = cheqnum.IndexOf(")");
                        var cheq = cheqnum.Substring(0, --index2);

                        Paid.Value = sales.Paid[$"CHEQUE|{cheq}"];
                        PayBy.Text = "CHEQUE";
                        CHEQUE.Text = cheq;
                    }
                }
            }
        }
    }
}
