namespace LibraryManagementSystem;

public class RegisterRequestDTO
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }

    public RegisterRequestDTO() { }
}

public class LoginRequestDTO
{
    public string Email { get; set; }
    public string Password { get; set; }

    public LoginRequestDTO() { }
}

public class AuthResponseDTO
{
    public string Token { get; set; }
    public string Email { get; set; }

    public AuthResponseDTO() { }
}