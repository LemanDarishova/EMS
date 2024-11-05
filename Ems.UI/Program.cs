using Ems.BusinessLogic;
using Ems.BusinessLogic.Abstract;
using Ems.BusinessLogic.Concrete;
using Ems.BusinessLogic.Dtos;
using Ems.BusinessLogic.Validations;
using Ems.DataAccessLayer.Abstract;
using Ems.DataAccessLayer.EntityFrameworkCore.Concrete;
using Ems.DataAccessLayer.EntityFrameworkCore.Contexts;
using Ems.ExternalServices.Interfaces;
using Ems.ExternalServices.Models;
using Ems.ExternalServices.Services;
using Ems.UI.Middleware;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services
    .AddDbContext<EmsContext>(options => options.UseSqlServer(connectionString));

var emailConfigurations = builder.Configuration.GetSection("SmtpSetting");
builder.Services.Configure<SmtpSetting>(emailConfigurations);

builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 104857600; // 100 MB kimi bir limit t?yin ed? bil?rsiniz
});

builder.Services.AddTransient<IEmailService, EmailService>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserDetailRepository, UserDetailRepository>();

builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

builder.Services.AddScoped<IEstateRepository, EstateRepository>();
builder.Services.AddScoped<IEstateService, EstateService>();

builder.Services.AddScoped<IValidator<CreateUserDto>, CreateUserDtoValidator>();
builder.Services.AddScoped<IValidator<SigninUserDto>, SigninUserDtoValidator>();
builder.Services.AddScoped<IValidator<AddEstateDto>, AddEstateDtoValidator>();

builder.Services.AddAutoMapper(typeof(IServiceRegistration));

builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/auth/account/signin";
        options.AccessDeniedPath = "/auth/account/signin";
        options.SlidingExpiration = true;
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.UseMiddleware<AuthenticationHandlerMiddleware>();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area}/{controller=Estate}/{action=Index}/{id?}");


app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Account}/{action=Register}/{id?}");



app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();
