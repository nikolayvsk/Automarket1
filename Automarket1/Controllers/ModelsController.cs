using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Automarket1;
using ClosedXML;
using ClosedXML.Excel;

namespace Automarket1.Controllers
{
    public class ModelsController : Controller
    {
        private readonly laba1Context _context;

        public ModelsController(laba1Context context)
        {
            _context = context;
        }

        // GET: Models
        public async Task<IActionResult> Index(string error, string message)
        {
            ViewBag.error = error;
            ViewBag.message = message;
            return View(await _context.Models.ToListAsync());
        }

        // GET: Models/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await _context.Models
                .FirstOrDefaultAsync(m => m.Id == id);
            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        // GET: Models/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Models/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Model1")] Model model)
        {
            if (IsUnique(model.Model1))
            {
                if (ModelState.IsValid)
                {
                    _context.Add(model);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            else
            {
                ViewData["ErrorMessage"] = "Така модель вже існує!";
            }
            return View(model);
        }
        bool IsUnique(string modelforV)
        {
            var s = (from model in _context.Models
                     where model.Model1 == modelforV
                     select model).ToList();
            if (s.Count == 0) { return true; }
            return false;
        }

        // GET: Models/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await _context.Models.FindAsync(id);
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }

        // POST: Models/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Model1")] Model model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }
            if (IsUnique(model.Model1))
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(model);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!ModelExists(model.Id))
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
            else
            {
                ViewData["ErrorMessage"] = "Така модель вже існує!";
            }
            return View(model);
        }

        // GET: Models/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await _context.Models
                .FirstOrDefaultAsync(m => m.Id == id);
            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        // POST: Models/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var model = await _context.Models.FindAsync(id);
            int countCars = _context.Cars.Where(s => s.ModelId == id).Count();
            if (countCars != 0)
            {
                ViewData["ErrorMessage"] = "Видалення неможливе, бо ця модель зарезервована";
                return View(model);
            }
            _context.Models.Remove(model);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ModelExists(int id)
        {
            return _context.Models.Any(e => e.Id == id);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Import(IFormFile fileExcel)
        {
            string error = null;
            if (ModelState.IsValid)
            {
                if(fileExcel != null)
                {
                    var stream = new FileStream(fileExcel.FileName, FileMode.Create);
                    await fileExcel.CopyToAsync(stream);
                    try
                    {
                        XLWorkbook workBook = new XLWorkbook(stream, XLEventTracking.Disabled);
                        foreach (IXLWorksheet worksheet in workBook.Worksheets)
                        {
                            Model newmod;
                            var c = (from mod in _context.Models
                                     where mod.Model1.Contains(worksheet.Name)
                                     select mod).ToList();
                            if (c.Count > 0)
                            {
                                newmod = c[0];
                            }
                            else
                            {
                                newmod = new Model();
                                newmod.Model1 = worksheet.Name;
                                _context.Models.Add(newmod);
                            }
                            var presenceErrors = FillingAllRows(worksheet, newmod);
                            if (presenceErrors != null) { error += presenceErrors; }
                        }
                        workBook.Dispose();
                        stream.Dispose();
                    }
                    catch
                    {
                        return RedirectToAction("Index", "Models", new { error = "Помилка, перевірте коректність данних" });
                    } 
                }
                else
                {
                    return RedirectToAction("Index", "Models", new { error = "Не прикріплений файл" });
                }
                await _context.SaveChangesAsync();
            }
            if (error != null)
            {

                return RedirectToAction("Index", "Models", new { error = "Ці серійні номери мають повтори: " + error, message = "Перевірте коректність данних і спробуйте ще раз :)"});
            }
            return RedirectToAction(nameof(Index));
        }

        public string? FillingAllRows(IXLWorksheet worksheet, Model newmod)
        {
            var carfil = new List<Car>();
            string error = null;
            var i = 1;
            foreach (IXLRow row in worksheet.RowsUsed().Skip(1))
            {
                Car car = new Car();
                car.SerialNumber = row.Cell(1).Value.ToString();
                car.Price = Convert.ToInt32(row.Cell(2).Value.ToString());
                car.Model = newmod;
                if (IsUniqueEXCEL(car.SerialNumber) && IsUniqueExcelFile(carfil, car.SerialNumber))
                {
                    /*
                    if (IsUniqueExcelFile(carfil, car.SerialNumber) == false) { error += "("+ i +") " + car.SerialNumber + "; "; }
                    { 
                        i++; 
                        continue; 
                    }
                    */
                    _context.Cars.Add(car);
                }
            }
            //if (error != null) { return error; }
            return null;
        }


        bool IsUniqueEXCEL(string serialNumber)
        {
            var a = (from car in _context.Cars
                     where car.SerialNumber == serialNumber
                     select car).ToList();
            if (a.Count == 0) { return true; }
            return false;
        }

        bool IsUniqueExcelFile(List<Car> cars, string serialNumber)
        {
            var b = (from car in cars
                     where car.SerialNumber == serialNumber
                     select car).ToList();
            if (b.Count == 0) { return true; }
            return false;
        }


        public ActionResult Export()
        {
            using (XLWorkbook workbook = ExportFunc())
            {

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Flush();

                    return new FileContentResult(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        FileDownloadName = $"automarket1.{DateTime.UtcNow.ToLongDateString()}.xlsx"
                    };
                }
            }
        }

        XLWorkbook ExportFunc()
        {
            XLWorkbook workbook = new XLWorkbook(XLEventTracking.Disabled);
            var models = _context.Models.Include("Cars").ToList();
            foreach(var m in models)
            {
                var worksheet = workbook.Worksheets.Add(m.Model1);
                worksheet.Cells("A1").Value = "Серійний номер";
                worksheet.Column("A").Width = 25;
                worksheet.Row(1).Style.Font.Bold = true;

                worksheet.Cells("B1").Value = "Ціна";
                worksheet.Column("B").Width = 25;
                worksheet.Row(1).Style.Font.Bold = true;

                var cars = m.Cars.ToList();

                for (int i = 0; i < cars.Count; i++)
                {
                    worksheet.Cell(i + 2, 1).Value = cars[i].SerialNumber;
                    worksheet.Cell(i + 2, 2).Value = cars[i].Price;
                }

            }
            return workbook;
        }
    }   


}
