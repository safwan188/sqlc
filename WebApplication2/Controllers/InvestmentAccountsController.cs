using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication2;
using WebApplication2.Data;

namespace WebApplication2.Controllers
{
    public class InvestmentAccountsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InvestmentAccountsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: InvestmentAccounts
        public async Task<IActionResult> Index()
        {
            var investmentAccounts = await _context.InvestmentAccount
                .FromSqlRaw("SELECT * FROM Investments.InvestmentAccounts")
                .ToListAsync();

            foreach (var investmentAccount in investmentAccounts)
            {
                investmentAccount.Customer = await _context.Customers.SingleOrDefaultAsync(c => c.Id == investmentAccount.CustomerId);
            }

            return View(investmentAccounts);
        }


        // GET: InvestmentAccounts/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.InvestmentAccount == null)
            {
                return NotFound();
            }

            var investmentAccount = await _context.InvestmentAccount
                .Include(i => i.Customer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (investmentAccount == null)
            {
                return NotFound();
            }

            return View(investmentAccount);
        }

        // GET: InvestmentAccounts/Create
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Id");
            return View();
        }

        // POST: InvestmentAccounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CreatedDate,Balance,Type,CustomerId")] InvestmentAccount investmentAccount)
        {
            if (ModelState.IsValid)
            {
                investmentAccount.Id = Guid.NewGuid();
                await _context.Database.ExecuteSqlInterpolatedAsync($"INSERT INTO Investments.InvestmentAccounts (Id, CreatedDate, Balance, Type, CustomerId) VALUES ({investmentAccount.Id}, {investmentAccount.CreatedDate}, {investmentAccount.Balance}, {investmentAccount.Type}, {investmentAccount.CustomerId})");
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Id", investmentAccount.CustomerId);
            return View(investmentAccount);
        }


        // GET: InvestmentAccounts/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.InvestmentAccount == null)
            {
                return NotFound();
            }

            var investmentAccount = await _context.InvestmentAccount.FindAsync(id);
            if (investmentAccount == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Id", investmentAccount.CustomerId);
            return View(investmentAccount);
        }

        // POST: InvestmentAccounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,CreatedDate,Balance,Type,CustomerId")] InvestmentAccount investmentAccount)
        {
            if (id != investmentAccount.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _context.Database.ExecuteSqlInterpolatedAsync($"UPDATE Investments.InvestmentAccounts SET CreatedDate = {investmentAccount.CreatedDate}, Balance = {investmentAccount.Balance}, Type = {investmentAccount.Type}, CustomerId = {investmentAccount.CustomerId} WHERE Id = {id}");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InvestmentAccountExists(investmentAccount.Id))
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
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Id", investmentAccount.CustomerId);
            return View(investmentAccount);
        }


        // GET: InvestmentAccounts/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.InvestmentAccount == null)
            {
                return NotFound();
            }

            var investmentAccount = await _context.InvestmentAccount
                .Include(i => i.Customer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (investmentAccount == null)
            {
                return NotFound();
            }

            return View(investmentAccount);
        }

        // POST: InvestmentAccounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _context.Database.ExecuteSqlInterpolatedAsync($"DELETE FROM Investments.InvestmentAccounts WHERE Id = {id}");
            return RedirectToAction(nameof(Index));
        }


        private bool InvestmentAccountExists(Guid id)
        {
          return (_context.InvestmentAccount?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
