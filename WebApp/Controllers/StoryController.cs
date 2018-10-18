using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FicRecs.DatabaseLib;
using FicRecs.WebApp.Models;

namespace WebApp.Controllers
{
    public class StoryController : Controller
    {
        private readonly FicRecsDbContext _context;

        public StoryController(FicRecsDbContext context)
        {
            _context = context;
        }

        // GET: StoryDetails
        public async Task<IActionResult> Index()
        {
            return View(await _context.StoryDetails.ToListAsync());
        }

        public async Task<IActionResult> Similar(int storyId)
        {
            var selectedFic = _context.StoryDetails.Where(d => d.StoryId == storyId);
            var similarFics = _context.StoryMatrix
                                .Where(m => m.StoryA == storyId)
                                .OrderByDescending(m => m.Similarity)
                                .Join(_context.StoryDetails, m => m.StoryB, d => d.StoryId, (m, d) => d);
            

            var model = new StorySimilarViewModel
            {
                SelectedFic = await selectedFic.SingleOrDefaultAsync(),
                SimilarFics = await similarFics.Take(20).ToListAsync(),
            };
            return View(model);
        }

        // GET: StoryDetails/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var storyDetails = await _context.StoryDetails
                .FirstOrDefaultAsync(m => m.StoryId == id);
            if (storyDetails == null)
            {
                return NotFound();
            }

            return View(storyDetails);
        }

        private bool StoryDetailsExists(int id)
        {
            return _context.StoryDetails.Any(e => e.StoryId == id);
        }
    }
}
