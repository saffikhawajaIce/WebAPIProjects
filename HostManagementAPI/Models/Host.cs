public class Host
{
    public int Id { get; set; }
    public string Hostname { get; set; }
    public string IpAddress { get; set; }
    public int Port { get; set; }
    public string Environment { get; set; }
    public string OperatingSystem { get; set; }
    public string Owner { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string Status { get; set; }
    public string Description { get; set; }
    public string Tags { get; set; }
    public bool IsActive { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime LastSeenAt { get; set; }
    public string Notes { get; set; }
    public Host() { }

    public enum HostStatus
    {
        Online,
        Offline,
        Unknown
    }

    public HostStatus GetHostStatus()
    {
        if (Status.Equals("online", StringComparison.OrdinalIgnoreCase))
        {
            return HostStatus.Online;
        }
        else if (Status.Equals("offline", StringComparison.OrdinalIgnoreCase))
        {
            return HostStatus.Offline;
        }
        else
        {
            return HostStatus.Unknown;
        }
    }
}