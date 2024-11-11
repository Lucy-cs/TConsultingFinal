using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using TConsultigSA.Models;
using TConsultigSA.Repositories;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using TConsultigSA.Services;

namespace TConsultigSA.Controllers
{
    public class EmpleadosController : Controller
    {
        private readonly EmpleadoRepositorio _empleadoRepositorio;
        private readonly PuestoRepositorio _puestoRepositorio;  // Repositorio de puestos
        private readonly DepartamentoRepositorio _departamentoRepositorio;  // Repositorio de departamentos
        private readonly NominaService _nominaService;

        public EmpleadosController(EmpleadoRepositorio empleadoRepositorio, PuestoRepositorio puestoRepositorio, DepartamentoRepositorio departamentoRepositorio, NominaService nominaService)
        {
            _empleadoRepositorio = empleadoRepositorio;
            _puestoRepositorio = puestoRepositorio;
            _departamentoRepositorio = departamentoRepositorio;
            _nominaService = nominaService;

        }

        // Método Index con filtros
        public async Task<IActionResult> Index(string searchDPI, string searchNombre, DateTime? searchFecha, decimal? searchSalario)
        {
            var empleados = await _empleadoRepositorio.GetAll();

            // Aplicar filtros si los parámetros no son nulos o vacíos
            if (!string.IsNullOrEmpty(searchDPI))
            {
                empleados = empleados.Where(e => e.DPI.Contains(searchDPI));
            }

            if (!string.IsNullOrEmpty(searchNombre))
            {
                empleados = empleados.Where(e => e.Nombre.Contains(searchNombre, StringComparison.OrdinalIgnoreCase));
            }

            if (searchFecha.HasValue)
            {
                empleados = empleados.Where(e => e.FechaContratado.Date == searchFecha.Value.Date);
            }

            if (searchSalario.HasValue)
            {
                empleados = empleados.Where(e => e.Salario == searchSalario.Value);
            }

            // Pasar los parámetros de búsqueda actuales a ViewBag para mantenerlos en la vista después de la búsqueda
            ViewBag.SearchDPI = searchDPI;
            ViewBag.SearchNombre = searchNombre;
            ViewBag.SearchFecha = searchFecha?.ToString("yyyy-MM-dd");
            ViewBag.SearchSalario = searchSalario;

            return View(empleados);
        }

        // Método para mostrar los detalles del empleado
        public async Task<IActionResult> GetEmpleadoDetails(int id)
        {
            var empleado = await _empleadoRepositorio.GetById(id);
            if (empleado == null)
            {
                return NotFound();
            }
            return PartialView("_EmpleadoDetails", empleado);
        }

        // Métodos para agregar un nuevo empleado
        public async Task<IActionResult> Create()
        {
            await CargarPuestosYDepartamentos(); // Cargar listas de puestos y departamentos
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Empleado empleado)
        {
            if (ModelState.IsValid)
            {
                await _empleadoRepositorio.Add(empleado);
                return RedirectToAction(nameof(Index));
            }

            await CargarPuestosYDepartamentos(); // Volver a cargar las listas en caso de error
            return View(empleado);
        }

        // Métodos para editar un empleado existente
        public async Task<IActionResult> Edit(int id)
        {
            var empleado = await _empleadoRepositorio.GetById(id);
            if (empleado == null)
            {
                return NotFound();
            }

            await CargarPuestosYDepartamentos(); // Cargar listas de puestos y departamentos
            return View(empleado);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Empleado empleado)
        {
            if (id != empleado.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _empleadoRepositorio.Update(empleado);
                return RedirectToAction(nameof(Index));
            }

            await CargarPuestosYDepartamentos(); // Volver a cargar las listas en caso de error
            return View(empleado);
        }

        // Métodos para eliminar un empleado
        public async Task<IActionResult> Delete(int id)
        {
            var empleado = await _empleadoRepositorio.GetById(id);
            if (empleado == null)
            {
                return NotFound();
            }
            return View(empleado);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _empleadoRepositorio.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch (SqlException ex)
            {
                // Verifica si el error es por conflicto de restricción de clave externa
                if (ex.Number == 547) // Código de error para conflicto de clave externa en SQL Server
                {
                    TempData["ErrorMessage"] = "No se puede eliminar al empleado porque está relacionado con otros registros en el sistema.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Ocurrió un error inesperado al intentar eliminar al empleado.";
                }
                return RedirectToAction(nameof(Index));
            }
        }


        // Método para cargar la lista de puestos y departamentos en el ViewBag
        private async Task CargarPuestosYDepartamentos()
        {
            var puestos = await _puestoRepositorio.GetAll();
            var departamentos = await _departamentoRepositorio.GetAll();

            ViewBag.Puestos = puestos.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = p.Descripcion
            }).ToList();

            ViewBag.Departamentos = departamentos.Select(d => new SelectListItem
            {
                Value = d.Id.ToString(),
                Text = d.DepartamentoNombre
            }).ToList();
        }

        public async Task<IActionResult> HistorialAumentos(int idEmpleado)
        {
            var historialAumentos = await _nominaService.ObtenerHistorialAumentos(idEmpleado);
            ViewBag.NombreEmpleado = (await _nominaService.ObtenerEmpleadoPorId(idEmpleado))?.Nombre;
            return View(historialAumentos);
        }


        // Acción para mostrar la vista del formulario de aumento salarial
        public async Task<IActionResult> AgregarAumento(int idEmpleado)
        {
            var empleado = await _empleadoRepositorio.GetById(idEmpleado);
            if (empleado == null)
            {
                return NotFound();
            }

            ViewBag.NombreEmpleado = empleado.Nombre;
            ViewBag.EmpleadoId = empleado.Id;
            return View();
        }

        // Acción para registrar el aumento salarial
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegistrarAumento(int idEmpleado, decimal cantidadAumento)
        {
            if (cantidadAumento <= 0)
            {
                TempData["ErrorMessage"] = "El aumento salarial debe ser mayor que cero.";
                return RedirectToAction("AgregarAumento", new { idEmpleado });
            }

            try
            {
                await _nominaService.RegistrarAumento(idEmpleado, cantidadAumento);
                TempData["SuccessMessage"] = "Aumento salarial registrado exitosamente.";
                return RedirectToAction("HistorialAumentos", new { idEmpleado });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Ocurrió un error al registrar el aumento: {ex.Message}";
                return RedirectToAction("AgregarAumento", new { idEmpleado });
            }
        }

    }
}
