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
    public string ConvertJsonToVVFormat(string jsonResponse, string proxyServer = null, string proxyPort = null)
    {
        // Parse the JSON response
        var jsonData = JObject.Parse(jsonResponse)["data"];
        
        string server = String.Empty;
        string port = String.Empty;
        if (proxyServer == null) { server = _Api._ServerUserData.server; } else { server = proxyServer; }
        if (proxyServer == null) { port = "3128"; } else { port = proxyPort; }

        // Format the .vv file content
        
        string vvFileData = String.Empty; 
        vvFileData  = "[virt-viewer]\n\r";
        vvFileData += $"toggle-fullscreen="+jsonData["toggle-fullscreen"].ToString()+"\n\r";
        vvFileData += $"proxy=http://"+server+":"+port+"\n\r";
        vvFileData += $"title="+jsonData["title"].ToString()+"\n\r";
        vvFileData += $"host-subject="+jsonData["host-subject"].ToString()+"\n\r";
        vvFileData += $"secure-attention="+jsonData["secure-attention"].ToString()+"\n\r";
        vvFileData += $"ca="+jsonData["ca"].ToString()+"\n\r";
        vvFileData += $"type="+jsonData["type"].ToString()+"\n";
        vvFileData += $"tls-port="+jsonData["tls-port"].ToString()+"\n\r";
        vvFileData += $"password="+jsonData["password"].ToString()+"\n\r";
        vvFileData += $"host="+jsonData["host"].ToString()+"\n\r";
        vvFileData += $"delete-this-file="+jsonData["delete-this-file"].ToString()+"\n\r";
        vvFileData += $"release-cursor="+jsonData["release-cursor"].ToString()+"\n\r";
        
        return vvFileData.Trim();
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
            // Clean up the temporary file
            if (File.Exists(tempFilePath))
            {
                File.Delete(tempFilePath);
            }
        }
    }
    
    
    
    
    
}