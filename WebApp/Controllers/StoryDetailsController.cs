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
    public class StoryDetailsController : Controller
    {
        private readonly FicRecsDbContext _context;

        public StoryDetailsController(FicRecsDbContext context)
        {
            _context = context;
        }

        // GET: StoryDetails
        public async Task<IActionResult> Index()
        {
            return View(await _context.StoryDetails.ToListAsync());
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

        // GET: StoryDetails/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: StoryDetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StoryId,Title,Author,Summary,Characters,Chapters,Words,Reviews,Favs,Follows,Published,Complete,Url")] StoryDetails storyDetails)
        {
            if (ModelState.IsValid)
            {
                //_context.Add(storyDetails);
                //await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(storyDetails);
        }

        // GET: StoryDetails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var storyDetails = await _context.StoryDetails.FindAsync(id);
            if (storyDetails == null)
            {
                return NotFound();
            }
            return View(storyDetails);
        }

        // POST: StoryDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StoryId,Title,Author,Summary,Characters,Chapters,Words,Reviews,Favs,Follows,Published,Complete,Url")] StoryDetails storyDetails)
        {
            if (id != storyDetails.StoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //_context.Update(storyDetails);
                    //await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StoryDetailsExists(storyDetails.StoryId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(storyDetails);
        }

        // GET: StoryDetails/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        // POST: StoryDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var storyDetails = await _context.StoryDetails.FindAsync(id);
            //_context.StoryDetails.Remove(storyDetails);
            //await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StoryDetailsExists(int id)
        {
            return _context.StoryDetails.Any(e => e.StoryId == id);
        }
    }
}
