using Cial.Areas.Admin.ViewModels.OurTeamViewModels;
using Cial.Contexts;
using Cial.Helpers;
using Cial.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cial.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OurTeamsController : Controller
    {
        readonly CialDbContext _context;

        public OurTeamsController(CialDbContext context)
        {
            _context = context;
        }

        // Index
        public async Task<IActionResult> Index()
        {
            var teamsFromDb = await _context.OurTeams.Select(t => new OurTeamDetailsViewModel
            {
                Id  = t.Id,
                JobTitle = t.JobTitle,
                Image = t.Image,
                IsDeleted = t.IsDeleted,
            }).ToListAsync();

            return View(teamsFromDb);
        }

        // Create

        // Get
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // Post
        [HttpPost]
        public async Task<IActionResult> Create(OurTeamCreateViewModel vm)
        {
            if (!ModelState.IsValid) return BadRequest();

            if (!vm.ImageFile.IsCorrectType())
            {
                ModelState.AddModelError("ImageFile", "Image file type is incorrect.");
                return View(vm);
            }

            if (!vm.ImageFile.IsCorrectSize(300))
            {
                ModelState.AddModelError("ImageFile", "Image file size is incorrect. Must be less");
                return View(vm);
            }

            OurTeam newTeamMember = new()
            {
                JobTitle = vm.JobTitle,
                Image = vm.ImageFile.SaveImageAsync(PathConstants.ImageFilesLocation).Result ?? throw new Exception(),
            };

            await _context.AddAsync(newTeamMember);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // Edit

        // Get
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (!Common.IsValidId(id)) return BadRequest();

            var teamFromDb = await _context.OurTeams.FindAsync(id);

            if (teamFromDb == null) return NotFound();

            OurTeamEditViewModel vm = new()
            {
                JobTitle = teamFromDb.JobTitle
            };

            return View(vm);
        }

        // Post
        [HttpPost]
        public async Task<IActionResult> Edit(int id, OurTeamEditViewModel vm)
        {
            if (!Common.IsValidId(id)) return BadRequest();

            if (!ModelState.IsValid) return View(vm);

            var teamFromDb = await _context.OurTeams.FindAsync(id);

            teamFromDb.JobTitle = vm.JobTitle;
            teamFromDb.Image = vm.ImageFile.SaveImageAsync(PathConstants.ImageFilesLocation).Result ?? throw new Exception();

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // Delete

        public async Task<IActionResult> DeleteAsync(int id)
        {
            if (!Common.IsValidId(id)) return BadRequest();

            var teamFromDb = await _context.OurTeams.FindAsync(id);

            if (teamFromDb == null) return NotFound();

            _context.Remove(teamFromDb);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
