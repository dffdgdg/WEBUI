using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebUI.Migrations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<NewsDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login"; // Путь к странице входа
        options.AccessDeniedPath = "/AccessDenied"; // Путь к странице отказа в доступе
    });

    // Добавление авторизации
builder.Services.AddAuthorization();

var app = builder.Build();

// Обработка ошибок 404
app.UseStatusCodePagesWithReExecute("/error/404");

// Обработка общих ошибок
app.UseExceptionHandler("/error");

app.UseHsts();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Включение аутентификации и авторизации
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "login",
    pattern: "Login",
    defaults: new { controller = "Users", action = "Authentication" });
app.MapControllerRoute(
    name: "accessDenied",
    pattern: "AccessDenied",
    defaults: new { controller = "Users", action = "AccessDenied" });
app.MapControllerRoute(
    name: "logout",
    pattern: "Logout",
    defaults: new { controller = "Users", action = "Logout" });
app.MapControllerRoute(
    name: "news",
    pattern: "News",
    defaults: new { controller = "News", action = "Index" });
app.MapControllerRoute(
    name: "create",
    pattern: "Create",
    defaults: new { controller = "News", action = "Create" });
app.MapControllerRoute(
    name: "register",
    pattern: "Register",
    defaults: new { controller = "Users", action = "Register" });
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=News}/{action=Index}/{id?}");
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});
app.Run();