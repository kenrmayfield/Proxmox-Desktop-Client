using System.ComponentModel;

namespace Proxmox_Desktop_Client.Panels;

partial class SpiceProxyPanel
{
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private IContainer components = null;

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
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SpiceProxyPanel));
        this.button_save = new System.Windows.Forms.Button();
        this.button_cancel = new System.Windows.Forms.Button();
        this.checkBox_proxyEnable = new System.Windows.Forms.CheckBox();
        this.label_proxyPort = new System.Windows.Forms.Label();
        this.textBox_proxyServer = new System.Windows.Forms.TextBox();
        this.label_proxyServer = new System.Windows.Forms.Label();
        this.label1 = new System.Windows.Forms.Label();
        this.textBox_proxyPort = new System.Windows.Forms.TextBox();
        this.SuspendLayout();
        // 
        // button_save
        // 
        this.button_save.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
        this.button_save.BackColor = System.Drawing.SystemColors.Highlight;
        this.button_save.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        this.button_save.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.button_save.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
        this.button_save.Location = new System.Drawing.Point(12, 108);
        this.button_save.Name = "button_save";
        this.button_save.Size = new System.Drawing.Size(75, 30);
        this.button_save.TabIndex = 12;
        this.button_save.Text = "SAVE";
        this.button_save.UseVisualStyleBackColor = false;
        this.button_save.Click += new System.EventHandler(this.button_save_Click);
        // 
        // button_cancel
        // 
        this.button_cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
        this.button_cancel.BackColor = System.Drawing.SystemColors.Highlight;
        this.button_cancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        this.button_cancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.button_cancel.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
        this.button_cancel.Location = new System.Drawing.Point(217, 108);
        this.button_cancel.Name = "button_cancel";
        this.button_cancel.Size = new System.Drawing.Size(75, 30);
        this.button_cancel.TabIndex = 13;
        this.button_cancel.Text = "CANCEL";
        this.button_cancel.UseVisualStyleBackColor = false;
        this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
        // 
        // checkBox_proxyEnable
        // 
        this.checkBox_proxyEnable.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
        this.checkBox_proxyEnable.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
        this.checkBox_proxyEnable.Location = new System.Drawing.Point(217, 72);
        this.checkBox_proxyEnable.Name = "checkBox_proxyEnable";
        this.checkBox_proxyEnable.Size = new System.Drawing.Size(75, 24);
        this.checkBox_proxyEnable.TabIndex = 14;
        this.checkBox_proxyEnable.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        this.checkBox_proxyEnable.UseVisualStyleBackColor = true;
        // 
        // label_proxyPort
        // 
        this.label_proxyPort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
        this.label_proxyPort.Location = new System.Drawing.Point(12, 72);
        this.label_proxyPort.Name = "label_proxyPort";
        this.label_proxyPort.Size = new System.Drawing.Size(93, 23);
        this.label_proxyPort.TabIndex = 16;
        this.label_proxyPort.Text = "Proxy Port:";
        this.label_proxyPort.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
        // 
        // textBox_proxyServer
        // 
        this.textBox_proxyServer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
        this.textBox_proxyServer.Location = new System.Drawing.Point(111, 48);
        this.textBox_proxyServer.Name = "textBox_proxyServer";
        this.textBox_proxyServer.Size = new System.Drawing.Size(181, 20);
        this.textBox_proxyServer.TabIndex = 17;
        this.textBox_proxyServer.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
        // 
        // label_proxyServer
        // 
        this.label_proxyServer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
        this.label_proxyServer.Location = new System.Drawing.Point(12, 45);
        this.label_proxyServer.Name = "label_proxyServer";
        this.label_proxyServer.Size = new System.Drawing.Size(93, 23);
        this.label_proxyServer.TabIndex = 18;
        this.label_proxyServer.Text = "Proxy Server:";
        this.label_proxyServer.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
        // 
        // label1
        // 
        this.label1.Location = new System.Drawing.Point(12, 9);
        this.label1.Name = "label1";
        this.label1.Size = new System.Drawing.Size(280, 27);
        this.label1.TabIndex = 19;
        this.label1.Text = "Enter and check checkbox to enable an alternate proxy server for SPICE connection" + "s.";
        this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        // 
        // textBox_proxyPort
        // 
        this.textBox_proxyPort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
        this.textBox_proxyPort.Location = new System.Drawing.Point(111, 74);
        this.textBox_proxyPort.MaxLength = 5;
        this.textBox_proxyPort.Name = "textBox_proxyPort";
        this.textBox_proxyPort.Size = new System.Drawing.Size(108, 20);
        this.textBox_proxyPort.TabIndex = 20;
        this.textBox_proxyPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
        this.textBox_proxyPort.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextBox_KeyPress);
        // 
        // SpiceProxyPanel
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(304, 150);
        this.Controls.Add(this.textBox_proxyPort);
        this.Controls.Add(this.label1);
        this.Controls.Add(this.label_proxyServer);
        this.Controls.Add(this.textBox_proxyServer);
        this.Controls.Add(this.label_proxyPort);
        this.Controls.Add(this.checkBox_proxyEnable);
        this.Controls.Add(this.button_cancel);
        this.Controls.Add(this.button_save);
        this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
        this.MaximizeBox = false;
        this.MaximumSize = new System.Drawing.Size(320, 200);
        this.MinimizeBox = false;
        this.MinimumSize = new System.Drawing.Size(320, 39);
        this.Name = "SpiceProxyPanel";
        this.ShowInTaskbar = false;
        this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        this.Text = "Spice Proxy Settings";
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    private System.Windows.Forms.TextBox textBox_proxyPort;

    private System.Windows.Forms.Label label1;

    private System.Windows.Forms.TextBox textBox_proxyServer;
    private System.Windows.Forms.Label label_proxyServer;

    private System.Windows.Forms.Label label_proxyPort;

    private System.Windows.Forms.Button button_save;
    private System.Windows.Forms.Button button_cancel;

    private System.Windows.Forms.CheckBox checkBox_proxyEnable;

    #endregion
}