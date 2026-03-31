//i basically want to use this class for the Ghost host objects, implementing a SoftDelete rather than a permanent delete, and also adding some extra properties such as Environment, OperatingSystem, Owner, etc. to make it more useful for my use case. I also want to add a method to check the status of the host based on its last check-in time and its current status (online/offline).

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HostManagementAPI;

public class GhostHost
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(255)]
    public string Hostname { get; set; }

    [Required]
    [MaxLength(15)]
    public string IpAddress { get; set; }

    [Required]
    public int Port { get; set; }

    [MaxLength(255)]
    public string Environment { get; set; }

    [MaxLength(255)]
    public string OperatingSystem { get; set; }

    [MaxLength(255)]
    public string Owner { get; set; }

    [MaxLength(255)]
    public string Description { get; set; }

    [MaxLength(255)]
    public string Tags { get; set; }

    [MaxLength(255)]
    public string Notes { get; set; }

    public GhostHost() { }

    //i also want a mapper function to convert a Host object to a GhostHost object, and vice versa, to make it easier to work with both types of objects in the service layer.
    public static GhostHost FromHost(Host host)
    {
        return new GhostHost
        {
            Id = host.Id,
            Hostname = host.Hostname,
            IpAddress = host.IpAddress,
            Port = host.Port,
            Environment = host.Environment,
            OperatingSystem = host.OperatingSystem,
            Owner = host.Owner,
            Description = host.Description,
            Tags = host.Tags,
            Notes = host.Notes
        };
    }

    public Host ToHost()
    {
        return new Host
        {
            Id = this.Id,
            Hostname = this.Hostname,
            IpAddress = this.IpAddress,
            Port = this.Port,
            Environment = this.Environment,
            OperatingSystem = this.OperatingSystem,
            Owner = this.Owner,
            Description = this.Description,
            Tags = this.Tags,
            Notes = this.Notes
        };
    }

    public enum HostStatus
    {
        Online,
        Offline,
        Unknown
    }

}