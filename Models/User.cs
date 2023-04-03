using Microsoft.AspNetCore.Identity;

namespace Store444.Models;

public class User : IdentityUser
{
    public string FirstName { get; set; }
    public string SurName { get; set; }
}
