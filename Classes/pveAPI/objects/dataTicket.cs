using System;

namespace Proxmox_Desktop_Client.Classes.pveAPI
{
	public class dataTicket
	{
		public string CSRFPreventionToken { get; set; }
		public string ticket { get; set; }
		public string username { get; set; }
	}
}