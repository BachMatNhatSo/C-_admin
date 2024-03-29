using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebsiteAdmin.Data;
using WebsiteAdmin.Models;

namespace WebsiteAdmin.Controllers
{

    
    [Route("api/[controller]")]
    [ApiController]
    public class SachesApiController : ControllerBase
    {
        private readonly WebsiteAdminContext _context;

        public SachesApiController(WebsiteAdminContext context)
        {
            _context = context;
        }
        public class ApiResponse<T>
        {
            public bool Success { get; set; }
            public T Data { get; set; }
            public string Message { get; set; }
        }
        // GET: api/SachesApi

        [HttpGet]
        public async Task<ActionResult<ApiResponsePaging<IEnumerable<Sach>>>> GetSach(int page , int pageSize, string sortBy ,string orderBy)
        {
            try
            {
                var query = _context.Sach.AsQueryable();
                if(!string.IsNullOrEmpty(sortBy) && !string.IsNullOrEmpty(orderBy))
                {
                    query= ApplySorting(query,sortBy,orderBy);
                }
                var totalItems = await query.CountAsync();
                var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
                var sachList = await query.Skip((page - 1) * pageSize)
                                          .Take(pageSize)
                                          .ToListAsync();

                var meta = new PaginationMeta
                {
                    Page = page,
                    PageSize = pageSize,
                    TotalPages = totalPages,
                    TotalItems = totalItems
                };

                return new ApiResponsePaging<IEnumerable<Sach>>
                {
                    Success = true,
                    Data = sachList,
                    Message = "Success",
                    Meta = meta // Include pagination metadata in the response
                };
            }
            catch (Exception ex)
            {
                // Handle exception
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponsePaging<IEnumerable<Sach>> { Success = false, Message = "Internal server error occurred." });
            }
        }
        private IQueryable<Sach> ApplySorting(IQueryable<Sach> query, string SortBy,string orderBy)
        {
            switch (SortBy.ToLower())
            {
                case "tensach":
                    {
                        query= orderBy.ToLower()=="asc"? query.OrderBy(x => x.tenSach) : query.OrderByDescending(x => x.tenSach);
                        break;
                    }
                case "tacgia":
                    {
                        query = orderBy.ToLower() == "asc" ? query.OrderBy(x => x.tacGia) : query.OrderByDescending(x => x.tacGia);
                        break;
                    }
                case "giatien":
                    {
                        query = orderBy.ToLower() == "asc" ? query.OrderBy(x => x.giaTien) : query.OrderByDescending(x => x.giaTien);
                        break;
                    }
                case "nxb":
                    {
                        query = orderBy.ToLower() == "asc" ? query.OrderBy(x => x.nxb) : query.OrderByDescending(x => x.nxb);
                        break;
                    }
                default:
                    {
                        query = query.OrderBy(x=>x.giaTien);
                        break;
                    }
            }
            
            return query;
        } 



        // GET: api/SachesApi/5

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<Sach>>> GetSach(Guid id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (_context.Sach == null)
                {
                    return NotFound(new ApiResponse<Sach> { Success = false, Message = "No Sach found." });
                }

                var sach = await _context.Sach.FindAsync(id);

                if (sach == null)
                {
                    return NotFound(new ApiResponse<Sach> { Success = false, Message = "Sach not found." });
                }

                return new ApiResponse<Sach> { Success = true, Data = sach, Message = "Success" };
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as per your application's requirements.
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<Sach> { Success = false, Message = "Internal server error occurred." });
            }
        }

        // PUT: api/SachesApi/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> PutSach(Guid id, Sach sach)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (id != sach.Id)
                {
                    return BadRequest(new ApiResponse<object> { Success = false, Message = "Invalid ID provided." });
                }

                _context.Entry(sach).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SachExists(id))
                    {
                        return NotFound(new ApiResponse<object> { Success = false, Message = "Sach not found." });
                    }
                    else
                    {
                        throw;
                    }
                }

                // Fetch the updated sach object from the database to include in the response
                var updatedSach = await _context.Sach.FindAsync(id);

                return new ApiResponse<object> { Success = true, Data = updatedSach, Message = "Sach updated successfully." };
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as per your application's requirements.
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<object> { Success = false, Message = "Internal server error occurred." });
            }
        }


        // POST: api/SachesApi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ApiResponse<Sach>>> PostSach(Sach sach)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (sach == null)
                {
                    return BadRequest(new ApiResponse<Sach> { Success = false, Message = "Invalid Sach data provided." });
                }

                if (_context.Sach == null)
                {
                    return Problem("Entity set 'WebsiteAdminContext.Sach' is null.", statusCode: StatusCodes.Status500InternalServerError);
                }
                sach.Id = Guid.NewGuid();
                _context.Sach.Add(sach);
                await _context.SaveChangesAsync();

                // Return a custom ApiResponse with success status, data, and message
                return new ApiResponse<Sach> { Success = true, Data = sach, Message = "Success" };
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as per your application's requirements.
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<Sach> { Success = false, Message = "Internal server error occurred." });
            }
        }


        // DELETE: api/SachesApi/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteSach(Guid id)
        {
            try
            {
                if (_context.Sach == null)
                {
                    return NotFound(new ApiResponse<object> { Success = false, Message = "No Sach found." });
                }

                var sach = await _context.Sach.FindAsync(id);
                if (sach == null)
                {
                    return NotFound(new ApiResponse<object> { Success = false, Message = "Sach not found." });
                }

                _context.Sach.Remove(sach);
                await _context.SaveChangesAsync();

                return new ApiResponse<object> { Success = true, Data = null, Message = "Sach deleted successfully." };
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as per your application's requirements.
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<object> { Success = false, Message = "Internal server error occurred." });
            }
        }

        private bool SachExists(Guid id)
        {
            return (_context.Sach?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
