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
    public class InvestmentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InvestmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Investments
        public async Task<IActionResult> Index()
        {
            var investments = await _context.Investment
                .FromSqlRaw("SELECT * FROM Investments.Investments")
                .ToListAsync();

            foreach (var investment in investments)
            {
                investment.InvestmentAccount = await _context.Set<InvestmentAccount>().FromSqlRaw("SELECT * FROM Investments.InvestmentAccounts WHERE Id = {0}", investment.AccountId).SingleOrDefaultAsync();
                investment.Manager = await _context.Manager.FromSqlRaw("SELECT * FROM Investments.Managers WHERE Id = {0}", investment.ManagerId).SingleOrDefaultAsync();
            }
            return View(investments);
        }


        // GET: Investments/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var investment = await _context.Investment
                .FromSqlRaw("SELECT * FROM Investments.Investments WHERE Id = {0}", id.Value)
                .SingleOrDefaultAsync();

            if (investment == null)
            {
                return NotFound();
            }

            investment.InvestmentAccount = await _context.Set<InvestmentAccount>().FromSqlRaw("SELECT * FROM Investments.InvestmentAccounts WHERE Id = {0}", investment.AccountId).SingleOrDefaultAsync();
            investment.Manager = await _context.Manager.FromSqlRaw("SELECT * FROM Investments.Managers WHERE Id = {0}", investment.ManagerId).SingleOrDefaultAsync();

            return View(investment);
        }


        // GET: Investments/Create
        public IActionResult Create()
        {
            ViewData["AccountId"] = new SelectList(_context.Set<InvestmentAccount>(), "Id", "Id");
            ViewData["ManagerId"] = new SelectList(_context.Manager, "Id", "Id");
            return View();
        }

        // POST: Investments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("InvestmentType,Amount,InvestmentDate,AccountId,ManagerId")] Investment investment)
        {
            if (ModelState.IsValid)
            {
                investment.Id = Guid.NewGuid();
                await _context.Database.ExecuteSqlInterpolatedAsync($"INSERT INTO Investments.Investments (Id, InvestmentType, Amount, InvestmentDate, AccountId, ManagerId) VALUES ({investment.Id}, {investment.InvestmentType}, {investment.Amount}, {investment.InvestmentDate}, {investment.AccountId}, {investment.ManagerId})");
                return RedirectToAction(nameof(Index));
            }
            ViewData["AccountId"] = new SelectList(_context.Set<InvestmentAccount>(), "Id", "Id", investment.AccountId);
            ViewData["ManagerId"] = new SelectList(_context.Manager, "Id", "Id", investment.ManagerId);
            return View(investment);
        }


        // GET: Investments/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Investment == null)
            {
                return NotFound();
            }

            var investment = await _context.Investment.FindAsync(id);
            if (investment == null)
            {
                return NotFound();
            }
            ViewData["AccountId"] = new SelectList(_context.Set<InvestmentAccount>(), "Id", "Id", investment.AccountId);
            ViewData["ManagerId"] = new SelectList(_context.Manager, "Id", "Id", investment.ManagerId);
            return View(investment);
        }

        // POST: Investments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,InvestmentType,Amount,InvestmentDate,AccountId,ManagerId")] Investment investment)
        {
            if (id != investment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _context.Database.ExecuteSqlInterpolatedAsync($"UPDATE Investments.Investments SET InvestmentType = {investment.InvestmentType}, Amount = {investment.Amount}, InvestmentDate = {investment.InvestmentDate}, AccountId = {investment.AccountId}, ManagerId = {investment.ManagerId} WHERE Id = {id}");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InvestmentExists(investment.Id))
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
            ViewData["AccountId"] = new SelectList(_context.Set<InvestmentAccount>(), "Id", "Id", investment.AccountId);
            ViewData["ManagerId"] = new SelectList(_context.Manager, "Id", "Id", investment.ManagerId);
            return View(investment);
        }


        // GET: Investments/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Investment == null)
            {
                return NotFound();
            }

            var investment = await _context.Investment
                .Include(i => i.InvestmentAccount)
                .Include(i => i.Manager)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (investment == null)
            {
                return NotFound();
            }

            return View(investment);
        }

        // POST: Investments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _context.Database.ExecuteSqlInterpolatedAsync($"DELETE FROM Investments.Investments WHERE Id = {id}");
            return RedirectToAction(nameof(Index));
        }


        private bool InvestmentExists(Guid id)
        {
          return (_context.Investment?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
