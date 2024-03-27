using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebsiteAdmin.Data;
using Microsoft.OpenApi.Models;
using WebsiteAdmin.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebsiteAdmin.Services;
using OfficeOpenXml;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton(builder.Configuration);
builder.Services.AddDbContext<WebsiteAdminContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("connectionString") ?? throw new InvalidOperationException("Connection string 'WebsiteAdminContext' not found.")));
// Add services to the container.
builder.Services.AddIdentity<User, IdentityRole>(option =>
{
    option.Password.RequiredUniqueChars = 0;
    option.Password.RequireUppercase = false;
    option.Password.RequireLowercase = false;
    option.Password.RequiredLength = 8;
    option.Password.RequireNonAlphanumeric = false;
}).AddEntityFrameworkStores<WebsiteAdminContext>().AddDefaultTokenProviders();

ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My Api", Version = "v1" });
});
builder.Services.AddSingleton<JwtService>();
builder.Services.AddSession(options =>
{
    options.IdleTimeout=TimeSpan.FromMinutes(5);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddHttpContextAccessor();
/*builder.Services.AddRazorPages();*/
var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    });
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseSession(); 
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Saches}/{action=Index}/{id?}");

app.Run();
