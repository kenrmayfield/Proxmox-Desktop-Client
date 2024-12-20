using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Proxmox_Desktop_Client.Classes.consoles;
using Proxmox_Desktop_Client.Classes.pveAPI.objects;
using Label = System.Windows.Forms.Label;
using Timer = System.Timers.Timer;

namespace Proxmox_Desktop_Client.Panels;

public partial class MainPanel : Form
{
    private Timer _refreshTimer;
    private Timer _refreshTicket;
    private NotifyIcon _notifyIcon;
    private ContextMenuStrip _contextMenu;

    public MainPanel()
    {
        Load += RefreshContent;
        InitializeComponent();
        InitializeNotifyIcon();
        Resize += ClientLogin_Resize;

        // Initialize and configure the timer
        // Reload Panel
        _refreshTimer = new Timer(60000); // 60 seconds
        _refreshTimer.Elapsed += RefreshContent;
        _refreshTimer.AutoReset = true; // Repeat every interval
        _refreshTimer.Enabled = true; // Start the timer
        
        _refreshTicket = new Timer(5400000); // 1 hour and 30 minutes
        _refreshTicket.Elapsed += Program._Api.RenewTicket;
        _refreshTicket.AutoReset = true; // Repeat every interval
        _refreshTicket.Enabled = true; // Start the timer
        
        Show();
    }

    private void RefreshContent(object sender, EventArgs e)
    {
    
        if (InvokeRequired)
        {
            Invoke(new Action(() => RefreshContent(sender, e)));
            return;
        }
        
        GetAllMachines();
        machinePanel.Controls.Clear();
        ProcessMachineList();
    }
    
