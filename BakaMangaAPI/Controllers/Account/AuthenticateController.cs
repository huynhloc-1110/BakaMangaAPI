using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using BakaMangaAPI.DTOs;
using BakaMangaAPI.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace BakaMangaAPI.Controllers.Account;

[Route("account")]
[ApiController]
public class AuthenticateController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;

    public AuthenticateController(UserManager<ApplicationUser> userManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _configuration = configuration;
    }

    private string CreateToken(ApplicationUser user, IEnumerable<string> userRoles, DateTime expiredDate)
    {
        var authClaims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        foreach (var userRole in userRoles)
        {
            authClaims.Add(new Claim(ClaimTypes.Role, userRole));
        }

        var authenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

        var token = new JwtSecurityToken(
            issuer: _configuration["JWT:ValidIssuer"],
            audience: _configuration["JWT:ValidAudience"],
            expires: expiredDate,
            claims: authClaims,
            signingCredentials: new SigningCredentials(authenKey, SecurityAlgorithms.HmacSha512)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    [HttpPost("signup")]
    public async Task<IActionResult> SignUpAsync(SignUpDTO dto)
    {
        var user = new ApplicationUser
        {
            Name = dto.Name,
            Email = dto.Email,
            UserName = dto.Email,
        };
        var result = await _userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
        {
            return BadRequest(result.Errors.First().Description);
        }
        await _userManager.AddToRoleAsync(user, "User");

        var userRoles = await _userManager.GetRolesAsync(user);
        var expiredDate = DateTime.UtcNow.AddDays(1);
        var tokenString = CreateToken(user, userRoles, expiredDate);

        return Ok(new
        {
            token = tokenString,
            expiration = expiredDate
        });
    }

    [HttpPost("signin")]
    public async Task<IActionResult> SignInAsync(SignInDTO dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user != null && await _userManager.CheckPasswordAsync(user, dto.Password))
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            var expiredDate = DateTime.UtcNow.AddDays(1);
            var tokenString = CreateToken(user, userRoles, expiredDate);

            return Ok(new
            {
                token = tokenString,
                expiration = expiredDate
            });
        }
        return Unauthorized();
    }

    [Authorize]
    [HttpPost("extend")]
    public async Task<IActionResult> ExtendAsync()
    {
        var currentUser = await _userManager.GetUserAsync(User);
        var userRoles = await _userManager.GetRolesAsync(currentUser);
        var expiredDate = DateTime.UtcNow.AddDays(1);
        var tokenString = CreateToken(currentUser, userRoles, expiredDate);

        return Ok(new
        {
            token = tokenString,
            expiration = expiredDate
        });
    }
}
