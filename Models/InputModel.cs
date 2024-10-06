using System.ComponentModel.DataAnnotations;

namespace WebApp1.Models
{
    public class InputModel
    {
        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [Phone]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Description { get; set; }
        public string Genre { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Boolean isParticipating { get; set; }
        public Boolean voted { get; set; }
        public int noVotes { get; set; }
    }
}
