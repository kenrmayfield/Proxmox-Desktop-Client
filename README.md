# Proxmox Desktop Client
A Windows Desktop Client for Proxmox VE.

## About
It's purpose is as designed.  A way to remote virtual machines in my homelab without having to log in to Proxmox WebGUI.
The intent mostly was for the use of SPICE (virt-viewer) connectivity as some of my remote needs I can't use RDP but console view.

## Requirements for SPICE
- Virt-viewer && UsbDk (https://www.spice-space.org/download.html).
- Dot Net 4.8.1 or Newer
- WebView2 (Edge)

## Functionality
- Plain & TOTP Login
- Remote (NoVNC/SPICE/xtermJS)
- Ability to provide alternate SPICE Proxy Information

- ## Known Issues
- Spice doesn't always get enabled if the VM was the "last" to be loaded from the server.
- Full-screen/scale issues with NoVNC & xtermJS. 

