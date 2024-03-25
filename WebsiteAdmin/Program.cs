using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebsiteAdmin.Data;
using Microsoft.OpenApi.Models;
using WebsiteAdmin.Models;
using Microsoft.AspNetCore.Identity;


var builder = WebApplication.CreateBuilder(args);

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

builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My Api", Version = "v1" });
});
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


app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Saches}/{action=Index}/{id?}");

app.Run();
