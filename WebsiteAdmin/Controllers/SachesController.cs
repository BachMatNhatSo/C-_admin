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
using WebsiteAdmin.Data;
using WebsiteAdmin.Models;
using System.IO;
using OfficeOpenXml;
using DocumentFormat.OpenXml.Office2010.Excel;
using System.Security.Claims;
using DocumentFormat.OpenXml.Spreadsheet;

namespace WebsiteAdmin.Controllers
{
    [Authorize]
    public class SachesController : Controller
    {
        private readonly WebsiteAdminContext _context;
        private readonly IHttpContextAccessor httpContextAccessor;
        public SachesController(WebsiteAdminContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<IActionResult> Index()
        {
            var httpContext = httpContextAccessor.HttpContext;

            // Kiểm tra xem người dùng đã đăng nhập chưa
            if (httpContext.User.Identity.IsAuthenticated)
            {
                // Lấy thông tin của người dùng từ ClaimsPrincipal
                var userName = httpContext.User.FindFirst(ClaimTypes.Name)?.Value;

                // Sử dụng thông tin của người dùng
                httpContextAccessor.HttpContext.Session.SetString("UserName", userName);
            }
            var sachs = await _context.Sach.ToListAsync();
            return View(sachs);
        }
        public async Task<IActionResult> SachData()
        {
            var sachs = await _context.Sach.ToListAsync();
            return PartialView("_SachDataPartial", sachs);
        }
        // GET: Saches/Create
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
      
        public async Task<IActionResult> Create(Sach sach)
        {
            if (ModelState.IsValid)
            {
                sach.Id = Guid.NewGuid();
                _context.Add(sach);
                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        // GET: Saches/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Sach == null)
            {
                return NotFound();
            }

            var sach = await _context.Sach.FindAsync(id);
            if (sach == null)
            {
                return NotFound();
            }
            return View(sach);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Sach sach)
        {
            if (id != sach.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sach);
                    await _context.SaveChangesAsync();
                    return Json(new { success = true });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SachExists(sach.Id))
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

        // GET: Saches/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Sach == null)
            {
                return NotFound();
            }

            var sach = await _context.Sach
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sach == null)
            {
                return NotFound();
            }

            return View(sach);
        }

        // POST: Saches/Delete/5
        [HttpPost, ActionName("Delete")]

        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var sach = await _context.Sach.FindAsync(id);
                if (sach == null)
                {
                    return Json(new { success = false, message = "Book not found." });
                }
                if (id == null)
                {
                    return Json(new { success = false, message = "id not found." });
                }
                _context.Sach.Remove(sach);
                await _context.SaveChangesAsync();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        private bool SachExists(Guid id)
        {
          return (_context.Sach?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        [HttpGet]
        public async Task<FileResult> ExportExcel()
        {
            var item = await _context.Sach.ToListAsync();
            var filename = "DanhSachBook.xlsx";
            return GenerateExcel(filename, item);
        }

        private FileResult GenerateExcel(string fileName, IEnumerable<Sach> saches)
        {
            
            System.Data.DataTable dataTable = new System.Data.DataTable("Sach");
            dataTable.Columns.AddRange(new DataColumn[]
            {
                new DataColumn("Id"),
                new DataColumn("Tên Sách"),
                new DataColumn("Tác Giả"),
                new DataColumn("Giá Tiền"),
                new DataColumn("NXB"),
            });
            foreach (var item in saches)
            {
                dataTable.Rows.Add(item.Id, item.tenSach, item.tacGia, item.giaTien, item.nxb);
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
            var list = new List<Sach>();
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
                        var tenSach = worksheet.Cells[row, 2].Value?.ToString()?.Trim();
                        var tacGia = worksheet.Cells[row, 3].Value?.ToString()?.Trim();
                        double giaTien;
                        if (!double.TryParse(worksheet.Cells[row, 4].Value?.ToString()?.Trim(), out giaTien))
                        {
                            // Handle parsing failure, maybe set a default value or log an error
                        }

                        var nxb = worksheet.Cells[row, 5].Value?.ToString()?.Trim();

                        var sach = new Sach
                        {
                            tenSach = tenSach,
                            tacGia = tacGia,
                            giaTien = giaTien,
                            nxb = nxb
                        };
                        list.Add(sach);
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
