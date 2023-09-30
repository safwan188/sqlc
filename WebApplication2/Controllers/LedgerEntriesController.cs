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
    public class LedgerEntriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LedgerEntriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: LedgerEntries
        public async Task<IActionResult> Index()
        {
            var ledgerEntries = await _context.LedgerEntry
                .FromSqlRaw("SELECT * FROM Ledger.Ledger")
                .ToListAsync();

            // For each ledger entry, load related entities separately
            foreach (var ledgerEntry in ledgerEntries)
            {
                ledgerEntry.Account = await _context.Account.SingleOrDefaultAsync(a => a.Id == ledgerEntry.AccountId);
            }

            return View(ledgerEntries);
        }


        // GET: LedgerEntries/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ledgerEntry = await _context.LedgerEntry
                .FromSqlRaw("SELECT * FROM Ledger.Ledger WHERE Id = {0}", id.Value)
                .SingleOrDefaultAsync();

            if (ledgerEntry == null)
            {
                return NotFound();
            }

            // Load related entity separately
            ledgerEntry.Account = await _context.Account.SingleOrDefaultAsync(a => a.Id == ledgerEntry.AccountId);

            return View(ledgerEntry);
        }


        // GET: LedgerEntries/Create
        public IActionResult Create()
        {
            ViewData["AccountId"] = new SelectList(_context.Account, "Id", "Id");
            return View();
        }

        // POST: LedgerEntries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Timestamp,Amount,Type,AccountId")] LedgerEntry ledgerEntry)
        {
            if (ModelState.IsValid)
            {
                ledgerEntry.Id = Guid.NewGuid();
                await _context.Database.ExecuteSqlRawAsync(
                    "INSERT INTO Ledger.Ledger (Id, Timestamp, Amount, Type, AccountId) VALUES ({0}, {1}, {2}, {3}, {4})",
                    ledgerEntry.Id, ledgerEntry.Timestamp, ledgerEntry.Amount, ledgerEntry.Type, ledgerEntry.AccountId
                );
                return RedirectToAction(nameof(Index));
            }
            ViewData["AccountId"] = new SelectList(_context.Account, "Id", "Id", ledgerEntry.AccountId);
            return View(ledgerEntry);
        }


        // GET: LedgerEntries/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ledgerEntry = await _context.LedgerEntry
                .FromSqlRaw("SELECT * FROM Ledger.Ledger WHERE Id = {0}", id.Value)
                .SingleOrDefaultAsync();

            if (ledgerEntry == null)
            {
                return NotFound();
            }

            ViewData["AccountId"] = new SelectList(_context.Account, "Id", "Id", ledgerEntry.AccountId);
            return View(ledgerEntry);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Timestamp,Amount,Type,AccountId")] LedgerEntry ledgerEntry)
        {
            if (id != ledgerEntry.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _context.Database.ExecuteSqlRawAsync(
                        "UPDATE Ledger.Ledger SET Timestamp = {0}, Amount = {1}, Type = {2}, AccountId = {3} WHERE Id = {4}",
                        ledgerEntry.Timestamp, ledgerEntry.Amount, ledgerEntry.Type, ledgerEntry.AccountId, ledgerEntry.Id
                    );
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LedgerEntryExists(ledgerEntry.Id))
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
            ViewData["AccountId"] = new SelectList(_context.Account, "Id", "Id", ledgerEntry.AccountId);
            return View(ledgerEntry);
        }

        // GET: LedgerEntries/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ledgerEntry = await _context.LedgerEntry
                .FromSqlRaw("SELECT * FROM Ledger.Ledger WHERE Id = {0}", id.Value)
                .SingleOrDefaultAsync();

            if (ledgerEntry == null)
            {
                return NotFound();
            }

            // Load related entity separately
            ledgerEntry.Account = await _context.Account.SingleOrDefaultAsync(a => a.Id == ledgerEntry.AccountId);

            return View(ledgerEntry);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _context.Database.ExecuteSqlRawAsync("DELETE FROM Ledger.Ledger WHERE Id = {0}", id);
            return RedirectToAction(nameof(Index));
        }

        private bool LedgerEntryExists(Guid id)
        {
          return (_context.LedgerEntry?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
