using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FicRecs.DatabaseLib;

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
            var similarFics = _context.StoryMatrix
                                .Where(m => m.StoryA == storyId)
                                .OrderByDescending(m => m.Similarity)
                                .Join(_context.StoryDetails, m => m.StoryB, s => s.StoryId, (m, s) => s);
            var page = similarFics.Take(10);    
            return View(await page.ToListAsync());
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
