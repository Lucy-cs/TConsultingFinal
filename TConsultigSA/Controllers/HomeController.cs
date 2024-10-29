using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TConsultigSA.Models;
using Microsoft.AspNetCore.Authorization;

namespace TConsultigSA.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // Acción para la página principal
        [Authorize]
        public IActionResult Index()
        {
            // Muestra la vista Index solo si el usuario está autenticado
            return View();
        }

        // Acción para la página de bienvenida
        [AllowAnonymous]
        public IActionResult Welcome()
        {
            return View();
        }

        // Acción para la página de privacidad
        public IActionResult Privacy()
        {
            return View();
        }

        // Acción para la página de error
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
