using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Project.Context;
using Project.DTOs;
using Project.Helpers;
using Project.Models;

namespace Project.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SecurityController : ControllerBase
{
    private readonly MyDbContext _context;
    private readonly IConfiguration _configuration;

    public SecurityController(MyDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }
    
    
    [AllowAnonymous]
    [HttpPost("register")]
    public IActionResult Register(LoginRegisterRequest registerRequest)
    {
        var hashedPasswordAndSalt = SecurityHelper.GetHashedPasswordAndSalt(registerRequest.Password);

        var user = new AppUser
        {
            Username = registerRequest.Login,
            Password = hashedPasswordAndSalt.Item1,
            Role = "Employee",
            Salt = hashedPasswordAndSalt.Item2,
            RefreshToken = SecurityHelper.GenerateRefreshToken(),
            RefreshTokenExp = DateTime.Now.AddDays(1)
        };

        _context.AppUsers.Add(user);
        _context.SaveChanges();

        return Ok();
    }
    
    [AllowAnonymous]
    [HttpPost("login")]
    public IActionResult Login(LoginRegisterRequest loginRequest)
    {
        AppUser user = _context.AppUsers.Where(u => u.Username == loginRequest.Login).FirstOrDefault();

        string passwordHashFromDb = user.Password;
        string curHashedPassword = SecurityHelper.GetHashedPasswordWithSalt(loginRequest.Password, user.Salt);

        if (passwordHashFromDb != curHashedPassword)
        {
            return Unauthorized();
        }
        
        var claims = new[]
        {
            new Claim("role", user.Role)
        };

        SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));

        SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        JwtSecurityToken token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            expires: DateTime.Now.AddMinutes(10),
            claims: claims,
            signingCredentials: creds
        );

        user.RefreshToken = SecurityHelper.GenerateRefreshToken();
        user.RefreshTokenExp = DateTime.Now.AddDays(1);
        _context.SaveChanges();

        return Ok(new
        {
            accessToken = new JwtSecurityTokenHandler().WriteToken(token),
            refreshToken = user.RefreshToken
        });
    }
    
    [Authorize(AuthenticationSchemes = "IgnoreTokenExpirationScheme")]
    [HttpPost("refresh")]
    public IActionResult Refresh(RefreshTokenRequest refreshToken)
    {
        AppUser user = _context.AppUsers.Where(u => u.RefreshToken == refreshToken.RefreshToken).FirstOrDefault();
        if (user == null)
        {
            throw new SecurityTokenException("Invalid refresh token");
        }

        if (user.RefreshTokenExp < DateTime.Now)
        {
            throw new SecurityTokenException("Refresh token expired");
        }
        
        var claims = new[]
        {
            new Claim("role", user.Role)
        };

        SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));

        SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        JwtSecurityToken jwtToken = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(10),
            signingCredentials: creds
        );

        user.RefreshToken = SecurityHelper.GenerateRefreshToken();
        user.RefreshTokenExp = DateTime.Now.AddDays(1);
        _context.SaveChanges();

        return Ok(new
        {
            accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken),
            refreshToken = user.RefreshToken
        });
    }
}