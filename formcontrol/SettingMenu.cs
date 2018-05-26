using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using CSharpOskaAPI.Parse;
using CSharpOskaAPI.Client;
using CSharpOskaAPI.GoogleApis;
using CSharpOskaAPI.Error;
using System.Windows.Threading;
using System.Threading;
using System.IO;
using Newtonsoft.Json;
using CSharpOskaAPI.MultiThread;
using CSharpOskaAPI.IO;
using System.Diagnostics;

namespace HaoProgram
{
    public partial class SettingMenu : UserControl, IUserAction<ActionEventArgs>
    {
        public static string Prefix = "SM";
        public SettingMenu()
        {
            InitializeComponent();
            DoubleBuffered = true;
            tabControl1.ItemSize = new Size(tabControl1.Width / tabControl1.TabCount, 0); 
            UpdateUI();
            DataManager.InitializeSKey(this, Prefix);
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

        public void UpdateUI()
        {
            UserInfomation userinfo = DataManager.UserInfo;
            CompanyName.Text = ObjectParse.ObjectParseString(userinfo.CompanyName);
            SSM.Text = ObjectParse.ObjectParseString(userinfo.SSM);
            Address.Text = ObjectParse.ObjectParseString(userinfo.Address);
            TelNo.Text = ObjectParse.ObjectParseString(userinfo.TelNo);
            GSTRegNo.Text = ObjectParse.ObjectParseString(userinfo.GSTReg);
            footer.Lines = userinfo.InvoiceFooter;
            SQty.Text = userinfo.EditQTY;
            SUp.Text = userinfo.EditUP;
            Sd.Text = userinfo.EditDiscount;
            lastbk.Text = userinfo.LastBackup != null ? userinfo.LastBackup.ToString("yyyy-MM-dd hh:mm:ss tt") : "";
        }

        private void UpdateInfo_Click(object sender, EventArgs e)
        {
            DataManager.UserInfo.CompanyName = CompanyName.Text;
            DataManager.UserInfo.SSM = SSM.Text;
            DataManager.UserInfo.Address = Address.Text;
            DataManager.UserInfo.TelNo = TelNo.Text;
            DataManager.UserInfo.GSTReg = GSTRegNo.Text;
            DataManager.UserInfo.InvoiceFooter = footer.Lines;
            DataManager.UserInfo.SaveJson();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (SQty.Focused)
            {
                SQty.Text = keyData.ToString();
                DataManager.UserInfo.EditQTY = SQty.Text;
            }
            if (SUp.Focused)
            {
                SUp.Text = keyData.ToString();
                DataManager.UserInfo.EditUP = SUp.Text;
            }
            if (Sd.Focused)
            {
                Sd.Text = keyData.ToString();
                DataManager.UserInfo.EditDiscount = Sd.Text;
            }
            foreach (var sk in DataManager.UserInfo.ShortKeyData.Where((x) => x.Key.StartsWith(Prefix)))
            {
                if (sk.Value == keyData.ToString())
                {
                    (Controls.Find(sk.Key.Split('.')[1], true)[0] as Button).PerformClick();
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void BackupBtn_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
            {
                DataManager.UserInfo.LastBackup = DateTime.Now;
                DataManager.UserInfo.SaveJson();
                ZipCompress.Compress(DataManager.BASE_PATH, fbd.SelectedPath + "/HaoProgramBackup.cfcbkdata");
                UpdateUI();
                Process.Start("explorer.exe", "/select,\"" + fbd.SelectedPath + "\\HaoProgramBackup.cfcbkdata\"");
            }
        }

        private void RestoreBtn_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("*Warning : Restore from backup will replaced existing data!", "Restore from backup", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog();
                ofd.Filter = "CFC Backup Data (*.cfcbkdata)|*.cfcbkdata";
                if (ofd.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(ofd.FileName))
                {
                    ZipCompress.Decompress(ofd.FileName, DataManager.BASE_PATH, true);
                    new DataManager();
                    MainWindow.getInstance.InitializeUI();
                    MessageBox.Show("Restored sucessfully!");
                }
            }
        }
    }
}
