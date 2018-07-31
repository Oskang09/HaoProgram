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
using CSharpOskaAPI.Form;
using System.Windows.Input;
using CSharpOskaAPI;

namespace HaoProgram
{
    public partial class AgentDataDisplay : UserControl, IUserAction<ActionEventArgs>
    {
        static string[] sortbyList = { "Name", "NRIC" };
        public event EventHandler<ActionEventArgs> ActionEvent;
        public static string Prefix = "ADD";
        protected override CreateParams CreateParams
        {
            get
            {
                var parms = base.CreateParams;
                parms.Style &= ~0x02000000;  // Turn off WS_CLIPCHILDREN
                return parms;
            }
        }
        public AgentDataDisplay()
        {
            InitializeComponent();
            DoubleBuffered = true;
            RefreshUI(null);

            foreach (var str in sortbyList)
            {
                SortBy.Items.Add(str);
            }
            SortBy.SelectedIndex = 0;
            
            DataManager.InitializeSKey(this, Prefix);
        }
        

        public void RefreshUI(Agent selected)
        {
            while (AgentDataDGV.CurrentRow != null)
            {
                AgentDataDGV.Rows.Remove(AgentDataDGV.CurrentRow);
            }
            foreach (var agent in new Agent().GetList())
            {
                int addedrows = AgentDataDGV.Rows.Add(
                    agent.Name,
                    agent.NRIC,
                    agent.Contact,
                    ObjectParse.ArrayParseString(agent.Address.ToArray()),
                    agent.DateOfJoin.ToString("dd/MM/yyyy"),
                    agent.AgentCommission);

                if (selected != null && selected.Name == agent.Name)
                {
                    AgentDataDGV.Rows[addedrows].Selected = true;
                }
            }
        }
        
        public void RefreshUI(IEnumerable<Agent> queries, Agent selected)
        {
            while (AgentDataDGV.CurrentRow != null)
            {
                AgentDataDGV.Rows.Remove(AgentDataDGV.CurrentRow);
            }
            foreach (var agent in queries)
            {
                int addedrows = AgentDataDGV.Rows.Add(
                    agent.Name, 
                    agent.NRIC, 
                    agent.Contact, 
                    ObjectParse.ArrayParseString(agent.Address.ToArray()), 
                    agent.DateOfJoin.ToString("dd/MM/yyyy"),
                    agent.AgentCommission);

                if (selected != null && selected.Name == agent.Name)
                {
                    AgentDataDGV.Rows[addedrows].Selected = true;
                }
            }
        }

        private void AddBtn_Click(object sender, EventArgs e)
        {
            ActionEvent(sender, new ActionEventArgs()
            {
                ActionString = "OpenRequest"
            });
        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            if (AgentDataDGV.SelectedRows.Count != 0 && AgentDataDGV.SelectedRows.Count > 0)
            {
                var agentname = AgentDataDGV.SelectedRows[0].Cells[0];
                Agent agent = new Agent().LoadJson(agentname.Value.ToString());
                var temp = agent;
                agent = new Agent()
                {
                    Name = AgentName.Text,
                    Address = AgentAddress.Lines.ToList(),
                    Contact = AgentContact.Text,
                    NRIC = AgentIC.Text,
                    DateOfJoin = AgentDOJ.Value,
                    AgentCommission = AgentComission.Text,
                    ComissionGained = agent.ComissionGained
                };
                agent.SaveJson(agent.Name);
                if (temp.Name != agent.Name)
                {
                    new Agent().DeleteJson(temp.Name);
                }
                DataManager.UpdateData(temp, agent);
                MainWindow.getInstance.CDD.UpdateAgent();
                MainWindow.getInstance.POS.UpdateAgent();
                MainWindow.getInstance.RM.UpdateAgent();
                RefreshUI(agent);
            }
        }

        private void DeleteBtn_Click(object sender, EventArgs e)
        {
            if (AgentDataDGV.SelectedRows.Count != 0 && AgentDataDGV.SelectedRows.Count > 0)
            {
                if (MessageBox.Show("Did you want to delete selected agent?\n* Warning : After delete cant be recover without restore from backup", "Delete Agent", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    var agentname = AgentDataDGV.SelectedRows[0].Cells[0];
                    Agent agent = new Agent().LoadJson(agentname.Value.ToString());
                    agent.DeleteJson(agentname.Value.ToString());
                    DataManager.UpdateData(agent, null);
                    RefreshUI(null);
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

        private void OnSortTyping(object sender, EventArgs e)
        {
            IEnumerable<Agent> query = null;
            switch (SortBy.SelectedIndex)
            {
                case 0:
                    query = from obj in new Agent().GetList()
                            where StringExtension.Contains(obj.Name, SortBox.Text, StringComparison.OrdinalIgnoreCase)
                            where obj.Name.Contains(SortBox.Text)
                            select obj;
                    break;
                case 1:
                    query = from obj in new Agent().GetList()
                            where StringExtension.Contains(obj.NRIC, SortBox.Text, StringComparison.OrdinalIgnoreCase)
                            select obj;
                    break;
            }
            RefreshUI(query, null);
        }

        private void DGVSelect(object sender, EventArgs e)
        {
            foreach (var ctl in FunctionUtil.GetAllChildren(InputLayout))
            {
                if (ctl is TextBox)
                    ((TextBox)ctl).ResetText();
                if (ctl is RichTextBox)
                    ((RichTextBox)ctl).ResetText();
                if (ctl is ComboBox)
                    ((ComboBox)ctl).SelectedIndex = -1;
                if (ctl is DateTimePicker)
                    ((DateTimePicker)ctl).ResetText();
            }

            var rowsCount = AgentDataDGV.SelectedRows.Count;
            if (rowsCount == 0 || rowsCount > 1) return;

            var agentName = AgentDataDGV.SelectedRows[0].Cells[0];
            var agent = new Agent().LoadJson(agentName.Value.ToString());
            if (agent != null)
            {
                AgentName.Text = ObjectParse.ObjectParseString(agent.Name);
                AgentIC.Text = ObjectParse.ObjectParseString(agent.NRIC);
                AgentContact.Text = ObjectParse.ObjectParseString(agent.Contact);
                AgentAddress.Lines = ObjectParse.ArrayParseStringArray(agent.Address.ToArray());
                AgentDOJ.Value = agent.DateOfJoin;
                AgentComission.Text = ObjectParse.ObjectParseString(
                    agent.AgentCommission);
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

        private void EditBtn_Click(object sender, EventArgs e)
        {
            foreach (var ctl in FunctionUtil.GetAllChildren(InputLayout))
            {
                if (ctl is TextBox)
                    ((TextBox)ctl).ReadOnly = !((TextBox)ctl).ReadOnly;
                if (ctl is RichTextBox)
                    ((RichTextBox)ctl).ReadOnly = !((RichTextBox)ctl).ReadOnly;
                if (ctl is ComboBox)
                    ((ComboBox)ctl).Enabled = !((ComboBox)ctl).Enabled;
                if (ctl is DateTimePicker)
                    ((DateTimePicker)ctl).Enabled = !((DateTimePicker)ctl).Enabled;
            }
        }
    }
}
