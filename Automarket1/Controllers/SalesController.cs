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
    public class SalesController : Controller
    {
        private readonly laba1Context _context;

        public SalesController(laba1Context context)
        {
            _context = context;
        }

        // GET: Sales
        public async Task<IActionResult> Index(int? id, string? name)
        {
            if (id == null) return RedirectToAction("Customers", "Index");
            if(name == null) name = _context.Customers.Where(c => c.Id == id).FirstOrDefault().FullName;
            ViewBag.CustomerId = id;
            ViewBag.CustomerFullName = name;

            var salesByCustomer = _context.Sales.Where(b => b.CustomerId == id).Include(b => b.Car).Include(b => b.Customer);
            //var laba1Context = _context.Sales.Include(s => s.Car).Include(s => s.Customer);
            
            return View(await salesByCustomer.ToListAsync());
        }

        // GET: Sales/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sale = await _context.Sales
                .Include(s => s.Car)
                .Include(s => s.Customer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sale == null)
            {
                return NotFound();
            }

            return View(sale);
        }

        // GET: Sales/Create
        public IActionResult Create(int customerId)
        {
            ViewData["CarId"] = new SelectList(_context.Cars, "Id", "SerialNumber");
            //ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "FullName");
            ViewBag.CustomerId = customerId;
            ViewBag.CustomerFullName = _context.Customers.Where(c => c.Id == customerId).FirstOrDefault().FullName;
            return View();
        }

        // POST: Sales/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int customerId, [Bind("Id,CustomerId,CarId,DateSale")] Sale sale)
        {
            sale.CustomerId = customerId;
            if (IsUnique(sale.CustomerId, sale.CarId, sale.DateSale) == 0)
            {
                if (ModelState.IsValid)
                {
                    _context.Add(sale);
                    await _context.SaveChangesAsync();
                    //return RedirectToAction(nameof(Index));
                    return RedirectToAction("Index", "Sales", new { id = customerId, FullName = _context.Customers.Where(c => c.Id == customerId).FirstOrDefault().FullName });
                }
            }
            else if (IsUnique(sale.CustomerId, sale.CarId, sale.DateSale) == 1)
            {
                ViewData["ErrorMessage"] = "Такий продаж вже існує";
                ViewData["CarId"] = new SelectList(_context.Cars, "Id", "SerialNumber", sale.CarId);
                //return RedirectToAction("Index", "Sales", new { id = customerId, FullName = _context.Customers.Where(c => c.Id == customerId).FirstOrDefault().FullName });
                ViewBag.CustomerId = customerId;
                ViewBag.CustomerFullName = _context.Customers.Where(c => c.Id == customerId).FirstOrDefault().FullName;
                return View(sale);
            }
            else if (IsUnique(sale.CustomerId, sale.CarId, sale.DateSale) == 2)
            {
                ViewData["ErrorMessage"] = "Така пара покупця і машини вже створена";
                ViewData["CarId"] = new SelectList(_context.Cars, "Id", "SerialNumber", sale.CarId);
                //return RedirectToAction("Index", "Sales", new { id = customerId, FullName = _context.Customers.Where(c => c.Id == customerId).FirstOrDefault().FullName });
                ViewBag.CustomerId = customerId;
                ViewBag.CustomerFullName = _context.Customers.Where(c => c.Id == customerId).FirstOrDefault().FullName;
                return View(sale);

            }
            else if (IsUnique(sale.CustomerId, sale.CarId, sale.DateSale) == 3)
            {
                ViewData["ErrorMessage"] = "Цю машину в цей же час вже продали";
                ViewData["CarId"] = new SelectList(_context.Cars, "Id", "SerialNumber", sale.CarId);
                //return RedirectToAction("Index", "Sales", new { id = customerId, FullName = _context.Customers.Where(c => c.Id == customerId).FirstOrDefault().FullName });
                ViewBag.CustomerId = customerId;
                ViewBag.CustomerFullName = _context.Customers.Where(c => c.Id == customerId).FirstOrDefault().FullName;
                return View(sale);
            }
            else if (IsUnique(sale.CustomerId, sale.CarId, sale.DateSale) == 4)
            {
                ViewData["ErrorMessage"] = "Така машина з серійним номером вже є в продажі";
                ViewData["CarId"] = new SelectList(_context.Cars, "Id", "SerialNumber", sale.CarId);
                //return RedirectToAction("Index", "Sales", new { id = customerId, FullName = _context.Customers.Where(c => c.Id == customerId).FirstOrDefault().FullName });
                ViewBag.CustomerId = customerId;
                ViewBag.CustomerFullName = _context.Customers.Where(c => c.Id == customerId).FirstOrDefault().FullName;
                return View(sale);
            }
            ViewData["CarId"] = new SelectList(_context.Cars, "Id", "SerialNumber", sale.CarId);
            //ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "FullName", sale.CustomerId);
            ViewBag.CustomerId = customerId;
            ViewBag.CustomerFullName = _context.Customers.Where(c => c.Id == customerId).FirstOrDefault().FullName;
            return View(sale);
            //return RedirectToAction("Index", "Sales", new { id = sale.CustomerId, FullName = _context.Customers.Where(c => c.Id == sale.CustomerId).FirstOrDefault().FullName });
        }

        int IsUnique(int customerID, int carID, DateTime dataSale)
        {
            var a = (from sale in _context.Sales
                     where sale.CustomerId == customerID && sale.CarId == carID && sale.DateSale == dataSale
                     select sale).ToList();
            if (a.Count != 0) { return 1; }

            var b = (from sale in _context.Sales
                     where sale.CustomerId == customerID && sale.CarId == carID
                     select sale).ToList();
            if (b.Count != 0) { return 2; }

            var c = (from sale in _context.Sales
                     where sale.CarId == carID && sale.DateSale == dataSale
                     select sale).ToList();
            if (c.Count != 0) { return 3; }

            var d = (from sale in _context.Sales
                     where sale.CarId == carID
                     select sale).ToList();
            if(d.Count != 0) { return 4; }


            return 0;
        }


        // GET: Sales/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sale = await _context.Sales.FindAsync(id);
            if (sale == null)
            {
                return NotFound();
            }
            ViewData["CarId"] = new SelectList(_context.Cars, "Id", "SerialNumber", sale.CarId);
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "FullName", sale.CustomerId);
            //ViewBag.CustomerFullName = _context.Customers.Where(c => c.Id == customerId).FirstOrDefault().FullName;
            return View(sale);
        }

        // POST: Sales/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CustomerId,CarId,DateSale")] Sale sale)
        {
            if (id != sale.Id)
            {
                return NotFound();
            }
            if (IsUniqueEd(sale.DateSale))
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(sale);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!SaleExists(sale.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    return RedirectToAction("Index", "Sales", new { id = sale.CustomerId, FullName = _context.Customers.Where(c => c.Id == sale.CustomerId).FirstOrDefault().FullName });
                }
            }
            else
            {
                ViewData["ErrorMessage"] = "Ця машина в цей же час і була продана";
                /*
                ViewData["CarId"] = new SelectList(_context.Cars, "Id", "SerialNumber", sale.CarId);
                ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "FullName", sale.CustomerId);
                return View(sale);
                */
            }
            ViewData["CarId"] = new SelectList(_context.Cars, "Id", "SerialNumber", sale.CarId);
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "FullName", sale.CustomerId);
            return View(sale);
        }


        bool IsUniqueEd(DateTime dateSale)
        {
            var a = (from sale in _context.Sales
                     where sale.DateSale == dateSale
                     select sale).ToList();
            if (a.Count == 0) { return true; }
            return false;
        }
        // GET: Sales/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sale = await _context.Sales
                .Include(s => s.Car)
                .Include(s => s.Customer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sale == null)
            {
                return NotFound();
            }

            return View(sale);
        }

        // POST: Sales/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sale = _context.Sales.Where(a => a.Id == id).Include(b => b.Customer).Include(c => c.Car).FirstOrDefault();
            var c_curators = _context.Curators.Where(s => s.SaleId == id).Count();
            if (c_curators != 0)
            {
                ViewData["ErrorMessage"] = "Видалення неможливе";
                return View(sale);
            }
            else 
            {
                _context.Sales.Remove(sale);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Sales", new { id = sale.CustomerId, FullName = _context.Customers.Where(c => c.Id == sale.CustomerId).FirstOrDefault().FullName });
            }
        }

        private bool SaleExists(int id)
        {
            return _context.Sales.Any(e => e.Id == id);
        }
    }
}
