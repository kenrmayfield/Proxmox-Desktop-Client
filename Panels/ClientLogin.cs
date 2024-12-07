using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Proxmox_Desktop_Client.Classes.pveAPI;
using Proxmox_Desktop_Client.Classes.pveAPI.objects;

namespace Proxmox_Desktop_Client.Panels
{
    public partial class ClientLogin : Form
    {
    
        public ClientLogin()
        {
            // Init ClientForm
            InitializeComponent();
            
            // Store Panel for Global Reference
            Program._Panels.Add("ClientLogin", this);
            
            // Preset Client Login Form & Shrink Window
            panel_credentials.Visible = false;
            Width = 290;
            
            // Load Credentials from Storage and test if server accessible
            LoadCredentials();
        }
        
        private void ClickOtp(object sender, EventArgs e)
        {
            textBox_otp.Text = string.Empty;
            textBox_otp.Visible = checkBox_otp.Checked;
        }

        private void ClickCheckServer(object sender = null, EventArgs e = null)
        {
            // Define & Pull Variables from Form
            string server = textBox_server.Text;
            string port = textBox_port.Text;
            bool validateSsl = checkBox_ssl.Checked;
            
            // Disable Fields & Shrink ClientLogin Window
            panel_credentials.Enabled = false;
            panel_server.Enabled = false;
            Width = 290;
            
            // Recenter Screen
            CenterToScreen();
            
            // Test Connection
            Program._Api = new ApiClient(server, port, validateSsl);
            
            // Get Realms            
            List<RealmData> realms = Program._Api.GetRealms();
            
            // Realms Valid
            if (realms != null && realms.Count > 0)
            {
                // Attach Data to Realms comboBox
                comboBox_realm.DataSource = realms;
                comboBox_realm.DisplayMember = "Comment";
                comboBox_realm.ValueMember = "Realm";
                
                // Set Default Selected
                string savedRealm = (string)Program._Config.GetSetting("Login_Realm");
                if (!string.IsNullOrEmpty(savedRealm))
                {
                    comboBox_realm.SelectedValue = savedRealm;
                } else
                {
                    comboBox_realm.SelectedValue = "pam";
                }
                
                // Expand Client Login for next step & enable fields.
                panel_credentials.Visible = true;
                Width = 570;
            }
            else
            {   
                // If Server is Offline, Reset State
                comboBox_realm.DataSource = null;
                Width = 290;
            }
            
            // Enable Fields & Center ClientLogin Window
            CenterToScreen();
            panel_credentials.Enabled = true;
            panel_server.Enabled = true;
        }

        private void ValidateLogin(object sender, EventArgs e)
        {
            string server = textBox_server.Text;
            string port = textBox_port.Text;
            bool validateSsl = checkBox_ssl.Checked;
            string username = textBox_username.Text;
            string password = textBox_password.Text;
            string realm = comboBox_realm.SelectedValue as string;
            string otp = checkBox_otp.Checked ? textBox_otp.Text : null;

            panel_credentials.Enabled = false;
            panel_server.Enabled = false;
            Program._Api = new ApiClient(server, port, validateSsl);

            if (Program._Api.LoginRequest(username, password, realm, otp))
            {
                SaveCredentials();
                
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
            bool remember = checkBox_remember.Checked;

            Program._Config.SetSetting("Login_Server", textBox_server.Text);
            Program._Config.SetSetting("Login_Port", textBox_port.Text);
            Program._Config.SetSetting("Login_CheckSSL", checkBox_ssl.Checked);
            
            if (remember)
            {
                Program._Config.SetSetting("Login_Username", textBox_username.Text);
                Program._Config.SetSetting("Login_Password", textBox_password.Text);
                Program._Config.SetSetting("Login_Realm", comboBox_realm.SelectedValue);
                Program._Config.SetSetting("Login_OTP", checkBox_otp.Checked);
            }
            else
            {
                Program._Config.SetSetting("Login_Username", "");
                Program._Config.SetSetting("Login_Password", "");
                Program._Config.SetSetting("Login_Realm", "pam");
                Program._Config.SetSetting("Login_OTP", false);
            }

            Program._Config.SetSetting("Login_Remember", remember);
        }

        public void LoadCredentials()
        {
            bool remember = (bool?) Program._Config.GetSetting("Login_Remember") ?? false;

            if (remember)
            {
                textBox_server.Text = (string)Program._Config.GetSetting("Login_Server");
                textBox_port.Text = (string)Program._Config.GetSetting("Login_Port");
                checkBox_ssl.Checked = (bool)Program._Config.GetSetting("Login_CheckSSL");    
                
                ClickCheckServer();
            }

            if (remember)
            {
                textBox_username.Text = (string)Program._Config.GetSetting("Login_Username");
                textBox_password.Text = (string)Program._Config.GetSetting("Login_Password");
                checkBox_otp.Checked = (bool)Program._Config.GetSetting("Login_OTP");
            }
            else
            {
                textBox_username.Text = "";
                textBox_password.Text = "";
                checkBox_otp.Checked = false;
            }

            checkBox_remember.Checked = remember;
            textBox_otp.Visible = checkBox_otp.Checked;
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
