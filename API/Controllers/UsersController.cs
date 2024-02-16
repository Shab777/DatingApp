using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;


// [ApiController]// 'Annotation'- gives extra power to controller
// [Route("api/[Controller]")] //framework cant direct http reques to the apporpriate controller & endpoint 
// // local host/5001/controller/ users     
// we have deleted the above tribute and route becoz we have an inherited controller which will save our 
// from writting the same code over and over again.

//public class UsersController : ControllerBase- change this to inherited controller name
[Authorize]
public class UsersController : BaseApiController
{
    private readonly DataContext _context;

    public UsersController(DataContext context)
    {
        _context = context;
    }
     
     [AllowAnonymous]
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
