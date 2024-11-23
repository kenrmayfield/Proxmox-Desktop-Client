using System.Collections.Generic;
using System.Text.Json;

// Currently not in use at this time... 

namespace Proxmox_Desktop_Client.Classes.pveAPI;

public class UserPermissions
{
    private readonly Dictionary<string, Dictionary<string, int>> _permissionsData;

    public UserPermissions(string jsonString)
    {
        var jsonDocument = JsonDocument.Parse(jsonString);
        _permissionsData = new Dictionary<string, Dictionary<string, int>>();

        foreach (var element in jsonDocument.RootElement.GetProperty("data").EnumerateObject())
        {
            var path = element.Name;
            var permissions = new Dictionary<string, int>();

            foreach (var permission in element.Value.EnumerateObject())
            {
                permissions[permission.Name] = permission.Value.GetInt32();
            }

            _permissionsData[path] = permissions;
        }
    }

    public bool HasAnyPermission(string action)
    {
        foreach (var pathPermissions in _permissionsData.Values)
        {
            if (pathPermissions.ContainsKey(action) && pathPermissions[action] == 1)
            {
                return true;
            }
        }
        return false;
    }
}