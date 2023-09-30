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
    public class AccountsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Accounts
        public async Task<IActionResult> Index()
        {
            var accounts = await _context.Account
                .FromSqlRaw("SELECT * FROM Ledger.Accounts")
                .ToListAsync();

            // For each account, load related entities separately
            foreach (var account in accounts)
            {
                account.Banker = await _context.Bankers.SingleOrDefaultAsync(b => b.Id == account.BankerId);
                account.Customer = await _context.Customers.SingleOrDefaultAsync(c => c.Id == account.CustomerId);
            }

            return View(accounts);
        }


        // GET: Accounts/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Account
                .FromSqlRaw("SELECT * FROM Ledger.Accounts WHERE Id = {0}", id.Value)
                .SingleOrDefaultAsync();

            if (account == null)
            {
                return NotFound();
            }

            // Load related entities separately
            account.Banker = await _context.Bankers.SingleOrDefaultAsync(b => b.Id == account.BankerId);
            account.Customer = await _context.Customers.SingleOrDefaultAsync(c => c.Id == account.CustomerId);

            return View(account);
        }


        // GET: Accounts/Create
        public IActionResult Create()
        {
            ViewData["BankerId"] = new SelectList(_context.Bankers, "Id", "Id");
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Id");
            return View();
        }

        // POST: Accounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CreatedDate,Balance,OverdraftLimit,Type,CustomerId,BankerId")] Account account)
        {
            if (ModelState.IsValid)
            {
                account.Id = Guid.NewGuid();
                await _context.Database.ExecuteSqlRawAsync(
                    "INSERT INTO Ledger.Accounts (Id, CreatedDate, Balance, OverdraftLimit, Type, CustomerId, BankerId) VALUES ({0}, {1}, {2}, {3}, {4}, {5}, {6})",
                    account.Id, account.CreatedDate, account.Balance, account.OverdraftLimit, account.Type, account.CustomerId, account.BankerId
                );
                return RedirectToAction(nameof(Index));
            }
            ViewData["BankerId"] = new SelectList(_context.Bankers, "Id", "Id", account.BankerId);
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Id", account.CustomerId);
            return View(account);
        }


        // GET: Accounts/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Account
                .FromSqlRaw("SELECT * FROM Ledger.Accounts WHERE Id = {0}", id.Value)
                .SingleOrDefaultAsync();

            if (account == null)
            {
                return NotFound();
            }

            ViewData["BankerId"] = new SelectList(_context.Bankers, "Id", "Id", account.BankerId);
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Id", account.CustomerId);
            return View(account);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,CreatedDate,Balance,OverdraftLimit,Type,CustomerId,BankerId")] Account account)
        {
            if (id != account.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _context.Database.ExecuteSqlRawAsync(
                        "UPDATE Ledger.Accounts SET CreatedDate = {0}, Balance = {1}, OverdraftLimit = {2}, Type = {3}, CustomerId = {4}, BankerId = {5} WHERE Id = {6}",
                        account.CreatedDate, account.Balance, account.OverdraftLimit, account.Type, account.CustomerId, account.BankerId, account.Id
                    );
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccountExists(account.Id))
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
            ViewData["BankerId"] = new SelectList(_context.Bankers, "Id", "Id", account.BankerId);
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Id", account.CustomerId);
            return View(account);
        }


        // GET: Accounts/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Account
                .FromSqlRaw("SELECT * FROM Ledger.Accounts WHERE Id = {0}", id.Value)
                .SingleOrDefaultAsync();

            if (account == null)
            {
                return NotFound();
            }

            // Load related entities separately
            account.Banker = await _context.Bankers.SingleOrDefaultAsync(b => b.Id == account.BankerId);
            account.Customer = await _context.Customers.SingleOrDefaultAsync(c => c.Id == account.CustomerId);

            return View(account);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _context.Database.ExecuteSqlRawAsync("DELETE FROM Ledger.Accounts WHERE Id = {0}", id);
            return RedirectToAction(nameof(Index));
        }


        private bool AccountExists(Guid id)
        {
          return (_context.Account?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
