using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace HostManagementAPI;

public interface IHostService
{
    Task<HostDto> GetHostByIdAsync(int id);
    Task<IEnumerable<HostDto>> GetAllHostsAsync();
    Task<HostDto> CreateHostAsync(CreateHostDto request);
    Task<HostDto> UpdateHostAsync(int id, UpdateHostDto request);
    Task<bool> DeleteHostAsync(int id);
    Task<bool> IsHostActiveAsync(int id);
}

public class HostService : IHostService
{
    private readonly ApplicationDbContext _context;
    private readonly IHostValidationService _validationService;

    public HostService(ApplicationDbContext context, IHostValidationService validationService)
    {
        _context = context;
        _validationService = validationService;
    }

    public async Task<HostDto> GetHostByIdAsync(int id)
    {
        var host = await _context.Hosts
            .AsNoTracking()
            .FirstOrDefaultAsync(h => h.Id == id);

        if (host == null)
        {
            throw new KeyNotFoundException($"Host with id {id} was not found.");
        }

        return MapToDto(host);
    }

    public async Task<IEnumerable<HostDto>> GetAllHostsAsync()
    {
        var hosts = await _context.Hosts
            .AsNoTracking()
            .ToListAsync();

        return hosts.ConvertAll(MapToDto);
    }

    public async Task<HostDto> CreateHostAsync(CreateHostDto request)
    {
        ValidateRequest(request.Name, request.IpAddress, request.Port);

        var now = DateTime.UtcNow;
        var host = new Host
        {
            Hostname = request.Name,
            IpAddress = request.IpAddress,
            Port = request.Port,
            IsActive = true,
            Status = "online",
            CreatedAt = now,
            UpdatedAt = now,
            LastSeenAt = now,
            Environment = string.Empty,
            OperatingSystem = string.Empty,
            Owner = string.Empty,
            Description = string.Empty,
            Tags = string.Empty,
            Notes = string.Empty
        };

        _context.Hosts.Add(host);
        await _context.SaveChangesAsync();

        return MapToDto(host);
    }

    public async Task<HostDto> UpdateHostAsync(int id, UpdateHostDto request)
    {
        ValidateRequest(request.Name, request.IpAddress, request.Port);

        var host = await _context.Hosts.FirstOrDefaultAsync(h => h.Id == id);

        if (host == null)
        {
            throw new KeyNotFoundException($"Host with id {id} was not found.");
        }

        host.Hostname = request.Name;
        host.IpAddress = request.IpAddress;
        host.Port = request.Port;
        host.IsActive = request.IsActive;
        host.Status = request.IsActive ? "online" : "offline";
        host.UpdatedAt = DateTime.UtcNow;
        host.LastSeenAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return MapToDto(host);
    }

    public async Task<bool> DeleteHostAsync(int id)
    {
        var host = await _context.Hosts.FirstOrDefaultAsync(h => h.Id == id);

        if (host == null)
        {
            return false;
        }

        _context.Hosts.Remove(host);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> IsHostActiveAsync(int id)
    {
        var host = await _context.Hosts
            .AsNoTracking()
            .FirstOrDefaultAsync(h => h.Id == id);

        if (host == null)
        {
            throw new KeyNotFoundException($"Host with id {id} was not found.");
        }

        return host.IsActive;
    }

    private void ValidateRequest(string hostName, string ipAddress, int port)
    {
        if (!_validationService.IsValidHostName(hostName))
        {
            throw new ArgumentException("Host name is invalid.", nameof(hostName));
        }

        if (!_validationService.IsValidIpAddress(ipAddress))
        {
            throw new ArgumentException("IP address is invalid.", nameof(ipAddress));
        }

        if (!_validationService.IsValidPort(port))
        {
            throw new ArgumentException("Port is invalid.", nameof(port));
        }
    }

    private static HostDto MapToDto(Host host)
    {
        return new HostDto
        {
            Id = host.Id,
            Name = host.Hostname,
            IpAddress = host.IpAddress,
            Port = host.Port,
            IsActive = host.IsActive,
            CreatedAt = host.CreatedAt
        };
    }
}

