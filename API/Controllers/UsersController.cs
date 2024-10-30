using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using API.Entities;
using API.Data;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
  [ApiController]
  [Route("api/[controller]")] // api/users
  public class UsersController(DataContext context) : ControllerBase
  {
    [HttpGet] // api/users
    public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
    {
      var users = await context.Users.ToListAsync();

      return users;
    }

    [HttpGet("{id:int}")] // api/users/3
    public async Task<ActionResult<AppUser>> GetUser(int id)
    {
      var user = await context.Users.FindAsync(id);
      if (user == null)
      {
        return NotFound(new { Message = "User not found" });
      }
      return user;
    }
  }
}
