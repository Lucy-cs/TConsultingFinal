using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
using TConsultigSA.Models;
using TConsultigSA.Repositories;

namespace TConsultigSA.Controllers
{
    public class TiposPrestamoController : Controller
    {
        private readonly TipoPrestamoRepositorio _tipoPrestamoRepositorio;

        public TiposPrestamoController(TipoPrestamoRepositorio tipoPrestamoRepositorio)
        {
            _tipoPrestamoRepositorio = tipoPrestamoRepositorio;
        }

        public async Task<IActionResult> Index()
        {
            var tiposPrestamo = await _tipoPrestamoRepositorio.GetAll();
            return View(tiposPrestamo);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TipoPrestamo tipoPrestamo)
        {
            if (ModelState.IsValid)
            {
                await _tipoPrestamoRepositorio.Add(tipoPrestamo);
                return RedirectToAction(nameof(Index));
            }

            return View(tipoPrestamo);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var tipoPrestamo = await _tipoPrestamoRepositorio.GetById(id);
            if (tipoPrestamo == null)
            {
                return NotFound();
            }

            return View(tipoPrestamo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TipoPrestamo tipoPrestamo)
        {
            if (id != tipoPrestamo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _tipoPrestamoRepositorio.Update(tipoPrestamo);
                return RedirectToAction(nameof(Index));
            }

            return View(tipoPrestamo);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var tipoPrestamo = await _tipoPrestamoRepositorio.GetById(id);
            if (tipoPrestamo == null)
            {
                return NotFound();
            }
            return View(tipoPrestamo);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _tipoPrestamoRepositorio.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch (SqlException ex)
            {
                // Verifica si el error es por conflicto de restricción de clave externa
                if (ex.Number == 547) // Código de error para conflicto de clave externa en SQL Server
                {
                    TempData["ErrorMessage"] = "No se puede eliminar este tipo de préstamo.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Ocurrió un error al intentar eliminar el tipo de préstamo.";
                }
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
