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
    public class AccountsController : Controller
    {
        private readonly ReferralDBContext _context;

        public AccountsController(ReferralDBContext context)
        {
            _context = context;
        }

        // GET: Accounts
        public async Task<IActionResult> Index()
        {
              return _context.AccountSets != null ? 
                          View(await _context.AccountSets.ToListAsync()) :
                          Problem("Entity set 'ReferralDBContext.AccountSets'  is null.");
        }

        // GET: Accounts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.AccountSets == null)
            {
                return NotFound();
            }

            var account = await _context.AccountSets
                .FirstOrDefaultAsync(m => m.AccountId == id);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        // GET: Accounts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Accounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AccountId,UserName,Password,Type,Email,Phone")] Account account)
        {
            if (ModelState.IsValid)
            {
                account.SetupTime = DateTime.Now;
                _context.Add(account);
                await _context.SaveChangesAsync();
                return RedirectToAction("Success");
            }
            return View(account);
        }

        public async Task<IActionResult> Success()
        {
            return View();
        }

        // GET: Accounts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.AccountSets == null)
            {
                return NotFound();
            }

            var account = await _context.AccountSets.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }
            return View(account);
        }

        // POST: Accounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AccountId,UserName,SetupTime,Password,Type,Email,Phone")] Account account)
        {
            if (id != account.AccountId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(account);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccountExists(account.AccountId))
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
            return View(account);
        }

        // GET: Accounts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.AccountSets == null)
            {
                return NotFound();
            }

            var account = await _context.AccountSets
                .FirstOrDefaultAsync(m => m.AccountId == id);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        // POST: Accounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.AccountSets == null)
            {
                return Problem("Entity set 'ReferralDBContext.AccountSets'  is null.");
            }
            var account = await _context.AccountSets.FindAsync(id);
            if (account != null)
            {
                _context.AccountSets.Remove(account);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Fail()
        {
            Account account = new Account(_context);
            Dictionary<string, string> dataList = account.ExportToDictionary();

            return View();
        }

        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Login(Account model)
        {
            Account account = new Account(_context);
            Dictionary<string, string> dataList = account.ExportToDictionary();
            if (model.Password == dataList[model.UserName])
            {
                var loggedInAccount = _context.AccountSets.FirstOrDefault(a => a.UserName == model.UserName);
                if (loggedInAccount != null)
                {
                    int accountId = loggedInAccount.AccountId;
                    HttpContext.Session.SetInt32("_AccountID", accountId);
                    HttpContext.Session.SetInt32("_Login", 1);
                    HttpContext.Session.SetString("_Uname", loggedInAccount.UserName);

                    return RedirectToAction("Index", "Referrals"/*, new { id = accountId }*/);
                }
                return RedirectToAction("Fail");
            }
            else
            {
                return RedirectToAction("Fail");
            }
            //return RedirectToAction("Fail");
        }

        
        public IActionResult Logout()
        {
            
          HttpContext.Session.SetInt32("_AccountID", 0);
          HttpContext.Session.SetInt32("_Login", 0);
            //return RedirectToAction("Index", "Referrals");
            return View();
        }

        public async Task<IActionResult> AccountInfo()
        {
            var id = HttpContext.Session.GetInt32("_AccountID");

            return _context.ReferralSets != null ?
                          View(new { ReferralList = await _context.ReferralSets.ToListAsync(), Id = id }) :
                          Problem("Entity set 'ReferralDBContext.ReferralSets'  is null.");
        }
        private bool AccountExists(int id)
        {
          return (_context.AccountSets?.Any(e => e.AccountId == id)).GetValueOrDefault();
        }
    }
}
