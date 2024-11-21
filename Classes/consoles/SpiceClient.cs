using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Proxmox_Desktop_Client.Classes.pveAPI;
using Proxmox_Desktop_Client.Classes.pveAPI.objects;

namespace Proxmox_Desktop_Client.Classes.consoles;

//
// Test Spice
// SpiceClient spiceClient = new SpiceClient(_Api, _Api.Machines.FirstOrDefault(m => m.Vmid == 8001));
// await spiceClient.RequestSpiceConnection();

public class SpiceClient
{
    private ApiClient _Api;
    private MachineData _Machine;

    public SpiceClient(ApiClient Api, MachineData Machine)
    {
    
        _Api = Api;
        _Machine = Machine;
        
    }
    public async Task RequestSpiceConnection() {
        Dictionary<string, string> postData = new Dictionary<string, string>();
        
        postData.Add("node", _Machine.NodeName);
        postData.Add("vmid", _Machine.Vmid.ToString());
        
        string results = await _Api.PostAsync("nodes/"+ _Machine.NodeName +"/qemu/"+_Machine.Vmid+"/spiceproxy", postData);
        var rootObject = ConvertJsonToVVFormat(results);
        LaunchVirtViewer(rootObject);
    }
     public string ConvertJsonToVVFormat(string jsonResponse)
    {
        // Parse the JSON response
        var jsonData = JObject.Parse(jsonResponse)["data"];

        string proxyServer = Program._Config.GetSetting("SpiceProxy_Server") as string ?? string.Empty;
        string proxyPort = Program._Config.GetSetting("SpiceProxy_Port") as string ?? string.Empty;
        bool proxyEnabled = Program._Config.GetSetting("SpiceProxy_Enable") as bool? ?? false;
        
        // Extract necessary fields
        string toggleFullscreen = jsonData["toggle-fullscreen"].ToString();
        //string proxy = jsonData["proxy"].ToString();
        string proxy = "http://" + _Api._ServerUserData.server + ":3128";
        if (proxyEnabled)
        {
            proxy = "http://" + proxyServer + ":" + proxyPort;
        }
        string title = jsonData["title"].ToString();
        string hostSubject = jsonData["host-subject"].ToString();
        string secureAttention = jsonData["secure-attention"].ToString();
        string caCertificate = jsonData["ca"].ToString();// Summarized
        string type = jsonData["type"].ToString();
        int tlsPort = (int)jsonData["tls-port"];
        string password = jsonData["password"].ToString();
        string host = jsonData["host"].ToString();
        string deleteThisFile = jsonData["delete-this-file"].ToString();
        string releaseCursor = jsonData["release-cursor"].ToString();

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
        vvFileContent += $"title={title}";

        return vvFileContent.Trim();
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
                FileName = "C:\\Program Files\\VirtViewer v11.0-256\\bin\\remote-viewer.exe",
                Arguments = $"\"{tempFilePath}\"",
                UseShellExecute = false,
                RedirectStandardError = true // Redirect standard error
            };

            using (Process process = Process.Start(startInfo))
            {
                // Optionally read the error stream to log or ignore
                using (StreamReader reader = process.StandardError)
                {
                    string errorOutput = reader.ReadToEnd();
                    // Log the error output if needed
                    // Console.WriteLine(errorOutput); // Uncomment to log errors
                }

                process.WaitForExit();
            }
        }
        finally
        {
            // DO NOTHING
        }
    }
    
    
    
    
}