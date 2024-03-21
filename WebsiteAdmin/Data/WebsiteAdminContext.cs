using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebsiteAdmin.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace WebsiteAdmin.Data
{
    public class WebsiteAdminContext : IdentityDbContext<User>
    {
        public DbSet<WebsiteAdmin.Models.Sach> Sach { get; set; } = default!;
        public DbSet<WebsiteAdmin.Models.SinhVien> SinhVien { get; set; } = default!;
        public DbSet<WebsiteAdmin.Models.SinhVienSach> SinhVienSach { get; set; } = default!;

        public WebsiteAdminContext (DbContextOptions<WebsiteAdminContext> options)
            : base(options)
        {
        }
        

    }
}
