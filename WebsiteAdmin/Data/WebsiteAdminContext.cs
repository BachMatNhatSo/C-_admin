using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebsiteAdmin.Models;

namespace WebsiteAdmin.Data
{
    public class WebsiteAdminContext : DbContext
    {
        public WebsiteAdminContext (DbContextOptions<WebsiteAdminContext> options)
            : base(options)
        {
        }

        public DbSet<WebsiteAdmin.Models.Sach> Sach { get; set; } = default!;
        public DbSet<WebsiteAdmin.Models.SinhVien> SinhVien { get; set; } = default!;
    }
}
