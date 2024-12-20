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
            InitializeComponent();
            Program._Panels.Add("ClientLogin", this);
            panel_credentials.Visible = false;
            Width = 290;
            LoadCredentials();
        }

        private void ClickOtp(object sender, EventArgs e)
        {
            textBox_otp.Text = string.Empty;
            textBox_otp.Visible = checkBox_otp.Checked;
        }

        private void ClickCheckServer(object sender = null, EventArgs e = null)
        {
            string server = textBox_server.Text;
            string port = textBox_port.Text;
            bool validateSsl = checkBox_ssl.Checked;

            panel_credentials.Enabled = false;
            panel_server.Enabled = false;
            Width = 290;
            CenterToScreen();

            try
            {
                Program._Api = new ApiClient(server, port, validateSsl);
                List<RealmData> realms = Program._Api.GetRealms();

                if (realms != null && realms.Count > 0)
                {
                    comboBox_realm.DataSource = realms;
                    comboBox_realm.DisplayMember = "Comment";
                    comboBox_realm.ValueMember = "Realm";
                    comboBox_realm.SelectedValue = (string)Program._Config.GetSetting("Login_Realm") ?? "pam";
                    panel_credentials.Visible = true;
                    Width = 570;
                }
                else
                {
                    comboBox_realm.DataSource = null;
                    Width = 290;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error connecting to server: {ex.Message}");
            }
            finally
            {
                CenterToScreen();
                panel_credentials.Enabled = true;
                panel_server.Enabled = true;
            }
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

            try
            {
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
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Login failed: {ex.Message}");
            }
            finally
            {
                panel_credentials.Enabled = true;
                panel_server.Enabled = true;
            }
        }

        private void SaveCredentials()
        {
            bool remember = checkBox_remember.Checked;
            SetConfigSetting("Login_Server", textBox_server.Text);
            SetConfigSetting("Login_Port", textBox_port.Text);
            SetConfigSetting("Login_CheckSSL", checkBox_ssl.Checked);
            if (remember)
            {
                SetConfigSetting("Login_Username", textBox_username.Text);
                SetConfigSetting("Login_Password", textBox_password.Text);
                SetConfigSetting("Login_Realm", comboBox_realm.SelectedValue);
                SetConfigSetting("Login_OTP", checkBox_otp.Checked);
            }
            else
            {
                SetConfigSetting("Login_Username", "");
                SetConfigSetting("Login_Password", "");
                SetConfigSetting("Login_Realm", "pam");
                SetConfigSetting("Login_OTP", false);
            }
            SetConfigSetting("Login_Remember", remember);
        }

        private void SetConfigSetting(string key, object value)
        {
            Program._Config.SetSetting(key, value);
        }

        public void LoadCredentials()
        {
            bool remember = (bool?)Program._Config.GetSetting("Login_Remember") ?? false;
            if (remember)
            {
                textBox_server.Text = (string)Program._Config.GetSetting("Login_Server");
                textBox_port.Text = (string)Program._Config.GetSetting("Login_Port");
                checkBox_ssl.Checked = (bool)Program._Config.GetSetting("Login_CheckSSL");
                ClickCheckServer();
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