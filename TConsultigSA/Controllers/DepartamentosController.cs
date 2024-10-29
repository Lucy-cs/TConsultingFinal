using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TConsultigSA.Models;
using TConsultigSA.Repositories;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace TConsultigSA.Controllers
{
    public class DepartamentosController : Controller
    {
        private readonly DepartamentoRepositorio _departamentoRepositorio;
        private readonly EmpleadoRepositorio _empleadoRepositorio;
        private readonly IEmpresaRepositorio _empresaRepositorio;

        public DepartamentosController(DepartamentoRepositorio departamentoRepositorio,
                                       EmpleadoRepositorio empleadoRepositorio,
                                       IEmpresaRepositorio empresaRepositorio)
        {
            _departamentoRepositorio = departamentoRepositorio;
            _empleadoRepositorio = empleadoRepositorio;
            _empresaRepositorio = empresaRepositorio;
        }

        public async Task<IActionResult> Index()
        {
            var departamentos = await _departamentoRepositorio.GetAll();
            var viewModelList = new List<DepartamentoViewModel>();

            foreach (var departamento in departamentos)
            {
                var empresa = await _empresaRepositorio.GetEmpresaByIdAsync(departamento.IdEmpresa);
                var lider = await _empleadoRepositorio.GetById(departamento.IdLider ?? 0);

                var viewModel = new DepartamentoViewModel
                {
                    Id = departamento.Id,
                    DepartamentoNombre = departamento.DepartamentoNombre,
                    EmpresaNombre = empresa?.Nombre ?? "N/A",
                    LiderNombre = lider?.Nombre ?? "Sin Líder"
                };

                viewModelList.Add(viewModel);
            }

            return View(viewModelList);
        }

        public async Task<IActionResult> Create()
        {
            var empleados = await _empleadoRepositorio.GetAll();
            var empresas = await _empresaRepositorio.GetAllEmpresasAsync();

            ViewBag.Empleados = empleados.Select(e => new SelectListItem
            {
                Value = e.Id.ToString(),
                Text = e.Nombre
            }).ToList();

            ViewBag.Empresas = empresas.Select(e => new SelectListItem
            {
                Value = e.Id.ToString(),
                Text = e.Nombre
            }).ToList();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Departamento departamento)
        {
            if (ModelState.IsValid)
            {
                await _departamentoRepositorio.Add(departamento);
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Empleados = (await _empleadoRepositorio.GetAll())
                .Select(e => new SelectListItem { Value = e.Id.ToString(), Text = e.Nombre })
                .ToList();

            ViewBag.Empresas = (await _empresaRepositorio.GetAllEmpresasAsync())
                .Select(e => new SelectListItem { Value = e.Id.ToString(), Text = e.Nombre })
                .ToList();

            return View(departamento);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var departamento = await _departamentoRepositorio.GetById(id);
            if (departamento == null)
            {
                return NotFound();
            }

            ViewBag.Empleados = (await _empleadoRepositorio.GetAll()).Select(e => new SelectListItem
            {
                Value = e.Id.ToString(),
                Text = e.Nombre
            }).ToList();

            ViewBag.Empresas = (await _empresaRepositorio.GetAllEmpresasAsync()).Select(e => new SelectListItem
            {
                Value = e.Id.ToString(),
                Text = e.Nombre
            }).ToList();

            return View(departamento);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Departamento departamento)
        {
            if (id != departamento.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _departamentoRepositorio.Update(departamento);
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Empleados = (await _empleadoRepositorio.GetAll()).Select(e => new SelectListItem
            {
                Value = e.Id.ToString(),
                Text = e.Nombre
            }).ToList();

            ViewBag.Empresas = (await _empresaRepositorio.GetAllEmpresasAsync()).Select(e => new SelectListItem
            {
                Value = e.Id.ToString(),
                Text = e.Nombre
            }).ToList();

            return View(departamento);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var departamento = await _departamentoRepositorio.GetById(id);
            if (departamento == null)
            {
                return NotFound();
            }
            return View(departamento);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _departamentoRepositorio.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch (SqlException ex)
            {
                // Verifica si el error es por conflicto de restricción de clave externa
                if (ex.Number == 547) // Código de error para conflicto de clave externa en SQL Server
                {
                    TempData["ErrorMessage"] = "No se puede eliminar el departamento porque tiene empleados asociados.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Ocurrió un error al intentar eliminar el departamento.";
                }
                return RedirectToAction(nameof(Index));
            }
        }

        private async Task CargarLideresYEmpresas()
        {
            var empleados = await _empleadoRepositorio.GetAll();
            var empresas = await _empresaRepositorio.GetAllEmpresasAsync();

            ViewBag.Empleados = empleados.Select(e => new SelectListItem
            {
                Value = e.Id.ToString(),
                Text = e.Nombre
            }).ToList();

            ViewBag.Empresas = empresas.Select(e => new SelectListItem
            {
                Value = e.Id.ToString(),
                Text = e.Nombre
            }).ToList();
        }
    }
}
