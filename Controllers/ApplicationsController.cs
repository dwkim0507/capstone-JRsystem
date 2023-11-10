using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JRSystem.Models;
using Microsoft.AspNetCore.Routing;

namespace JRSystem.Controllers
{
    public class ApplicationsController : Controller
    {
        private readonly ReferralDBContext _context;

        public ApplicationsController(ReferralDBContext context)
        {
            _context = context;
        }

        // GET: Applications
        public async Task<IActionResult> Index()
        {
              return _context.ApplicationSets != null ? 
                          View(await _context.ApplicationSets.ToListAsync()) :
                          Problem("Entity set 'ReferralDBContext.ApplicationSets'  is null.");
        }

        // GET: Applications/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ApplicationSets == null)
            {
                return NotFound();
            }

            var application = await _context.ApplicationSets
                .FirstOrDefaultAsync(m => m.ApplicationId == id);
            if (application == null)
            {
                return NotFound();
            }

            return View(application);
        }

        // GET: Applications/Create
        public IActionResult Create(string id)
        {
            int flag = HttpContext.Session.GetInt32("_Login") ?? 0;
            if (flag != 1)
            {
                return RedirectToAction("pleaseLogin","Referrals");
            }
            else
            {
                return View();
            }
        }

        // POST: Applications/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        
        public async Task<IActionResult> Create(string id, [Bind("ApplicationId,Summary")] Application application)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    application.ReferralId = id;
                    application.ApplierId = HttpContext.Session.GetInt32("_AccountID") ?? 0;
                    application.FileId = $"{application.ReferralId}.{application.ApplierId}";
                    TempData["ReferralId"] = application.ReferralId;
                    TempData["FileId"] = application.FileId;

                    _context.Add(application);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("index", "file", new { FileId = application.FileId });
                }
                return View(application);
            }
            catch (Exception ex)
            {
                // 输出日志，查看是否有异常信息
                Console.WriteLine(ex.Message);
                throw;
            }

        }

       

        // GET: Applications/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ApplicationSets == null)
            {
                return NotFound();
            }

            var application = await _context.ApplicationSets.FindAsync(id);
            if (application == null)
            {
                return NotFound();
            }
            return View(application);
        }

        // POST: Applications/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ApplicationId,Summary,ApplierId,ReferralId")] Application application)
        {
            if (id != application.ApplicationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(application);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApplicationExists(application.ApplicationId))
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
            return View(application);
        }

        // GET: Applications/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ApplicationSets == null)
            {
                return NotFound();
            }

            var application = await _context.ApplicationSets
                .FirstOrDefaultAsync(m => m.ApplicationId == id);
            if (application == null)
            {
                return NotFound();
            }

            return View(application);
        }

        // POST: Applications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ApplicationSets == null)
            {
                return Problem("Entity set 'ReferralDBContext.ApplicationSets'  is null.");
            }
            var application = await _context.ApplicationSets.FindAsync(id);
            if (application != null)
            {
                _context.ApplicationSets.Remove(application);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> ViewApplications(string id)
        {
            var applications = await _context.ApplicationSets
        .Where(a => a.ReferralId == id)
        .ToListAsync();

            return View(applications);
        }
        private bool ApplicationExists(int id)
        {
          return (_context.ApplicationSets?.Any(e => e.ApplicationId == id)).GetValueOrDefault();
        }
    }
}
