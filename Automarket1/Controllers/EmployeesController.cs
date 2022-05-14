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
    public class EmployeesController : Controller
    {
        private readonly laba1Context _context;

        public EmployeesController(laba1Context context)
        {
            _context = context;
        }

        // GET: Employees
        public async Task<IActionResult> Index()
        {
            var laba1Context = _context.Employees.Include(e => e.Position);
            return View(await laba1Context.ToListAsync());
        }

        // GET: Employees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .Include(e => e.Position)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            //return View(employee);
            return RedirectToAction("Index", "Curators", new {id = employee.Id, name = employee.FullName});
        }

        // GET: Employees/Create
        public IActionResult Create()
        {
            ViewData["PositionId"] = new SelectList(_context.Positions, "Id", "Position1");
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FullName,DateBirth,PassportNumber,PhoneNumber,PositionId")] Employee employee)
        {
            if (IsUnique(employee.FullName, employee.DateBirth, employee.PassportNumber, employee.PhoneNumber, employee.PositionId) == 0)
            {
                if (ModelState.IsValid)
                {
                    _context.Add(employee);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            else if (IsUnique(employee.FullName, employee.DateBirth, employee.PassportNumber, employee.PhoneNumber, employee.PositionId) == 1)
            {
                ViewData["ErrorMessage_Passport"] = "Такий паспорт вже існує!";
                ViewData["PositionId"] = new SelectList(_context.Positions, "Id", "Position1", employee.PositionId);
                return View(employee);
            }
            else if (IsUnique(employee.FullName, employee.DateBirth, employee.PassportNumber, employee.PhoneNumber, employee.PositionId) == 2)
            {
                ViewData["ErrorMessage_Phone"] = "Такий номер телефону вже існує!";
                ViewData["PositionId"] = new SelectList(_context.Positions, "Id", "Position1", employee.PositionId);
                return View(employee);
            }
            else if (IsUnique(employee.FullName, employee.DateBirth, employee.PassportNumber, employee.PhoneNumber, employee.PositionId) == 3)
            {
                ViewData["ErrorMessage"] = "Така комбінація П.І.П. і номера паспорта вже існує!";
                ViewData["PositionId"] = new SelectList(_context.Positions, "Id", "Position1", employee.PositionId);
                return View(employee);
            }
            else if (IsUnique(employee.FullName, employee.DateBirth, employee.PassportNumber, employee.PhoneNumber, employee.PositionId) == 4)
            {
                ViewData["ErrorMessage"] = "Така комбінація П.І.П. і номера телефону вже існує!";
                ViewData["PositionId"] = new SelectList(_context.Positions, "Id", "Position1", employee.PositionId);
                return View(employee);
            }
            else if (IsUnique(employee.FullName, employee.DateBirth, employee.PassportNumber, employee.PhoneNumber, employee.PositionId) == 5)
            {
                ViewData["ErrorMessage"] = "Така комбінація дати народження й номера паспорта вже існує!";
                ViewData["PositionId"] = new SelectList(_context.Positions, "Id", "Position1", employee.PositionId);
                return View(employee);
            }
            else if (IsUnique(employee.FullName, employee.DateBirth, employee.PassportNumber, employee.PhoneNumber, employee.PositionId) == 6)
            {
                ViewData["ErrorMessage"] = "Така комбінація дати народження й номера телефону вже існує!";
                ViewData["PositionId"] = new SelectList(_context.Positions, "Id", "Position1", employee.PositionId);
                return View(employee);
            }
            else if (IsUnique(employee.FullName, employee.DateBirth, employee.PassportNumber, employee.PhoneNumber, employee.PositionId) == 7)
            {
                ViewData["ErrorMessage"] = "Така комбінація номера паспорта й номера телефону вже існує!";
                ViewData["PositionId"] = new SelectList(_context.Positions, "Id", "Position1", employee.PositionId);
                return View(employee);
            }
            else if (IsUnique(employee.FullName, employee.DateBirth, employee.PassportNumber, employee.PhoneNumber, employee.PositionId) == 8)
            {
                ViewData["ErrorMessage"] = "Така комбінація: П.І.П., номер паспорта, номер телефону - вже існує!";
                ViewData["PositionId"] = new SelectList(_context.Positions, "Id", "Position1", employee.PositionId);
                return View(employee);
            }
            else if (IsUnique(employee.FullName, employee.DateBirth, employee.PassportNumber, employee.PhoneNumber, employee.PositionId) == 9)
            {
                ViewData["ErrorMessage"] = "Ця посада вже зайнята!";
                ViewData["PositionId"] = new SelectList(_context.Positions, "Id", "Position1", employee.PositionId);
                return View(employee);
            }
            else if (IsUnique(employee.FullName, employee.DateBirth, employee.PassportNumber, employee.PhoneNumber, employee.PositionId) == 10)
            {
                ViewData["ErrorMessage"] = "Такий працівник вже існує!";
                ViewData["PositionId"] = new SelectList(_context.Positions, "Id", "Position1", employee.PositionId);
                return View(employee);
            }
            ViewData["PositionId"] = new SelectList(_context.Positions, "Id", "Position1", employee.PositionId);
            return View(employee);
        }

        int IsUnique(string fullName, DateTime dateOfBirth, string passportNumber, string phoneNumber, int position1)
        {
            var j = (from employee in _context.Employees
                     where employee.FullName == fullName && employee.DateBirth == dateOfBirth && employee.PassportNumber == passportNumber && employee.PhoneNumber == phoneNumber && employee.PositionId == position1
                     select employee).ToList();
            if (j.Count != 0) { return 10; }

            var i = (from employee in _context.Employees
                     where (employee.PositionId == position1)
                     select employee).ToList();
            if (i.Count != 0) { return 9; }

            var h = (from employee in _context.Employees
                     where employee.FullName == fullName && employee.PassportNumber == passportNumber && employee.PhoneNumber == phoneNumber
                     select employee).ToList();
            if(h.Count != 0) { return 8; }

            var g = (from employee in _context.Employees
                     where employee.PassportNumber == passportNumber && employee.PhoneNumber == phoneNumber
                     select employee).ToList();
            if (g.Count != 0) { return 7; }

            var f = (from employee in _context.Employees
                     where employee.DateBirth == dateOfBirth && employee.PhoneNumber == phoneNumber
                     select employee).ToList();
            if (f.Count != 0) { return 6; }

            var e = (from employee in _context.Employees
                     where employee.DateBirth == dateOfBirth && employee.PassportNumber == passportNumber
                     select employee).ToList();
            if (e.Count != 0) { return 5; }

            var d = (from employee in _context.Employees
                     where employee.FullName == fullName && employee.PhoneNumber == phoneNumber
                     select employee).ToList();
            if (d.Count != 0) { return 4; }

            var c = (from employee in _context.Employees
                     where employee.FullName == fullName && employee.PassportNumber == passportNumber
                     select employee).ToList();
            if (c.Count != 0) { return 3; }

            var b = (from employee in _context.Employees
                     where employee.PhoneNumber == phoneNumber
                     select employee).ToList();
            if (b.Count != 0) { return 2; }


            var a = (from employee in _context.Employees
                     where employee.PassportNumber == passportNumber
                     select employee).ToList();
            if (a.Count != 0) { return 1; }

            return 0;
        }

        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            ViewData["PositionId"] = new SelectList(_context.Positions, "Id", "Position1", employee.PositionId);
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FullName,DateBirth,PassportNumber,PhoneNumber,PositionId")] Employee employee)
        {
            if (id != employee.Id)
            {
                return NotFound();
            }
            if (IsUnique(employee.FullName, employee.DateBirth, employee.PassportNumber, employee.PhoneNumber, employee.PositionId) == 0)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(employee);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!EmployeeExists(employee.Id))
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
            else if (IsUnique(employee.FullName, employee.DateBirth, employee.PassportNumber, employee.PhoneNumber, employee.PositionId) == 1)
            {
                ViewData["ErrorMessage_Passport"] = "Такий паспорт вже існує!";
                ViewData["PositionId"] = new SelectList(_context.Positions, "Id", "Position1", employee.PositionId);
                return View(employee);
            }
            else if (IsUnique(employee.FullName, employee.DateBirth, employee.PassportNumber, employee.PhoneNumber, employee.PositionId) == 2)
            {
                ViewData["ErrorMessage_Phone"] = "Такий номер телефону вже існує!";
                ViewData["PositionId"] = new SelectList(_context.Positions, "Id", "Position1", employee.PositionId);
                return View(employee);
            }
            else if (IsUnique(employee.FullName, employee.DateBirth, employee.PassportNumber, employee.PhoneNumber, employee.PositionId) == 3)
            {
                ViewData["ErrorMessage"] = "Така комбінація П.І.П. і номера паспорта вже існує!";
                ViewData["PositionId"] = new SelectList(_context.Positions, "Id", "Position1", employee.PositionId);
                return View(employee);
            }
            else if (IsUnique(employee.FullName, employee.DateBirth, employee.PassportNumber, employee.PhoneNumber, employee.PositionId) == 4)
            {
                ViewData["ErrorMessage"] = "Така комбінація П.І.П. і номера телефону вже існує!";
                ViewData["PositionId"] = new SelectList(_context.Positions, "Id", "Position1", employee.PositionId);
                return View(employee);
            }
            else if (IsUnique(employee.FullName, employee.DateBirth, employee.PassportNumber, employee.PhoneNumber, employee.PositionId) == 5)
            {
                ViewData["ErrorMessage"] = "Така комбінація дати народження й номера паспорта вже існує!";
                ViewData["PositionId"] = new SelectList(_context.Positions, "Id", "Position1", employee.PositionId);
                return View(employee);
            }
            else if (IsUnique(employee.FullName, employee.DateBirth, employee.PassportNumber, employee.PhoneNumber, employee.PositionId) == 6)
            {
                ViewData["ErrorMessage"] = "Така комбінація дати народження й номера телефону вже існує!";
                ViewData["PositionId"] = new SelectList(_context.Positions, "Id", "Position1", employee.PositionId);
                return View(employee);
            }
            else if (IsUnique(employee.FullName, employee.DateBirth, employee.PassportNumber, employee.PhoneNumber, employee.PositionId) == 7)
            {
                ViewData["ErrorMessage"] = "Така комбінація номера паспорта й номера телефону вже існує!";
                ViewData["PositionId"] = new SelectList(_context.Positions, "Id", "Position1", employee.PositionId);
                return View(employee);
            }
            else if (IsUnique(employee.FullName, employee.DateBirth, employee.PassportNumber, employee.PhoneNumber, employee.PositionId) == 8)
            {
                ViewData["ErrorMessage"] = "Така комбінація: П.І.П., номер паспорта, номер телефону - вже існує!";
                ViewData["PositionId"] = new SelectList(_context.Positions, "Id", "Position1", employee.PositionId);
                return View(employee);
            }
            else if (IsUnique(employee.FullName, employee.DateBirth, employee.PassportNumber, employee.PhoneNumber, employee.PositionId) == 9)
            {
                ViewData["ErrorMessage"] = "Такий працівник вже існує!";
                ViewData["PositionId"] = new SelectList(_context.Positions, "Id", "Position1", employee.PositionId);
                return View(employee);
            }

            ViewData["PositionId"] = new SelectList(_context.Positions, "Id", "Position1", employee.PositionId);
            return View(employee);
        }

        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .Include(e => e.Position)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //var employee = await _context.Employees.FindAsync(id);
            var employee = _context.Employees.Where(a => a.Id == id).Include(b => b.Position).FirstOrDefault();
            int e_curators = _context.Curators.Where(s => s.EmployeeId == id).Count();
            if (e_curators != 0)
            {
                ViewData["ErrorMessage"] = "Видалення неможливе, бо цей працівник є куратором";
                return View(employee);
            }
            else
                _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }
    }
}
