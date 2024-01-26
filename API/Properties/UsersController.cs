using System.Runtime.CompilerServices;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]// 'Annotation'- gives extra power to controller
[Route("api/[Controller]")] //framework cant direct http reques to the apporpriate controller & endpoint 
// local host/5001/controller/ users     
public class UsersController : ControllerBase
{
    private readonly DataContext _context;

    public UsersController(DataContext context)
    {
        _context = context;
    }
     [HttpGet] //first end point
    public async Task< ActionResult<IEnumerable<AppUser>>> GetUsers()
    {
        var users = await _context.Users.ToListAsync();

        return users;
    }

    [HttpGet("{id}")] // 2nd end point

    public async Task<ActionResult<AppUser>> GetUser(int id) //api/users/2
    {
 
        var user = await _context.Users.FindAsync(id);
        return user;
    }
}
