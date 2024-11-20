// ClientLogin.cs
using System;
using System.Text.Json;
using System.Windows.Forms;
using Newtonsoft.Json;
using Proxmox_Desktop_Client.Classes;
using Proxmox_Desktop_Client.Classes.pveAPI;
using Proxmox_Desktop_Client.Classes.pveAPI.objects;

namespace Proxmox_Desktop_Client;
    public partial class ClientLogin : Form
    {
        // Form Objects
        private MainPanel       MainWindow;
        private ClientLogin     TheWindow;
        // The System Ticker
        private Ticker          _Ticker;
        // API
        private ApiClient       _Api;
        
        public ClientLogin()
        {
            _Ticker = Program._Ticker;
        
            InitializeComponent();
            Program._Panels.Add("ClientLogin", this);
            TheWindow = this;
            TheWindow.panel_credentials.Visible = false;
            TheWindow.Width = 290;
            LoadCred();
            
            _Ticker.Start("SessionCheck", 60000, null);
            
        }


        private void click_otp(object sender, EventArgs e)
        {
            textBox_otp.Text = string.Empty;
            textBox_otp.Visible = checkBox_otp.Checked;
        }

        private async void click_checkServer(object sender = null, EventArgs e = null)
        {
           
            string server = TheWindow.textBox_server.Text;
            string port = TheWindow.textBox_port.Text;
            bool validateSsl = TheWindow.checkBox_ssl.Checked;
            
            _Api = new ApiClient(server, port, validateSsl);
            
            var realms = await _Api.GetRealmsAsync();
            
            if (realms != null && realms.Count > 0)
            {
                comboBox_realm.DataSource = realms;
                comboBox_realm.DisplayMember = "Comment"; // Display the comment in the ComboBox
                comboBox_realm.ValueMember = "Realm";     // Use the realm as the value
                
                // Retrieve the saved realm from settings
                string savedRealm = (string)Program._Config.GetSetting("Login_Realm");

                // Set the selected item based on the saved realm
                if (!string.IsNullOrEmpty(savedRealm))
                {
                    comboBox_realm.SelectedValue = savedRealm;
                }
                
                TheWindow.panel_credentials.Visible = true;
                TheWindow.Width = 570;
                TheWindow.CenterToScreen(); 
            }
            else
            {
                // Handle the case where there are no realms or the call failed
                comboBox_realm.DataSource = null;
                TheWindow.Width = 290;
            }
        }
        
        private async void ValidateLogin(object sender, EventArgs e)
        {
            string server = TheWindow.textBox_server.Text;
            string port = TheWindow.textBox_port.Text;
            bool validateSsl = TheWindow.checkBox_ssl.Checked;
            string username = TheWindow.textBox_username.Text;
            string password = TheWindow.textBox_password.Text;
            string realm = TheWindow.comboBox_realm.SelectedValue as string;
            string otp = TheWindow.textBox_otp.Text;

            TheWindow.panel_credentials.Enabled = false;
            TheWindow.panel_server.Enabled = false;
            _Api = new ApiClient(server, port, validateSsl);
            if (await _Api.LoginAsync(username, password, realm, otp))
            {
                SaveCred();
                Program._Api = _Api;
                MainPanel newWindow = new MainPanel();
                Program._Panels.Add("MainPanel", newWindow);
                TheWindow.Hide();
            }
            TheWindow.panel_credentials.Enabled = true;
            TheWindow.panel_server.Enabled = true;
        }
        
        private void SaveCred()
        {
            Program._Config.SetSetting("Login_Server", TheWindow.textBox_server.Text);
            Program._Config.SetSetting("Login_Port", TheWindow.textBox_port.Text);
            Program._Config.SetSetting("Login_CheckSSL", TheWindow.checkBox_ssl.Checked);
            Program._Config.SetSetting("Login_Username", TheWindow.textBox_username.Text);
            Program._Config.SetSetting("Login_Password", TheWindow.textBox_password.Text);
            Program._Config.SetSetting("Login_Realm", TheWindow.comboBox_realm.SelectedValue);
            Program._Config.SetSetting("Login_OTP", TheWindow.checkBox_otp.Checked);
            Program._Config.SetSetting("Login_Remember", TheWindow.checkBox_remember.Checked);
        }
        
        private void LoadCred()
        {
            if (Program._Config.GetSetting("Login_Remember") == null) { return; }
        
            TheWindow.textBox_server.Text = (string) Program._Config.GetSetting("Login_Server");
            TheWindow.textBox_port.Text = (string) Program._Config.GetSetting("Login_Port");
            TheWindow.checkBox_ssl.Checked = (bool) Program._Config.GetSetting("Login_CheckSSL");
            click_checkServer();
            TheWindow.textBox_username.Text = (string) Program._Config.GetSetting("Login_Username");
            TheWindow.textBox_password.Text = (string) Program._Config.GetSetting("Login_Password");
            TheWindow.checkBox_otp.Checked = (bool) Program._Config.GetSetting("Login_OTP");
            TheWindow.checkBox_remember.Checked = (bool) Program._Config.GetSetting("Login_Remember");
        }
        
        private void TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Check if the key is a digit or a control key (e.g., backspace)
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // Prevent the character from being entered
            }
        }
        
    }
