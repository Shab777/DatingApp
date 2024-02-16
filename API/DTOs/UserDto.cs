
namespace API.DTOs
{
    public class UserDto
    {
        // return a token when user logsin or registers
        public string UserName { get; set; }
        public string Token { get; set; }
        
    }
}