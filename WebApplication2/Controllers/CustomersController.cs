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
    public class CustomersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CustomersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Customers
        public async Task<IActionResult> Index()
        {
            var customers = await _context.Customers.FromSqlRaw("SELECT * FROM Core.Customers").ToListAsync();
            return View(customers);
        }


        // GET: Customers/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
                return NotFound();

            var customer = await _context.Customers.FromSqlRaw("SELECT * FROM Core.Customers WHERE Id = {0}", id).FirstOrDefaultAsync();
            if (customer == null)
                return NotFound();

            return View(customer);
        }


        // GET: Customers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstName,LastName,SalaryAmount")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                customer.Id = Guid.NewGuid();
                await _context.Database.ExecuteSqlRawAsync("INSERT INTO Core.Customers (Id, FirstName, LastName, SalaryAmount) VALUES ({0}, {1}, {2}, {3})",
                    customer.Id, customer.FirstName, customer.LastName, customer.SalaryAmount);
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }


        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Customers == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,FirstName,LastName,SalaryAmount")] Customer customer)
        {
            if (id != customer.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                await _context.Database.ExecuteSqlRawAsync("UPDATE Core.Customers SET FirstName = {0}, LastName = {1}, SalaryAmount = {2} WHERE Id = {3}",
                    customer.FirstName, customer.LastName, customer.SalaryAmount, customer.Id);
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }


        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Customers == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _context.Database.ExecuteSqlRawAsync("DELETE FROM Core.Customers WHERE Id = {0}", id);
            return RedirectToAction(nameof(Index));
        }


        private bool CustomerExists(Guid id)
        {
          return (_context.Customers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
