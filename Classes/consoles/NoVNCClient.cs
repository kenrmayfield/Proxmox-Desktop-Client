using System;
using System.IO;
using System.Windows.Forms;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;
using Newtonsoft.Json;
using Proxmox_Desktop_Client.Classes.pveAPI.objects;

namespace Proxmox_Desktop_Client.Classes.consoles
{
    public partial class NoVncClient : Form
    {
        private readonly MachineData machine;
        private WebView2 webView;

        public NoVncClient(MachineData machine, string remote = "novcs")
        {
            this.machine = machine;

            InitializeComponent();
            CenterToScreen();

            InitializeWebView(remote);
        }

        private async void InitializeWebView(string remoteType)
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

                string ticketValue = Program._Api.DataTicket.ticket;

                var cookie = webView.CoreWebView2.CookieManager.CreateCookie(
                    "PVEAuthCookie",
                    ticketValue,
                    Program._Api.DataServerInfo.server,
                    "/"
                );

                cookie.IsHttpOnly = true;
                cookie.SameSite = CoreWebView2CookieSameSiteKind.Lax;

                webView.CoreWebView2.CookieManager.AddOrUpdateCookie(cookie);
                
                string consoleType = machine.Type == "qemu" ? "kvm" : machine.Type;
                string noVncUrl = $"https://{Program._Api.DataServerInfo.server}:{Program._Api.DataServerInfo.port}/?console={consoleType}&{remoteType}=1&vmid={machine.Vmid}&vmname={machine.Name}&node={machine.NodeName}&resize=scale&cmd=";
                Program.DebugPoint($"Navigating to URL: {noVncUrl}");

                webView.CoreWebView2.Navigate(noVncUrl);
                
                // Inject JavaScript to call C# method on button click
                string script = @"
                    document.addEventListener('DOMContentLoaded', function() {
                        var fullscreenButton = document.getElementById('noVNC_fullscreen_button');
                        if (fullscreenButton) {
                            fullscreenButton.addEventListener('click', function() {
                                window.chrome.webview.postMessage('toggleFullscreen');
                            });
                        } else {
                            console.error('Fullscreen button not found.');
                        }
                    });
                ";
                await webView.CoreWebView2.ExecuteScriptAsync(script);

                // Add message handler for fullscreen toggle
                webView.CoreWebView2.WebMessageReceived += WebView_WebMessageReceived;
            }
            catch (Exception ex)
            {
                Program.DebugPoint(JsonConvert.SerializeObject(ex));
            }

            Show();
        }

        private void WebView_WebMessageReceived(object sender, CoreWebView2WebMessageReceivedEventArgs e)
        {
            string message = e.TryGetWebMessageAsString();
            Program.DebugPoint($"Message received: {message}");
            if (message == "toggleFullscreen")
            {
                ToggleFullScreenMode();
            }
        }

        private void ToggleFullScreenMode()
        {
            if (WindowState == FormWindowState.Normal)
            {
                FormBorderStyle = FormBorderStyle.None;
                WindowState = FormWindowState.Maximized;
            }
            else
            {
                FormBorderStyle = FormBorderStyle.Sizable;
                WindowState = FormWindowState.Normal;
            }
        }
    }
}
