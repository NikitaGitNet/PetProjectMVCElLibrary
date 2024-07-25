using AutoMapper;
using BLL.Interfaces;
using BLL.Services.Book;
using DAL.Domain;
using DAL.Domain.Entities;
using DAL.Domain.Interfaces.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PetProjectMVCElLibrary.Service;
using PetProjectMVCElLibrary.Service.Logger;
using PetProjectMVCElLibrary.Service.Mapper;

var builder = WebApplication.CreateBuilder(args);
// ������������ BookService � DI, ��� ����, ���� ������� ���� ���
builder.Services.AddTransient<IBookService, BookService>();
// ������������� ���� ��� ������������, ��������� ������������ � ������� �������� Logging ���� �� �������� ������� WebApplication
builder.Logging.AddFile(Path.Combine(Directory.GetCurrentDirectory(), "logger.txt"));
// ��������� ������� ���� �������
builder.Services.AddAutoMapper(typeof(DefaultProfile));
// ������������ IHttpContextAccessor ��� ��������� ������� � �������� HttpContext ����� �������� HttpContext.
builder.Services.AddHttpContextAccessor();
// ������������ Context, ��������� �������� ������ �����������
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});
// ������ ����������� ����������� ��� ����������� ������� ������ ������������
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(opts =>
{
    opts.User.RequireUniqueEmail = true;
    opts.Password.RequiredLength = 6;
    opts.Password.RequireNonAlphanumeric = false;
    opts.Password.RequireLowercase = false;
    opts.Password.RequireUppercase = false;
    opts.Password.RequireDigit = false;
}).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
// ������� ��������� Cookie
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.Name = "myCompanyAuth";
    options.Cookie.HttpOnly = true;
    options.LoginPath = "/account/login";
    options.AccessDeniedPath = "/account/accessdenied";
    options.SlidingExpiration = true;
});
// �������� �����������
builder.Services.AddAuthorization(x =>
{
    x.AddPolicy("AdminArea", policy => { policy.RequireRole("admin"); });
    x.AddPolicy("ModeratorArea", policy => { policy.RequireRole("moderator"); });
});
// ������������� �� ����� �� ����
builder.Services.AddControllersWithViews(x =>
{
    x.Conventions.Add(new AdminAreaAuthorization("Admin", "AdminArea"));
    x.Conventions.Add(new ModeratorAreaAuthorization("Moderator", "ModeratorArea"));
});

var app = builder.Build();

app.UseDeveloperExceptionPage();


app.UseStaticFiles();

app.UseRouting();

app.UseCookiePolicy();
app.UseAuthentication();
app.UseAuthorization();

// ����������� �������������
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute("admin", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
    endpoints.MapControllerRoute("moderator", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
    endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
});

// ��������� �������
app.Run(async (context) =>
{
    app.Logger.LogInformation($"Path: {context.Request.Path}  Time:{DateTime.Now.ToLongTimeString()}");
    await context.Response.WriteAsync("��������� ������");
});

app.Run();
