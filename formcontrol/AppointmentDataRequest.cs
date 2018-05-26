using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NTwain.Data;
using System.Reflection;
using CSharpOskaAPI.Printer;
using CSharpOskaAPI.GoogleApis;
using CSharpOskaAPI.Form;
using CSharpOskaAPI.Client;
using CSharpOskaAPI;

namespace HaoProgram
{
    public partial class AppointmentDataRequest : Form, IUserAction<ActionEventArgs>
    {
        public AppointmentDataRequest()
        {
            InitializeComponent();
            DoubleBuffered = true;

            VehicleNumber.Items.Add("");
            foreach (var vn in new Customer().GetListString())
            {
                VehicleNumber.Items.Add(vn);
            }
            foreach (var str in DataManager.AppointmentTypes)
            {
                AppointmentType.Items.Add(str.Key);
            }
            AppointmentDate.MinDate = DateTime.Now;
        }

        public event EventHandler<ActionEventArgs> ActionEvent;
        
        public Appointment generateObject()
        {
            List<string> kvp = new List<string>();
            foreach (var item in AppointmentType.CheckedItems)
            {
                kvp.Add(item.ToString());
            }

            Appointment app = new Appointment()
            {
                Id = Appointment.getCurrentId(true),
                Customer = VehicleNumber.Text,
                Checked = kvp,
                Remarks = Remarks.Lines,
                Reminder = AppointmentDate.Value,
                RemindDate = AppointmentDate.Value.AddDays(-3)
            };
            return app;
        }

        private void SaveCloseBtn_Click(object sender, EventArgs e)
        {
            if (VehicleNumber.SelectedIndex > 0)
                if (MessageBox.Show($"Did you want to select {AppointmentDate.Value.ToString("dddd , dd MMMM yyyy")} for appointment?", "Date Verify", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                    ActionEvent(sender, new ActionEventArgs()
                    {
                        ActionString = "Save",
                        ActionObject = generateObject()
                    });
        }

        private void ScanSaveButton_Click(object sender, EventArgs e)
        {
            if (VehicleNumber.SelectedIndex > 0)
                if (MessageBox.Show($"Did you want to select {AppointmentDate.Value.ToString("dddd , dd MMMM yyyy")} for appointment?", "Date Verify", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                    ActionEvent(sender, new ActionEventArgs()
                    {
                        ActionString = "StartScan",
                        ActionObject = generateObject()
                    });
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            ActionEvent(sender, new ActionEventArgs()
            {
                ActionString = "ExitRequest"
            });
        }

        private async void AppointmentDate_ValueChangedAsync(object sender, EventArgs e)
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
        
        private void OnComboSort(object sender, EventArgs e)
        {
            foreach (var ctl in FunctionUtil.GetAllChildren(InputLayout))
            {
                if (ctl is TextBox)
                    ((TextBox)ctl).ResetText();
                if (ctl is RichTextBox)
                    ((RichTextBox)ctl).ResetText();
                if (ctl is ComboBox)
                    if (ctl.Name != VehicleNumber.Name)
                        ((ComboBox)ctl).SelectedIndex = -1;
                if (ctl is DateTimePicker)
                    ((DateTimePicker)ctl).ResetText();
                if (ctl is CheckedListBox)
                    for (int i = 0; i < ((CheckedListBox)ctl).Items.Count; i++)
                        ((CheckedListBox)ctl).SetItemChecked(i, false);
            }
            VehicleNumber.Items.Clear();
            VehicleNumber.Items.Add("");
            foreach (var vn in new Customer().GetListString().Where((x) => StringExtension.Contains(x, VehicleNumber.Text, StringComparison.OrdinalIgnoreCase)))
            {
                VehicleNumber.Items.Add(vn);
            }
            VehicleNumber.DroppedDown = true;
            VehicleNumber.Select(VehicleNumber.Text.Length, 0);
        }

        private void VehicleNumber_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (VehicleNumber.SelectedIndex > 0)
            {
                var customer = new Customer().LoadJson(VehicleNumber.Text);
                if (customer != null)
                {
                    Namebox.Text = customer.Name;
                    ICorSSM.Text = customer.NRIC;
                    AddressBox.Lines = customer.Address.ToArray();
                    Phone_Number.Text = customer.Contact;
                    Agent.Text = customer.Agent;
                }
            }
        }
    }
}
