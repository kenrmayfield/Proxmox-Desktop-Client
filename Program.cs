using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Proxmox_Desktop_Client.Classes;
using Proxmox_Desktop_Client.Classes.pveAPI;

namespace Proxmox_Desktop_Client
{
    static class Program
    {
        // Create a new instance of Configurations with automatic saving
        public static Configurations    _Config = new Configurations();
        public static Ticker            _Ticker = new Ticker();
        public static Dictionary<string, object> _Panels = new Dictionary<string, object>();
        public static ApiClient         _Api;
        public static UserPermissions   _Permissions;
    
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ClientLogin());
        }
        
        public static void DebugPoint(string content)
        {
            bool debugMode = false;
            #if DEBUG
                debugMode = true;
            #endif
            
            Console.WriteLine(content);
        }
        
    }
}