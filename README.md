[![Screenshot-2024-11-24-022723.png](https://i.postimg.cc/13Gsf5rW/Screenshot-2024-11-24-022723.png)](https://postimg.cc/DSwtNhDs)

# Proxmox Desktop Client

A Windows Desktop Client for Proxmox VE.

## About
It's purpose is as designed. A way to remote virtual machines in my homelab without having to log in to Proxmox WebGUI.
The intent was to quickly remote my virtual machines without the need to login and navigate the full Proxmox GUI. The
other solution out there only supported SPICE and I have containers and Non-Spice virtual machines I want to access at the
Console level.

## Requirements for SPICE
- Virt-viewer && UsbDk (https://www.spice-space.org/download.html).
- Dot Net 4.8.1 or Newer
- WebView2 (Edge)

## Functionality
- Plain & TOTP Login
- Remote (NoVNC/SPICE/xtermJS)
- Click Tile to Launch (Attempts in Order: SPICE/xTermJS/NoVNC)
- Power Controls (Move improvements in later release)
- VM Panel refreshes every 30 seconds, 5 seconds after a power state request.
- Ability to provide alternate SPICE Proxy Information

### Minimum Permissions for a User to See and Remote Virtual Machine
- VM.Audit
- VM.Console

### Minimum Permissions for Power Control
- VM.PowerMgmt

## Known Issues
- Check out Issue section.

## Future Road Map
- Status Icon (Online/Offline), Currently Menu disabled if offline.

## Additional Images
[![Screenshot-2024-11-24-022701.png](https://i.postimg.cc/d3ftmHh4/Screenshot-2024-11-24-022701.png)](https://postimg.cc/WqMvs76k)
[![Screenshot-2024-11-24-022723.png](https://i.postimg.cc/13Gsf5rW/Screenshot-2024-11-24-022723.png)](https://postimg.cc/DSwtNhDs)
[![Screenshot-2024-11-24-022755.png](https://i.postimg.cc/k57983j7/Screenshot-2024-11-24-022755.png)](https://postimg.cc/dhNzPXPf)
[![Screenshot-2024-11-24-022815.png](https://i.postimg.cc/0QQsWGR3/Screenshot-2024-11-24-022815.png)](https://postimg.cc/ns6gMDLY)
[![Screenshot-2024-11-24-022854.png](https://i.postimg.cc/G90nmZ8w/Screenshot-2024-11-24-022854.png)](https://postimg.cc/MfYFs3md)
[![Screenshot-2024-11-24-024236.png](https://i.postimg.cc/XNrZXdGQ/Screenshot-2024-11-24-024236.png)](https://postimg.cc/K4ycHK5M)