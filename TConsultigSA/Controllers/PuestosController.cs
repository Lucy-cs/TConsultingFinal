using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
using TConsultigSA.Models;
using TConsultigSA.Repositories;

namespace TConsultigSA.Controllers
{
    public class PuestosController : Controller
    {
        private readonly PuestoRepositorio _puestoRepositorio;

        public PuestosController(PuestoRepositorio puestoRepositorio)
        {
            _puestoRepositorio = puestoRepositorio;
        }

        public async Task<IActionResult> Index()
        {
            var puestos = await _puestoRepositorio.GetAll();
            return View(puestos);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Puesto puesto)
        {
            if (ModelState.IsValid)
            {
                await _puestoRepositorio.Add(puesto);
                return RedirectToAction(nameof(Index));
            }
            return View(puesto);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var puesto = await _puestoRepositorio.GetById(id);
            if (puesto == null)
            {
                return NotFound();
            }
            return View(puesto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Puesto puesto)
        {
            if (id != puesto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _puestoRepositorio.Update(puesto);
                return RedirectToAction(nameof(Index));
            }
            return View(puesto);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var puesto = await _puestoRepositorio.GetById(id);
            if (puesto == null)
            {
                return NotFound();
            }
            return View(puesto);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _puestoRepositorio.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch (SqlException ex)
            {
                // Verifica si el error es por conflicto de restricción de clave externa
                if (ex.Number == 547) // Código de error para conflicto de clave externa en SQL Server
                {
                    TempData["ErrorMessage"] = "No se puede eliminar este puesto.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Ocurrió un error al intentar eliminar el puesto.";
                }
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
