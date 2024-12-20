using System;
using System.IO;
using System.Windows.Forms;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;
using Newtonsoft.Json;

namespace Proxmox_Desktop_Client.Classes.consoles
{
    public partial class WebGUI : Form
    {
        private WebView2 webView;

        public WebGUI()
        {
            InitializeComponent();
            CenterToScreen();
            InitializeWebView();
            this.WindowState = FormWindowState.Maximized;
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

                string userDataFolder = Path.Combine(Program._Config.AppDataFolder, "WebView2Data");
                var env = await CoreWebView2Environment.CreateAsync(null, userDataFolder, new CoreWebView2EnvironmentOptions("--ignore-certificate-errors"));
                await webView.EnsureCoreWebView2Async(env);

                if (webView.CoreWebView2 == null)
                {
                    Program.DebugPoint("WebView2 initialization failed.");
                    return;
                }

                // Enable JavaScript
                webView.CoreWebView2.Settings.IsScriptEnabled = true;

                // Handle navigation events
                webView.CoreWebView2.NavigationCompleted += WebView_NavigationCompleted;

                string ticketValue = Program._Api.DataTicket.ticket;

                var cookie = webView.CoreWebView2.CookieManager.CreateCookie(
                    "PVEAuthCookie",
                    ticketValue,
                    Program._Api.DataServerInfo.server,
                    "/"
                );

                cookie.IsHttpOnly = false;
                cookie.SameSite = CoreWebView2CookieSameSiteKind.Lax;

                webView.CoreWebView2.CookieManager.AddOrUpdateCookie(cookie);
                
                string WebURL = $"https://{Program._Api.DataServerInfo.server}:{Program._Api.DataServerInfo.port}/#v1:0:18:4:::::::";

                webView.CoreWebView2.Navigate(WebURL);
                
            }
            catch (Exception ex)
            {
                Program.DebugPoint(JsonConvert.SerializeObject(ex));
            }
        }

        private void WebView_NavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            if (e.IsSuccess)
            {
                Program.DebugPoint("Navigation completed successfully.");
            }
            else
            {
                Program.DebugPoint($"Navigation failed with error: {e.WebErrorStatus}");
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            webView.Dispose();
            Dispose();
        }
    }
}
