using Microsoft.AspNetCore.Identity;

namespace WebApp1.Models
{
    public class MyUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? Genre { get; set; }
        public Boolean voted = false;
    }
}
