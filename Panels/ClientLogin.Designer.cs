namespace Proxmox_Desktop_Client
{
    partial class ClientLogin
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ClientLogin));
            this.label_username = new System.Windows.Forms.Label();
            this.label_password = new System.Windows.Forms.Label();
            this.textBox_username = new System.Windows.Forms.TextBox();
            this.textBox_password = new System.Windows.Forms.TextBox();
            this.label_realm = new System.Windows.Forms.Label();
            this.comboBox_realm = new System.Windows.Forms.ComboBox();
            this.textBox_otp = new System.Windows.Forms.TextBox();
            this.checkBox_otp = new System.Windows.Forms.CheckBox();
            this.checkBox_remember = new System.Windows.Forms.CheckBox();
            this.button_login = new System.Windows.Forms.Button();
            this.textBox_port = new System.Windows.Forms.TextBox();
            this.label_port = new System.Windows.Forms.Label();
            this.textBox_server = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.checkBox_ssl = new System.Windows.Forms.CheckBox();
            this.logo = new System.Windows.Forms.PictureBox();
            this.panel_credentials = new System.Windows.Forms.Panel();
            this.button_check = new System.Windows.Forms.Button();
            this.panel_server = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.logo)).BeginInit();
            this.panel_credentials.SuspendLayout();
            this.panel_server.SuspendLayout();
            this.SuspendLayout();
            // 
            // label_username
            // 
            this.label_username.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_username.Location = new System.Drawing.Point(12, 92);
            this.label_username.Name = "label_username";
            this.label_username.Size = new System.Drawing.Size(100, 23);
            this.label_username.TabIndex = 0;
            this.label_username.Text = "Username:";
            this.label_username.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label_password
            // 
            this.label_password.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_password.Location = new System.Drawing.Point(12, 121);
            this.label_password.Name = "label_password";
            this.label_password.Size = new System.Drawing.Size(100, 23);
            this.label_password.TabIndex = 0;
            this.label_password.Text = "Password:";
            this.label_password.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBox_username
            // 
            this.textBox_username.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox_username.Location = new System.Drawing.Point(118, 92);
            this.textBox_username.MaxLength = 20;
            this.textBox_username.Name = "textBox_username";
            this.textBox_username.Size = new System.Drawing.Size(150, 23);
            this.textBox_username.TabIndex = 5;
            this.textBox_username.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox_password
            // 
            this.textBox_password.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox_password.Location = new System.Drawing.Point(118, 121);
            this.textBox_password.MaxLength = 64;
            this.textBox_password.Name = "textBox_password";
            this.textBox_password.PasswordChar = '*';
            this.textBox_password.Size = new System.Drawing.Size(150, 23);
            this.textBox_password.TabIndex = 6;
            this.textBox_password.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label_realm
            // 
            this.label_realm.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_realm.Location = new System.Drawing.Point(12, 150);
            this.label_realm.Name = "label_realm";
            this.label_realm.Size = new System.Drawing.Size(100, 23);
            this.label_realm.TabIndex = 0;
            this.label_realm.Text = "Realm:";
            this.label_realm.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBox_realm
            // 
            this.comboBox_realm.FormattingEnabled = true;
            this.comboBox_realm.Location = new System.Drawing.Point(118, 150);
            this.comboBox_realm.Name = "comboBox_realm";
            this.comboBox_realm.Size = new System.Drawing.Size(150, 24);
            this.comboBox_realm.TabIndex = 7;
            // 
            // textBox_otp
            // 
            this.textBox_otp.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox_otp.Location = new System.Drawing.Point(118, 180);
            this.textBox_otp.MaxLength = 6;
            this.textBox_otp.Name = "textBox_otp";
            this.textBox_otp.Size = new System.Drawing.Size(150, 23);
            this.textBox_otp.TabIndex = 9;
            this.textBox_otp.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBox_otp.Visible = false;
            this.textBox_otp.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextBox_KeyPress);
            // 
            // checkBox_otp
            // 
            this.checkBox_otp.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBox_otp.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox_otp.Location = new System.Drawing.Point(12, 180);
            this.checkBox_otp.Name = "checkBox_otp";
            this.checkBox_otp.Padding = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this.checkBox_otp.Size = new System.Drawing.Size(100, 24);
            this.checkBox_otp.TabIndex = 8;
            this.checkBox_otp.Text = "OTP";
            this.checkBox_otp.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBox_otp.UseVisualStyleBackColor = true;
            this.checkBox_otp.Click += new System.EventHandler(this.click_otp);
            // 
            // checkBox_remember
            // 
            this.checkBox_remember.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBox_remember.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.checkBox_remember.Location = new System.Drawing.Point(12, 218);
            this.checkBox_remember.Name = "checkBox_remember";
            this.checkBox_remember.Padding = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this.checkBox_remember.Size = new System.Drawing.Size(175, 24);
            this.checkBox_remember.TabIndex = 10;
            this.checkBox_remember.Text = "Remember Login?";
            this.checkBox_remember.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBox_remember.UseVisualStyleBackColor = true;
            // 
            // button_login
            // 
            this.button_login.BackColor = System.Drawing.SystemColors.Highlight;
            this.button_login.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_login.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_login.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.button_login.Location = new System.Drawing.Point(193, 214);
            this.button_login.Name = "button_login";
            this.button_login.Size = new System.Drawing.Size(75, 30);
            this.button_login.TabIndex = 11;
            this.button_login.Text = "LOGIN";
            this.button_login.UseVisualStyleBackColor = false;
            this.button_login.Click += new System.EventHandler(this.ValidateLogin);
            // 
            // textBox_port
            // 
            this.textBox_port.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox_port.Location = new System.Drawing.Point(76, 32);
            this.textBox_port.MaxLength = 5;
            this.textBox_port.Name = "textBox_port";
            this.textBox_port.Size = new System.Drawing.Size(150, 23);
            this.textBox_port.TabIndex = 2;
            this.textBox_port.Text = "8006";
            this.textBox_port.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBox_port.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextBox_KeyPress);
            // 
            // label_port
            // 
            this.label_port.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_port.Location = new System.Drawing.Point(3, 32);
            this.label_port.Name = "label_port";
            this.label_port.Size = new System.Drawing.Size(64, 23);
            this.label_port.TabIndex = 0;
            this.label_port.Text = "Port:";
            this.label_port.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBox_server
            // 
            this.textBox_server.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox_server.Location = new System.Drawing.Point(76, 3);
            this.textBox_server.MaxLength = 255;
            this.textBox_server.Name = "textBox_server";
            this.textBox_server.Size = new System.Drawing.Size(150, 23);
            this.textBox_server.TabIndex = 1;
            this.textBox_server.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "Server:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // checkBox_ssl
            // 
            this.checkBox_ssl.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBox_ssl.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.checkBox_ssl.Location = new System.Drawing.Point(3, 64);
            this.checkBox_ssl.Name = "checkBox_ssl";
            this.checkBox_ssl.Padding = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this.checkBox_ssl.Size = new System.Drawing.Size(142, 24);
            this.checkBox_ssl.TabIndex = 3;
            this.checkBox_ssl.Text = "Skip SSL?";
            this.checkBox_ssl.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBox_ssl.UseVisualStyleBackColor = true;
            // 
            // logo
            // 
            this.logo.ErrorImage = null;
            this.logo.Image = ((System.Drawing.Image)(resources.GetObject("logo.Image")));
            this.logo.InitialImage = null;
            this.logo.Location = new System.Drawing.Point(91, 12);
            this.logo.Name = "logo";
            this.logo.Size = new System.Drawing.Size(150, 150);
            this.logo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.logo.TabIndex = 18;
            this.logo.TabStop = false;
            // 
            // panel_credentials
            // 
            this.panel_credentials.Controls.Add(this.label_username);
            this.panel_credentials.Controls.Add(this.label_password);
            this.panel_credentials.Controls.Add(this.textBox_username);
            this.panel_credentials.Controls.Add(this.textBox_password);
            this.panel_credentials.Controls.Add(this.label_realm);
            this.panel_credentials.Controls.Add(this.comboBox_realm);
            this.panel_credentials.Controls.Add(this.button_login);
            this.panel_credentials.Controls.Add(this.checkBox_remember);
            this.panel_credentials.Controls.Add(this.textBox_otp);
            this.panel_credentials.Controls.Add(this.checkBox_otp);
            this.panel_credentials.Location = new System.Drawing.Point(254, 12);
            this.panel_credentials.Name = "panel_credentials";
            this.panel_credentials.Size = new System.Drawing.Size(285, 256);
            this.panel_credentials.TabIndex = 19;
            this.panel_credentials.Visible = false;
            // 
            // button_check
            // 
            this.button_check.BackColor = System.Drawing.SystemColors.Highlight;
            this.button_check.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_check.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_check.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.button_check.Location = new System.Drawing.Point(151, 62);
            this.button_check.Name = "button_check";
            this.button_check.Size = new System.Drawing.Size(75, 30);
            this.button_check.TabIndex = 4;
            this.button_check.Text = "CHECK";
            this.button_check.UseVisualStyleBackColor = false;
            this.button_check.Click += new System.EventHandler(this.click_checkServer);
            // 
            // panel_server
            // 
            this.panel_server.Controls.Add(this.textBox_server);
            this.panel_server.Controls.Add(this.button_check);
            this.panel_server.Controls.Add(this.label_port);
            this.panel_server.Controls.Add(this.textBox_port);
            this.panel_server.Controls.Add(this.label1);
            this.panel_server.Controls.Add(this.checkBox_ssl);
            this.panel_server.Location = new System.Drawing.Point(12, 162);
            this.panel_server.Name = "panel_server";
            this.panel_server.Size = new System.Drawing.Size(229, 106);
            this.panel_server.TabIndex = 20;
            // 
            // ClientLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(554, 271);
            this.Controls.Add(this.panel_server);
            this.Controls.Add(this.panel_credentials);
            this.Controls.Add(this.logo);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(570, 310);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(290, 310);
            this.Name = "ClientLogin";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Proxmox VE Login";
            ((System.ComponentModel.ISupportInitialize)(this.logo)).EndInit();
            this.panel_credentials.ResumeLayout(false);
            this.panel_credentials.PerformLayout();
            this.panel_server.ResumeLayout(false);
            this.panel_server.PerformLayout();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Panel panel_server;

        private System.Windows.Forms.Panel panel_credentials;
        private System.Windows.Forms.Button button_check;

        private System.Windows.Forms.CheckBox checkBox_ssl;
        private System.Windows.Forms.PictureBox logo;

        private System.Windows.Forms.TextBox textBox_port;
        private System.Windows.Forms.Label label_port;
        private System.Windows.Forms.TextBox textBox_server;
        private System.Windows.Forms.Label label1;

        private System.Windows.Forms.Button button_login;

        private System.Windows.Forms.CheckBox checkBox_remember;

        private System.Windows.Forms.CheckBox checkBox_otp;

        private System.Windows.Forms.Label label_username;
        private System.Windows.Forms.Label label_password;
        private System.Windows.Forms.TextBox textBox_username;
        private System.Windows.Forms.TextBox textBox_password;
        private System.Windows.Forms.Label label_realm;
        private System.Windows.Forms.ComboBox comboBox_realm;
        private System.Windows.Forms.TextBox textBox_otp;

        #endregion
    }
}