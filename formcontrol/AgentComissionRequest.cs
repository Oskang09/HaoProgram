using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HaoProgram
{
    public partial class AgentComissionRequest : Form, IUserAction<ActionEventArgs>
    {
        public AgentComissionRequest()
        {
            InitializeComponent();
            DoubleBuffered = true;
        }

        public event EventHandler<ActionEventArgs> ActionEvent;

        public void ActiveKeyEvent()
        {
            App.Current.MainWindow.KeyDown += MainWindow_KeyDown; ;
        }

        private void MainWindow_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void AddBtn_Click(object sender, EventArgs e)
        {
            if (new AgentCommission().Exists(ComCategory.Text))
            {
                if (MessageBox.Show("Did you want to view the existing commission category?", "Repeated Commission Category!", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    ActionEvent(sender, new ActionEventArgs()
                    {
                        ActionString = "ExistingCom",
                        ActionObject = new AgentCommission()
                        {
                            CategoryName = ComCategory.Text
                        }
                    });
                }
            }
            else
            {
                if (ComCategory.Text == "")
                {
                    MessageBox.Show("Commission Category cannot be empty!");
                }
                else
                {
                    ActionEvent(sender, new ActionEventArgs()
                    {
                        ActionString = "NewCom",
                        ActionObject = new AgentCommission()
                        {
                            CategoryName = ComCategory.Text,
                            Product = new List<Tuple<Product, decimal, CommissionType>>()
                        }
                    });
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ActionEvent(sender, new ActionEventArgs()
            {
                ActionString = "ExitRequest"
            });
        }
    }
}
