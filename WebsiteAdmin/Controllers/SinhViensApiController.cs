﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebsiteAdmin.Data;
using WebsiteAdmin.Models;

namespace WebsiteAdmin.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class SinhViensApiController : ControllerBase
    {
        private readonly WebsiteAdminContext _context;

        public SinhViensApiController(WebsiteAdminContext context)
        {
            _context = context;
        }
        public class ApiResponse<T>
        {
            public bool Success { get; set; }
            public T Data { get; set; }
            public string Message { get; set; }
        }
        // GET: api/SinhViensApi
        [HttpGet]
        public async Task<ActionResult<ApiResponsePaging<IEnumerable<SinhVien>>>> GetSinhVien(int page, int pageSize,string sortBy,string orderBy)
        {
           

            try
            {
                var query = _context.SinhVien.AsQueryable();
                if (!string.IsNullOrEmpty(sortBy)&&!string.IsNullOrEmpty(orderBy))
                {
                    query = ApplySorting(query,sortBy,orderBy);
                }

                var totalItems = await query.CountAsync();
                var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
                var listSinhVien = await query.Skip((page - 1) * pageSize)
                                    .Take(pageSize)
                                    .ToListAsync();
                var meta = new PaginationMeta
                {
                    Page = page,
                    PageSize = pageSize,
                    TotalItems = totalItems,
                    TotalPages = totalPages,
                };
                return new ApiResponsePaging<IEnumerable<SinhVien>>
                {
                    Success = true,
                    Data= listSinhVien,
                    Message="Success",
                    Meta = meta
                };
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as per your application's requirements.
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponsePaging<IEnumerable<SinhVien>> { Success = false, Message = "Internal server error occurred." });
            }
        }
        private IQueryable<SinhVien> ApplySorting(IQueryable<SinhVien> query,string sortBy,string orderBy)
        {
            switch (sortBy.ToLower())
            {
                case "tensinhvien":
                    {
                        query = orderBy.ToLower() == "asc" ? query.OrderBy(x => x.tensinhvien) : query.OrderByDescending(x => x.tensinhvien);
                        break;
                    }
                case "mssv":
                    {
                        query = orderBy.ToLower() == "asc" ? query.OrderBy(x => x.mssv) : query.OrderByDescending(x => x.mssv);
                        break;
                    }
                case "dienthoai":
                    {
                        query = orderBy.ToLower() == "asc" ? query.OrderBy(x => x.dienthoai) : query.OrderByDescending(x => x.dienthoai);
                        break;
                    }
                case "diachi":
                    {
                        query = orderBy.ToLower() == "asc" ? query.OrderBy(x => x.diachi) : query.OrderByDescending(x => x.diachi);
                        break;
                    }
                default :
                    {
                        query = query.OrderBy(x => x.Id);
                        break;
                    }
            }
            return query;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<SinhVien>>> GetSinhVien(Guid id)
        {
            try
            {
                if (_context.SinhVien == null)
                {
                    return NotFound(new ApiResponse<SinhVien> { Success = false, Message = "No SinhVien found." });
                }

                var sinhVien = await _context.SinhVien.FindAsync(id);

                if (sinhVien == null)
                {
                    return NotFound(new ApiResponse<SinhVien> { Success = false, Message = "SinhVien not found." });
                }

                return new ApiResponse<SinhVien> { Success = true, Data = sinhVien, Message = "Success" };
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as per your application's requirements.
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<SinhVien> { Success = false, Message = "Internal server error occurred." });
            }
        }

        // PUT: api/SinhViensApi/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<SinhVien>>> PutSinhVien(Guid id, SinhVien sinhVien)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (id != sinhVien.Id)
                {
                    return BadRequest(new ApiResponse<SinhVien> { Success = false, Message = "Invalid ID provided." });
                }

                _context.Entry(sinhVien).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                    return new ApiResponse<SinhVien> { Success = true, Data = sinhVien, Message = "Success" };
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SinhVienExists(id))
                    {
                        return NotFound(new ApiResponse<SinhVien> { Success = false, Message = "SinhVien not found." });
                    }
                    else
                    {
                        throw;
                    }
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as per your application's requirements.
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<SinhVien> { Success = false, Message = "Internal server error occurred." });
            }
        }

        // POST: api/SinhViensApi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ApiResponse<SinhVien>>> PostSinhVien(SinhVien sinhVien)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (sinhVien == null)
                {
                    return BadRequest(new ApiResponse<SinhVien> { Success = false, Message = "Invalid SinhVien data provided." });
                }

                if (_context.SinhVien == null)
                {
                    return Problem("Entity set 'WebsiteAdminContext.SinhVien' is null.", statusCode: StatusCodes.Status500InternalServerError);
                }
                sinhVien.Id = Guid.NewGuid();
                _context.SinhVien.Add(sinhVien);
                await _context.SaveChangesAsync();

                // Return a custom ApiResponse with success status, data, and message
                return new ApiResponse<SinhVien> { Success = true, Data = sinhVien, Message = "Success" };
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as per your application's requirements.
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<SinhVien> { Success = false, Message = "Internal server error occurred." });
            }
        }


        // DELETE: api/SinhViensApi/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<SinhVien>>> DeleteSinhVien(Guid id)
        {
            try
            {
                if (_context.SinhVien == null)
                {
                    return NotFound(new ApiResponse<SinhVien> { Success = false, Message = "No SinhVien found." });
                }

                var sinhVien = await _context.SinhVien.FindAsync(id);

                if (sinhVien == null)
                {
                    return NotFound(new ApiResponse<SinhVien> { Success = false, Message = "SinhVien not found." });
                }

                _context.SinhVien.Remove(sinhVien);
                await _context.SaveChangesAsync();
                return new ApiResponse<SinhVien> { Success = true, Data = null, Message = "Sach deleted successfully." };
              
               
               
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as per your application's requirements.
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<SinhVien> { Success = false, Message = "Internal server error occurred." });
            }
        }


        private bool SinhVienExists(Guid id)
        {
            return (_context.SinhVien?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
