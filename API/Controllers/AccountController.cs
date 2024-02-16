using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;
using System.Text;
using API.Controllers;
using API.Data;
using API.DTOs;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API;

public class AccountController : BaseApiController
{
    private readonly DataContext _context;
    private readonly ITokenService _tokenService;

    // add new constructor and inject the datacontext
    // after adding interface, implementation and userDto inject IToken service
    public AccountController(DataContext context, ITokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }

    
    // add an endpoint for users to register themselves
    [HttpPost("register")] // Post : api/account/register

    // write a method to register new user
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
    {
        // call userExists method to chech if the user name is already exist
        if( await UserExists(registerDto.UserName)) 
        {
            return BadRequest("This user names is already exist.");
        }
        
        using var hmac = new HMACSHA512(); // hmacsha.. its an inbuilt class/method which creates a random
                                           // key that will use to store pwsalt
        //set up an object of AppUser tbl
        var user = new AppUser
        {
            UserName = registerDto.UserName.ToLower(),
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
            PasswordSalt = hmac.Key
        };

        // add new user details to DB
        _context.Users.Add(user);

        await _context.SaveChangesAsync();

        return new UserDto
        {
            UserName = user.UserName,
            Token = _tokenService.CreateToken(user)
        };

    }


    //Method to check whether the user name exist or not
    private async Task<bool> UserExists(string username)
    {
        return await _context.Users.AnyAsync(x => x.UserName == username.ToLower());
    }


    //add another endpoint for login page
    [HttpPost("login")]

    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == loginDto.UserName.ToLower());

        if (user == null)
        {
            return Unauthorized("Invalid username");
        }
        // set up an object of existing user's password salt key
        using var hmac = new HMACSHA512(user.PasswordSalt);
        
        // compute password key which entered by user
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

        for(int i = 0; i< computedHash.Length; i++)
        {
            if (computedHash[i] != user.PasswordHash[i])
            {
                return Unauthorized("Invalid Password");
            }
        }


        return new UserDto
        {
            UserName = user.UserName,
            Token = _tokenService.CreateToken(user)
        };
    }
}
