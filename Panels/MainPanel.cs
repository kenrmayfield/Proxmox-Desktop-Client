using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Proxmox_Desktop_Client.Classes.consoles;
using Proxmox_Desktop_Client.Classes.pveAPI;
using Proxmox_Desktop_Client.Classes.pveAPI.objects;

namespace Proxmox_Desktop_Client.Panels;

public partial class MainPanel : Form
{
    private readonly ApiClient _api;

    public MainPanel()
    {
        Load += RefreshContent;
        _api = Program._Api;
        InitializeComponent();
        Show();
    }

    private void RefreshContent(object sender, EventArgs e)
    {
        GetNodesAndMachines();
        ProcessMachineList();
    }

    public void GetNodesAndMachines()
    {
        var allMachines = new List<MachineData>();
        string nodesJson = _api.GetRequest("nodes");

        if (nodesJson != null)
        {
            var nodesRoot = JsonConvert.DeserializeObject<RootObject<NodeData>>(nodesJson);

            foreach (var node in nodesRoot.Data)
            {
                AddMachinesFromNode(node, "lxc", allMachines);
                AddMachinesFromNode(node, "qemu", allMachines);
            }
        }

        _api.Machines = allMachines.OrderBy(data => data.Vmid).ToList();
    }

    private void AddMachinesFromNode(NodeData node, string type, List<MachineData> allMachines)
    {
        string path = $"nodes/{node.Node}/{type}";
        string json = _api.GetRequest(path);

        if (json != null)
        {
            var machinesRoot = JsonConvert.DeserializeObject<RootObject<MachineData>>(json);
            foreach (var machine in machinesRoot.Data)
            {
                machine.NodeName = node.Node;
                allMachines.Add(machine);
            }
        }
    }

    private void AddMachine2Panel(MachineData machineData)
    {
        const int groupBoxWidth = 150;
        const int groupBoxHeight = 150;
        const int padding = 20;
        const int margin = 10;
        const int topMargin = 10;
        const int columns = 4;

        var newGroupBox = new GroupBox
        {
            Size = new System.Drawing.Size(groupBoxWidth, groupBoxHeight),
            Text = $"{machineData.Name} ( {machineData.Vmid} )"
        };

        int index = panel_machines.Controls.Count;
        int row = index / columns;
        int column = index % columns;

        newGroupBox.Location = new System.Drawing.Point(
            margin + column * (groupBoxWidth + padding),
            topMargin + row * (groupBoxHeight + padding)
        );

        var newPbDots = new PictureBox
        {
            Size = new System.Drawing.Size(20, 20),
            Location = new System.Drawing.Point(119, 119),
            Image = Properties.Resources.icons_dots,
            SizeMode = PictureBoxSizeMode.StretchImage
        };

        // Shutdown/Reboot/Pause/Hibernate/Stop/Reset
        var contextMenu = new ContextMenuStrip();
        var menuItem1 = new ToolStripMenuItem("Console");
        var menuItem2 = new ToolStripMenuItem("Power");
        
        var powerContextMenu = new ContextMenuStrip();
        var pMenuItem1 = new ToolStripMenuItem("Start");
        var pMenuItem2 = new ToolStripMenuItem("Shutdown");
        var pMenuItem3 = new ToolStripMenuItem("Reboot");
        var pMenuItem4 = new ToolStripMenuItem("Pause");
        var pMenuItem5 = new ToolStripMenuItem("Hibernate");
        var pMenuItem6 = new ToolStripMenuItem("Stop");
        var pMenuItem7 = new ToolStripMenuItem("Reset");

        var remoteContextMenu  = new ContextMenuStrip();
        var remoteOptionItem1 = new ToolStripMenuItem("NoVNC");
        var remoteOptionItem2 = new ToolStripMenuItem("Spice");
        var remoteOptionItem3 = new ToolStripMenuItem("xTermJS");
        remoteOptionItem1.Click += (_, _) => WebClient(machineData, "novnc");
        remoteOptionItem2.Click += (_, _) => Spice_Client(machineData.Vmid);
        remoteOptionItem3.Click += (_, _) => WebClient(machineData, "xtermjs");
        remoteOptionItem1.Enabled = false;
        remoteOptionItem2.Enabled = false;
        remoteOptionItem3.Enabled = false;
        
        contextMenu.Items.AddRange(new ToolStripItem[] { menuItem1, menuItem2 });
        remoteContextMenu.Items.AddRange(new ToolStripItem[] { remoteOptionItem1, remoteOptionItem2, remoteOptionItem3 });
        powerContextMenu.Items.AddRange(new ToolStripItem[] { pMenuItem1, pMenuItem2, pMenuItem3, pMenuItem4, pMenuItem5, pMenuItem6, pMenuItem7 });
        
        newPbDots.ContextMenuStrip = contextMenu;
        menuItem1.DropDown = remoteContextMenu;
        menuItem2.DropDown = powerContextMenu;

        newPbDots.MouseClick += (_, e) =>
        {
            if (e.Button == MouseButtons.Left || e.Button == MouseButtons.Right)
            {
                contextMenu.Show(newPbDots, e.Location);
            }
        };

        var newPbImg = new PictureBox
        {
            Size = new System.Drawing.Size(126, 126),
            Location = new System.Drawing.Point(13, 13),
            Image = machineData.Type == "lxc" ? 
                Properties.Resources.lxc_logo : 
                Properties.Resources.vm_logo,
            SizeMode = PictureBoxSizeMode.StretchImage
        };

        newGroupBox.Controls.Add(newPbDots);
        newGroupBox.Controls.Add(newPbImg);
            
        panel_machines.Controls.Add(newGroupBox);
        
        // RemoteMenu Options Changes    
        if (machineData.Status == "running")
        {
            remoteOptionItem1.Enabled = true;
            if(machineData.Type == "lxc" || machineData.Serial == 1)
            {
                remoteOptionItem3.Enabled = true;    
            }
            
            if(machineData.Type == "qemu")
            {
                remoteOptionItem2.Enabled = CheckSpiceAble(machineData);
            }    
        } 
    }

