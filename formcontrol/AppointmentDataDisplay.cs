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
using CSharpOskaAPI.GoogleApis;
using CSharpOskaAPI.Parse;
using System.IO;
using CSharpOskaAPI.Client;
using System.Drawing.Printing;

namespace HaoProgram
{
    public partial class AppointmentDataDisplay : UserControl, IUserAction<ActionEventArgs>
    {
        public event EventHandler<ActionEventArgs> ActionEvent;
        public DataView DataView { get; set; }
        public DataTable DataTable { get; set; }
        public static string Prefix = "APDD";
        public AppointmentDataDisplay()
        {
            InitializeComponent();
            DoubleBuffered = true;
            foreach (var str in DataManager.AppointmentTypes)
            {
                AppointmentType.Items.Add(str.Key);
            }
            InitializeView();
            MonthBox.Text = DateTime.Now.Month >= 10 ? DateTime.Now.Month.ToString() : $"0{DateTime.Now.Month}";
            YearBox.Text = DateTime.Now.Year.ToString();
            DataManager.InitializeSKey(this, Prefix);
        }

        public void InitializeView()
        {
            DataTable = new DataTable();
            DataTable.Columns.Add("ID");
            DataTable.Columns.Add("Date");
            DataTable.Columns.Add("Vehicle No");
            DataTable.Columns.Add("Type");
            DataTable.Columns.Add("Customer Name");
            DataTable.Columns.Add("Contact");
            DataTable.Columns.Add("Remark");
            DataView = DataTable.DefaultView;
            AppointmentDataDGV.DataSource = DataView;
            AppointmentDataDGV.Columns[0].Visible = false;
            foreach (var apt in new Appointment().GetList())
            {
                List<string> shortname = new List<string>();
                foreach (var name in apt.Checked)
                {
                    shortname.Add(DataManager.AppointmentTypes[name]);
                }

                var customer = new Customer().LoadJson(apt.Customer);
                DataTable.Rows.Add(
                    apt.Id,
                    apt.Reminder.ToString("yyyy/MM/dd"),
                    apt.Customer,
                    string.Join(" , ", shortname),
                    customer.Name,
                    customer.Contact,
                    ObjectParse.ArrayParseString(apt.Remarks));
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
        private void ExitBtn_Click(object sender, EventArgs e)
        {
            ActionEvent(sender, new ActionEventArgs()
            {
                ActionString = "CloseCurrent"
            });
        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            if (AppointmentDataDGV.SelectedRows.Count != 0 && AppointmentDataDGV.SelectedRows.Count < 2)
            {
                List<string> kvp = new List<string>();
                foreach (var item in AppointmentType.CheckedItems)
                {
                    kvp.Add(item.ToString());
                }

                var id = AppointmentDataDGV.SelectedRows[0].Cells[0];
                var tempvn = int.Parse(id.Value.ToString());
                Appointment apt = new Appointment()
                {
                    Id = tempvn,
                    Customer = VehicleNumber.Text,
                    Checked = kvp,
                    Reminder = AppointmentDate.Value,
                    RemindDate = AppointmentDate.Value.AddDays(-3),
                    Remarks = Remarks.Lines
                };
                apt.SaveJson(apt.Id.ToString());
                if (apt.Id != tempvn)
                {
                    new Appointment().DeleteJson(id.Value.ToString());
                }
                DataRow[] dr = DataTable.Select($"[ID]='{tempvn}'");
                dr[0].Delete();

                List<string> shortname = new List<string>();
                foreach (var name in apt.Checked)
                {
                    shortname.Add(DataManager.AppointmentTypes[name]);
                }

                var customer = new Customer().LoadJson(apt.Customer);

                DataTable.Rows.Add(
                    apt.Id,
                    apt.Reminder.ToString("yyyy/MM/dd"),
                    apt.Customer,
                    string.Join(" , ", shortname),
                    customer.Name,
                    customer.Contact,
                    ObjectParse.ArrayParseString(apt.Remarks));
            }
        }

        private void HistoryBtn_Click(object sender, EventArgs e)
        {
            if (AppointmentDataDGV.SelectedRows.Count != 0 && AppointmentDataDGV.SelectedRows.Count < 2)
            {
                var id = AppointmentDataDGV.SelectedRows[0].Cells[0];
                ActionEvent(sender, new ActionEventArgs()
                {
                    ActionString = "ViewScan",
                    ActionObject = new Appointment().LoadJson(id.Value.ToString())
                });
            }
        }

        private async void AppointmentDate_ValueChanged(object sender, EventArgs e)
        {
            HolidayEvent.ResetText();
            try
            {
                CalendarJSON calendar = await new CalendarUtil()
                {
                    TimeMin = AppointmentDate.Value,
                    MaxResult = 1
                }.GetCalendarAsync();
                if (calendar.items[0] != null)
                    if (calendar.items[0].start.date == AppointmentDate.Value.ToString("yyyy-MM-dd"))
                        HolidayEvent.Text = calendar.items[0].summary;
            }
            catch
            {
                HolidayEvent.Text = "Internet not available.";
            }
        }

        private void DataSelect(object sender, EventArgs e)
        {
            foreach (var ctl in FunctionUtil.GetAllChildren(InputLayout))
            {
                if (ctl is TextBox)
                    ((TextBox)ctl).ResetText();
                if (ctl is RichTextBox)
                    ((RichTextBox)ctl).ResetText();
                if (ctl is ComboBox)
                    if (VehicleNumber.Name != ctl.Name)
                        ((ComboBox)ctl).SelectedIndex = -1;
                if (ctl is DateTimePicker)
                    ((DateTimePicker)ctl).ResetText();
                if (ctl is CheckedListBox)
                    for (int i = 0; i < ((CheckedListBox)ctl).Items.Count; i++)
                        ((CheckedListBox)ctl).SetItemChecked(i, false);
            }
            var rowsCount = AppointmentDataDGV.SelectedRows.Count;
            if (rowsCount == 0 || rowsCount > 1) return;

            var id = AppointmentDataDGV.SelectedRows[0].Cells[0];
            var appointment = new Appointment().LoadJson(id.Value.ToString());
            if (appointment != null)
            {
                VehicleNumber.Text = appointment.Customer;
                AppointmentDate.Value = appointment.Reminder;
                Remarks.Lines = appointment.Remarks;
                for (int i = 0; i < AppointmentType.Items.Count; i++)
                {
                    foreach (var check in appointment.Checked)
                    {
                        if (check == AppointmentType.Items[i].ToString())
                            AppointmentType.SetItemChecked(i, true);
                    }
                }
                var customer = new Customer().LoadJson(appointment.Customer);
                Namebox.Text = customer.Name;
                ICorSSM.Text = customer.NRIC;
                AddressBox.Lines = customer.Address.ToArray();
                Phone_Number.Text = customer.Contact;
                Agent.Text = customer.Agent;
            }
        }

        private void AddBtn_Click(object sender, EventArgs e)
        {
            ActionEvent(sender, new ActionEventArgs()
            {
                ActionString = "OpenRequest"
            });
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

        private void MonthBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataView.RowFilter = $"[Date] LIKE '{YearBox.Text}/{MonthBox.Text}*'";
        }

        private void YearBox_TextChanged(object sender, EventArgs e)
        {
            DataView.RowFilter = $"[Date] LIKE '{YearBox.Text}/{MonthBox.Text}*'";
        }

        private void AppointmentDataDGV_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (AppointmentDataDGV.SelectedRows.Count == 1)
            {
                tabControl1.SelectedIndex = 0;
            }
        }

        private void PrintAppointment_Click(object sender, EventArgs e)
        {
            if (YearBox.Text != "" && MonthBox.Text != "")
            {
                PrintDocument pd = new PrintDocument();
                pd.PrintPage += (obj, ppe) =>
                {
                    buildAppointmentPrinting(ppe);
                };
                PageSetupDialog psd = new PageSetupDialog();
                psd.Document = pd;
                psd.AllowMargins = false;
                if (psd.ShowDialog() == DialogResult.OK)
                {
                    pd.PrinterSettings = psd.PrinterSettings;
                    PrintPreviewDialog pvd = new PrintPreviewDialog();
                    pvd.Document = pd;
                    ((Form)pvd).WindowState = FormWindowState.Maximized;
                    pvd.PrintPreviewControl.Zoom = 1;
                    pvd.ShowDialog();
                }
            }
        }
        public void buildAppointmentPrinting(PrintPageEventArgs e)
        {
            // Font List
            Font ab12 = new Font("Arial Black", 12, FontStyle.Bold);
            Font ab11 = new Font("Arial Black", 11, FontStyle.Bold);
            Font an10 = new Font("Arial Narrow", 10, FontStyle.Regular);
            Font ab10 = new Font("Arial Black", 10, FontStyle.Bold);
            Font ab9 = new Font("Arial Black", 9, FontStyle.Regular);
            Font an9 = new Font("Arial Narrow", 9, FontStyle.Regular);
            // Brush ( Black aja )
            Brush brush = Brushes.Black;
            // Pen
            Pen pen = new Pen(brush);
            // Drawing
            e.Graphics.DrawString($"{DataManager.UserInfo.CompanyName}", ab12, brush, new Point(20, 30));
            e.Graphics.DrawString($"Appointment {MonthBox.Text}/{YearBox.Text}", ab12, brush, new Point(600, 30));

            e.Graphics.DrawLine(pen, new Point(20, 60), new Point(800, 60));

            e.Graphics.DrawString("Date", ab10, brush, new Point(65, 60));
            e.Graphics.DrawString("Vehicle", ab10, brush, new Point(140, 60));
            e.Graphics.DrawString("Type", ab10, brush, new Point(240, 60));
            e.Graphics.DrawString("Customer", ab10, brush, new Point(320, 60));
            e.Graphics.DrawString("Contact", ab10, brush, new Point(550, 60));
            e.Graphics.DrawString("Remark", ab10, brush, new Point(650, 60));

            e.Graphics.DrawLine(pen, new Point(20, 80), new Point(800, 80));

            int count = 1;
            
            foreach (var apt in new Appointment().GetList().Where((x) => x.Reminder.ToString("yyyyMM") == YearBox.Text + MonthBox.Text).OrderBy(s => s.RemindDate))
            {
                var customer = new Customer().LoadJson(apt.Customer);
                List<string> shortname = new List<string>();
                foreach (var value in apt.Checked)
                {
                    shortname.Add(DataManager.AppointmentTypes[value]);
                }
                e.Graphics.DrawString(count.ToString(), an9, brush, new Point(40, 70 + (count * 20)));
                e.Graphics.DrawString(apt.Reminder.ToString("( ddd ) dd"), an9, brush, new Point(60, 70 + (count * 20)));
                e.Graphics.DrawString(customer.Vehicle, an9, brush, new Point(140, 70 + (count * 20)));
                e.Graphics.DrawString(string.Join(", ", shortname), an9, brush, new Point(240, 70 + (count * 20)));
                e.Graphics.DrawString(customer.Name, an9, brush, new Point(320, 70 + (count * 20)));
                e.Graphics.DrawString(customer.Contact, an9, brush, new Point(550, 70 + (count * 20)));
                e.Graphics.DrawString(string.Join(", " ,apt.Remarks), an9, brush, new Point(650, 70 + (count * 20)));
                count++;
            }
        }

        private void DeleteBtn_Click(object sender, EventArgs e)
        {
            if (AppointmentDataDGV.SelectedRows.Count == 1)
            {
                if (MessageBox.Show("Did you want to delete selected appointment?\n* Warning : After delete cant be recover without restore from backup", "Delete Appointment", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    var id = AppointmentDataDGV.SelectedRows[0].Cells[0];
                    new Appointment().DeleteJson(id.Value.ToString());
                    DataRow[] dr = DataTable.Select($"[ID]='{id.Value.ToString()}'");
                    dr[0].Delete();
                }
            }
        }

        private void AppointmentDataDGV_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            if (e.Column.Name == "ID")
            {
                e.Column.Visible = false;
            }
        }
    }
}
