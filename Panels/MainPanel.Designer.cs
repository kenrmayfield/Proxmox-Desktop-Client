using System.ComponentModel;

namespace Proxmox_Desktop_Client.Panels;

partial class MainPanel
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
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainPanel));
        this.machinePanel = new System.Windows.Forms.Panel();
        this.menuStrip1 = new System.Windows.Forms.MenuStrip();
        this.rootItem_File = new System.Windows.Forms.ToolStripMenuItem();
        this.File_childItemLogout = new System.Windows.Forms.ToolStripMenuItem();
        this.File_childItemExit = new System.Windows.Forms.ToolStripMenuItem();
        this.webGUI = new System.Windows.Forms.ToolStripMenuItem();
        this.File_childItemAbout = new System.Windows.Forms.ToolStripMenuItem();
        this.File_childItemSettings = new System.Windows.Forms.ToolStripMenuItem();
        this.spiceProxyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.menuStrip1.SuspendLayout();
        this.SuspendLayout();
        // 
        // machinePanel
        // 
        this.machinePanel.AutoScroll = true;
        this.machinePanel.BackColor = System.Drawing.Color.Transparent;
        this.machinePanel.CausesValidation = false;
        this.machinePanel.Dock = System.Windows.Forms.DockStyle.Fill;
        this.machinePanel.Location = new System.Drawing.Point(0, 24);
        this.machinePanel.Margin = new System.Windows.Forms.Padding(20);
        this.machinePanel.Name = "machinePanel";
        this.machinePanel.Padding = new System.Windows.Forms.Padding(20, 20, 20, 40);
        this.machinePanel.Size = new System.Drawing.Size(694, 437);
        this.machinePanel.TabIndex = 1;
        // 
        // menuStrip1
        // 
        this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { this.rootItem_File, this.webGUI });
        this.menuStrip1.Location = new System.Drawing.Point(0, 0);
        this.menuStrip1.Name = "menuStrip1";
        this.menuStrip1.Size = new System.Drawing.Size(694, 24);
        this.menuStrip1.TabIndex = 0;
        this.menuStrip1.Text = "menuStrip1";
        // 
        // rootItem_File
        // 
        this.rootItem_File.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { this.File_childItemAbout, this.File_childItemSettings, this.File_childItemLogout, this.File_childItemExit });
        this.rootItem_File.Name = "rootItem_File";
        this.rootItem_File.Size = new System.Drawing.Size(37, 20);
        this.rootItem_File.Text = "File";
        // 
        // File_childItemLogout
        // 
        this.File_childItemLogout.Name = "File_childItemLogout";
        this.File_childItemLogout.Size = new System.Drawing.Size(152, 22);
        this.File_childItemLogout.Text = "Logout";
        this.File_childItemLogout.Click += new System.EventHandler(this.ActionLogout);
        // 
        // File_childItemExit
        // 
        this.File_childItemExit.Name = "File_childItemExit";
        this.File_childItemExit.Size = new System.Drawing.Size(152, 22);
        this.File_childItemExit.Text = "Exit";
        this.File_childItemExit.Click += new System.EventHandler(this.ActionExitApplication);
        // 
        // webGUI
        // 
        this.webGUI.Name = "webGUI";
        this.webGUI.Size = new System.Drawing.Size(78, 20);
        this.webGUI.Text = "Cluster GUI";
        this.webGUI.Click += new System.EventHandler(this.webGUI_Click);
        // 
        // File_childItemAbout
        // 
        this.File_childItemAbout.Name = "File_childItemAbout";
        this.File_childItemAbout.Size = new System.Drawing.Size(152, 22);
        this.File_childItemAbout.Text = "About";
        this.File_childItemAbout.Click += new System.EventHandler(this.click_openAbout);
        // 
        // File_childItemSettings
        // 
        this.File_childItemSettings.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { this.spiceProxyToolStripMenuItem });
        this.File_childItemSettings.Name = "File_childItemSettings";
        this.File_childItemSettings.Size = new System.Drawing.Size(152, 22);
        this.File_childItemSettings.Text = "Settings";
        // 
        // spiceProxyToolStripMenuItem
        // 
        this.spiceProxyToolStripMenuItem.Name = "spiceProxyToolStripMenuItem";
        this.spiceProxyToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
        this.spiceProxyToolStripMenuItem.Text = "SPICE Proxy";
        this.spiceProxyToolStripMenuItem.Click += new System.EventHandler(this.OpenSpiceProxyPanel);
        // 
        // MainPanel
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
        this.ClientSize = new System.Drawing.Size(694, 461);
        this.Controls.Add(this.machinePanel);
        this.Controls.Add(this.menuStrip1);
        this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
        this.MainMenuStrip = this.menuStrip1;
        this.MaximizeBox = false;
        this.MaximumSize = new System.Drawing.Size(710, 500);
        this.MinimumSize = new System.Drawing.Size(710, 500);
        this.Name = "MainPanel";
        this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        this.Text = "Proxmox VE";
        this.Resize += new System.EventHandler(this.ClientLogin_Resize);
        this.menuStrip1.ResumeLayout(false);
        this.menuStrip1.PerformLayout();
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    private System.Windows.Forms.ToolStripMenuItem File_childItemSettings;
    private System.Windows.Forms.ToolStripMenuItem spiceProxyToolStripMenuItem;

    private System.Windows.Forms.ToolStripMenuItem File_childItemAbout;

    private System.Windows.Forms.ToolStripMenuItem webGUI;

    private System.Windows.Forms.Panel machinePanel;

    private System.Windows.Forms.ToolStripMenuItem File_childItemLogout;

    private System.Windows.Forms.ToolStripMenuItem rootItem_File;
    private System.Windows.Forms.ToolStripMenuItem File_childItemExit;

    private System.Windows.Forms.MenuStrip menuStrip1;

    #endregion
}