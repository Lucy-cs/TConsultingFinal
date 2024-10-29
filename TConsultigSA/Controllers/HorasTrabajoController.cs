using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TConsultigSA.Models;
using TConsultigSA.Repositories;
using Microsoft.AspNetCore.Mvc.Rendering;
using TConsultingSA.Repositories;

namespace TConsultigSA.Controllers
{
    public class HorasTrabajoController : Controller
    {
        private readonly IHorasTrabajoRepositorio _horasRepositorio;
        private readonly EmpleadoRepositorio _empleadoRepositorio;

        public HorasTrabajoController(IHorasTrabajoRepositorio horasRepositorio, EmpleadoRepositorio empleadoRepositorio)
        {
            _horasRepositorio = horasRepositorio;
            _empleadoRepositorio = empleadoRepositorio;
        }

        public async Task<IActionResult> Index()
        {
            var horas = await _horasRepositorio.GetAll();
            return View(horas);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Empleados = new SelectList(await _empleadoRepositorio.GetAll(), "Id", "Nombre");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(HorasTrabajo horasTrabajo)
        {
            // Validar si el empleado existe
            var empleadoExiste = await _empleadoRepositorio.GetById(horasTrabajo.IdEmpleado) != null;
            if (!empleadoExiste)
            {
                ModelState.AddModelError("IdEmpleado", "El empleado seleccionado no existe.");
            }

            // Validar rango de fecha para SQL Server
            if (horasTrabajo.Fecha < new DateTime(1753, 1, 1) || horasTrabajo.Fecha > new DateTime(9999, 12, 31))
            {
                ModelState.AddModelError("Fecha", "La fecha debe estar entre 01/01/1753 y 31/12/9999.");
            }

            if (ModelState.IsValid)
            {
                await _horasRepositorio.Add(horasTrabajo);
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Empleados = new SelectList(await _empleadoRepositorio.GetAll(), "Id", "Nombre");
            return View(horasTrabajo);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var horasTrabajo = await _horasRepositorio.GetById(id);
            if (horasTrabajo == null)
            {
                return NotFound();
            }

            // Mostrar el nombre del empleado en lugar de un select
            ViewBag.EmpleadoNombre = horasTrabajo.Empleado?.Nombre;
            ViewBag.IdEmpleado = horasTrabajo.IdEmpleado; // Incluimos el IdEmpleado para el ViewBag

            return View(horasTrabajo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, HorasTrabajo horasTrabajo)
        {
            if (id != horasTrabajo.Id)
            {
                return NotFound();
            }

            // Verificar que el empleado no cambie y validar si existe
            var empleado = await _empleadoRepositorio.GetById(horasTrabajo.IdEmpleado);
            if (empleado == null)
            {
                ModelState.AddModelError("IdEmpleado", "El empleado seleccionado no existe.");
            }

            // Validar rango de fecha para SQL Server
            if (horasTrabajo.Fecha < new DateTime(1753, 1, 1) || horasTrabajo.Fecha > new DateTime(9999, 12, 31))
            {
                ModelState.AddModelError("Fecha", "La fecha debe estar entre 01/01/1753 y 31/12/9999.");
            }

            if (ModelState.IsValid)
            {
                await _horasRepositorio.Update(horasTrabajo);
                return RedirectToAction(nameof(Index));
            }

            // **MANTENEMOS el nombre del empleado en caso de error de validación**
            ViewBag.EmpleadoNombre = empleado?.Nombre;  // Esta línea asegura que el nombre se mantenga en la vista
            return View(horasTrabajo);
        }


        public async Task<IActionResult> Details(int id)
        {
            var horasTrabajo = await _horasRepositorio.GetById(id);
            if (horasTrabajo == null)
            {
                return NotFound();
            }

            // Calcular horas extras (si es más de 160 horas al mes)
            const int horasSemanales = 40;
            const int semanasPorMes = 4;
            var horasNormales = horasSemanales * semanasPorMes;
            var horasExtras = horasTrabajo.TotalHoras > horasNormales ? horasTrabajo.TotalHoras - horasNormales : 0;

            var viewModel = new DetallesHorasViewModel
            {
                NombreEmpleado = horasTrabajo.Empleado?.Nombre,
                Mes = horasTrabajo.Fecha.ToString("MM/yyyy"),
                HorasTrabajadas = horasTrabajo.TotalHoras,
                HorasExtras = horasExtras,
                Observaciones = horasTrabajo.Observaciones,
                Estado = horasTrabajo.Aprobado ? "Aprobado" : "Pendiente"
            };

            return PartialView("_DetallesHorasModal", viewModel);
        }

        // Método para mostrar la confirmación de eliminación de horas trabajadas
        public async Task<IActionResult> Delete(int id)
        {
            var horasTrabajo = await _horasRepositorio.GetById(id);
            if (horasTrabajo == null)
            {
                return NotFound();  // Error 404 si no se encuentra el registro
            }
            return View(horasTrabajo);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var horasTrabajo = await _horasRepositorio.GetById(id);
            if (horasTrabajo == null)
            {
                return NotFound();
            }

            await _horasRepositorio.Delete(id);
            return RedirectToAction(nameof(Index));  // Redirige a la página principal después de eliminar
        }

    }
}
