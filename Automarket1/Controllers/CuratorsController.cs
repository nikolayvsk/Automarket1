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
    public class CuratorsController : Controller
    {
        private readonly laba1Context _context;

        public CuratorsController(laba1Context context)
        {
            _context = context;
        }

        // GET: Curators
        public async Task<IActionResult> Index(int? id, string? name)
        {
            if(id == null) return RedirectToAction("Employees", "Index");
            if(name == null) name = _context.Employees.Where(c => c.Id == id).FirstOrDefault().FullName;
            //if (id == null) return RedirectToAction("Index", "Employees");
            ViewBag.EmployeeId = id;
            ViewBag.EmployeeFullName = name;

            var curatorsByEmployee = _context.Curators.Where(b => b.EmployeeId == id).Include(c => c.Employee).Include(c => c.Sale);
            //var laba1Context = _context.Curators.Include(c => c.Employee).Include(c => c.Sale);

            return View(await curatorsByEmployee.ToListAsync());
        }

        // GET: Curators/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var curator = await _context.Curators
                .Include(c => c.Employee)
                .Include(c => c.Sale)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (curator == null)
            {
                return NotFound();
            }

            return View(curator);
        }

        // GET: Curators/Create
        public IActionResult Create(int employeeId)
        {
            //ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FullName");
            ViewBag.EmployeeId = employeeId;
            ViewBag.EmployeeFullName = _context.Employees.Where(c => c.Id == employeeId).FirstOrDefault().FullName;
            ViewData["SaleId"] = new SelectList(_context.Sales, "Id", "Id");
            return View();
        }

        // POST: Curators/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int employeeId, [Bind("Id,EmployeeId,SaleId")] Curator curator)
        {
            curator.EmployeeId = employeeId;
            if (IsUnique(curator.EmployeeId, curator.SaleId))
            {
                if (ModelState.IsValid)
                {
                    _context.Add(curator);
                    await _context.SaveChangesAsync();
                    //return RedirectToAction(nameof(Index));
                    return RedirectToAction("Index", "Curators", new { id = employeeId, FullName = _context.Employees.Where(c => c.Id == employeeId).FirstOrDefault().FullName });
                }
            }
            else
            {
                ViewData["ErrorMessage"] = "Таке куратурство вже існує";
            }
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FullName", curator.EmployeeId);
            ViewData["SaleId"] = new SelectList(_context.Sales, "Id", "Id", curator.SaleId);
            ViewBag.EmployeeId = employeeId;
            ViewBag.EmployeeFullName = _context.Employees.Where(c => c.Id == employeeId).FirstOrDefault().FullName;
            return View(curator);
        }

        bool IsUnique(int employeeID, int saleID)
        {
            var s = (from curator in _context.Curators
                     where curator.EmployeeId == employeeID && curator.SaleId == saleID
                     select curator).ToList();
            if (s.Count == 0) { return true; }
            return false;
        }

        // GET: Curators/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var curator = await _context.Curators.FindAsync(id);
            if (curator == null)
            {
                return NotFound();
            }
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FullName", curator.EmployeeId);
            ViewData["SaleId"] = new SelectList(_context.Sales, "Id", "Id", curator.SaleId);
            return View(curator);
        }

        // POST: Curators/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,EmployeeId,SaleId")] Curator curator)
        {
            if (id != curator.Id)
            {
                return NotFound();
            }
            if (IsUnique(curator.EmployeeId, curator.SaleId))
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(curator);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!CuratorExists(curator.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    return RedirectToAction("Index", "Curators", new { id = curator.EmployeeId, FullName = _context.Employees.Where(c => c.Id == curator.EmployeeId).FirstOrDefault().FullName });
                }
            }
            else
            {
                ViewData["ErrorMessage"] = "Таке куратурство вже існує";
            }
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FullName", curator.EmployeeId);
            ViewData["SaleId"] = new SelectList(_context.Sales, "Id", "Id", curator.SaleId);
            return View(curator);
        }

        // GET: Curators/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var curator = await _context.Curators
                .Include(c => c.Employee)
                .Include(c => c.Sale)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (curator == null)
            {
                return NotFound();
            }

            return View(curator);
        }

        // POST: Curators/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var curator = _context.Curators.Where(a => a.Id == id).Include(b => b.Employee).Include(c => c.Sale).FirstOrDefault();
            _context.Curators.Remove(curator);
            await _context.SaveChangesAsync();
            //return RedirectToAction(nameof(Index));
            return RedirectToAction("Index", "Curators", new { id = curator.EmployeeId, FullName = _context.Employees.Where(c => c.Id == curator.EmployeeId).FirstOrDefault().FullName });
        }

        private bool CuratorExists(int id)
        {
            return _context.Curators.Any(e => e.Id == id);
        }
    }
}
