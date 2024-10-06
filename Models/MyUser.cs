using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace WebApp1.Models
{
    public class MyUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; } = DateTime.UtcNow;
        public string? Genre { get; set; } = "Choose genre";
        public Boolean isParticipating { get; set; } = false;
        public Boolean voted { get; set; } = false; 
        public int noVotes { get; set; } = 0;
    }
}
