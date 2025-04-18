﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Proxmox_Desktop_Client.Classes;
using Proxmox_Desktop_Client.Classes.pveAPI;
using Proxmox_Desktop_Client.Panels;

namespace Proxmox_Desktop_Client
{
    static class Program
    {
        // Create a new instance of Configurations with automatic saving
        public static Configurations                _Config = new Configurations();
        public static Dictionary<string, object>    _Panels = new Dictionary<string, object>();
        public static ApiClient                     _Api;
        public static string                        AppVersion = "1.2.7";
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
        
            string processName = Process.GetCurrentProcess().ProcessName;
            Process[] processes = Process.GetProcessesByName(processName);

            if (processes.Length > 1)
            {
                MessageBox.Show("Program is already running. Check your system tray.", "Already Running");
                Application.Exit();
            } else
            {
                try
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new ClientLogin());    
                } catch(Exception ex)
                {
                    DebugPoint("Program Failure (MAIN): " + Environment.NewLine + ex.StackTrace);
                }
            }
            
        }
        
        public static void DebugPoint(string content)
        {
            if (Debugger.IsAttached)
            {
                Console.WriteLine(content);
            }
            else
            {
                MessageBox.Show(content, "Program Error");
            }
        }
        
    }
}