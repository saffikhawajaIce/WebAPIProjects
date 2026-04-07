using System.ComponentModel.DataAnnotations;

public class RegisterRequestDTO
{
    [Required]
    public string Name { get; set; }
    [Required]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
}

public class LoginRequestDTO
{
    [Required]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
}

public class UserDTO
{
    public string Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
}

public class HostDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string IpAddress { get; set; }
    public int Port { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateHostDto
{
    public string Name { get; set; }
    public string IpAddress { get; set; }
    public int Port { get; set; }
}

public class UpdateHostDto
{
    public string Name { get; set; }
    public string IpAddress { get; set; }
    public int Port { get; set; }
    public bool IsActive { get; set; }
}

public class ImportHostDto
{
    public string Name { get; set; }
    public string IpAddress { get; set; }
    public int Port { get; set; }
    public string Owner { get; set; }
}
