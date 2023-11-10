using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JRSystem.Models;
using static System.Net.Mime.MediaTypeNames;

namespace JRSystem.Controllers
{
    public class ReferralsController : Controller
    {
        private readonly ReferralDBContext _context;

        public ReferralsController(ReferralDBContext context)
        {
            _context = context;
        }

        // GET: Referrals
        public async Task<IActionResult> Index()
        {
            var id = HttpContext.Session.GetInt32("_AccountID");
           
            return _context.ReferralSets != null ?
                          View(new { ReferralList = await _context.ReferralSets.ToListAsync(), Id = id }):
                          Problem("Entity set 'ReferralDBContext.ReferralSets'  is null.");
        }

        // GET: Referrals/Details/5
        public async Task<IActionResult> Details(int id)
        {

            if (id == null || _context.ReferralSets == null)
            {
                return NotFound();
            }

            var referral = await _context.ReferralSets
                .FirstOrDefaultAsync(m => m.ReferralId == id);
            if (referral == null)
            {
                return NotFound();
            }

            return View(referral);
        }

        // GET: Referrals/Create

        public IActionResult pleaseLogin()
        {
            return View();
        }
        public IActionResult Create()
        {
            int flag = HttpContext.Session.GetInt32("_Login") ?? 0;
            if (flag!= 1)
            {
                return RedirectToAction("pleaseLogin");
            }
            else
            {
                return View();
            }
            
        }

        // POST: Referrals/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ReferralId,ReferralName,AccountID,ReferralDate,deadline,JobTitle,Num_seats")] Referral referral)
        {

            if (ModelState.IsValid)
            {
                referral.AccountID = HttpContext.Session.GetInt32("_AccountID") ?? 0;
                referral.JobId = $"{referral.ReferralId}.{referral.JobTitle}";
                TempData["JobId"] = referral.JobId;
                _context.Add(referral);
                await _context.SaveChangesAsync();
                return RedirectToAction("Create","Jobs");
            }
            return View(referral);
        }

        // GET: Referrals/Edit/5
        public async Task<IActionResult> Edit(string id)
        {

            if (id == null || _context.ReferralSets == null)
            {
                return NotFound();
            }

            var referral = await _context.ReferralSets.FindAsync(id);
            if (referral == null)
            {
                return NotFound();
            }
            return View(referral);
        }

        // POST: Referrals/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ReferralId,ReferralName,AccountID,ReferralDate,deadline,JobTitle")] Referral referral)
        {

            if (id != referral.ReferralId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(referral);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReferralExists(referral.ReferralId))
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
            return View(referral);
        }

        // GET: Referrals/Delete/5
        public async Task<IActionResult> Delete(int id)
        {

            if (id == null || _context.ReferralSets == null)
            {
                return NotFound();
            }

            var referral = await _context.ReferralSets
                .FirstOrDefaultAsync(m => m.ReferralId == id);
            if (referral == null)
            {
                return NotFound();
            }

            return View(referral);
        }

        // POST: Referrals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {

            if (_context.ReferralSets == null)
            {
                return Problem("Entity set 'ReferralDBContext.ReferralSets'  is null.");
            }
            var referral = await _context.ReferralSets.FindAsync(id);
            if (referral != null)
            {
                _context.ReferralSets.Remove(referral);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReferralExists(int id)
        {

            return (_context.ReferralSets?.Any(e => e.ReferralId == id)).GetValueOrDefault();
        }
    }
}
