using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Automarket1;

namespace Automarket1.Controllers
{
    public class CustomersController : Controller
    {
        private readonly laba1Context _context;

        public CustomersController(laba1Context context)
        {
            _context = context;
        }

        // GET: Customers
        public async Task<IActionResult> Index()
        {
            return View(await _context.Customers.ToListAsync());
        }

        // GET: Customers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            //return View(customer);
            return RedirectToAction("Index", "Sales", new { id = customer.Id, name = customer.FullName });
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
        public async Task<IActionResult> Create([Bind("Id,FullName,DateBirth,PassportNumber,PhoneNumber")] Customer customer)
        {
            if (IsUnique(customer.FullName, customer.DateBirth, customer.PassportNumber, customer.PhoneNumber) == 0)
            {
                if (ModelState.IsValid)
                {
                    _context.Add(customer);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            else if (IsUnique(customer.FullName, customer.DateBirth, customer.PassportNumber, customer.PhoneNumber) == 1)
            {
                ViewData["ErrorMessage_Passport"] = "Такий паспорт вже існує!";
                return View(customer);
            }
            else if (IsUnique(customer.FullName, customer.DateBirth, customer.PassportNumber, customer.PhoneNumber) == 2)
            {
                ViewData["ErrorMessage_Phone"] = "Такий номер телефону вже існує!";
                return View(customer);
            }
            else if (IsUnique(customer.FullName, customer.DateBirth, customer.PassportNumber, customer.PhoneNumber) == 3)
            {
                ViewData["ErrorMessage"] = "Така комбінація П.І.П. і номера паспорта вже існує!";
                return View(customer);
            }
            else if (IsUnique(customer.FullName, customer.DateBirth, customer.PassportNumber, customer.PhoneNumber) == 4)
            {
                ViewData["ErrorMessage"] = "Така комбінація П.І.П. і номера телефону вже існує!";
                return View(customer);
            }
            else if (IsUnique(customer.FullName, customer.DateBirth, customer.PassportNumber, customer.PhoneNumber) == 5)
            {
                ViewData["ErrorMessage"] = "Така комбінація дати народження й номера паспорта вже існує!";
                return View(customer);
            }
            else if (IsUnique(customer.FullName, customer.DateBirth, customer.PassportNumber, customer.PhoneNumber) == 6)
            {
                ViewData["ErrorMessage"] = "Така комбінація дати народження й номера телефону вже існує!";
                return View(customer);
            }
            else if (IsUnique(customer.FullName, customer.DateBirth, customer.PassportNumber, customer.PhoneNumber) == 7)
            {
                ViewData["ErrorMessage"] = "Така комбінація номера паспорта й номера телефону вже існує!";
                return View(customer);
            }
            else if (IsUnique(customer.FullName, customer.DateBirth, customer.PassportNumber, customer.PhoneNumber) == 8)
            {
                ViewData["ErrorMessage"] = "Така комбінація: П.І.П., номер паспорта, номер телефону - вже існує!";
                return View(customer);
            }
            else if (IsUnique(customer.FullName, customer.DateBirth, customer.PassportNumber, customer.PhoneNumber) == 9)
            {
                ViewData["ErrorMessage"] = "Такий покупець вже існує!";
                return View(customer);
            }
            return View(customer);
        }

        int IsUnique(string fullName, DateTime dateOfBirth, string passportNumber, string phoneNumber)
        {
            var i = (from customer in _context.Customers
                     where customer.FullName == fullName && customer.DateBirth == dateOfBirth && customer.PassportNumber == passportNumber && customer.PhoneNumber == phoneNumber
                     select customer).ToList();
            if(i.Count != 0) { return 9; }

            var h = (from customer in _context.Customers
                     where customer.FullName == fullName && customer.PassportNumber == passportNumber && customer.PhoneNumber == phoneNumber
                     select customer).ToList();
            if(h.Count != 0) { return 8; }

            var g = (from customer in _context.Customers
                     where customer.PassportNumber == passportNumber && customer.PhoneNumber == phoneNumber
                     select customer).ToList();
            if (g.Count != 0) { return 7; }

            var f = (from customer in _context.Customers
                     where customer.DateBirth == dateOfBirth && customer.PhoneNumber == phoneNumber
                     select customer).ToList();
            if (f.Count != 0) { return 6; }

            var e = (from customer in _context.Customers
                     where customer.DateBirth == dateOfBirth && customer.PassportNumber == passportNumber
                     select customer).ToList();
            if (e.Count != 0) { return 5; }

            var d = (from customer in _context.Customers
                     where customer.FullName == fullName && customer.PhoneNumber == phoneNumber
                     select customer).ToList();
            if (d.Count != 0) { return 4; }

            var c = (from customer in _context.Customers
                     where customer.FullName == fullName && customer.PassportNumber == passportNumber
                     select customer).ToList();
            if (c.Count != 0) { return 3; }

            var b = (from customer in _context.Customers
                     where customer.PhoneNumber == phoneNumber
                     select customer).ToList();
            if (b.Count != 0) { return 2; }


            var a = (from customer in _context.Customers
                     where customer.PassportNumber == passportNumber
                     select customer).ToList();
            if (a.Count != 0) { return 1; }

            return 0;
        }

        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,FullName,DateBirth,PassportNumber,PhoneNumber")] Customer customer)
        {
            if (id != customer.Id)
            {
                return NotFound();
            }
            if (IsUnique(customer.FullName, customer.DateBirth, customer.PassportNumber, customer.PhoneNumber) == 0)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(customer);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!CustomerExists(customer.Id))
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
            }
            else if (IsUnique(customer.FullName, customer.DateBirth, customer.PassportNumber, customer.PhoneNumber) == 1)
            {
                ViewData["ErrorMessage_Passport"] = "Такий паспорт вже існує!";
                return View(customer);
            }
            else if (IsUnique(customer.FullName, customer.DateBirth, customer.PassportNumber, customer.PhoneNumber) == 2)
            {
                ViewData["ErrorMessage_Phone"] = "Такий номер телефону вже існує!";
                return View(customer);
            }
            else if (IsUnique(customer.FullName, customer.DateBirth, customer.PassportNumber, customer.PhoneNumber) == 3)
            {
                ViewData["ErrorMessage"] = "Така комбінація П.І.П. і номера паспорта вже існує!";
                return View(customer);
            }
            else if (IsUnique(customer.FullName, customer.DateBirth, customer.PassportNumber, customer.PhoneNumber) == 4)
            {
                ViewData["ErrorMessage"] = "Така комбінація П.І.П. і номера телефону вже існує!";
                return View(customer);
            }
            else if (IsUnique(customer.FullName, customer.DateBirth, customer.PassportNumber, customer.PhoneNumber) == 5)
            {
                ViewData["ErrorMessage"] = "Така комбінація дати народження й номера паспорта вже існує!";
                return View(customer);
            }
            else if (IsUnique(customer.FullName, customer.DateBirth, customer.PassportNumber, customer.PhoneNumber) == 6)
            {
                ViewData["ErrorMessage"] = "Така комбінація дати народження й номера телефону вже існує!";
                return View(customer);
            }
            else if (IsUnique(customer.FullName, customer.DateBirth, customer.PassportNumber, customer.PhoneNumber) == 7)
            {
                ViewData["ErrorMessage"] = "Така комбінація номера паспорта й номера телефону вже існує!";
                return View(customer);
            }
            else if (IsUnique(customer.FullName, customer.DateBirth, customer.PassportNumber, customer.PhoneNumber) == 8)
            {
                ViewData["ErrorMessage"] = "Така комбінація: П.І.П., номер паспорта, номер телефону - вже існує!";
                return View(customer);
            }
            else if (IsUnique(customer.FullName, customer.DateBirth, customer.PassportNumber, customer.PhoneNumber) == 9)
            {
                ViewData["ErrorMessage"] = "Такий покупець вже існує!";
                return View(customer);
            }
            return View(customer);
        }

        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
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
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            int c_sales = _context.Sales.Where(s => s.CustomerId == id).Count();
            if (c_sales != 0)
            {
                ViewData["ErrorMessage"] = "Видалення неможливе, бо в продажах є цей клієнт";
                return View(customer);
            }
            else
            {
                _context.Customers.Remove(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.Id == id);
        }
    }
}
