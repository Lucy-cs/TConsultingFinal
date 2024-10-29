using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
using TConsultigSA.Models;
using TConsultigSA.Repositories;

namespace TConsultigSA.Controllers
{
    public class EmpresasController : Controller
    {
        private readonly IEmpresaRepositorio _empresaRepositorio;

        // Constructor que inyecta la interfaz IEmpresaRepositorio
        public EmpresasController(IEmpresaRepositorio empresaRepositorio)
        {
            _empresaRepositorio = empresaRepositorio;
        }

        // Acción para listar todas las empresas
        public async Task<IActionResult> Index()
        {
            var empresas = await _empresaRepositorio.GetAllEmpresasAsync();
            return View(empresas);  // Verifica que la vista existe en /Views/Empresas/Index.cshtml
        }

        // Acción GET para mostrar el formulario de creación de empresa
        public IActionResult Create()
        {
            return View();  // Verifica que la vista existe en /Views/Empresas/Create.cshtml
        }

        // Acción POST para crear una nueva empresa
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Empresa empresa)
        {
            if (ModelState.IsValid)
            {
                await _empresaRepositorio.AddEmpresaAsync(empresa);
                return RedirectToAction(nameof(Index));  // Redirige al índice después de crear la empresa
            }
            return View(empresa);  // Si la validación falla, vuelve a mostrar el formulario con los errores
        }

        // Acción GET para editar una empresa
        public async Task<IActionResult> Edit(int id)
        {
            var empresa = await _empresaRepositorio.GetEmpresaByIdAsync(id);
            if (empresa == null)
            {
                return NotFound();  // Devuelve un error 404 si no se encuentra la empresa
            }
            return View(empresa);  // Verifica que la vista existe en /Views/Empresas/Edit.cshtml
        }

        // Acción POST para actualizar una empresa existente
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Empresa empresa)
        {
            if (id != empresa.Id)
            {
                return NotFound();  // Verifica si el ID coincide con la empresa que se está editando
            }

            if (ModelState.IsValid)
            {
                await _empresaRepositorio.UpdateEmpresaAsync(empresa);
                return RedirectToAction(nameof(Index));  // Redirige al índice después de actualizar la empresa
            }
            return View(empresa);  // Si la validación falla, vuelve a mostrar el formulario con los errores
        }

        // Acción GET para confirmar la eliminación de una empresa
        public async Task<IActionResult> Delete(int id)
        {
            var empresa = await _empresaRepositorio.GetEmpresaByIdAsync(id);
            if (empresa == null)
            {
                return NotFound();
            }
            return View(empresa);
        }

        // Acción POST para eliminar una empresa
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _empresaRepositorio.DeleteEmpresaAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (SqlException ex) when (ex.Number == 547)
            {
                TempData["ErrorMessage"] = "No se puede eliminar la empresa porque tiene registros asociados.";
                return RedirectToAction(nameof(Index));
            }
            catch (SqlException)
            {
                TempData["ErrorMessage"] = "Ocurrió un error al intentar eliminar la empresa.";
                return RedirectToAction(nameof(Index));
            }
        }

    }
}
