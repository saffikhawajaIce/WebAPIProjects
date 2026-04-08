using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HostManagementAPI;

//this class is used to create the repository for the host, 
//it is used to store the hosts in memory and perform CRUD operations on them. 
//It implements the IHostRepositoryService interface, which defines the methods for the repository
// he HostRepositoryService class uses a List<Host> to store the hosts and a private field _nextId to generate unique IDs for new hosts.
// The methods in the HostRepositoryService class perform the necessary operations on the list of hosts and return the appropriate results.


//i basically want the ihostservice to use this repository service to perform the operations on the hosts, 
//and the ihostservice will be responsible for validating the input and calling the appropriate methods in the repository service.
// The ihostservice will also be responsible for logging the operations using the logger service.

// Repository should instead have things like:
// GetByIdAsync(id)
// GetAllAsync()
// GetByIpAndPortAsync(ip, port)
// AddAsync(host)
// UpdateAsync(host)

public interface IHostRepositoryService
{
    Task<IEnumerable<Host>> GetAllHostsAsync();
    Task<Host> GetHostByIdAsync(int id);
    Task<Host> GetHostByIpAndPortAsync(string ip, int port);
    Task<Host> AddHostAsync(Host host);
    Task<Host> UpdateHostAsync(Host host);
    Task DeleteHostAsync(int id);
    Task<Host> ExistsByIpAndPortAsync(string ip, int port);
}

public class HostRepositoryService : IHostRepositoryService
{
    private readonly List<Host> _hosts = new List<Host>();
    private int _nextId = 1;

    public Task<IEnumerable<Host>> GetAllHostsAsync()
    {
        return Task.FromResult(_hosts.Where(h => !h.IsDeleted).AsEnumerable());
    }

    public Task<Host> GetHostByIdAsync(int id)
    {
        var host = _hosts.FirstOrDefault(h => h.Id == id && !h.IsDeleted);
        if (host == null)
        {
            throw new KeyNotFoundException($"Host with ID {id} not found.");
        }
        return Task.FromResult(host);
    }

    public Task<Host> GetHostByIpAndPortAsync(string ip, int port)
    {
        var host = _hosts.FirstOrDefault(h => h.IpAddress == ip && h.Port == port && !h.IsDeleted);
        if (host == null)
        {
            throw new KeyNotFoundException($"Host with IP {ip} and Port {port} not found.");
        }
        return Task.FromResult(host);
    }

    public Task<Host> AddHostAsync(Host host)
    {
        host.Id = _nextId++;
        _hosts.Add(host);
        return Task.FromResult(host);
    }

    public Task<Host> UpdateHostAsync(Host host)
    {
        var existingHost = _hosts.FirstOrDefault(h => h.Id == host.Id && !h.IsDeleted);
        if (existingHost == null)
        {
            throw new KeyNotFoundException($"Host with ID {host.Id} not found.");
        }
        existingHost.Hostname = host.Hostname;
        existingHost.IpAddress = host.IpAddress;
        existingHost.Port = host.Port;
        existingHost.Owner = host.Owner;
        return Task.FromResult(existingHost);
    }

    //i want to refactor this to instead implement a soft delete instead of a hard delete, so instead of removing the host from the list, i want to set a property called IsDeleted to true, and then filter out the deleted hosts in the GetAllHostsAsync method.
    public Task DeleteHostAsync(int id)
    {
        var host = _hosts.FirstOrDefault(h => h.Id == id && !h.IsDeleted);
        if (host == null)
        {
            throw new KeyNotFoundException($"Host with ID {id} not found.");
        }
        host.IsDeleted = true;
        return Task.CompletedTask;
    }

    public Task<Host> ExistsByIpAndPortAsync(string ip, int port)
    {
        var host = _hosts.FirstOrDefault(h => h.IpAddress == ip && h.Port == port && !h.IsDeleted);
        return Task.FromResult(host);
    }
}