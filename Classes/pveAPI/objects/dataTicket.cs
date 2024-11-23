namespace Proxmox_Desktop_Client.Classes.pveAPI.objects
{
	public class DataTicket
	{
		// ReSharper disable once InconsistentNaming
		public string CSRFPreventionToken { get; set; }
		// ReSharper disable once InconsistentNaming
		public string ticket { get; set; }
		// ReSharper disable once InconsistentNaming
		public string username { get; set; }
	}
}