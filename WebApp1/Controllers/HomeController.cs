using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SQLitePCL;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Security.Claims;
using WebApp1.Data;
using WebApp1.Models;

namespace WebApp1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<MyUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;

        public HomeController(ILogger<HomeController> logger, UserManager<MyUser> userManager, ApplicationDbContext context, RoleManager<IdentityRole> roleManager) //user manager este injectat prin controller
        {
            _logger = logger;
            _userManager = userManager;
            _context = context;
            _roleManager = roleManager;
        }

        public InputModel Input { get; set; }

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

        [AllowAnonymous]
        public IActionResult Index()
        {
            var user = _userManager.GetUserAsync(User).Result;

            if(user != null) {
                ViewBag.allowVote = true;

                Input = new InputModel
                {
                    voted = user.voted,
                };

                ViewBag.voted = user.voted;

                if (_userManager.IsInRoleAsync(user, "Admin").Result == true)
                {
                    ViewBag.permision = true;
                }
            }
            else
            {
                ViewBag.allowVote = false;
            }

            var users = _userManager.Users.Where(u => u.isParticipating == true).OrderByDescending(x => x.noVotes);
            return View(users);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Rankings()
        {

            return View(_context.UserRankingsTable.OrderByDescending(x => x.round).ThenByDescending(x => x.noVotes).ToList());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
