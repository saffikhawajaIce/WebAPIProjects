namespace LibraryManagementSystem;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using System.Linq;
using System.Security.Claims;
using System.Text;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{

    // The IConfiguration interface is used to access configuration settings in the application, such as connection strings, JWT settings, and other application-specific configurations.
    //  It allows you to retrieve values from configuration files (like appsettings.json) or environment variables.
    private readonly IConfiguration _configuration;

    // In a real application, you would typically inject a user service or repository to manage user data and authentication logic.
    // UserManager is a service provided by ASP.NET Core Identity that allows you to manage user accounts, including creating users, validating credentials, and managing user roles and claims.
    private readonly UserManager<AppUser> _userManager;

    // SignInManager is used to handle user sign-in and authentication processes. It provides methods for signing in users, checking passwords, and managing user sessions.
    private readonly SignInManager<AppUser> _signInManager;

    // The constructor of the AuthController class takes three parameters:
    //  IConfiguration, UserManager<AppUser>, and SignInManager<AppUser>.
    //  These dependencies are injected into the controller, allowing it to access configuration settings and manage user authentication.
    public AuthController(IConfiguration configuration, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
    {
        _configuration = configuration;
        _userManager = userManager;
        _signInManager = signInManager;
    }


    // The Register method is an HTTP POST endpoint that allows users to register for an account. 
    // It takes a RegisterRequestDTO object as input, which contains the user's registration details (such as name, email, and password).
    //  The method creates a new AppUser object based on the provided registration details and uses the UserManager to create a new user account.
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequestDTO dto)
    {
        var user = new AppUser
        {
            FullName = dto.Name,
            Email = dto.Email,
            UserName = dto.Email
        };

        var result = await _userManager.CreateAsync(user, dto.Password);

        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description);
            return BadRequest(errors);
        }

        return Ok(new { message = "User registered successfully" });
    }
}
