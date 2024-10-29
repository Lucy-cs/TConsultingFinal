using Microsoft.AspNetCore.Mvc;
using TConsultigSA.Models;
using TConsultigSA.Repositories;
using BCrypt.Net;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

public class AuthController : Controller
{
    private readonly UsuarioRepositorio _usuarioRepositorio;

    public AuthController(UsuarioRepositorio usuarioRepositorio)
    {
        _usuarioRepositorio = usuarioRepositorio;
    }

    // Acción para mostrar el formulario de registro (GET)
    [AllowAnonymous]
    public IActionResult Registro()
    {
        return View();
    }

    // Acción para manejar el POST del registro (POST)
    [HttpPost]
    [ValidateAntiForgeryToken]
    [AllowAnonymous]
    public async Task<IActionResult> Registro(Usuario usuario)
    {
        if (ModelState.IsValid)
        {
            // Verificar si el usuario ya existe
            var usuarioExistente = await _usuarioRepositorio.ObtenerPorNombre(usuario.Nombre);
            if (usuarioExistente != null)
            {
                ModelState.AddModelError("", "El nombre de usuario ya está registrado.");
                return View(usuario);
            }

            // Hashear la contraseña antes de guardarla
            usuario.Contrasenia = BCrypt.Net.BCrypt.HashPassword(usuario.Contrasenia);
            usuario.IdRol = 2; // Asignar un rol por defecto, por ejemplo, 2 para "Usuario"

            await _usuarioRepositorio.Registrar(usuario);
            return RedirectToAction("Login");
        }

        return View(usuario);
    }

    // Acción para mostrar el formulario de login (GET)
    [AllowAnonymous]
    public IActionResult Login()
    {
        return View();
    }

    // Acción para manejar el POST del login (POST)
    [HttpPost]
    [ValidateAntiForgeryToken]
    [AllowAnonymous]
    public async Task<IActionResult> Login(string nombre, string contrasenia)
    {
        if (ModelState.IsValid)
        {
            var usuario = await _usuarioRepositorio.ObtenerPorNombre(nombre);
            if (usuario != null && BCrypt.Net.BCrypt.Verify(contrasenia, usuario.Contrasenia))
            {
                // Crear claims para el usuario
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, usuario.Nombre),
                    new Claim(ClaimTypes.Role, usuario.IdRol.ToString())
                };

                // Crear el principal
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true
                };

                // Iniciar sesión con las credenciales del usuario
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                // Redirigir al Home/Index
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Nombre de usuario o contraseña incorrectos.");
        }

        return View();
    }

    // Acción para cerrar sesión
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login");
    }
}
