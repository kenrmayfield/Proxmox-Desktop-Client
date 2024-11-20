using System;
using System.Windows.Forms;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;
using Proxmox_Desktop_Client.Classes.pveAPI;
using Proxmox_Desktop_Client.Classes.pveAPI.objects;

namespace Proxmox_Desktop_Client.Classes.consoles
{
    public partial class NoVNCClient : Form
    {
        private WebView2 webView;
        private bool skipSSL;
        private dataServerUser _ServerUserData;
        private MachineData _Machine;
        private ApiClient _Api;

        public NoVNCClient(ApiClient Api, MachineData Machine)
        {
            _Api = Api;
            _ServerUserData = Api._ServerUserData;
            _Machine = Machine;
            skipSSL = Api._ServerUserData.skipSSL;

            InitializeComponent();
            this.WindowState = FormWindowState.Maximized; // Maximize the window
            Show();
            CenterToScreen();
            InitializeWebView();

            this.KeyDown += NoVNCClient_KeyDown;
        }

        private async void InitializeWebView()
        {
            webView = new WebView2
            {
                Dock = DockStyle.Fill
            };
            this.Controls.Add(webView);

            var env = await CoreWebView2Environment.CreateAsync(null, null, new CoreWebView2EnvironmentOptions("--ignore-certificate-errors"));
            await webView.EnsureCoreWebView2Async(env);

            string server = _ServerUserData.server;
            string ticketValue = _Api._apiTicket.ticket;

            var cookie = webView.CoreWebView2.CookieManager.CreateCookie(
                "PVEAuthCookie",
                ticketValue,
                server,
                "/"
            );

            cookie.IsHttpOnly = true;
            cookie.SameSite = CoreWebView2CookieSameSiteKind.Lax;

            webView.CoreWebView2.CookieManager.AddOrUpdateCookie(cookie);

            string port = _ServerUserData.port;
            string consoleType = _Machine.Type == "qemu" ? "kvm" : _Machine.Type;
            string vmid = _Machine.Vmid.ToString();
            string machineName = _Machine.Name;
            string nodeName = _Machine.NodeName;

            string noVncUrl = $"https://{server}:{port}/?console={consoleType}&novnc=1&vmid={vmid}&vmname={machineName}&node={nodeName}&resize=scale&cmd=";

            webView.CoreWebView2.Navigate(noVncUrl);

            // Handle full-screen requests
            webView.CoreWebView2.ContainsFullScreenElementChanged += CoreWebView2_ContainsFullScreenElementChanged;
        }

        private void CoreWebView2_ContainsFullScreenElementChanged(object sender, object o)
        {
            if (webView.CoreWebView2.ContainsFullScreenElement)
            {
                this.FormBorderStyle = FormBorderStyle.None;
                this.WindowState = FormWindowState.Maximized;
            }
            else
            {
                this.FormBorderStyle = FormBorderStyle.Sizable;
                this.WindowState = FormWindowState.Maximized;
            }
        }

        private void NoVNCClient_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F11)
            {
                ToggleMaximize();
            }
        }

        private void ToggleMaximize()
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal;
            }
            else
            {
                this.WindowState = FormWindowState.Maximized;
            }
        }
    }
}
