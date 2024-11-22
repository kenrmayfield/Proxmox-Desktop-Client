using System;
using System.IO;
using System.Windows.Forms;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;
using Newtonsoft.Json;
using Proxmox_Desktop_Client.Classes.pveAPI;
using Proxmox_Desktop_Client.Classes.pveAPI.objects;

namespace Proxmox_Desktop_Client.Classes.consoles
{
    public partial class NoVNCClient : Form
    {
        private ApiClient _Api;
        private MachineData _Machine;
        private WebView2 webView;

        private string varServer;
        private string varPort;
        private string consoleType;
        private string vmid;
        private string vmName;
        private string nodeName;
        private string remoteType;

        public NoVNCClient(ApiClient Api, MachineData Machine, string Remote = "novcs")
        {
            _Api = Api;
            _Machine = Machine;

            InitializeComponent();

            // Windows Configs
            CenterToScreen();
            varServer = Api._ServerUserData.server;
            varPort = Api._ServerUserData.port;
            consoleType = Machine.Type == "qemu" ? "kvm" : Machine.Type;
            remoteType = Remote;
            vmid = Machine.Vmid.ToString();
            vmName = Machine.Name;
            nodeName = Machine.NodeName;

            InitializeWebView();
        }

        private async void InitializeWebView()
        {
            try
            {
                webView = new WebView2
                {
                    Dock = DockStyle.Fill
                };
                this.Controls.Add(webView);

                // Specify a custom user data folder
                string userDataFolder = Path.Combine(Program._Config.appDataFolder, "WebView2Data");
                
                var env = await CoreWebView2Environment.CreateAsync(null, userDataFolder, new CoreWebView2EnvironmentOptions("--ignore-certificate-errors"));
                await webView.EnsureCoreWebView2Async(env);

                // Check if WebView2 is initialized
                if (webView.CoreWebView2 == null)
                {
                    Program.DebugPoint("WebView2 initialization failed.");
                    return;
                }

                string ticketValue = _Api._apiTicket.ticket;

                var cookie = webView.CoreWebView2.CookieManager.CreateCookie(
                    "PVEAuthCookie",
                    ticketValue,
                    varServer,
                    "/"
                );

                cookie.IsHttpOnly = true;
                cookie.SameSite = CoreWebView2CookieSameSiteKind.Lax;

                webView.CoreWebView2.CookieManager.AddOrUpdateCookie(cookie);
                
                string noVncUrl = $"https://{varServer}:{varPort}/?console={consoleType}&{remoteType}=1&vmid={vmid}&vmname={vmName}&node={nodeName}&resize=scale&cmd=";
                Program.DebugPoint($"Navigating to URL: {noVncUrl}");

                webView.CoreWebView2.Navigate(noVncUrl);
            }
            catch (Exception ex)
            {
                Program.DebugPoint(JsonConvert.SerializeObject(ex));
            }

            Show();
        }
    }
}
