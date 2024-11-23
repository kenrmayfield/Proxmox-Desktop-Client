using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Proxmox_Desktop_Client.Classes.pveAPI.objects;

public class RootObject<T>
{
    [JsonProperty("data")]
    public List<T> Data { get; set; }
}

public class NodeData
{
    [JsonProperty("level")]
    public string Level { get; set; }

    [JsonProperty("type")]
    public string Type { get; set; }

    [JsonProperty("node")]
    public string Node { get; set; }

    [JsonProperty("status")]
    public string Status { get; set; }

    [JsonProperty("ssl_fingerprint")]
    public string SslFingerprint { get; set; }

    [JsonProperty("id")]
    public string Id { get; set; }
}
public class MachineData
{
    [JsonProperty("pid")]
    public int Pid { get; set; }

    [JsonProperty("vmid")]
    public int Vmid { get; set; }
    
    [JsonProperty("name")]
    public string Name { get; set; }
    
    [JsonProperty("nodename")]
    public string NodeName { get; set; }
    
    [JsonProperty("type")]
    public string Type { get; set; } = "qemu";  // If not set by PVE then QEMU.
   
    [JsonProperty("cpu")]
    public double Cpu { get; set; }
    
    [JsonProperty("cpus")]
    public int Cpus { get; set; }
    
    [JsonProperty("mem")]
    public long Mem { get; set; }

    [JsonProperty("maxmem")]
    public long MaxMem { get; set; }

    [JsonProperty("netin")]
    public long NetIn { get; set; }
    
    [JsonProperty("netout")]
    public long NetOut { get; set; }
    
    [JsonProperty("disk")]
    public long Disk { get; set; }

    [JsonProperty("diskread")]
    public long DiskRead { get; set; }

    [JsonProperty("diskwrite")]
    public long DiskWrite { get; set;} 
    
    [JsonProperty("maxdisk")]
    public long MaxDisk { get; set; }
    
    [JsonProperty("swap")]
    public long Swap { get; set; }
    
    [JsonProperty("maxswap")]
    public long MaxSwap { get; set; }
    
    [JsonProperty("tags")]
    public string Tags { get; set; }

    [JsonProperty("uptime")]
    public long Uptime { get; set; }

    [JsonProperty("status")]
    public string Status { get; set; }
    
    [JsonProperty("serial")]
    public int Serial { get; set; }
}
public class RealmData
{ 
    [JsonProperty("realm")]
    public string Realm { get; set; }
    [JsonProperty("comment")]
    public string Comment { get; set; }
    [JsonProperty("type")]
    public string Type { get; set; }
}

public class SpiceObject
{
    [JsonProperty("title")]
    public string Title { get; set; }

    [JsonProperty("toggle-fullscreen")]
    public string ToggleFullscreen { get; set; }

    [JsonProperty("proxy")]
    public string Proxy { get; set; }

    [JsonProperty("type")]
    public string Type { get; set; }

    [JsonProperty("ca")]
    public string Ca { get; set; }

    [JsonProperty("host-subject")]
    public string HostSubject { get; set; }

    [JsonProperty("secure-attention")]
    public string SecureAttention { get; set; }

    [JsonProperty("tls-port")]
    public int TlsPort { get; set; }

    [JsonProperty("password")]
    public string Password { get; set; }

    [JsonProperty("release-cursor")]
    public string ReleaseCursor { get; set; }

    [JsonProperty("delete-this-file")]
    public int DeleteThisFile { get; set; }

    [JsonProperty("host")]
    public string Host { get; set; }
}

public class dataServerInfo
{
    public string server; 
    public string port; 
    public bool skipSSL;
    public string username;
    public string password;
    public string realm;
}
