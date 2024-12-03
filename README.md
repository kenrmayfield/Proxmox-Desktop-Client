# Proxmox Desktop Client

A Windows Desktop Client for Proxmox VE.

## About
It's purpose is as designed. A way to remote virtual machines in my homelab without having to log in to Proxmox WebGUI.
The intent was to quickly remote my virtual machines without the need to login and navigate the full Proxmox GUI. The
other solution out there only supported SPICE and I have containers and Non-Spice virtual machines I want to access at the
Console level.

## Screenshots
- Placed in Screenshot Folder

## Requirements
- (Program) .Net 4.8.1 or Newer
- (SPICE) Virt-viewer && UsbDk (https://www.spice-space.org/download.html).
- (NoVNC/xTermJS) WebView2 Runtime (https://developer.microsoft.com/en-us/microsoft-edge/webview2/?form=MA13LH)

## Functionality
- Plain & TOTP Login
- Remote (NoVNC/SPICE/xtermJS)
- Click Tile to Launch (Attempts in Order: SPICE/xTermJS/NoVNC)
- Power Controls (Move improvements in later release)
- VM Panel refreshes every 60 seconds, 5 seconds after a power state request.
- Ability to provide alternate SPICE Proxy Information

### Minimum Permissions for a User to See and Remote Virtual Machine
- VM.Audit
- VM.Console

### Minimum Permissions for Power Control
- VM.PowerMgmt

## Known Issues
- Check out Issue section.

## Future Road Map
