using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CSharpOskaAPI.Printer;
using System.IO;
using CSharpOskaAPI.Error;
using CSharpOskaAPI.MultiThread;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.CodeDom.Compiler;
using System.Diagnostics;

namespace HaoProgram
{
    public partial class AppointmentScanRequest : Form, IUserAction<ActionEventArgs>
    {
        public event EventHandler<ActionEventArgs> ActionEvent;
        private Customer customer;
        private string direc_path;
        private List<string> images;

        public AppointmentScanRequest(Customer cmr)
        {
            customer = cmr;
            direc_path = $"{AppDomain.CurrentDomain.BaseDirectory}/data/customer/{customer.Vehicle}/";
            images = new List<string>();
            InitializeComponent();
            if (!Directory.Exists(direc_path))
            {
                Directory.CreateDirectory(direc_path);
            }
            
            images = Directory.GetFiles(direc_path).ToList();
            foreach (var img in images)
            {
                ImageChecker.Items.Add($"{Path.GetFileName(img)}");
            }
        }

        private void ImageChecker_SelectedIndexChanged(object sender, EventArgs e)
        {
            string[] format = new string[] { ".png", ".jpg", ".jpeg" };
            if (ImageChecker.SelectedIndex > -1)
            {
                if (format.Contains(Path.GetExtension(images[ImageChecker.SelectedIndex])))
                {
                    using (var bmp = new Bitmap(images[ImageChecker.SelectedIndex]))
                    {
                        ScannerImage.Image = new Bitmap(bmp);
                    }
                }
                else
                {
                    ScannerImage.Image = null;
                }
            }
        }

        private void DoneBtn_Click(object sender, EventArgs e)
        {
            ActionEvent(sender, new ActionEventArgs()
            {
                ActionObject = customer,
                ActionString = "Accept"
            });
            Close();
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            if (ImageChecker.SelectedIndex > -1)
            {
                File.Delete(images[ImageChecker.SelectedIndex]);
                images.RemoveAt(ImageChecker.SelectedIndex);
                ImageChecker.Items.RemoveAt(ImageChecker.SelectedIndex);
            }
        }

        private void AcceptBtn_Click_1(object sender, EventArgs e)
        {
            ScannerUtil snu = new ScannerUtil(null,
            (sender1, data) =>
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    string nfp = $"{AppDomain.CurrentDomain.BaseDirectory}/data/customer/{customer.Vehicle}/{((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds()}.png";
                    var fs = File.Create(nfp);
                    data.GetNativeImageStream().CopyTo(fs);
                    fs.Close();
                    images.Add(nfp);
                    CrossThread.Perform(ImageChecker, () =>
                        ImageChecker.Items.Add($"{Path.GetFileName(nfp)}"));
                }
            },
            (sender1, error) =>
            {
                ErrorMessage.WriteErrorLog(DataManager.ERROR_TRACKER_PATH, error.ReturnCode.ToString());
            });

            SCANNER:
            if (!snu.StartScanner(DataManager.ERROR_TRACKER_PATH))
            {
                if (MessageBox.Show("Did you want to retry to scan?", "Retry scanner", MessageBoxButtons.RetryCancel, MessageBoxIcon.Question) == DialogResult.Retry)
                    goto SCANNER;
            }
        }

        private void ScannerImage_Click(object sender, EventArgs e)
        {
            if (ImageChecker.SelectedIndex > -1)
            {
                Process.Start(images[ImageChecker.SelectedIndex]);
            }
        }

        private void LoadBtn_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "All files (*.*)|*.*";
            ofd.Multiselect = false;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string nfp = $"{AppDomain.CurrentDomain.BaseDirectory}/data/customer/{customer.Vehicle}/{((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds()}{Path.GetExtension(ofd.FileName)}";
                File.Copy(ofd.FileName, nfp);
                images.Add(nfp);
                ImageChecker.Items.Add($"{Path.GetFileName(nfp)}");
            }
        }
    }
}
