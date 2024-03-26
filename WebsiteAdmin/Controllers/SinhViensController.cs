using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using WebsiteAdmin.Data;
using WebsiteAdmin.Models;

namespace WebsiteAdmin.Controllers
{
    [Authorize]
    public class SinhViensController : Controller
    {
        private readonly WebsiteAdminContext _context;

        public SinhViensController(WebsiteAdminContext context)
        {
            _context = context;
        }

        // GET: SinhViens
        public async Task<IActionResult> Index()
        {
            var sinhviens = await _context.SinhVien.ToListAsync();
            return View(sinhviens);
        }
        public async Task<IActionResult> SinhVienData()
        {
            var sinhviens = await _context.SinhVien.ToListAsync();
            return PartialView("_SinhVienDataPartial", sinhviens);
        }
        // GET: SinhViens/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.SinhVien == null)
            {
                return NotFound();
            }

            var sinhVien = await _context.SinhVien
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sinhVien == null)
            {
                return NotFound();
            }

            return View(sinhVien);
        }

        // GET: SinhViens/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SinhViens/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SinhVien sinhvien)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sinhvien);
                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        // GET: SinhViens/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.SinhVien == null)
            {
                return NotFound();
            }

            var sinhVien = await _context.SinhVien.FindAsync(id);
            if (sinhVien == null)
            {
                return NotFound();
            }
            return View(sinhVien);
        }

        // POST: SinhViens/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, SinhVien sinhVien)
        {
            if (id != sinhVien.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sinhVien);
                    await _context.SaveChangesAsync();
                    return Json(new { success = true });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SinhVienExists(sinhVien.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

            }
            return Json(new { success = false });
        }

        // GET: SinhViens/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.SinhVien == null)
            {
                return NotFound();
            }

            var sinhVien = await _context.SinhVien
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sinhVien == null)
            {
                return NotFound();
            }

            return View(sinhVien);
        }

        // POST: SinhViens/Delete/5
        [HttpPost, ActionName("Delete")]

        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var sinhvien = await _context.SinhVien.FindAsync(id);
                if (sinhvien == null)
                {
                    return Json(new { success = false, message = "Book not found." });
                }
                if (id == null)
                {
                    return Json(new { success = false, message = "id not found." });
                }
                _context.SinhVien.Remove(sinhvien);
                await _context.SaveChangesAsync();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        private bool SinhVienExists(Guid id)
        {
            return (_context.SinhVien?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        [HttpGet]
        public async Task<FileResult> ExportExcel()
        {
            var item = await _context.SinhVien.ToListAsync();
            var filename = "DanhSachSinhVien.xlsx";
            return GenerateExcel(filename, item);
        }

        private FileResult GenerateExcel(string fileName, IEnumerable<SinhVien> sinhViens)
        {

            System.Data.DataTable dataTable = new System.Data.DataTable("SinhVien");
            dataTable.Columns.AddRange(new DataColumn[]
            {
                new DataColumn("Id"),
                new DataColumn("Tên Sinh Viên"),
                new DataColumn("MSSV"),
                new DataColumn("Điện Thoại"),
                new DataColumn("Địa Chỉ"),
            });
            foreach (var item in sinhViens)
            {
                dataTable.Rows.Add(item.Id, item.tensinhvien, item.mssv, item.dienthoai, item.diachi);
            }
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dataTable);
                using (MemoryStream ms = new MemoryStream())
                {
                    wb.SaveAs(ms);
                    return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }


        public async Task<IActionResult> ImportExcel(IFormFile formFile)
        {
            var list = new List<SinhVien>();
            using (var stream = new MemoryStream())
            {

                if (formFile == null)
                {
                    // Display a SweetAlert to inform the user that no file was uploaded
                    TempData["Message"] = "error";
                    return RedirectToAction(nameof(Index)); // Redirect to the same page
                }
                await formFile.CopyToAsync(stream);
                using (var package = new ExcelPackage())
                {
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // Set the license context
                    package.Load(stream);
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    var rowcount = worksheet.Dimension.Rows;
                    for (int row = 2; row <= rowcount; row++)
                    {
                        var tensinhvien = worksheet.Cells[row, 2].Value?.ToString()?.Trim();
                        var mssv = worksheet.Cells[row, 3].Value?.ToString()?.Trim();

                        var dienthoai = worksheet.Cells[row, 4].Value?.ToString()?.Trim();
                        var diachi = worksheet.Cells[row, 5].Value?.ToString()?.Trim();

                        var sinhvien = new SinhVien
                        {
                            tensinhvien = tensinhvien,
                            mssv = mssv,
                            dienthoai = dienthoai,
                            diachi = diachi
                        };
                        list.Add(sinhvien);
                    }
                }
            }

            // Add imported Sach objects to the database context
            _context.AddRange(list);

            // Save changes to the database
            await _context.SaveChangesAsync();

            // Redirect to a specific action or page
            return RedirectToAction("Index"); // Replace "Index" with the name of the action or page you want to redirect to
        }


    }


}
