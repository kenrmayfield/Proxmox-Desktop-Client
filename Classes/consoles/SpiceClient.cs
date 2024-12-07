using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using Proxmox_Desktop_Client.Classes.pveAPI.objects;

namespace Proxmox_Desktop_Client.Classes.consoles;

//
// Test Spice
// SpiceClient spiceClient = new SpiceClient(_Api, _Api.Machines.FirstOrDefault(m => m.Vmid == 8001));
// await spiceClient.RequestSpiceConnection();

public class SpiceClient
{
    private MachineData _machine;

    public SpiceClient(MachineData machine)
    {
        _machine = machine;
    }
    public void RequestSpiceConnection() {
        Dictionary<string, string> postData = new Dictionary<string, string>();
        
        postData.Add("node", _machine.NodeName);
        postData.Add("vmid", _machine.Vmid.ToString());
        
        string results = Program._Api.PostRequest("nodes/"+ _machine.NodeName +"/qemu/"+_machine.Vmid+"/spiceproxy", postData);
        if (results == "403")
        {
            MessageBox.Show("You do not have sufficent permissions to access the console.","Permission Denied");
            return;            
        }
        var rootObject = ConvertJsonToVvFormat(results);
        LaunchVirtViewer(rootObject);
    }
    private string ConvertJsonToVvFormat(string jsonResponse)
    {
        // Parse the JSON response
        var jsonData = JObject.Parse(jsonResponse)["data"];

        string proxyServer = Program._Config.GetSetting("SpiceProxy_Server") as string ?? string.Empty;
        string proxyPort = Program._Config.GetSetting("SpiceProxy_Port") as string ?? string.Empty;
        bool proxyEnabled = Program._Config.GetSetting("SpiceProxy_Enable") as bool? ?? false;
        
        // Extract necessary fields
        string toggleFullscreen = jsonData!["toggle-fullscreen"]!.ToString();
        string proxy = "http://" + Program._Api.DataServerInfo.server + ":3128";
        if (proxyEnabled)
        {
            proxy = "http://" + proxyServer + ":" + proxyPort;
        }
        string title = jsonData["title"]!.ToString();
        string hostSubject = jsonData["host-subject"]!.ToString();
        string secureAttention = jsonData["secure-attention"]!.ToString();
        string caCertificate = jsonData["ca"]!.ToString();// Summarized
        string type = jsonData["type"]!.ToString();
        int tlsPort = (int)jsonData["tls-port"];
        string password = jsonData["password"]!.ToString();
        string host = jsonData["host"]!.ToString();
        string deleteThisFile = jsonData["delete-this-file"]!.ToString();
        string releaseCursor = jsonData["release-cursor"]!.ToString();

        // Format the .vv file content
        string vvFileContent = "[virt-viewer]\n";
        vvFileContent += $"host={host}\n";
        vvFileContent += $"delete-this-file={deleteThisFile}\n";
        vvFileContent += $"release-cursor={releaseCursor}\n";
        vvFileContent += $"password={password}\n";
        vvFileContent += $"tls-port={tlsPort}\n";
        vvFileContent += $"secure-attention={secureAttention}\n";
        vvFileContent += $"host-subject={hostSubject}\n";
        vvFileContent += $"ca={caCertificate}\n";
        vvFileContent += $"type={type}\n";
        vvFileContent += $"proxy={proxy}\n";
        vvFileContent += $"toggle-fullscreen={toggleFullscreen}\n";
        vvFileContent += $"fullscreen=1\n";
        vvFileContent += $"title={title}";

        return vvFileContent.Trim();
    }
    
    public static string FindRemoteViewerPath()
    {
        string baseDirectory = @"C:\Program Files";
        string fileName = "remote-viewer.exe";

        // Get all directories starting with "VirtViewer"
        var directories = Directory.GetDirectories(baseDirectory, "VirtViewer*", SearchOption.TopDirectoryOnly);

        foreach (var directory in directories)
        {
            string binDirectory = Path.Combine(directory, "bin");
            string remoteViewerPath = Path.Combine(binDirectory, fileName);

            // Check if the "bin" directory exists and if remote-viewer.exe is present
            if (Directory.Exists(binDirectory) && File.Exists(remoteViewerPath))
            {
                return remoteViewerPath; // Returns the full file path including remote-viewer.exe
            }
        }

        return null; // or return an error message
    }
    
    public static void LaunchVirtViewer(string spiceObject)
    {
        // Create a temporary file
        string tempFilePath = Path.GetTempFileName();

        try
        {
            // Write the .vv file contents to the temporary file
            File.WriteAllText(tempFilePath, spiceObject);

            // Start virt-viewer with the temporary file as an argument
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = FindRemoteViewerPath(),
                Arguments = $"\"{tempFilePath}\"",
                UseShellExecute = false, // Use shell execute to run the application directly
                RedirectStandardError = true // Redirect standard error if needed
            };

            // Start the process without waiting for it to exit
            Process.Start(startInfo);
        }
        catch (Exception ex)
        {
            // Handle exceptions (e.g., logging)
            Console.Error.WriteLine($"Error launching Virt Viewer: {ex.Message}");
        }
        finally
        {

        }
    }

}