using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HostManagementAPI;

public interface IHostValidationService
{
    bool IsValidIpAddress(string ipAddress);
    bool IsValidPort(int port);
    bool IsValidHostName(string hostName);
    bool IsValidEnvironment(string environment);
    bool IsValidOperatingSystem(string operatingSystem);
    bool IsValidOwner(string owner);
}

public class HostValidationService : IHostValidationService
{
    public bool IsValidIpAddress(string ipAddress)
    {
        if (string.IsNullOrWhiteSpace(ipAddress))
        {
            return false;
        }
        else if (ipAddress.Length > 15)
        {
            return false;
        }
        else if (ipAddress.Any(c => !char.IsDigit(c) && c != '.'))
        {
            return false;
        }
        else if (ipAddress.StartsWith(".") || ipAddress.EndsWith("."))
        {
            return false;
        }
        else if (ipAddress.Contains(".."))
        {
            return false;
        }
        else if (ipAddress.Split('.').Length != 4)
        {
            return false;
        }
        else if (ipAddress.Split('.').Any(octet => octet.Length == 0 || octet.Length > 3))
        {
            return false;
        }
        else if (ipAddress.Split('.').Any(octet => octet.Any(c => !char.IsDigit(c))))
        {
            return false;
        }
        else if (ipAddress.Split('.').Any(octet => int.Parse(octet) < 0 || int.Parse(octet) > 255))
        {
            return false;
        }
        else if (ipAddress.Equals("0.0.0.0", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }
        else
        {
            return System.Net.IPAddress.TryParse(ipAddress, out _);
        }
    }

    public bool IsValidPort(int port)
    {
        if (port <= 0 || port > 65535)
        {
            return false;
        }
        else if (port >= 1024 && port <= 49151)
        {
            return true;
        }
        else if (port >= 49152 && port <= 65535)
        {
            return true;
        }
        else
        {
            return false;
        }
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

    public bool IsValidEnvironment(string environment)
    {
        // You can implement your own logic to validate the environment string
        if (string.IsNullOrWhiteSpace(environment))
        {
            return false;
        }
        else if (environment.Length > 255)
        {
            return false;
        }
        else if (environment.Any(c => !char.IsLetterOrDigit(c) && c != '-' && c != '_'))
        {
            return false;
        }
        else if (environment.StartsWith("-") || environment.EndsWith("-"))
        {
            return false;
        }
        else if (environment.StartsWith("_") || environment.EndsWith("_"))
        {
            return false;
        }
        else if (environment.Contains("--") || environment.Contains("__"))
        {
            return false;
        }
        else if (environment.Split('-').Any(label => label.Length == 0))
        {
            return false;
        }
        else if (environment.Split('_').Any(label => label.Length == 0))
        {
            return false;
        }
        else if (environment.Split('-').Any(label => label.All(c => char.IsDigit(c))))
        {
            return false;
        }
        else if (environment.Split('_').Any(label => label.All(c => char.IsDigit(c))))
        {
            return false;
        }
        else if (environment.Equals("production", StringComparison.OrdinalIgnoreCase) ||
                 environment.Equals("staging", StringComparison.OrdinalIgnoreCase) ||
                 environment.Equals("development", StringComparison.OrdinalIgnoreCase) ||
                 environment.Equals("testing", StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool IsValidOperatingSystem(string operatingSystem)
    {
        // You can implement your own logic to validate the operating system string
        if (string.IsNullOrWhiteSpace(operatingSystem))
        {
            return false;
        }
        else if (operatingSystem.Length > 255)
        {
            return false;
        }
        else if (operatingSystem.Any(c => !char.IsLetterOrDigit(c) && c != '-' && c != '_' && c != ' '))
        {
            return false;
        }
        else if (operatingSystem.StartsWith("-") || operatingSystem.EndsWith("-"))
        {
            return false;
        }
        else if (operatingSystem.StartsWith("_") || operatingSystem.EndsWith("_"))
        {
            return false;
        }
        else if (operatingSystem.Contains("--") || operatingSystem.Contains("__"))
        {
            return false;
        }
        else if (operatingSystem.Split('-').Any(label => label.Length == 0))
        {
            return false;
        }
        else if (operatingSystem.Split('_').Any(label => label.Length == 0))
        {
            return false;
        }
        else if (operatingSystem.Split(' ').Any(label => label.Length == 0))
        {
            return false;
        }
        else if (operatingSystem.Split('-').Any(label => label.All(c => char.IsDigit(c))))
        {
            return false;
        }
        else if (operatingSystem.Split('_').Any(label => label.All(c => char.IsDigit(c))))
        {
            return false;
        }
        else if (operatingSystem.Split(' ').Any(label => label.All(c => char.IsDigit(c))))
        {
            return false;
        }
        else if (operatingSystem.Equals("windows", StringComparison.OrdinalIgnoreCase) ||
                 operatingSystem.Equals("linux", StringComparison.OrdinalIgnoreCase) ||
                 operatingSystem.Equals("macos", StringComparison.OrdinalIgnoreCase) ||
                 operatingSystem.Equals("unix", StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool IsValidOwner(string owner)
    {
        // You can implement your own logic to validate the owner string
        if (string.IsNullOrWhiteSpace(owner))
        {
            return false;
        }
        else if (owner.Length > 255)
        {
            return false;
        }
        else if (owner.Any(c => !char.IsLetterOrDigit(c) && c != '-' && c != '_' && c != ' '))
        {
            return false;
        }
        else if (owner.StartsWith("-") || owner.EndsWith("-"))
        {
            return false;
        }
        else if (owner.StartsWith("_") || owner.EndsWith("_"))
        {
            return false;
        }
        else if (owner.Contains("--") || owner.Contains("__"))
        {
            return false;
        }
        else if (owner.Split('-').Any(label => label.Length == 0))
        {
            return false;
        }
        else if (owner.Split('_').Any(label => label.Length == 0))
        {
            return false;
        }
        else if (owner.Split(' ').Any(label => label.Length == 0))
        {
            return false;
        }
        else if (owner.Split('-').Any(label => label.All(c => char.IsDigit(c))))
        {
            return false;
        }
        else if (owner.Split('_').Any(label => label.All(c => char.IsDigit(c))))
        {
            return false;
        }
        else if (owner.Split(' ').Any(label => label.All(c => char.IsDigit(c))))
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}