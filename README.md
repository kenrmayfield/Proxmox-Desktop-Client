[![Screenshot-2024-11-24-022723.png](https://i.postimg.cc/13Gsf5rW/Screenshot-2024-11-24-022723.png)](https://postimg.cc/DSwtNhDs)

# Proxmox Desktop Client

A Windows Desktop Client for Proxmox VE.

## About

It's purpose is as designed. A way to remote virtual machines in my homelab without having to log in to Proxmox WebGUI.
The intent was to quickly remote my virtual machines without the need to login and navigate the full Proxmox GUI. The
other
solution out there only supported SPICE and I have containers and Non-Spice virtual machines I want to access at the
Console level.

## Requirements for SPICE

- Virt-viewer && UsbDk (https://www.spice-space.org/download.html).
- Dot Net 4.8.1 or Newer
- WebView2 (Edge)

## Functionality

- Plain & TOTP Login
- Remote (NoVNC/SPICE/xtermJS)
- Ability to provide alternate SPICE Proxy Information

## Known Issues

- Spice doesn't always get enabled if the VM was the "last" to be loaded from the server. (Fixed - Will be in the next
  release.)
- Full-screen/scale issues with NoVNC & xtermJS.  (Fixed - Will be in the next release.)

## Future Road Map

- Refresh VM List / Session Refresh
- Status Icon (Online/Offline), Currently Menu disabled if offline.

## Additional Images
[![Screenshot-2024-11-24-022701.png](https://i.postimg.cc/d3ftmHh4/Screenshot-2024-11-24-022701.png)](https://postimg.cc/WqMvs76k)
[![Screenshot-2024-11-24-022723.png](https://i.postimg.cc/13Gsf5rW/Screenshot-2024-11-24-022723.png)](https://postimg.cc/DSwtNhDs)
[![Screenshot-2024-11-24-022755.png](https://i.postimg.cc/k57983j7/Screenshot-2024-11-24-022755.png)](https://postimg.cc/dhNzPXPf)
[![Screenshot-2024-11-24-022815.png](https://i.postimg.cc/0QQsWGR3/Screenshot-2024-11-24-022815.png)](https://postimg.cc/ns6gMDLY)
[![Screenshot-2024-11-24-022854.png](https://i.postimg.cc/G90nmZ8w/Screenshot-2024-11-24-022854.png)](https://postimg.cc/MfYFs3md)
[![Screenshot-2024-11-24-024236.png](https://i.postimg.cc/XNrZXdGQ/Screenshot-2024-11-24-024236.png)](https://postimg.cc/K4ycHK5M)