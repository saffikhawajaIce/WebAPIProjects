namespace LibraryManagementSystem;

using Microsoft.AspNetCore.Identity;

// This class represents the application user and extends the IdentityUser class provided by ASP.NET Core Identity.
public class AppUser : IdentityUser
{
    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    // You can add additional properties as needed, such as FirstName, LastName, etc.

    public AppUser() : base()
    {
    }
}