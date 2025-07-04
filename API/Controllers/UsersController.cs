using System;
using Microsoft.AspNetCore.Mvc;
using API.Data;
using API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
namespace API.Controllers;

[Authorize]
public class UsersController(DataContext context) : BaseApiController
{
  [HttpGet]
  public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
  {
    var users = await context.Users.ToListAsync();
    return Ok(users);
  }


  [HttpGet("{id:int}")]
  public async Task<ActionResult<AppUser>> GetUser(int id)
  {
    var user = await context.Users.FindAsync(id);
    if (user == null)
    {
      return NotFound();
    }
    return Ok(user);
  }
}
