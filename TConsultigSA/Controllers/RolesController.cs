using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
using TConsultigSA.Models;
using TConsultigSA.Repositories;

namespace TConsultigSA.Controllers
{
    public class RolesController : Controller
    {
        private readonly IRolRepositorio _rolRepositorio;

        public RolesController(IRolRepositorio rolRepositorio)
        {
            _rolRepositorio = rolRepositorio;
        }

        public async Task<IActionResult> Index()
        {
            var roles = await _rolRepositorio.GetAll();
            return View(roles);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Rol rol)
        {
            if (ModelState.IsValid)
            {
                await _rolRepositorio.Add(rol);
                return RedirectToAction(nameof(Index));
            }
            return View(rol);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var rol = await _rolRepositorio.GetById(id);
            if (rol == null)
            {
                return NotFound();
            }
            return View(rol);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Rol rol)
        {
            if (id != rol.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _rolRepositorio.Update(rol);
                return RedirectToAction(nameof(Index));
            }
            return View(rol);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var rol = await _rolRepositorio.GetById(id);
            if (rol == null)
            {
                return NotFound();
            }
            return View(rol);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _rolRepositorio.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch (SqlException ex)
            {
                // Verifica si el error es por conflicto de restricción de clave externa
                if (ex.Number == 547) // Código de error para conflicto de clave externa en SQL Server
                {
                    TempData["ErrorMessage"] = "No se puede eliminar el rol porque tiene empleados asociados.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Ocurrió un error al intentar eliminar el rol.";
                }
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
