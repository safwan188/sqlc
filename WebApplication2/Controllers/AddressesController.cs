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
    public class AddressesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AddressesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Addresses
        public async Task<IActionResult> Index()
        {
            var addresses = await _context.Addresses.FromSqlRaw("SELECT * FROM Core.Address").ToListAsync();
            return View(addresses);
        }


        // GET: Addresses/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null) return NotFound();

            var address = await _context.Addresses
                .FromSqlRaw("SELECT * FROM Core.Address WHERE Id = {0}", id)
                .FirstOrDefaultAsync();

            if (address == null) return NotFound();

            return View(address);
        }


        // GET: Addresses/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Addresses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,State,City,Street,PostalCode")] Address address)
        {
            if (ModelState.IsValid)
            {
                address.Id = Guid.NewGuid();
                await _context.Database.ExecuteSqlRawAsync(
                    "INSERT INTO Core.Address (Id, State, City, Street, PostalCode) VALUES ({0}, {1}, {2}, {3}, {4})",
                    address.Id, address.State, address.City, address.Street, address.PostalCode);
                return RedirectToAction(nameof(Index));
            }
            return View(address);
        }


        // GET: Addresses/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Addresses == null)
            {
                return NotFound();
            }

            var address = await _context.Addresses.FindAsync(id);
            if (address == null)
            {
                return NotFound();
            }
            return View(address);
        }

        // POST: Addresses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,State,City,Street,PostalCode")] Address address)
        {
            if (id != address.Id) return NotFound();

            if (ModelState.IsValid)
            {
                await _context.Database.ExecuteSqlRawAsync(
                    "UPDATE Core.Address SET State = {0}, City = {1}, Street = {2}, PostalCode = {3} WHERE Id = {4}",
                    address.State, address.City, address.Street, address.PostalCode, address.Id);
                return RedirectToAction(nameof(Index));
            }
            return View(address);
        }


        // GET: Addresses/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Addresses == null)
            {
                return NotFound();
            }

            var address = await _context.Addresses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (address == null)
            {
                return NotFound();
            }

            return View(address);
        }

        // POST: Addresses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _context.Database.ExecuteSqlRawAsync("DELETE FROM Core.Address WHERE Id = {0}", id);
            return RedirectToAction(nameof(Index));
        }


        private bool AddressExists(Guid id)
        {
          return (_context.Addresses?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
