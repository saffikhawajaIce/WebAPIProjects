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
    private readonly ILogger<HostService> _logger;

    public HostService(ApplicationDbContext context, IHostValidationService validationService, ILogger<HostService> logger)
    {
        _context = context;
        _validationService = validationService;
        _logger = logger;
    }

    public async Task<HostDto> GetHostByIdAsync(int id)
    {
        try
        {
            var host = await _context.Hosts
                .AsNoTracking()
                .FirstOrDefaultAsync(h => h.Id == id);

            if (host == null)
            {
                _logger.LogError($"Host with id {id} was not found.");
                throw new KeyNotFoundException($"Host with id {id} was not found.");
            }

            return MapToDto(host);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while trying to get host by id.");
            throw;
        }
    }

    public async Task<IEnumerable<HostDto>> GetAllHostsAsync()
    {
        try
        {
            var hosts = await _context.Hosts
                .AsNoTracking()
                .ToListAsync();

            return hosts.ConvertAll(MapToDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while trying to get all hosts.");
            throw;
        }
    }

    public async Task<HostDto> CreateHostAsync(CreateHostDto request)
    {
        try
        {
            //i want to check if the request is valid before trying to create a new host, and if not,
            //  i want to throw an exception with a meaningful message that can be returned to the client.
            ValidateRequest(request.Name, request.IpAddress, request.Port);

            //i also then want to validate weather an existing host with the same ip address and port already exists, and if so, i want to throw an exception with a meaningful message that can be returned to the client.
            var existingHost = await _context.Hosts
                .AsNoTracking()
                .FirstOrDefaultAsync(h => h.IpAddress == request.IpAddress && h.Port == request.Port);

            if (existingHost != null)
            {
                _logger.LogError($"Host with ip address {request.IpAddress} and port {request.Port} already exists.");
                throw new ArgumentException($"Host with ip address {request.IpAddress} and port {request.Port} already exists.");
            }

            var now = DateTime.UtcNow;
            var host = new Host
            {
                //i want to set the hostname to the name provided in the request, and if the name is null or empty,
                // i want to set it to the ip address and port of the host.
                Hostname = string.IsNullOrEmpty(request.Name) ? $"{request.IpAddress}:{request.Port}" : request.Name,
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

            //i want to add the new host to the database, and then return the created host as a dto.
            _context.Hosts.Add(host);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Host with id {host.Id} has been created.");

            //i want to return the created host as a dto.
            return MapToDto(host);
        }
        catch (ArgumentException ex)
        {
            // i want to log the error with the exception message, and then rethrow the exception so that it can be caught by the controller and returned to the client.
            _logger.LogError(ex, "An error occurred while trying to create host.");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while trying to create host.");
            throw;
        }
    }

    public async Task<HostDto> UpdateHostAsync(int id, UpdateHostDto request)
    {
        try
        {
            var host = await _context.Hosts.FirstOrDefaultAsync(h => h.Id == id);

            //i want to check if the request is valid before trying to update the host, and if not, i want to throw an exception with a meaningful message that can be returned to the client.
            ValidateRequest(request.Name, request.IpAddress, request.Port);

            if (host == null)
            {
                _logger.LogError($"Host with id {id} was not found.");
                throw new KeyNotFoundException($"Host with id {id} was not found.");
            }

            //i also then want to validate weather an existing host with the same ip address and port already exists,
            //  and if so, i want to throw an exception with a meaningful message that can be returned to the client.

            else if (!string.IsNullOrEmpty(request.IpAddress) && request.Port.HasValue)
            {
                var existingHost = await _context.Hosts
                    .AsNoTracking()
                    .FirstOrDefaultAsync(h => h.Id != id && h.IpAddress == request.IpAddress && h.Port == request.Port);

                if (existingHost != null)
                {
                    _logger.LogError($"Host with ip address {request.IpAddress} and port {request.Port} already exists.");
                    throw new ArgumentException($"Host with ip address {request.IpAddress} and port {request.Port} already exists.");
                }
            }

            host.Hostname = request.Name;
            host.IpAddress = request.IpAddress;
            host.Port = request.Port;
            host.IsActive = request.IsActive;
            host.Status = request.IsActive ? "online" : "offline";
            host.UpdatedAt = DateTime.UtcNow;
            host.LastSeenAt = DateTime.UtcNow;

            //i want to update the host in the database, and then return the updated host as a dto.
            await _context.SaveChangesAsync();

            //i want to log the update with the host id, and then return the updated host as a dto.
            _logger.LogInformation($"Host with id {id} has been updated.");

            return MapToDto(host);
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, "An error occurred while trying to update host.");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while trying to update host.");
            throw;
        }
    }

    public async Task<bool> DeleteHostAsync(int id)
    {
        try
        {
            var host = await _context.Hosts.FirstOrDefaultAsync(h => h.Id == id);

            if (host == null)
            {
                _logger.LogError($"Host with id {id} was not found.");
                throw new KeyNotFoundException($"Host with id {id} was not found.");
            }

            host.IsActive = false;
            host.Status = "offline";

            await _context.SaveChangesAsync();

            _logger.LogInformation($"Host with id {id} has been marked as inactive.");

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while trying to delete host.");
            throw;
        }
    }

    public async Task<bool> IsHostActiveAsync(int id)
    {
        try
        {
            var host = await _context.Hosts.FirstOrDefaultAsync(h => h.Id == id);

            if (host == null)
            {
                _logger.LogError($"Host with id {id} was not found.");
                throw new KeyNotFoundException($"Host with id {id} was not found.");
            }

            return host.IsActive;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while trying to check if host is active.");
            throw;
        }
    }

    private void ValidateRequest(string name, string ipAddress, int port)
    {
        if (!_validationService.IsValidHostName(name))
        {
            throw new ArgumentException("Invalid host name.");
        }
        else if (!_validationService.IsValidIpAddress(ipAddress))
        {
            throw new ArgumentException("Invalid IP address.");
        }
        else if (!_validationService.IsValidPort(port))
        {
            throw new ArgumentException("Invalid port number.");
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

