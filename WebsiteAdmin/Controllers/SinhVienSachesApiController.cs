using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebsiteAdmin.Data;
using WebsiteAdmin.Models;

namespace WebsiteAdmin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SinhVienSachesApiController : ControllerBase
    {
        private readonly WebsiteAdminContext _context;

        public SinhVienSachesApiController(WebsiteAdminContext context)
        {
            _context = context;
        }
        public class ApiResponse<T>
        {
            public bool Success { get; set; }
            public T Data { get; set; }
            public string Message { get; set; }
        }
        // GET: api/SinhVienSachesApi
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<SinhVienSach>>>> GetSinhVienSach()
        {
            try
            {
                if (_context.SinhVienSach == null)
                {
                    return NotFound(new ApiResponse<IEnumerable<SinhVienSach>> { Success = false, Message = "No SinhVienSach found." });
                }

                var sinhVienSachList = await _context.SinhVienSach.ToListAsync();

                return new ApiResponse<IEnumerable<SinhVienSach>> { Success = true, Data = sinhVienSachList, Message = "Success" };
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as per your application's requirements.
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<IEnumerable<SinhVienSach>> { Success = false, Message = "Internal server error occurred." });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<SinhVienSach>>> GetSinhVienSach(Guid id)
        {
            try
            {
                if (_context.SinhVienSach == null)
                {
                    return NotFound(new ApiResponse<SinhVienSach> { Success = false, Message = "No SinhVienSach found." });
                }

                var sinhVienSach = await _context.SinhVienSach.FindAsync(id);

                if (sinhVienSach == null)
                {
                    return NotFound(new ApiResponse<SinhVienSach> { Success = false, Message = "SinhVienSach not found." });
                }

                return new ApiResponse<SinhVienSach> { Success = true, Data = sinhVienSach, Message = "Success" };
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as per your application's requirements.
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<SinhVienSach> { Success = false, Message = "Internal server error occurred." });
            }
        }

        // PUT: api/SinhVienSachesApi/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<SinhVienSach>>> PutSinhVienSach(Guid id, SinhVienSachCreateDTO sinhVienSachDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (id != sinhVienSachDTO.Id)
                {
                    return BadRequest(new ApiResponse<SinhVienSach> { Success = false, Message = "Invalid ID provided." });
                }

                var sinhVienSach = await _context.SinhVienSach.FindAsync(id);

                if (sinhVienSach == null)
                {
                    return NotFound(new ApiResponse<SinhVienSach> { Success = false, Message = "SinhVienSach not found." });
                }

                sinhVienSach.SinhVienId = sinhVienSachDTO.SinhVienId;
                sinhVienSach.SachId = sinhVienSachDTO.SachId;
                sinhVienSach.ngaymuon = sinhVienSachDTO.ngaymuon;
                sinhVienSach.ngaytra = sinhVienSachDTO.ngaytra;

                await _context.SaveChangesAsync();
                return new ApiResponse<SinhVienSach> { Success = true, Data = sinhVienSach, Message = "Success" };
               
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as per your application's requirements.
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<SinhVienSach> { Success = false, Message = "Internal server error occurred." });
            }
        }


        // POST: api/SinhVienSachesApi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ApiResponse<SinhVienSach>>> PostSinhVienSach(SinhVienSachCreateDTO sinhVienSachCreateDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (_context.SinhVienSach == null)
                {
                    return Problem("Entity set 'WebsiteAdminContext.SinhVienSach' is null.");
                }

                var sinhVienSach = new SinhVienSach
                {
                    SinhVienId = sinhVienSachCreateDTO.SinhVienId,
                    SachId = sinhVienSachCreateDTO.SachId,
                    ngaymuon = sinhVienSachCreateDTO.ngaymuon,
                    ngaytra = sinhVienSachCreateDTO.ngaytra
                };
                sinhVienSach.Id = Guid.NewGuid();

                _context.SinhVienSach.Add(sinhVienSach);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetSinhVienSach", new { id = sinhVienSach.Id }, new ApiResponse<SinhVienSach> { Success = true, Data = sinhVienSach, Message = "Success" });
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as per your application's requirements.
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<SinhVienSach> { Success = false, Message = "Internal server error occurred." });
            }
        }


        // DELETE: api/SinhVienSachesApi/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<SinhVienSach>>> DeleteSinhVienSach(Guid id)
        {
            try
            {
                var sinhVienSach = await _context.SinhVienSach.FindAsync(id);
                if (sinhVienSach == null)
                {
                    return NotFound(new ApiResponse<SinhVienSach> { Success = false, Message = "SinhVienSach not found." });
                }

                _context.SinhVienSach.Remove(sinhVienSach);
                await _context.SaveChangesAsync();

                return new ApiResponse<SinhVienSach> { Success = true, Data = sinhVienSach, Message = "Success" };
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as per your application's requirements.
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<SinhVienSach> { Success = false, Message = "Internal server error occurred." });
            }
        }

        private bool SinhVienSachExists(Guid id)
        {
            return (_context.SinhVienSach?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
