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

        List<UserRankings> list1 = new List<UserRankings>();

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

            var users = _userManager.Users.Where(u => u.isParticipating == true).OrderByDescending(x => x.noVotes);
            return View(users);
        }

        //public IActionResult UpdateVotes()
        //{
        //    /* get user for the button */

        //    /* get logged in user */
        //    var user = _userManager.GetUserAsync(User).Result;
        //    if (user == null)
        //        return NotFound();
        //    user.voted = true;
        //    await _userManager.UpdateAsync(user);

        //    return RedirectToAction("Index");
        //}

        public async Task<IActionResult> UpdateVotes(string buttonUserId)
        {
            /* get user for the button */
            var user =  _userManager.FindByIdAsync(buttonUserId).Result;
            if (user == null)
                return NotFound();
            user.noVotes += 1;
            await _userManager.UpdateAsync(user);

            /* update noVotes attribute for current user
            /* get logged in user */
            var currentuser = _userManager.GetUserAsync(User).Result;
            if (currentuser == null)
               return NotFound();
            currentuser.voted = true;
            await _userManager.UpdateAsync(currentuser);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> VotingRounds()
        {
            /* Copy the users from the current voting round to the ranking page */
            var users = _userManager.Users.Where(u => u.isParticipating == true);
            foreach (var user in users)
            {
                UserRankings u1 = new UserRankings();
                u1.Id = user.Id;
                u1.FirstName = user.FirstName;
                u1.LastName = user.LastName;
                u1.noVotes = user.noVotes;
                list1.Add(u1);

                user.noVotes = 0;
                await _userManager.UpdateAsync(user);
            }

            foreach(var entity in _context.UserRankingsTable)
            {
                _context.UserRankingsTable.Remove(entity);
            }

            _context.UserRankingsTable.AddRange(list1);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Rankings()
        {

            return View(_context.UserRankingsTable.OrderByDescending(x => x.noVotes).ToList());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
