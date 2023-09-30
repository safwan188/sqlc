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
    public class BankersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BankersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Bankers
        public async Task<IActionResult> Index()
        {
            var bankers = await _context.Bankers.FromSqlRaw("SELECT * FROM Core.Bankers").ToListAsync();

            // Manually loading related Branch entities.
            foreach (var banker in bankers)
            {
                // Explicitly loading the related Branch entity for each Banker.
                await _context.Entry(banker).Reference(b => b.Branch).LoadAsync();
            }

            return View(bankers);
        }


        // GET: Bankers/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Retrieve the Banker using a raw SQL query
            var banker = await _context.Bankers
                .FromSqlRaw("SELECT * FROM Core.Bankers WHERE Id = {0}", id)
                .FirstOrDefaultAsync();

            if (banker == null)
            {
                return NotFound();
            }

            // Manually load the related Branch for the retrieved Banker
            await _context.Entry(banker).Reference(b => b.Branch).LoadAsync();

            return View(banker);
        }


        // GET: Bankers/Create
        public IActionResult Create()
        {
            ViewData["BranchId"] = new SelectList(_context.Addresses, "Id", "Id");
            return View();
        }

        // POST: Bankers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstName,LastName,BranchId")] Banker banker)
        {
            // Remove Branch from ModelState
            ModelState.Remove("Branch");

            if (ModelState.IsValid)
            {
                banker.Id = Guid.NewGuid();
                await _context.Database.ExecuteSqlRawAsync(
                    "INSERT INTO Core.Bankers (Id, FirstName, LastName, BranchId) VALUES ({0}, {1}, {2}, {3})",
                    banker.Id, banker.FirstName, banker.LastName, banker.BranchId
                );
                return RedirectToAction(nameof(Index));
            }

            ViewData["BranchId"] = new SelectList(_context.Addresses, "Id", "Id", banker.BranchId);
            return View(banker);
        }



        // GET: Bankers/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Bankers == null)
            {
                return NotFound();
            }

            var banker = await _context.Bankers.FindAsync(id);
            if (banker == null)
            {
                return NotFound();
            }
            ViewData["BranchId"] = new SelectList(_context.Addresses, "Id", "Id", banker.BranchId);
            return View(banker);
        }

        // POST: Bankers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,FirstName,LastName,BranchId")] Banker banker)
        {
            if (id != banker.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _context.Database.ExecuteSqlRawAsync("UPDATE Core.Bankers SET FirstName = {1}, LastName = {2}, BranchId = {3} WHERE Id = {0}",
                        banker.Id, banker.FirstName, banker.LastName, banker.BranchId);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BankerExists(banker.Id))
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
            ViewData["BranchId"] = new SelectList(_context.Addresses, "Id", "Id", banker.BranchId);
            return View(banker);
        }


        // GET: Bankers/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Bankers == null)
            {
                return NotFound();
            }

            var banker = await _context.Bankers
                .Include(b => b.Branch)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (banker == null)
            {
                return NotFound();
            }

            return View(banker);
        }

        // POST: Bankers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _context.Database.ExecuteSqlRawAsync("DELETE FROM Core.Bankers WHERE Id = {0}", id);
            return RedirectToAction(nameof(Index));
        }


        private bool BankerExists(Guid id)
        {
          return (_context.Bankers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
