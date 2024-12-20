# Proxmox Desktop Client

A Windows Desktop Client for Proxmox VE.

## About
The Proxmox Desktop Client is designed to provide quick remote access to virtual machines in a homelab environment without the need to log into the Proxmox WebGUI. This client supports various types of virtual machines, including those that do not use SPICE, allowing for console-level access to both containers and non-SPICE VMs.

## Screenshots
- Available in the Screenshot Folder.

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

## Future Roadmap
- [Details to be added]
