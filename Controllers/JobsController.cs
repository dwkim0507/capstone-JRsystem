using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JRSystem.Models;

namespace JRSystem.Controllers
{
    public class JobsController : Controller
    {
        private readonly ReferralDBContext _context;

        public JobsController(ReferralDBContext context)
        {
            _context = context;
        }

        // GET: Jobs
        public async Task<IActionResult> Index()
        {
              return _context.JobSets != null ? 
                          View(await _context.JobSets.ToListAsync()) :
                          Problem("Entity set 'ReferralDBContext.JobSets'  is null.");
        }

        // GET: Jobs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.JobSets == null)
            {
                return NotFound();
            }

            var job = await _context.JobSets
                .FirstOrDefaultAsync(m => m.Id == id);
            if (job == null)
            {
                return NotFound();
            }

            return View(job);
        }

        // GET: Jobs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Jobs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,JobId,Title,Company,Job_type,Job_description,Start_time")] Job job)
        {
            if (ModelState.IsValid)
            {
                job.JobId = TempData["JobId"] as string;
                _context.Add(job);
                await _context.SaveChangesAsync();
                return RedirectToAction("Success");
            }
            return View(job);
        }
        public IActionResult Success()
        {
            return View();
        }

        // GET: Jobs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.JobSets == null)
            {
                return NotFound();
            }

            var job = await _context.JobSets.FindAsync(id);
            if (job == null)
            {
                return NotFound();
            }
            return View(job);
        }

        // POST: Jobs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,JobId,Title,Company,Job_type,Job_description,Start_time")] Job job)
        {
            if (id != job.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(job);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JobExists(job.Id))
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
            return View(job);
        }

        // GET: Jobs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.JobSets == null)
            {
                return NotFound();
            }

            var job = await _context.JobSets
                .FirstOrDefaultAsync(m => m.Id == id);
            if (job == null)
            {
                return NotFound();
            }

            return View(job);
        }

        // POST: Jobs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.JobSets == null)
            {
                return Problem("Entity set 'ReferralDBContext.JobSets'  is null.");
            }
            var job = await _context.JobSets.FindAsync(id);
            if (job != null)
            {
                _context.JobSets.Remove(job);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool JobExists(int id)
        {
          return (_context.JobSets?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
