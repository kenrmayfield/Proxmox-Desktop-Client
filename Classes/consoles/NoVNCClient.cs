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

        public NoVncClient(MachineData machine)
        {
            this.machine = machine;
            InitializeComponent();

        }
        public void RequestWebConnection(string remote = "novcs")
        {
            CenterToScreen();
            InitializeWebView(remote);
        }
        private async void InitializeWebView(string remoteType)
        {
            if (remoteType == "novnc")
            {
                Name = "NoVNC";
                Text = "NoVNC";
            } else
            {
                Name = "xTermJS";
                Text = "xTermJS";
            }
            
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

                webView.CoreWebView2.Navigate(noVncUrl);
                
                // Inject JavaScript to call C# method on button click and handle resource loading errors
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
                        
                        const statusNoVNCelement = document.getElementById('noVNC_status');
                        const observer = new MutationObserver((mutationsList) => {
                            // Step 3: Iterate through the mutations
                            for (const mutation of mutationsList) {
                                // Check if the mutation is related to attributes or child nodes
                                if (mutation.type === 'attributes' || mutation.type === 'childList') {
                                    // Step 4: Send the mutation details as a message
                                    if (statusNoVNCelement.innerHTML.includes('403')) {
                                        window.chrome.webview.postMessage('resource403Error');
                                    }
                                    if (statusNoVNCelement.innerHTML.includes('Connected')) {
                                    window.chrome.webview.postMessage('sessionConnected');
                                    }
                                    
                                }
                            }
                        });

                        if (statusNoVNCelement) {
                            observer.observe(statusNoVNCelement, { attributes: true, childList: true, subtree: true });
                        }
                        
                        const statusXJSelement = document.getElementById('status_bar');
                        const observer2 = new MutationObserver((mutationsList) => {
                            // Step 3: Iterate through the mutations
                            for (const mutation of mutationsList) {
                                // Check if the mutation is related to attributes or child nodes
                                if (mutation.type === 'attributes' || mutation.type === 'childList') {
                                    // Step 4: Send the mutation details as a message
                                    if (statusXJSelement.innerHTML.includes('403')) {
                                        window.chrome.webview.postMessage('resource403Error');
                                    }
                                    if (statusXJSelement.innerHTML.includes('Connected')) {
                                    window.chrome.webview.postMessage('sessionConnected');
                                    }
                                    
                                }
                            }
                        });

                        if (statusXJSelement) {
                            observer2.observe(statusXJSelement, { attributes: true, childList: true, subtree: true });
                        }
                        
                    });
                    
                ";
                await webView.CoreWebView2.ExecuteScriptAsync(script);

                // Add message handler for fullscreen toggle and resource error
                webView.CoreWebView2.WebMessageReceived += WebView_WebMessageReceived;
            }
            catch (Exception ex)
            {
                Program.DebugPoint(JsonConvert.SerializeObject(ex));
            }
            
        }

        private void WebView_WebMessageReceived(object sender, CoreWebView2WebMessageReceivedEventArgs e)
        {
            string message = e.TryGetWebMessageAsString();
            
            if (message == "toggleFullscreen")
            {
                ToggleFullScreenMode();
            } 
            else if (message == "sessionConnected")
            {
                Show();
            }
            else if (message == "resource403Error")
            {
                MessageBox.Show("You do not have sufficent permissions to access the console.","Permission Denied");
                Close();
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

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            webView.Dispose();
            Dispose();
        }
    }
}
