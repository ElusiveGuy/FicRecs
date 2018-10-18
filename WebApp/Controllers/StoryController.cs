using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FicRecs.DatabaseLib;
using FicRecs.WebApp.Models;
using System.Threading;

namespace WebApp.Controllers
{
    public class StoryController : Controller
    {
        const int pageSize = 20;
        ThreadLocal<Random> random;

        private readonly FicRecsDbContext _context;

        public StoryController(FicRecsDbContext context)
        {
            Random rand = new Random();
            random = new ThreadLocal<Random>(() =>
            {
                lock(rand)
                {
                    return new Random(rand.Next());
                }
            });

            _context = context;
        }

        // GET: StoryDetails
        public async Task<IActionResult> Index(bool showDetailed = false, int page = 1)
        {
            var fics = _context.StoryDetails;

            var model = new StoryIndexViewModel
            {
                ShowDetailed = showDetailed,
                Fics = await fics.Skip((page - 1) * pageSize).Take(page * pageSize).ToListAsync(),
                CurrentPage = page,
                TotalPages = await fics.CountAsync() / pageSize
            };
            return View(model);
        }

        public async Task<IActionResult> Similar(int storyId = -1, bool showDetailed = false, int page = 1)
        {
            var selectedFic = _context.StoryDetails.Where(d => d.StoryId == storyId);
            var similarFics = _context.StoryMatrix
                                .Where(m => m.StoryA == storyId)
                                .OrderByDescending(m => m.Similarity)
                                .Join(_context.StoryDetails, m => m.StoryB, d => d.StoryId, (m, d) => d);

            var model = new StorySimilarViewModel
            {
                StoryId = storyId,
                ShowDetailed = showDetailed,
                SelectedFic = await selectedFic.SingleOrDefaultAsync(),
                SimilarFics = await similarFics.Skip((page - 1) * pageSize).Take(page * pageSize).ToListAsync(),
                CurrentPage = page,
                TotalPages = await similarFics.CountAsync() / pageSize
            };
            return View(model);
        }

        public async Task<IActionResult> Random()
        {
            var storyId = _context.StoryDetails
                                .Skip(random.Value.Next(await _context.StoryDetails.CountAsync()))
                                .Select(s => s.StoryId)
                                .Single();

            return RedirectToAction("Similar", new { storyId = storyId });
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