    // Gets Available Virtual Machines from API
    public void GetAllMachines()
    {
        var allMachines = new List<MachineData>();
        string nodesJson = Program._Api.GetRequest("nodes");

        if (nodesJson != null)
        {
            var nodesRoot = JsonConvert.DeserializeObject<RootObject<NodeData>>(nodesJson);

            foreach (var node in nodesRoot.Data)
            {
                AddToMachineList(node, "lxc", allMachines);
                AddToMachineList(node, "qemu", allMachines);
            }
        }

        Program._Api.Machines = allMachines.OrderBy(data => data.Vmid).ToList();
    }
    // Generate List<MachineData> Machines
    private void AddToMachineList(NodeData node, string type, List<MachineData> allMachines)
    {
        string path = $"nodes/{node.Node}/{type}";
        string json = Program._Api.GetRequest(path);

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
    // Cycle though Machine List and Add to Panel
    private void ProcessMachineList()
    {
        foreach (var machine in Program._Api.Machines)
        {
            GenerateMachineTile(machine);
        }
    }
    // Add Machines to MainPanel (Content) of Object
    private void GenerateMachineTile(MachineData machineData)
    {
        // Constants 
        const int groupBoxWidth = 150;
        const int groupBoxHeight = 150;
        const int padding = 20;
        const int margin = 10;
        const int topMargin = 10;
        const int columns = 4;
        
        // Create New GroupBox
        var newGroupBox = new GroupBox
        {
            Name = machineData.Vmid.ToString(),
            Size = new Size(groupBoxWidth, groupBoxHeight),
            Text = $"{machineData.Name} ( {machineData.Vmid} )"
        };
        
        // Calculate Position
        int index = machinePanel.Controls.Count;
        int row = index / columns;
        int column = index % columns;
        // Set Positions to GroupBox
        newGroupBox.Location = new Point(
            margin + column * (groupBoxWidth + padding),
            topMargin + row * (groupBoxHeight + padding)
        );
        
        // Generate Object (Dots)
        var newPbDots = new PictureBox
        {
            Name = "Dots_" + machineData.Vmid.ToString(),
            Size = new Size(20, 20),
            Location = new Point(119, 119),
            Image = Properties.Resources.icons_dots,
            SizeMode = PictureBoxSizeMode.StretchImage
        };
        
        // Generate Object (Icon)
        var newPbImg = new PictureBox
        {
            Name = "Icon_"+machineData.Vmid.ToString(),
            Size = new Size(126, 126),
            Location = new Point(13, 13),
            Image = machineData.Type == "lxc" ? 
                Properties.Resources.lxc_logo : 
                Properties.Resources.vm_logo,
            SizeMode = PictureBoxSizeMode.StretchImage
        };
        
        // Label Status of Virtual Machine
        var newStatus = new Label()
        {
            Name = "Status_" + machineData.Vmid.ToString(),
            Location = new Point(0, 110),
            Anchor = AnchorStyles.Bottom | AnchorStyles.Left,
            Text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(machineData.Status.ToLower()),
            Font = new Font(Font.FontFamily, 7f, FontStyle.Bold),
            ForeColor = (machineData.Status.ToLower() == "running" ? Color.Green : Color.Red ) 
        };

        // Generate Context Menus
        var mainContextMenu = new ContextMenuStrip();
        var remoteContextMenu  = new ContextMenuStrip();
        var powerContextMenu = new ContextMenuStrip();

        // Top Level Items
        var menuItem1 = new ToolStripMenuItem("Console");
        var menuItem2 = new ToolStripMenuItem("Power");
        // Remote Menu Items
        var rMenuItem1 = new ToolStripMenuItem("NoVNC");
        var rMenuItem2 = new ToolStripMenuItem("Spice");
        var rMenuItem3 = new ToolStripMenuItem("xTermJS");
        // Power Menu Items
        var pMenuItem1 = new ToolStripMenuItem("Start");
        var pMenuItem2 = new ToolStripMenuItem("Shutdown");
        var pMenuItem3 = new ToolStripMenuItem("Reboot");
        var pMenuItem4 = new ToolStripMenuItem("Pause");
        var pMenuItem5 = new ToolStripMenuItem("Resume");
        var pMenuItem6 = new ToolStripMenuItem("Hibernate");
        var pMenuItem7 = new ToolStripMenuItem("Stop");
        var pMenuItem8 = new ToolStripMenuItem("Reset");
        
        // Set SubContext Menus
        menuItem1.DropDown = remoteContextMenu;
        menuItem2.DropDown = powerContextMenu;
        
        // Add Items to Context Menus
        mainContextMenu.Items.AddRange(new ToolStripItem[] { menuItem1, menuItem2 });
        remoteContextMenu.Items.AddRange(new ToolStripItem[] { rMenuItem1, rMenuItem2, rMenuItem3 });
        powerContextMenu.Items.AddRange(new ToolStripItem[] { pMenuItem1, pMenuItem2, pMenuItem3, pMenuItem4, pMenuItem5, pMenuItem6, pMenuItem7 });

        // Remote Set Click Actions
        rMenuItem1.Click += (_, _) => WebClient(machineData, "novnc");
        rMenuItem2.Click += (_, _) => SpiceClient(machineData.Vmid);
        rMenuItem3.Click += (_, _) => WebClient(machineData, "xtermjs");
        
        // Power Set Click Actions
        pMenuItem1.Click += (_, _) => PowerRequest("start", machineData);
        pMenuItem2.Click += (_, _) => PowerRequest("shutdown", machineData);
        pMenuItem3.Click += (_, _) => PowerRequest("reboot", machineData);
        pMenuItem4.Click += (_, _) => PowerRequest("suspend", machineData);
        pMenuItem5.Click += (_, _) => PowerRequest("start", machineData);
        pMenuItem6.Click += (_, _) => PowerRequest("suspend", machineData, true);
        pMenuItem7.Click += (_, _) => PowerRequest("stop", machineData);
        pMenuItem8.Click += (_, _) => PowerRequest("reset", machineData);
        
        // Set Click Action for Dots
        newPbDots.MouseClick += (_, e) =>
        {
            if (e.Button == MouseButtons.Left || e.Button == MouseButtons.Right)
            {
                mainContextMenu.Show(newPbDots, e.Location);
            }
        };
        
        // Set Click Style for Icon
        if (machineData.Status == "running")
        {
            newPbImg.MouseHover += (_, _) => { newPbImg.Cursor = Cursors.Hand; };
            newPbImg.MouseLeave += (_, _) => { newPbImg.Cursor = Cursors.Default; };    
        } else
        {
            newPbImg.MouseHover += (_, _) => { newPbImg.Cursor = Cursors.No; };
            newPbImg.MouseLeave += (_, _) => { newPbImg.Cursor = Cursors.Default; };
        }
        // Set Click Action for Icon
        newPbImg.MouseClick += (_, e) =>
        {
            if (e.Button == MouseButtons.Left)
            {
                if (machineData.Status == "running")
                {
                    if (rMenuItem2.Enabled) { SpiceClient(machineData.Vmid); }
                    else if (rMenuItem3.Enabled) { WebClient(machineData, "xtermjs"); }
                    else { WebClient(machineData, "novnc"); }
                }
            }
        };
        
        // Set States for Menu Items
        // Remote Items
        remoteContextMenu.Enabled = (machineData.Status == "running");
        rMenuItem2.Enabled = (machineData.Type == "qemu" && CheckSpiceAble(machineData));                               // SPICE if KVM & Spice Enabled.
        rMenuItem3.Enabled = (machineData.Type == "lxc" || machineData.Serial == 1);                                    // LXC/Serial Enabled
        // Power Items
        pMenuItem1.Enabled = (machineData.Status != "running" && machineData.Lock != "suspended" );
        pMenuItem2.Enabled = (machineData.Status == "running");
        pMenuItem3.Enabled = (machineData.Status == "running");
        pMenuItem4.Enabled = (machineData.Status == "running" && machineData.Type != "lxc");
        pMenuItem5.Enabled = (machineData.Status != "running" && machineData.Lock == "suspended" && machineData.Type != "lxc");
        pMenuItem6.Enabled = (machineData.Status == "running" && machineData.Type != "lxc");
        pMenuItem7.Enabled = (machineData.Status == "running");
        pMenuItem8.Enabled = (machineData.Status == "running" && machineData.Type != "lxc");

        // Add Objects to Tile 
        newPbDots.ContextMenuStrip = mainContextMenu;
        newGroupBox.Controls.Add(newPbDots);
        newGroupBox.Controls.Add(newPbImg);
        newPbImg.Controls.Add(newStatus);
        machinePanel.Controls.Add(newGroupBox);

    }
    // Check for Spice in Config
    private bool CheckSpiceAble(MachineData machine)
    {
        string node = machine.NodeName;
        string vmid = machine.Vmid.ToString();
            
        // Fetch the JSON string from the API
        string jsonString = Program._Api.GetRequest($"nodes/{node}/qemu/{vmid}/status/current");
            
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
    
    // Action for Power State Requests
    private async void PowerRequest(string state, MachineData machine, bool extra = false)
    {
        string node = machine.NodeName;
        string vmid = machine.Vmid.ToString();
        string type = machine.Type;
        Dictionary<string, string> postData = new Dictionary<string, string>();
        postData.Add("node", node);
        postData.Add("vmid", vmid);
        if (extra && state == "suspend") { postData.Add("todisk", "1"); }
        
        // Fetch the JSON string from the API
        string results = Program._Api.PostRequest($"/api2/json/nodes/{node}/{type}/{vmid}/status/{state}", postData);
        
        if (results == "403")
        {
            MessageBox.Show("You do not have sufficent permissions to change the power state.","Permission Denied");
        }

        // Trigger Refresh
        await Task.Delay(5000); // Wait for 5 seconds
        RefreshContent(null, null); // Trigger Refresh
    } 
    
    // Closing the Application/Logging out
    private void ActionLogout(object sender, EventArgs e)
    {
        Close();
    }
    private void ActionExitApplication(object sender, EventArgs e)
    {
        Application.Exit();
    }
    
    // Reopen ClientLogin Window on Close
    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        base.OnFormClosing(e);
        _refreshTimer.Stop();
        _refreshTimer.Dispose();
        _refreshTicket.Stop();
        _refreshTicket.Dispose();
        _notifyIcon.Dispose();
        var theWindow = (ClientLogin)Program._Panels["ClientLogin"];
        theWindow.Show();
        theWindow.LoadCredentials();
    }
    
    // Opening New Windows(Panels)
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
    
    // Opening Consoles
    private async void SpiceClient(int vmid)
    {
        await Task.Run(() =>
        {
            var spiceClient = new SpiceClient(Program._Api.Machines.FirstOrDefault(m => m.Vmid == vmid));
            spiceClient.RequestSpiceConnection(); // Assuming this is a blocking call
        });
    }

    private void WebClient(MachineData machineData, string remoteType)
    {
        var webClient = new NoVncClient(machineData);
        webClient.RequestWebConnection(remoteType); // Initialize the connection
        webClient.Show(); // Show the form as a non-blocking window
    }
    
    private void InitializeNotifyIcon()
    {
        _notifyIcon = new NotifyIcon();
        _notifyIcon.Icon = Properties.Resources.icon_proxmox; // Set your desired icon here
        _notifyIcon.Visible = true;
        _notifyIcon.DoubleClick += (sender, args) => Restore_Click(null, null);
        _contextMenu = new ContextMenuStrip();
        _contextMenu.Items.Add("Restore", null, Restore_Click);
        _contextMenu.Items.Add("Exit", null, ActionExitApplication);
        _notifyIcon.ContextMenuStrip = _contextMenu;
    }

    private void ClientLogin_Resize(object sender, EventArgs e)
    {
        if (WindowState == FormWindowState.Minimized)
        {
            Hide();
        }
    }

    private void Restore_Click(object sender, EventArgs e)
    {
        Show();
        WindowState = FormWindowState.Normal;
    }

    private void click_openAbout(object sender, EventArgs e)
    {
        (new About()).ShowDialog();
    }

    private void webGUI_Click(object sender, EventArgs e)
    {
        (new WebGUI()).ShowDialog();
    }
}