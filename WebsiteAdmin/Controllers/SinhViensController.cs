using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebsiteAdmin.Data;
using WebsiteAdmin.Models;

namespace WebsiteAdmin.Controllers
{
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
        public async Task<IActionResult> Details(int? id)
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
        public async Task<IActionResult> Edit(int? id)
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
        public async Task<IActionResult> Edit(int id, SinhVien sinhVien)
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
  

        /*  // POST: SinhViens/Delete/5
          [HttpPost, ActionName("Delete")]
          [ValidateAntiForgeryToken]
          public async Task<IActionResult> DeleteConfirmed(int id)
          {
              if (_context.SinhVien == null)
              {
                  return Problem("Entity set 'WebsiteAdminContext.SinhVien'  is null.");
              }
              var sinhVien = await _context.SinhVien.FindAsync(id);
              if (sinhVien != null)
              {
                  _context.SinhVien.Remove(sinhVien);
              }

              await _context.SaveChangesAsync();
              return RedirectToAction(nameof(Index));
          }
  */

        [HttpPost, ActionName("Delete")]

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var sinhvien = await _context.SinhVien.FindAsync(id);
                if (sinhvien == null)
                {
                    return Json(new { success = false, message = "Sinh Vien not found." });
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
        private bool SinhVienExists(int id)
        {
            return (_context.SinhVien?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
