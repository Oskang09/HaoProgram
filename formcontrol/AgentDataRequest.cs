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
    public partial class AgentDataRequest : Form, IUserAction<ActionEventArgs>, IUpdateAction
    {
        public AgentDataRequest()
        {
            InitializeComponent();
            DoubleBuffered = true;
            UpdateUI();
        }

        public event EventHandler<ActionEventArgs> ActionEvent;

        public void UpdateUI()
        {
            AgentComission.Items.Clear();
            foreach (var ac in new AgentCommission().GetList())
            {
                AgentComission.Items.Add(ac.CategoryName);
            }
        }

        private void AddBtn_Click(object sender, EventArgs e)
        {
            if (new Agent().Exists(AgentName.Text))
            {
                if (MessageBox.Show("Did you want to view the existing agent data?", "Repeated Agent Name!", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    ActionEvent(sender, new ActionEventArgs()
                    {
                        ActionString = "ExistingAgent",
                        ActionObject = new Agent()
                        {
                            Name = AgentName.Text
                        }
                    });
                }
            }
            else
            {
                if (AgentName.Text == "")
                {
                    MessageBox.Show("Agent name cannot be empty!");
                }
                else
                {
                    ActionEvent(sender, new ActionEventArgs()
                    {
                        ActionString = "NewAgent",
                        ActionObject = new Agent()
                        {
                            Name = AgentName.Text,
                            Address = AgentAddress.Lines.ToList(),
                            Contact = AgentContact.Text,
                            NRIC = AgentIC.Text,
                            DateOfJoin = AgentDOJ.Value,
                            AgentCommission = AgentComission.Text != "" ? AgentComission.Text : null,
                            ComissionGained = new Dictionary<string, decimal>()
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
    }
}
