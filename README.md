[![SS-2.png](https://i.postimg.cc/NFGVHcv3/SS-2.png)](https://postimg.cc/Q9PS2vtm)

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

## Known Issues
- Spice doesn't always get enabled if the VM was the "last" to be loaded from the server. (Fixed - Will be in the next release.)
- Full-screen/scale issues with NoVNC & xtermJS.  (Fixed - Will be in the next release.)

## Future Road Map
- Refresh VM List / Session Refresh
- Status Icon (Online/Offline), Currently Menu disabled if offline.

## Additional Images
[![SS-1.png](https://i.postimg.cc/4dgjXcLN/SS-1.png)](https://postimg.cc/06VVfz1L)
[![SS-2.png](https://i.postimg.cc/NFGVHcv3/SS-2.png)](https://postimg.cc/Q9PS2vtm)
[![SS-3.png](https://i.postimg.cc/nrdWKPHC/SS-3.png)](https://postimg.cc/fS95D5SN)
[![SS-4.png](https://i.postimg.cc/X7JmN48F/SS-4.png)](https://postimg.cc/5jhnncDN)
