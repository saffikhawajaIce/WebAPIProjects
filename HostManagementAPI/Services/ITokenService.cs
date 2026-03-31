//for JWT token generation and validation
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace HostManagementAPI;

public interface ITokenService
{
    // The ITokenService interface defines a contract for generating JWT tokens for authenticated users.
    //  It includes a single method, GenerateToken, which takes an AppUser object as input and returns a string representing the generated JWT token.
    string GenerateToken(AppUser user);
}

public class TokenService : ITokenService
{
    private readonly IConfiguration _config;

    public TokenService(IConfiguration config)
    {
        _config = config;
    }

    public string GenerateToken(AppUser user)
    {
        // Generate JWT token for the authenticated user.
        // The token includes claims such as the user's name, ID, and email, and is signed using a symmetric security key defined in the application's configuration.


        //i also need to add issuer and audience claims to the token, so that it can be validated by the authentication middleware.
        var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Iss, _config["Jwt:Issuer"]),
                new Claim(JwtRegisteredClaimNames.Aud, _config["Jwt:Audience"])
            };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        // The token is set to expire in 7 days, and the generated token is returned as a string to the caller.
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddDays(7),
            SigningCredentials = creds
        };

        // Create a new instance of JwtSecurityTokenHandler and use it to create a token based on the token descriptor.
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}
