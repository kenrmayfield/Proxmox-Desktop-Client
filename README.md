[![Screenshot-2024-11-24-022723.png](https://i.postimg.cc/13Gsf5rW/Screenshot-2024-11-24-022723.png)](https://postimg.cc/DSwtNhDs)

# Proxmox Desktop Client

A Windows Desktop Client for Proxmox VE.

## About
It's purpose is as designed. A way to remote virtual machines in my homelab without having to log in to Proxmox WebGUI.
The intent was to quickly remote my virtual machines without the need to login and navigate the full Proxmox GUI. The
other solution out there only supported SPICE and I have containers and Non-Spice virtual machines I want to access at the
Console level.

## Requirements
- **.NET Framework**: Version 4.8.1 or newer.
- **SPICE Support**: Virt-viewer and UsbDk (download from [SPICE](https://www.spice-space.org/download.html)).
- **NoVNC/xTermJS Support**: WebView2 Runtime (download from [Microsoft](https://developer.microsoft.com/en-us/microsoft-edge/webview2/?form=MA13LH)).

## Functionality
- **Cluster GUI Access**: Integrated WebView panel that auto-logs in using the same API token.
- **Authentication**: Supports both plain and TOTP login methods.
- **Remote Access**: Launch remote sessions via NoVNC, SPICE, or xTermJS in that order of preference.
- **Power Controls**: Basic power management features, with enhancements planned for future releases.
- **VM Panel Refresh**: Automatically refreshes every 60 seconds and 5 seconds after a power state change.
- **SPICE Proxy Configuration**: Option to provide alternate SPICE proxy information.

### Minimum Permissions Required
- **For Viewing and Remote Access**:
    - VM.Audit
    - VM.Console
- **For Power Control**:
    - VM.PowerMgmt

## Known Issues
- Refer to the Issues section for details.

## Future Road Map
- Status Icon (Online/Offline), Currently Menu disabled if offline.

## Additional Images
[![Screenshot-2024-11-24-022701.png](https://i.postimg.cc/d3ftmHh4/Screenshot-2024-11-24-022701.png)](https://postimg.cc/WqMvs76k)
[![Screenshot-2024-11-24-022723.png](https://i.postimg.cc/13Gsf5rW/Screenshot-2024-11-24-022723.png)](https://postimg.cc/DSwtNhDs)
[![Screenshot-2024-11-24-022755.png](https://i.postimg.cc/k57983j7/Screenshot-2024-11-24-022755.png)](https://postimg.cc/dhNzPXPf)
[![Screenshot-2024-11-24-022815.png](https://i.postimg.cc/0QQsWGR3/Screenshot-2024-11-24-022815.png)](https://postimg.cc/ns6gMDLY)
[![Screenshot-2024-11-24-022854.png](https://i.postimg.cc/G90nmZ8w/Screenshot-2024-11-24-022854.png)](https://postimg.cc/MfYFs3md)
[![Screenshot-2024-11-24-024236.png](https://i.postimg.cc/XNrZXdGQ/Screenshot-2024-11-24-024236.png)](https://postimg.cc/K4ycHK5M)
