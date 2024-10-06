using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp1.Data;
using WebApp1.Models;

namespace WebApp1.Controllers
{
    public class VotingController : Controller
    {
        private readonly UserManager<MyUser> _userManager;
        private readonly ApplicationDbContext _context;

        List<UserRankings> list1 = new List<UserRankings>();

        public VotingController(UserManager<MyUser> userManager, ApplicationDbContext context,
                                RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _context = context;
        }


        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> UpdateVotes(string buttonUserId)
        {
            /* get user for the button */
            var user = _userManager.FindByIdAsync(buttonUserId).Result;
            if (user == null)
            {
                return NotFound();
            }
            user.noVotes += 1;
            await _userManager.UpdateAsync(user);

            /* update noVotes attribute for current user
            /* get logged in user */
            var currentuser = _userManager.GetUserAsync(User).Result;
            if (currentuser == null)
            {
                return NotFound();
            }
            currentuser.voted = true;
            await _userManager.UpdateAsync(currentuser);

            return RedirectToAction("Index","Home");
        }

        public async Task<IActionResult> VotingRounds()
        {
            VotingSystem votingInstance = new VotingSystem();

            if (!_context.VotingSystemTable.Any())
            {
                votingInstance.currentRound = 1;
                _context.VotingSystemTable.Add(votingInstance);
                _context.SaveChanges();
            }
            else
            {
                votingInstance = _context.VotingSystemTable.First();
            }

            /* Copy the users from the current voting round to the ranking page */
            var users = _userManager.Users.Where(u => u.isParticipating == true);
            foreach (var user in users)
            {
                UserRankings u1 = new UserRankings();
                u1.Id = Guid.NewGuid().ToString();
                u1.FirstName = user.FirstName;
                u1.LastName = user.LastName;
                u1.noVotes = user.noVotes;
                u1.round = votingInstance.currentRound;
                list1.Add(u1);

                user.noVotes = 0;
                await _userManager.UpdateAsync(user);
            }

            _context.UserRankingsTable.AddRange(list1);
            votingInstance.currentRound++;
            _context.VotingSystemTable.Update(votingInstance);
            _context.SaveChanges();

            return RedirectToAction("Index","Home");
        }
    }
}
