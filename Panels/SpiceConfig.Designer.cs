using System.ComponentModel;

namespace Proxmox_Desktop_Client;

partial class SpiceConfig
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
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SpiceConfig));
        this.label_proxyServer = new System.Windows.Forms.Label();
        this.label_proxyPort = new System.Windows.Forms.Label();
        this.textBox1 = new System.Windows.Forms.TextBox();
        this.textBox2 = new System.Windows.Forms.TextBox();
        this.checkBox1 = new System.Windows.Forms.CheckBox();
        this.button_save = new System.Windows.Forms.Button();
        this.button_cancel = new System.Windows.Forms.Button();
        this.SuspendLayout();
        // 
        // label_proxyServer
        // 
        this.label_proxyServer.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.label_proxyServer.Location = new System.Drawing.Point(12, 9);
        this.label_proxyServer.Name = "label_proxyServer";
        this.label_proxyServer.Size = new System.Drawing.Size(87, 23);
        this.label_proxyServer.TabIndex = 0;
        this.label_proxyServer.Text = "Proxy Server:";
        this.label_proxyServer.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
        // 
        // label_proxyPort
        // 
        this.label_proxyPort.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.label_proxyPort.Location = new System.Drawing.Point(12, 35);
        this.label_proxyPort.Name = "label_proxyPort";
        this.label_proxyPort.Size = new System.Drawing.Size(87, 23);
        this.label_proxyPort.TabIndex = 1;
        this.label_proxyPort.Text = "Proxy Port:";
        this.label_proxyPort.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
        // 
        // textBox1
        // 
        this.textBox1.Location = new System.Drawing.Point(105, 12);
        this.textBox1.Name = "textBox1";
        this.textBox1.Size = new System.Drawing.Size(167, 20);
        this.textBox1.TabIndex = 2;
        // 
        // textBox2
        // 
        this.textBox2.Location = new System.Drawing.Point(105, 38);
        this.textBox2.Name = "textBox2";
        this.textBox2.Size = new System.Drawing.Size(100, 20);
        this.textBox2.TabIndex = 3;
        // 
        // checkBox1
        // 
        this.checkBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.checkBox1.Location = new System.Drawing.Point(52, 81);
        this.checkBox1.Name = "checkBox1";
        this.checkBox1.Size = new System.Drawing.Size(200, 32);
        this.checkBox1.TabIndex = 4;
        this.checkBox1.Text = "Use Alternate Proxy Server Info";
        this.checkBox1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
        this.checkBox1.UseVisualStyleBackColor = true;
        // 
        // button_save
        // 
        this.button_save.BackColor = System.Drawing.SystemColors.Highlight;
        this.button_save.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        this.button_save.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.button_save.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
        this.button_save.Location = new System.Drawing.Point(50, 119);
        this.button_save.Name = "button_save";
        this.button_save.Size = new System.Drawing.Size(75, 30);
        this.button_save.TabIndex = 12;
        this.button_save.Text = "SAVE";
        this.button_save.UseVisualStyleBackColor = false;
        // 
        // button_cancel
        // 
        this.button_cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
        this.button_cancel.BackColor = System.Drawing.SystemColors.Highlight;
        this.button_cancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        this.button_cancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.button_cancel.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
        this.button_cancel.Location = new System.Drawing.Point(181, 119);
        this.button_cancel.Name = "button_cancel";
        this.button_cancel.Size = new System.Drawing.Size(75, 30);
        this.button_cancel.TabIndex = 13;
        this.button_cancel.Text = "CANCEL";
        this.button_cancel.UseVisualStyleBackColor = false;
        // 
        // SpiceConfig
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(304, 161);
        this.Controls.Add(this.button_cancel);
        this.Controls.Add(this.button_save);
        this.Controls.Add(this.checkBox1);
        this.Controls.Add(this.textBox2);
        this.Controls.Add(this.textBox1);
        this.Controls.Add(this.label_proxyPort);
        this.Controls.Add(this.label_proxyServer);
        this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
        this.MaximizeBox = false;
        this.MaximumSize = new System.Drawing.Size(320, 200);
        this.MinimizeBox = false;
        this.MinimumSize = new System.Drawing.Size(320, 200);
        this.Name = "SpiceConfig";
        this.ShowInTaskbar = false;
        this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        this.Text = "SpiceConfig";
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    private System.Windows.Forms.Button button_save;
    private System.Windows.Forms.Button button_cancel;

    private System.Windows.Forms.TextBox textBox1;
    private System.Windows.Forms.TextBox textBox2;
    private System.Windows.Forms.CheckBox checkBox1;

    private System.Windows.Forms.Label label_proxyPort;

    private System.Windows.Forms.Label label_proxyServer;

    #endregion
}