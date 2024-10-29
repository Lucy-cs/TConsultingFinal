using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TConsultigSA.Models;
using TConsultigSA.Repositories;

namespace TConsultigSA.Controllers
{
    public class PermisosController : Controller
    {
        private readonly IPermisoRepositorio _permisoRepositorio;

        public PermisosController(IPermisoRepositorio permisoRepositorio)
        {
            _permisoRepositorio = permisoRepositorio;
        }

        // Acción para mostrar la lista de permisos
        public async Task<IActionResult> Index()
        {
            var permisos = await _permisoRepositorio.GetAll();
            return View(permisos);
        }

        // Acción para mostrar el formulario de creación (GET)
        public IActionResult Create()
        {
            return View();
        }

        // Acción para manejar el POST del formulario de creación (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Permiso permiso)
        {
            if (ModelState.IsValid)
            {
                await _permisoRepositorio.Add(permiso);
                return RedirectToAction(nameof(Index));
            }
            return View(permiso);
        }

        // Acción para mostrar el formulario de edición (GET)
        public async Task<IActionResult> Edit(int id)
        {
            var permiso = await _permisoRepositorio.GetById(id);
            if (permiso == null)
            {
                return NotFound();
            }
            return View(permiso);
        }

        // Acción para manejar el POST del formulario de edición (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Permiso permiso)
        {
            if (id != permiso.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _permisoRepositorio.Update(permiso);
                return RedirectToAction(nameof(Index));
            }
            return View(permiso);
        }

        // Acción para confirmar la eliminación
        public async Task<IActionResult> Delete(int id)
        {
            var permiso = await _permisoRepositorio.GetById(id);
            if (permiso == null)
            {
                return NotFound();
            }
            return View(permiso);
        }

        // Acción para manejar la eliminación
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _permisoRepositorio.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
