using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Security.Claims;
using WebApp1.Models;

namespace WebApp1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<MyUser> _userManager;
        
        public HomeController(ILogger<HomeController> logger, UserManager<MyUser> userManager) //user manager este injectat prin controller
        {
            _logger = logger;
            _userManager = userManager;
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

        [Authorize]
        public IActionResult Index()
        {
            var users = _userManager.Users;
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

        public async Task<IActionResult> UpdateVotes(string test)
        {
            /* get user for the button */
            //var user =  _userManager.FindByIdAsync(buttonUser.Id).Result;
            /* update noVotes attribute for current user

            /* get logged in user */
            //var user = _userManager.GetUserAsync(User).Result;
            //if (user == null)
            //    return NotFound();
            //user.voted = true;
            //await _userManager.UpdateAsync(user);

            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
