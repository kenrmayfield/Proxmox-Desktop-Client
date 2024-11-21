using System;
using System.Diagnostics;
using System.Windows.Forms;
using Proxmox_Desktop_Client.Classes;
using Proxmox_Desktop_Client.Classes.pveAPI;

namespace Proxmox_Desktop_Client
{
    public partial class ClientLogin : Form
    {
        private readonly Ticker _ticker;
        private ApiClient _api;

        public ClientLogin()
        {
            _ticker = Program._Ticker;
            InitializeComponent();
            Program._Panels.Add("ClientLogin", this);
            panel_credentials.Visible = false;
            Width = 290;
            LoadCredentials();
            _ticker.Start("SessionCheck", 60000, null);
        }

        private void ClickOtp(object sender, EventArgs e)
        {
            textBox_otp.Text = string.Empty;
            textBox_otp.Visible = checkBox_otp.Checked;
        }

        private async void ClickCheckServer(object sender = null, EventArgs e = null)
        {
            string server = textBox_server.Text;
            string port = textBox_port.Text;
            bool validateSsl = checkBox_ssl.Checked;
            
            this.panel_credentials.Enabled = false;
            this.panel_server.Enabled = false;
            this.Width = 290;
            CenterToScreen();
            
            Program.DebugPoint($"Connecting to server: {server}:{port}, SSL:{validateSsl}");
            _api = new ApiClient(server, port, validateSsl);
            
            Program.DebugPoint($"Collecting Realm Data...");
            var realms = await _api.GetRealmsAsync();

            Program.DebugPoint($"Validating Realm Data...");
            if (realms != null && realms.Count > 0)
            {
                comboBox_realm.DataSource = realms;
                comboBox_realm.DisplayMember = "Comment";
                comboBox_realm.ValueMember = "Realm";
                
                string savedRealm = (string)Program._Config.GetSetting("Login_Realm");
                if (!string.IsNullOrEmpty(savedRealm))
                {
                    comboBox_realm.SelectedValue = savedRealm;
                }
                
                Program.DebugPoint($"Has Realm Data...");
                panel_credentials.Visible = true;
                Width = 570;
            }
            else
            {   
                Program.DebugPoint($"No Realm Data/Connection Error...");
                comboBox_realm.DataSource = null;
                Width = 290;
            }
            
            CenterToScreen();
            this.panel_credentials.Enabled = true;
            this.panel_server.Enabled = true;
        }

        private async void ValidateLogin(object sender, EventArgs e)
        {
            string server = textBox_server.Text;
            string port = textBox_port.Text;
            bool validateSsl = checkBox_ssl.Checked;
            string username = textBox_username.Text;
            string password = textBox_password.Text;
            string realm = comboBox_realm.SelectedValue as string;
            string otp = textBox_otp.Text;

            panel_credentials.Enabled = false;
            panel_server.Enabled = false;
            _api = new ApiClient(server, port, validateSsl);

            if (!checkBox_otp.Checked) { otp = null; } 

            if (await _api.LoginAsync(username, password, realm, otp))
            {
                SaveCredentials();
                Program._Api = _api;
                
                if (Program._Panels.ContainsKey("MainPanel"))
                {
                    Program._Panels.Remove("MainPanel");
                }
                
                MainPanel newWindow = new MainPanel();
                Program._Panels.Add("MainPanel", newWindow);
                Hide();
            }

            panel_credentials.Enabled = true;
            panel_server.Enabled = true;
        }

        private void SaveCredentials()
        {
            Program._Config.SetSetting("Login_Server", textBox_server.Text);
            Program._Config.SetSetting("Login_Port", textBox_port.Text);
            Program._Config.SetSetting("Login_CheckSSL", checkBox_ssl.Checked);
            Program._Config.SetSetting("Login_Username", textBox_username.Text);
            Program._Config.SetSetting("Login_Password", textBox_password.Text);
            Program._Config.SetSetting("Login_Realm", comboBox_realm.SelectedValue);
            Program._Config.SetSetting("Login_OTP", checkBox_otp.Checked);
            Program._Config.SetSetting("Login_Remember", checkBox_remember.Checked);
        }

        private void LoadCredentials()
        {
            if (Program._Config.GetSetting("Login_Remember") == null) return;

            textBox_server.Text = (string)Program._Config.GetSetting("Login_Server");
            textBox_port.Text = (string)Program._Config.GetSetting("Login_Port");
            checkBox_ssl.Checked = (bool)Program._Config.GetSetting("Login_CheckSSL");
            ClickCheckServer();
            textBox_username.Text = (string)Program._Config.GetSetting("Login_Username");
            textBox_password.Text = (string)Program._Config.GetSetting("Login_Password");
            checkBox_otp.Checked = (bool)Program._Config.GetSetting("Login_OTP");
            checkBox_remember.Checked = (bool)Program._Config.GetSetting("Login_Remember");
            if ((bool)Program._Config.GetSetting("Login_OTP")) { textBox_otp.Visible = true; } else { textBox_otp.Visible = false; } 
            
        }

        private void TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

    }
}
