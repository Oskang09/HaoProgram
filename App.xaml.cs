using CSharpOskaAPI.Error;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace HaoProgram
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        public App()
        { 
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            if (!principal.IsInRole(WindowsBuiltInRole.Administrator))
            {
                if (System.Windows.Forms.MessageBox.Show("You must run this application as administrator.\n Do you want to restart the application in administrator mode.", "Permission required", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    System.Windows.Forms.Application.Exit();
                }
                else
                {
                    ProcessStartInfo proc = new ProcessStartInfo();
                    proc.UseShellExecute = true;
                    proc.WorkingDirectory = Environment.CurrentDirectory;
                    proc.FileName = System.Windows.Forms.Application.ExecutablePath;
                    proc.Verb = "runas";

                    try
                    {
                        Process.Start(proc);
                    }
                    catch
                    {
                        return;
                    }
                    Current.Shutdown();
                }
            }
            
            SplashScreen ss = new SplashScreen("assets/cfc_logo_ss.png");
            ss.Show(true, true);

            Dispatcher.UnhandledException += (sender, e) =>
            {
                ErrorMessage.WriteErrorLog(DataManager.ERROR_TRACKER_PATH, $"UI Thread Error : {e.Exception.Message}");
            };
            AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
            {
                ErrorMessage.WriteErrorLog(DataManager.ERROR_TRACKER_PATH, $"Unhandled Error : {e.ExceptionObject}");
            };
            System.Windows.Forms.Application.ThreadException += (sender, e) =>
            {
                ErrorMessage.WriteErrorLog(DataManager.ERROR_TRACKER_PATH, $"Thread Error : {e.Exception}");
            };
        }
    }
}
