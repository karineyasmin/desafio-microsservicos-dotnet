using AuthAndIdentity.DTOS;
using AuthAndIdentity.Interfaces;
using AuthAndIdentity.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Annotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PetsRegistration.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly string _issuer;
    private readonly string _audience;
    private readonly string _secretKey;

    public AuthController(IUserService userService, IConfiguration configuration)
    {
        _userService = userService;
        _issuer = configuration["JwtSettings:Issuer"];
        _audience = configuration["JwtSettings:Audience"];
        _secretKey = configuration["JwtSettings:SecretKey"];
    }

    [HttpPost("register")]
    [SwaggerOperation(Summary = "Registers a new user", Description = "Registers a new user with the specified username, password, and role.")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(string), 200)]
    [ProducesResponseType(typeof(string), 400)]
    public async Task<IActionResult> Register([FromBody] RegisterModel register)
    {
        var existingUser = await _userService.AuthenticateAsync(register.Username, register.Password);
        if (existingUser != null)
        {
            return BadRequest("User already exists.");
        }

        if (register.Role.ToLower() != "admin" && register.Role.ToLower() != "user")
        {
            return BadRequest("Invalid role. Role must be either 'admin' or 'user'.");
        }

        await _userService.RegisterAsync(register.Username, register.Password, register.Role);
        return Ok("User registered successfully.");
    }

    [HttpPost("login")]
    [SwaggerOperation(Summary = "Authenticates a user", Description = "Authenticates a user with the specified username and password, and generates a JWT token.")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(object), 200)]
    [ProducesResponseType(typeof(string), 401)]
    public async Task<IActionResult> Login([FromBody] LoginModel login)
    {
        var user = await _userService.AuthenticateAsync(login.Username, login.Password);
        if (user == null)
        {
            return Unauthorized("Invalid username or password.");
        }

        var token = GenerateJwtToken(user.Username, user.Role);
        return Ok(new { token });
    }

    private string GenerateJwtToken(string username, string role)
{
    var claims = new[]
    {
        new Claim(JwtRegisteredClaimNames.Sub, username),
        new Claim(ClaimTypes.Role, role)
    };

    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    var token = new JwtSecurityToken(
        issuer: _issuer,
        audience: _audience,
        claims: claims,
        expires: DateTime.Now.AddMinutes(30),
        signingCredentials: creds);

    return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