    private static void WebClient(MachineData machineData, string remoteType)
    {
        // ReSharper disable once UnusedVariable
        // ReSharper disable once ObjectCreationAsStatement
        new NoVncClient(machineData, remoteType);
    }
        
    private void Spice_Client(int vmid)
    {
        var spiceClient = new SpiceClient(_api.Machines.FirstOrDefault(m => m.Vmid == vmid));
        spiceClient.RequestSpiceConnection();
    }

    private bool CheckSpiceAble(MachineData machine)
    {
        string node = machine.NodeName;
        string vmid = machine.Vmid.ToString();
            
        // Fetch the JSON string from the API
        string jsonString = _api.GetRequest($"nodes/{node}/qemu/{vmid}/status/current");
            
        // Parse the JSON string
        var jsonObject = Newtonsoft.Json.Linq.JObject.Parse(jsonString);
            
        // Try to extract the Spice value
        var spiceToken = jsonObject["data"]?["spice"];
            
        // If Spice does not exist or is not equal to 1, return false
        if (spiceToken == null || (int)spiceToken != 1 || (string)spiceToken != "1")
        {
            return false;
        }
            
        // Return true if Spice equals 1
        return true;
    }

    private void ProcessMachineList()
    {
        foreach (var machine in _api.Machines)
        {
            AddMachine2Panel(machine);
        }
    }

    private void ActionLogout(object sender, EventArgs e)
    {
        Close();
    }

    private void ActionExitApplication(object sender, EventArgs e)
    {
        Application.Exit();
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        base.OnFormClosing(e);
        var theWindow = (ClientLogin)Program._Panels["ClientLogin"];
        theWindow.Show();
    }

    private void OpenSpiceProxyPanel(object sender, EventArgs e)
    {
        if (Program._Panels.ContainsKey("SpiceProxyPanel"))
        {
            Program._Panels.Remove("SpiceProxyPanel");
        }
            
        SpiceProxyPanel panelSpiceProxy = new SpiceProxyPanel();
        Program._Panels.Add("SpiceProxyPanel", panelSpiceProxy);
        panelSpiceProxy.Show();
    }
}