using System;
using System.Text;
using System.Security.Cryptography;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using API.Data;
using API.DTOs;
using Microsoft.EntityFrameworkCore;
using API.Interfaces;

namespace API.Controllers;

public class AccountController(DataContext context, ITokenService tokenService) : BaseApiController
{
  [HttpPost("register")] // api/account/register
  public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
  {
    if (await UserExists(registerDto.Username)) return BadRequest("Username is already taken");

    using var hmac = new HMACSHA512();
    var user = new AppUser
    {
      UserName = registerDto.Username.ToLower(),
      PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
      PasswordSalt = hmac.Key
    };

    context.Users.Add(user);
    await context.SaveChangesAsync();

    return new UserDto
    {
      Username = user.UserName,
      Token = tokenService.CreateToken(user)
    };
  }

  [HttpPost("login")]
  public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
  {
    var user = await context.Users.SingleOrDefaultAsync(x => x.UserName == loginDto.Username.ToLower());

    if (user == null) return Unauthorized("Invalid username");

    using var hmac = new HMACSHA512(user.PasswordSalt);

    var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

    for (int i = 0; i < computedHash.Length; i++)
    {
      if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid password");
    }

    return new UserDto
    {
      Username = user.UserName,
      Token = tokenService.CreateToken(user)
    };
  }

  private async Task<bool> UserExists(string username)
  {
    return await context.Users.AnyAsync(x => x.UserName.ToLower() == username.ToLower());
  }
}
