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
    public partial class AssignShorcutKey : Form, IUserAction<ActionEventArgs>
    {
        public AssignShorcutKey(string key)
        {
            InitializeComponent();
            ShortcutKey.Text = key;
        }

        public event EventHandler<ActionEventArgs> ActionEvent;

        private void OKBtn_Click(object sender, EventArgs e)
        {
            ActionEvent(sender, new ActionEventArgs()
            {
                ActionString = "Assign",
                ActionObject = ShortcutKey.Text
            });
            Close();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            ShortcutKey.Text = keyData.ToString();
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void ClearBtn_Click(object sender, EventArgs e)
        {
            ShortcutKey.Text = "";
        }
    }
}
