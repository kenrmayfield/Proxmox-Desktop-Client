using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Proxmox_Desktop_Client.Classes;
using Proxmox_Desktop_Client.Classes.consoles;
using Proxmox_Desktop_Client.Classes.pveAPI;
using Proxmox_Desktop_Client.Classes.pveAPI.objects;

namespace Proxmox_Desktop_Client
{
    public partial class MainPanel : Form
    {
        private readonly ApiClient _api;
        private readonly UserPermissions _permissions;

        public MainPanel()
        {
            Load += RefreshContent;
            _api = Program._Api;
            _permissions = Program._Permissions;
            InitializeComponent();
            Show();
        }

        private async void RefreshContent(object sender, EventArgs e)
        {
            await _api.GetPermissionsAsync();
            await GetNodesAndMachinesAsync();
            ProcessMachineList();
        }

        public async Task GetNodesAndMachinesAsync()
        {
            var allMachines = new List<MachineData>();
            string nodesJson = await _api.GetAsync("nodes");

            if (nodesJson != null)
            {
                var nodesRoot = JsonConvert.DeserializeObject<RootObject<NodeData>>(nodesJson);

                foreach (var node in nodesRoot.Data)
                {
                    await AddMachinesFromNodeAsync(node, "lxc", allMachines);
                    await AddMachinesFromNodeAsync(node, "qemu", allMachines);
                }
            }

            _api.Machines = allMachines.OrderBy(data => data.Vmid).ToList();
        }

        private async Task AddMachinesFromNodeAsync(NodeData node, string type, List<MachineData> allMachines)
        {
            string path = $"nodes/{node.Node}/{type}";
            string json = await _api.GetAsync(path);

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

        private async void AddMachine2Panel(int id, MachineData machineData)
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
                Image = global::Proxmox_Desktop_Client.Properties.Resources.icons_dots,
                SizeMode = PictureBoxSizeMode.StretchImage
            };

            var contextMenu = new ContextMenuStrip();
            var menuItem1 = new ToolStripMenuItem("NoVNC");
            var menuItem2 = new ToolStripMenuItem("Spice");
            var menuItem3 = new ToolStripMenuItem("xTermJS");

            menuItem1.Click += (sender, e) => webClient(machineData, "novnc");
            menuItem2.Click += (sender, e) => Spice_Client(machineData.Vmid);
            menuItem3.Click += (sender, e) => webClient(machineData, "xtermjs");
            
            menuItem1.Enabled = false;
            menuItem2.Enabled = false;
            menuItem3.Enabled = false;
            
            contextMenu.Items.AddRange(new ToolStripItem[] { menuItem1, menuItem2, menuItem3 });
            newPbDots.ContextMenuStrip = contextMenu;

            newPbDots.MouseClick += (sender, e) =>
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
                        global::Proxmox_Desktop_Client.Properties.Resources.lxc_logo : 
                        global::Proxmox_Desktop_Client.Properties.Resources.vm_logo,
                SizeMode = PictureBoxSizeMode.StretchImage
            };

            newGroupBox.Controls.Add(newPbDots);
            newGroupBox.Controls.Add(newPbImg);
            
            panel_machines.Controls.Add(newGroupBox);
            
            if (machineData.Status == "running")
            {
                menuItem1.Enabled = true;
                if(machineData.Type == "lxc" || machineData.Serial == 1)
                {
                    menuItem3.Enabled = true;    
                }
            
                if(machineData.Type == "qemu")
                {
                    menuItem2.Enabled = (bool) await CheckSpiceAble(machineData);
                }    
            }
            
            
            
        }

        private void webClient(MachineData machineData, string RemoteType)
        {
            NoVNCClient webClient = new NoVNCClient(Program._Api, machineData, RemoteType);
        }
        
        private async void Spice_Client(int vmid)
        {
            var spiceClient = new SpiceClient(_api, _api.Machines.FirstOrDefault(m => m.Vmid == vmid));
            await spiceClient.RequestSpiceConnection();
        }

        private async Task<bool> CheckSpiceAble(MachineData Machine)
        {
            string node = Machine.NodeName;
            string vmid = Machine.Vmid.ToString();
            
            // Fetch the JSON string from the API
            string jsonString = await _api.GetAsync($"nodes/{node}/qemu/{vmid}/status/current");
            
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
                AddMachine2Panel(machine.Vmid, machine);
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

        private void openSpiceProxyPanel(object sender, EventArgs e)
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
}
