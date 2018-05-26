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
    public partial class CustomerDataRequest : Form, IUserAction<ActionEventArgs>, IUpdateAction
    {
        public event EventHandler<ActionEventArgs> ActionEvent;

        public CustomerDataRequest()
        {
            InitializeComponent();
            DoubleBuffered = true;
            UpdateUI();
        }

        private void AddBtn_Click(object sender, EventArgs e)
        {
            if (VehicleNumberBox.Text == "")
            {
                MessageBox.Show("Vehicle number cannot be empty!");
                return;
            }
            if (new Customer().Exists(VehicleNumberBox.Text))
            {
                if (MessageBox.Show("Did you want to view the existing customer data?", "Repeated Vehicle Number!", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    ActionEvent(sender, new ActionEventArgs()
                    {
                        ActionString = "ExistingCustomer",
                        ActionObject = new Customer()
                        {
                            Vehicle = VehicleNumberBox.Text
                        }
                    });
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(VehicleNumberBox.Text))
                {
                    MessageBox.Show("Vehicle number cannot be empty!");
                }
                else
                {
                    ActionEvent(sender, new ActionEventArgs()
                    {
                        ActionString = "NewCustomer",
                        ActionObject = new Customer()
                        {
                            Vehicle = ObjectParse.ObjectParseString(VehicleNumberBox.Text),
                            NRIC = ObjectParse.ObjectParseString(ICorSSM.Text),
                            Name = ObjectParse.ObjectParseString(NameBox.Text),
                            Address = ObjectParse.ArrayParseStringArray(AddressBox.Lines).ToList(),
                            Contact = ObjectParse.ObjectParseString(PhoneBox.Text),
                            Agent = Agent.Text != "" ? Agent.Text : null
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

        public void UpdateUI()
        {
            Agent.Items.Clear();
            foreach (var agent in new Agent().GetList())
            {
                Agent.Items.Add(agent.Name);
            }
        }
    }
}
