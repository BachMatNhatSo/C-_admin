﻿using System;
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
    public class SinhVienSachesController : Controller
    {
        private readonly WebsiteAdminContext _context;

        public SinhVienSachesController(WebsiteAdminContext context)
        {
            _context = context;
        }

        // GET: SinhVienSaches
        public async Task<IActionResult> Index()
        {
            var sinhviensaches = await _context.SinhVienSach.ToListAsync();
            return View(sinhviensaches);
        }
        /* public async Task<IActionResult> SinhVienSachData()
         {
             var sinhviensaches = await _context.SinhVienSach.ToListAsync();
             return PartialView("_SinhVienSachDataPartial", sinhviensaches);
         }*/

        [HttpGet]
        public IActionResult GetData()
        {
            var data = _context.SinhVienSach
                .Include(ssv => ssv.SinhVien)
                .Include(ssv => ssv.Sach)
                .Select(ssv => new ViewModelSachSinhVien
                {
                    Id = ssv.Id,
                    tenSinhVien = ssv.SinhVien.tensinhvien,
                    tenSach = ssv.Sach.tenSach,
                    ngaymuon = DateOnly.FromDateTime(ssv.ngaymuon),
                    ngaytra = DateOnly.FromDateTime(ssv.ngaytra),
                    SachId = ssv.SachId,
                    SinhVienId = ssv.SinhVienId
                })
                .ToList();
            return Json(data);
        }


        public IActionResult GetSinhVienSachData()
        {
            var sinhVienSachData = _context.SinhVien.Select(ssv => new { ssv.Id, ssv.tensinhvien }).ToList();

            return Json(sinhVienSachData);
        }
        public IActionResult GetSachData()
        {
            var sinhVienSachData = _context.Sach.Select(ssv => new { ssv.Id, ssv.tenSach }).ToList();

            return Json(sinhVienSachData);
        }

        // GET: SinhVienSaches/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.SinhVienSach == null)
            {
                return NotFound();
            }

            var sinhVienSach = await _context.SinhVienSach
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sinhVienSach == null)
            {
                return NotFound();
            }

            return View(sinhVienSach);
        }

        // GET: SinhVienSaches/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SinhVienSaches/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.


        [HttpPost]
        public async Task<IActionResult> Create(SinhVienSach sinhVienSach)
        {
            sinhVienSach.Id = Guid.NewGuid(); // Generate a new GUID for the ID
            _context.Add(sinhVienSach);
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "SinhVienSach created successfully" });
        }

        // GET: SinhVienSaches/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.SinhVienSach == null)
            {
                return NotFound();
            }

            var sinhVienSach = await _context.SinhVienSach.FindAsync(id);
            if (sinhVienSach == null)
            {
                return NotFound();
            }
            return View(sinhVienSach);
        }

        // POST: SinhVienSaches/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]

        public async Task<IActionResult> Edit(Guid id, SinhVienSach sinhVienSach)
        {
            if (id != sinhVienSach.Id)
            {
                return NotFound();
            }
            try
            {
                _context.Update(sinhVienSach);
                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SinhVienSachExists(sinhVienSach.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }


            }

            return Json(new { success = false });
        }

        // GET: SinhVienSaches/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.SinhVienSach == null)
            {
                return NotFound();
            }

            var sinhVienSach = await _context.SinhVienSach
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sinhVienSach == null)
            {
                return NotFound();
            }

            return View(sinhVienSach);
        }

        // POST: SinhVienSaches/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var sinhVienSach = await _context.SinhVienSach.FindAsync(id);
                if (sinhVienSach == null)
                {
                    return Json(new { success = false, message = "Book not found." });
                }
                _context.SinhVienSach.Remove(sinhVienSach);
                await _context.SaveChangesAsync();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        private bool SinhVienSachExists(Guid id)
        {
            return (_context.SinhVienSach?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}