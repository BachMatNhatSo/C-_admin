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
    public class SachesController : Controller
    {
        private readonly WebsiteAdminContext _context;

        public SachesController(WebsiteAdminContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
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
    }
}
