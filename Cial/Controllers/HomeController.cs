using Cial.Contexts;
using Cial.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cial.Controllers
{
    public class HomeController : Controller
    {
        readonly CialDbContext _context;

        public HomeController(CialDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<OurTeam> teamsFromDb = await _context.OurTeams.ToListAsync();

            return View(teamsFromDb);
        }
    }
}