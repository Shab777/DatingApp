using System.ComponentModel.DataAnnotations;

namespace API;

public class RegisterDto
{
    //To check null/empty values- Required
    [Required]
    public string UserName { get; set; }
    [Required]
    public string Password { get; set; }

}
