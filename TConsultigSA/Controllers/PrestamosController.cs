using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TConsultigSA.Models;
using TConsultigSA.Repositories;

namespace TConsultigSA.Controllers
{
    public class PrestamosController : Controller
    {
        private readonly PrestamoRepositorio _prestamoRepositorio;
        private readonly EmpleadoRepositorio _empleadoRepositorio;
        private readonly TipoPrestamoRepositorio _tipoPrestamoRepositorio;

        // Inyección de dependencias a través del constructor
        public PrestamosController(PrestamoRepositorio prestamoRepositorio,
                                   EmpleadoRepositorio empleadoRepositorio,
                                   TipoPrestamoRepositorio tipoPrestamoRepositorio)
        {
            _prestamoRepositorio = prestamoRepositorio;
            _empleadoRepositorio = empleadoRepositorio;
            _tipoPrestamoRepositorio = tipoPrestamoRepositorio;
        }


        // Acción para mostrar la lista de préstamos
        public async Task<IActionResult> Index()
        {
            IEnumerable<PrestamoViewModel> prestamos = await _prestamoRepositorio.GetAll();
            return View(prestamos);
        }

        // Acción para crear un préstamo (GET)
        public async Task<IActionResult> Create()
        {
            ViewBag.Empleados = await _empleadoRepositorio.GetAll();
            ViewBag.TiposPrestamo = await _tipoPrestamoRepositorio.GetAll();
            return View();
        }

        // Acción para crear un préstamo (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Prestamo prestamo)
        {
            if (ModelState.IsValid)
            {
                await _prestamoRepositorio.Add(prestamo);
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Empleados = await _empleadoRepositorio.GetAll();
            ViewBag.TiposPrestamo = await _tipoPrestamoRepositorio.GetAll();
            return View(prestamo);
        }

        // Acción para editar un préstamo (GET)
        public async Task<IActionResult> Edit(int id)
        {
            var prestamo = await _prestamoRepositorio.GetById(id);
            if (prestamo == null)
            {
                return NotFound();
            }

            ViewBag.Empleados = await _empleadoRepositorio.GetAll();
            ViewBag.TiposPrestamo = await _tipoPrestamoRepositorio.GetAll();
            return View(prestamo);
        }

        // Acción para editar un préstamo (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Prestamo prestamo)
        {
            if (id != prestamo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _prestamoRepositorio.Update(prestamo);
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Empleados = await _empleadoRepositorio.GetAll();
            ViewBag.TiposPrestamo = await _tipoPrestamoRepositorio.GetAll();
            return View(prestamo);
        }

        // Acción para eliminar un préstamo (GET)
        public async Task<IActionResult> Delete(int id)
        {
            var prestamo = await _prestamoRepositorio.GetById(id);
            if (prestamo == null)
            {
                return NotFound();
            }
            return View(prestamo);
        }

        // Acción para confirmar la eliminación de un préstamo (POST)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _prestamoRepositorio.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
