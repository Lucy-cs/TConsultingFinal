using TConsultigSA;
using Microsoft.Data.SqlClient;
using Dapper;
using BCrypt.Net;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.Cookies;
using TConsultigSA.Repositories;
using TConsultingSA.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Configuraci�n de servicios y dependencias
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

builder.Services.AddScoped<EmpleadoRepositorio>();
builder.Services.AddScoped<PuestoRepositorio>();
builder.Services.AddScoped<DepartamentoRepositorio>();
builder.Services.AddScoped<UsuarioRepositorio>();
builder.Services.AddScoped<IRolRepositorio, RolRepositorio>();
builder.Services.AddScoped<IPermisoRepositorio, PermisoRepositorio>();
builder.Services.AddScoped<IHorasTrabajoRepositorio, HorasTrabajoRepositorio>();
builder.Services.AddScoped<IEmpresaRepositorio, EmpresaRepositorio>();
builder.Services.AddScoped<PuestoRepositorio>();
builder.Services.AddScoped<AusenciaRepositorio>();
builder.Services.AddScoped<PrestamoRepositorio>(); // Registro para el servicio de pr�stamos
builder.Services.AddScoped<TipoPrestamoRepositorio>();    // Repositorio de tipos de pr�stamo


builder.Services.AddScoped<AusenciaRepositorio>();

// Configuraci�n de autenticaci�n con cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";
        options.LogoutPath = "/Auth/Logout";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        options.SlidingExpiration = true;
    });

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Autenticaci�n y autorizaci�n
app.UseAuthentication();
app.UseAuthorization();

// Configuraci�n de la ruta predeterminada
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Welcome}/{id?}"); // Cambiar la ruta predeterminada a Welcome

app.Run();
