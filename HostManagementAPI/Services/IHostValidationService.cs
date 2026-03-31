using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HostManagementAPI;

public interface IHostValidationService
{
    bool IsValidIpAddress(string ipAddress);
    bool IsValidPort(int port);
    bool IsValidHostName(string hostName);
}

public class HostValidationService : IHostValidationService
{
    public bool IsValidIpAddress(string ipAddress)
    {
        return System.Net.IPAddress.TryParse(ipAddress, out _);
    }

    public bool IsValidPort(int port)
    {
        return port > 0 && port <= 65535;
    }

    public bool IsValidHostName(string hostName)
    {
        // Check if the host name is not null or whitespace
        if (string.IsNullOrWhiteSpace(hostName))
        {
            return false;
        }
        else if (hostName.Length > 255)
        {
            return false;
        }

        // Check if the host name contains any invalid characters
        else if (hostName.Any(c => !char.IsLetterOrDigit(c) && c != '-' && c != '.'))
        {
            return false;
        }

        else if (hostName.StartsWith(".") || hostName.EndsWith("."))
        {
            return false;
        }

        else if (hostName.Contains(".."))
        {
            return false;
        }

        else if (hostName.Split('.').Any(label => label.Length > 63))
        {
            return false;
        }

        else if (hostName.Split('.').Any(label => label.StartsWith("-") || label.EndsWith("-")))
        {
            return false;
        }

        else if (hostName.Split('.').Any(label => label.All(c => char.IsDigit(c))))
        {
            return false;
        }

        else if (hostName.Split('.').Any(label => label.Length == 0))
        {
            return false;
        }

        return true;
    }
}