using System;
using System.Text;
using AB.Ecommerce.Gros.Application;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace AB.Ecommerce.Gros;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly IClientAppService _clientAppService;
    private readonly IConfiguration _configuration;

    public AuthController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager,IClientAppService clientAppService, IConfiguration configuration)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
        _clientAppService = clientAppService;
    }

    

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByNameAsync(model.Username);

            if (user==null)
            {
                return Unauthorized(new { Message = "Nom d'utilisateur ou mot de passe incorrect." });
            }

            if (user.LockoutEnabled)
            {
                return Unauthorized(new { Message = "Utilisateur verrouillé." });
            }

            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, isPersistent: false, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    var accessToken = GenerateJwtToken(user);
                    var refreshToken = await GenerateRefreshTokenAsync(user);

                    var roles = await _userManager.GetRolesAsync(user);

                    // Include the expiration time of the access token in seconds
                    var expiresIn = (int)TimeSpan.FromHours(100).TotalSeconds; // Adjust as needed

                    return Ok(new
                    {
                        User = user,
                        Roles=roles,
                        AccessToken = accessToken,
                        RefreshToken = refreshToken,
                        ExpiresIn = expiresIn
                    });

                }

                return Unauthorized(new { Message = "Nom d'utilisateur ou mot de passe incorrect." });
            }

            return Unauthorized(new { Message = "Nom d'utilisateur ou mot de passe incorrect." });
        }

        return BadRequest(new { Message = "Requête invalide." });
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenModel model)
    {
        var user = await _userManager.FindByNameAsync(model.Username);

        if (user == null)
        {
            return Unauthorized(new { Message = "User not found" });
        }

        var isValidRefreshToken = await _userManager.VerifyUserTokenAsync(user, TokenOptions.DefaultProvider, "RefreshToken", model.RefreshToken);

        if (!isValidRefreshToken)
        {
            return Unauthorized(new { Message = "Invalid refresh token" });
        }

        var accessToken = GenerateJwtToken(user);
        var refreshToken = await GenerateRefreshTokenAsync(user);

        // Include the expiration time of the access token in seconds
        var expiresIn = (int)TimeSpan.FromHours(8).TotalSeconds; // Adjust as needed

        return Ok(new
        {
            User = user,
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiresIn = expiresIn
        });
    }


    private string GenerateJwtToken(IdentityUser user)
    {
        var key = Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]);
        var issuer = _configuration["Jwt:Issuer"];
        var audience = _configuration["Jwt:Audience"];

        var tokenHandler = new JwtSecurityTokenHandler();

        var claims = new List<Claim>
        {
         
            new Claim(JwtRegisteredClaimNames.Sub, user.Id), // Subject claim
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.GivenName, user.UserName), // Assuming you have a FirstName property
            new Claim(JwtRegisteredClaimNames.FamilyName, user.UserName), // Assuming you have a LastName property
            new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName), // Preferred username
            // Add other claims as needed
        };

        // Add roles to claims if using roles
        var roles = _userManager.GetRolesAsync(user).Result;
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(99),
            Audience = audience,
            Issuer  = issuer,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }


    private async Task<string> GenerateRefreshTokenAsync(IdentityUser user)
    {
        var refreshToken = await _userManager.GenerateUserTokenAsync(user, TokenOptions.DefaultProvider, "RefreshToken");

        return refreshToken;
    }




}

public class LoginModel
{
    public string Username { get; set; }
    public string Password { get; set; }
}



public class RefreshTokenModel
{
    public string Username { get; set; }
    public string RefreshToken { get; set; }
}
