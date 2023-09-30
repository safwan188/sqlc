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
    public class LoansController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LoansController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Loans
        public async Task<IActionResult> Index()
        {
            var loans = await _context.Loan
                .FromSqlRaw("SELECT * FROM Ledger.Loans")
                .ToListAsync();

            // For each loan, load related entities separately
            foreach (var loan in loans)
            {
                loan.Account = await _context.Account.SingleOrDefaultAsync(a => a.Id == loan.AccountId);
            }

            return View(loans);
        }


        // GET: Loans/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loan = await _context.Loan
                .FromSqlRaw("SELECT * FROM Ledger.Loans WHERE Id = {0}", id.Value)
                .SingleOrDefaultAsync();

            if (loan == null)
            {
                return NotFound();
            }

            // Load related entity separately
            loan.Account = await _context.Account.SingleOrDefaultAsync(a => a.Id == loan.AccountId);

            return View(loan);
        }


        // GET: Loans/Create
        public IActionResult Create()
        {
            ViewData["AccountId"] = new SelectList(_context.Account, "Id", "Id");
            return View();
        }

        // POST: Loans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ApprovedDate,Amount,Term,AccountId")] Loan loan)
        {
            if (ModelState.IsValid)
            {
                loan.Id = Guid.NewGuid();
                await _context.Database.ExecuteSqlRawAsync(
                    "INSERT INTO Ledger.Loans (Id, ApprovedDate, Amount, Term, AccountId) VALUES ({0}, {1}, {2}, {3}, {4})",
                    loan.Id, loan.ApprovedDate, loan.Amount, loan.Term, loan.AccountId
                );
                return RedirectToAction(nameof(Index));
            }
            ViewData["AccountId"] = new SelectList(_context.Account, "Id", "Id", loan.AccountId);
            return View(loan);
        }


        // GET: Loans/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loan = await _context.Loan
                .FromSqlRaw("SELECT * FROM Ledger.Loans WHERE Id = {0}", id.Value)
                .SingleOrDefaultAsync();

            if (loan == null)
            {
                return NotFound();
            }

            ViewData["AccountId"] = new SelectList(_context.Account, "Id", "Id", loan.AccountId);
            return View(loan);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,ApprovedDate,Amount,Term,AccountId")] Loan loan)
        {
            if (id != loan.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _context.Database.ExecuteSqlRawAsync(
                        "UPDATE Ledger.Loans SET ApprovedDate = {0}, Amount = {1}, Term = {2}, AccountId = {3} WHERE Id = {4}",
                        loan.ApprovedDate, loan.Amount, loan.Term, loan.AccountId, loan.Id
                    );
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LoanExists(loan.Id))
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
            ViewData["AccountId"] = new SelectList(_context.Account, "Id", "Id", loan.AccountId);
            return View(loan);
        }


        // GET: Loans/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loan = await _context.Loan
                .FromSqlRaw("SELECT * FROM Ledger.Loans WHERE Id = {0}", id.Value)
                .SingleOrDefaultAsync();

            if (loan == null)
            {
                return NotFound();
            }

            // Load related entity separately
            loan.Account = await _context.Account.SingleOrDefaultAsync(a => a.Id == loan.AccountId);

            return View(loan);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _context.Database.ExecuteSqlRawAsync("DELETE FROM Ledger.Loans WHERE Id = {0}", id);
            return RedirectToAction(nameof(Index));
        }

        private bool LoanExists(Guid id)
        {
          return (_context.Loan?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
