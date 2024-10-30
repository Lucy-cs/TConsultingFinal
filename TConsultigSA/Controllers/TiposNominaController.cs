using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TConsultigSA.Models;
using TConsultigSA.Repositories;

namespace TConsultigSA.Controllers
{
    public class TiposNominaController : Controller
    {
        private readonly TipoNominaRepositorio _tipoNominaRepositorio;

        // Inyección del repositorio en el constructor
        public TiposNominaController(TipoNominaRepositorio tipoNominaRepositorio)
        {
            _tipoNominaRepositorio = tipoNominaRepositorio;
        }

        // Acción para listar todos los tipos de nómina
        public async Task<IActionResult> Index()
        {
            var tiposNomina = await _tipoNominaRepositorio.GetAll();
            return View(tiposNomina);
        }

        // Acción para crear un nuevo tipo de nómina (GET)
        public IActionResult Create()
        {
            return View();
        }

        // Acción para crear un nuevo tipo de nómina (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TipoNomina tipoNomina)
        {
            if (ModelState.IsValid)
            {
                await _tipoNominaRepositorio.Add(tipoNomina);
                return RedirectToAction(nameof(Index));
            }

            return View(tipoNomina);
        }

        // Acción para editar un tipo de nómina (GET)
        public async Task<IActionResult> Edit(int id)
        {
            var tipoNomina = await _tipoNominaRepositorio.GetById(id);
            if (tipoNomina == null)
            {
                return NotFound();
            }
            return View(tipoNomina);
        }

        // Acción para editar un tipo de nómina (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TipoNomina tipoNomina)
        {
            if (id != tipoNomina.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _tipoNominaRepositorio.Update(tipoNomina);
                return RedirectToAction(nameof(Index));
            }

            return View(tipoNomina);
        }

        // Acción para eliminar un tipo de nómina (GET)
        public async Task<IActionResult> Delete(int id)
        {
            var tipoNomina = await _tipoNominaRepositorio.GetById(id);
            if (tipoNomina == null)
            {
                return NotFound();
            }
            return View(tipoNomina);
        }

        // Confirmación para eliminar un tipo de nómina (POST)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _tipoNominaRepositorio.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
