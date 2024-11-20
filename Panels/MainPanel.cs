using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Proxmox_Desktop_Client.Classes;
using Proxmox_Desktop_Client.Classes.consoles;
using Proxmox_Desktop_Client.Classes.pveAPI;
using Proxmox_Desktop_Client.Classes.pveAPI.objects;

namespace Proxmox_Desktop_Client;

public partial class MainPanel : Form
{
    private ApiClient _Api;
    private UserPermissions _Permissions;
    
    public MainPanel()
    {
        Load += RefreshContent;
        _Api = Program._Api;
        _Permissions = Program._Permissions;
        InitializeComponent();
        Show();
    }

    private async void RefreshContent(object sender, EventArgs e)
    {
        // Get Permissions
        await _Api.GetPermissionsAsync();
        // Collect Information
        await GetNodesAndMachinesAsync();
        // Process Machines
        ProcessMachineList();
    }

    public async Task GetNodesAndMachinesAsync()
    {
        var allMachines = new List<MachineData>();

        // Fetch the list of nodes
        string nodesJson = await _Api.GetAsync("nodes");
        if (nodesJson != null)
        {
            var nodesRoot = JsonConvert.DeserializeObject<RootObject<NodeData>>(nodesJson);

            foreach (var node in nodesRoot.Data)
            {
                // Fetch LXC machines
                string lxcPath = $"nodes/{node.Node}/lxc";
                string lxcJson = await _Api.GetAsync(lxcPath);
                if (lxcJson != null)
                {
                    var lxcMachinesRoot = JsonConvert.DeserializeObject<RootObject<MachineData>>(lxcJson);
                    foreach (var machine in lxcMachinesRoot.Data)
                    {
                        machine.NodeName = node.Node;
                        allMachines.Add(machine);
                    }
                }

                // Fetch QEMU machines
                string qemuPath = $"nodes/{node.Node}/qemu";
                string qemuJson = await _Api.GetAsync(qemuPath);
                if (qemuJson != null)
                {
                    var qemuMachinesRoot = JsonConvert.DeserializeObject<RootObject<MachineData>>(qemuJson);
                    foreach (var machine in qemuMachinesRoot.Data)
                    {
                        machine.NodeName = node.Node;
                        allMachines.Add(machine);
                    }
                    
                }
            }
        }

        _Api.Machines = allMachines;
    }

    private void AddMachine2Panel(int id, MachineData machineData)
    {
     
        // Define the size of each GroupBox
        int groupBoxWidth = 150;
        int groupBoxHeight = 150;
        int padding = 20; // Space between GroupBoxes
        int margin = 10; // Margin from the left edge
        int topMargin = 10; // Margin from the top edge

        // Fixed number of columns
        int columns = 4;

        // Create a new GroupBox
        System.Windows.Forms.GroupBox newGroupBox = new System.Windows.Forms.GroupBox();
        newGroupBox.Size = new System.Drawing.Size(groupBoxWidth, groupBoxHeight);
        newGroupBox.Text = machineData.Name + " ("+machineData.Type+")";

        // Determine the position based on the number of existing GroupBoxes
        int index = this.panel_machines.Controls.Count;
        int row = index / columns;
        int column = index % columns;

        int xPosition = margin + column * (groupBoxWidth + padding);
        int yPosition = topMargin + row * (groupBoxHeight + padding);

        newGroupBox.Location = new System.Drawing.Point(xPosition, yPosition);

        // Create a new PictureBox for dots
        System.Windows.Forms.PictureBox newPbDots = new System.Windows.Forms.PictureBox();
        newPbDots.Size = new System.Drawing.Size(20, 20);
        newPbDots.Location = new System.Drawing.Point(119, 119);
        newPbDots.Image = global::Proxmox_Desktop_Client.Properties.Resources.icons_dots;
        newPbDots.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;

        // Create a ContextMenuStrip for the PictureBox
        System.Windows.Forms.ContextMenuStrip contextMenu = new System.Windows.Forms.ContextMenuStrip();
        System.Windows.Forms.ToolStripMenuItem menuItem1 = new System.Windows.Forms.ToolStripMenuItem("NoVNC");
        System.Windows.Forms.ToolStripMenuItem menuItem2 = new System.Windows.Forms.ToolStripMenuItem("Spice");
        System.Windows.Forms.ToolStripMenuItem menuItem3 = new System.Windows.Forms.ToolStripMenuItem("xTermJS");
        contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { menuItem1, menuItem2, menuItem3});

        // Assign the ContextMenuStrip to the PictureBox
        newPbDots.ContextMenuStrip = contextMenu;

        // Event handlers for menu items
        menuItem1.Click += (sender, e) => NoVNC_Client(machineData);
        menuItem2.Click += (sender, e) => spice_Client(machineData.Vmid);
        menuItem3.Click += (sender, e) => xTermJS_Client(machineData);

        // Handle MouseClick event to show context menu on left click
        newPbDots.MouseClick += (sender, e) =>
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left || e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                contextMenu.Show(newPbDots, e.Location);
            }
        };

        // Create a new PictureBox for image
        System.Windows.Forms.PictureBox newPbImg = new System.Windows.Forms.PictureBox();
        newPbImg.Size = new System.Drawing.Size(126, 126);
        newPbImg.Location = new System.Drawing.Point(13, 13);
        newPbImg.Image = global::Proxmox_Desktop_Client.Properties.Resources.lxc_logo;
        newPbImg.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;

        // Add PictureBoxes to the GroupBox
        newGroupBox.Controls.Add(newPbDots);
        newGroupBox.Controls.Add(newPbImg);

        // Add the GroupBox to the panel
        this.panel_machines.Controls.Add(newGroupBox);
    }
    
    private void NoVNC_Client(MachineData machineData)
    {
        new NoVNCClient(_Api, machineData);
    }
    private void xTermJS_Client(MachineData machineData)
    {
        new NoVNCClient(_Api, machineData);
    }
    
    private void spice_Client(int vmid)
    {
        SpiceClient spiceClient = new SpiceClient(_Api, _Api.Machines.FirstOrDefault(m => m.Vmid == vmid));
        spiceClient.RequestSpiceConnection();
    }
    
    private void ProcessMachineList()
    {
        foreach (var machine in _Api.Machines)
        {
            AddMachine2Panel(machine.Vmid, machine);
        }
    }
    
    private void action_logout(object sender, EventArgs e)
    {
        this.Close();
    }
    private void action_exitApplication(object sender, EventArgs e)
    {
        Application.Exit();
    }
    
    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        base.OnFormClosing(e);
        
        ClientLogin TheWindow = (ClientLogin) Program._Panels["ClientLogin"];
        TheWindow.Show();
    }
}