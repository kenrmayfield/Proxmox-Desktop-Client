using System;
using System.Windows.Forms;

namespace Proxmox_Desktop_Client;

public partial class SpiceProxyPanel : Form
{
    public SpiceProxyPanel()
    {
        InitializeComponent();
        CenterToScreen();
        LoadConfig();
    }
    
    private void LoadConfig()
    {
        textBox_proxyServer.Text = Program._Config.GetSetting("SpiceProxy_Server") as string ?? string.Empty;
        textBox_proxyPort.Text = Program._Config.GetSetting("SpiceProxy_Port") as string ?? string.Empty;
        checkBox_proxyEnable.Checked = Program._Config.GetSetting("SpiceProxy_Enable") as bool? ?? false;
    }
    
    private void SaveConfig()
    {
        Program._Config.SetSetting("SpiceProxy_Server", textBox_proxyServer.Text);
        Program._Config.SetSetting("SpiceProxy_Port", textBox_proxyPort.Text);
        Program._Config.SetSetting("SpiceProxy_Enable", checkBox_proxyEnable.Checked);
    }
    

    private void button_cancel_Click(object sender, EventArgs e)
    {
        this.Close();
        this.Dispose();
    }

    private void button_save_Click(object sender, EventArgs e)
    {
        SaveConfig();
        this.Close();
        this.Dispose();
    }
    
    
    private void TextBox_KeyPress(object sender, KeyPressEventArgs e)
    {
        if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
        {
            e.Handled = true;
        }
    }
}