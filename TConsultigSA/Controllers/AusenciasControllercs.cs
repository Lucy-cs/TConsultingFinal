using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TConsultigSA.Models;
using TConsultigSA.Repositories;
using System;

namespace TConsultigSA.Controllers
{
    public class AusenciasController : Controller
    {
        private readonly AusenciaRepositorio _ausenciaRepositorio;
        private readonly EmpleadoRepositorio _empleadoRepositorio;  // Necesario para mostrar empleados

        public AusenciasController(AusenciaRepositorio ausenciaRepositorio, EmpleadoRepositorio empleadoRepositorio)
        {
            _ausenciaRepositorio = ausenciaRepositorio;
            _empleadoRepositorio = empleadoRepositorio;
        }

        // Listar ausencias
        public async Task<IActionResult> Index()
        {
            var ausencias = await _ausenciaRepositorio.GetAll();
            return View(ausencias);
        }

        // Crear nueva ausencia
        public async Task<IActionResult> Create()
        {
            ViewBag.Empleados = await _empleadoRepositorio.GetAll();  // Para seleccionar empleado
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Ausencia ausencia)
        {
            if (ModelState.IsValid)
            {
                // Calcula los días totales entre las fechas
                ausencia.TotalDias = (ausencia.FechaFin - ausencia.FechaInicio).Days;
                ausencia.FechaSolicitud = DateTime.Now; // Asegúrate de que FechaSolicitud tenga un valor

                await _ausenciaRepositorio.Add(ausencia);
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Empleados = await _empleadoRepositorio.GetAll();
            return View(ausencia);
        }

        // Editar una ausencia existente
        public async Task<IActionResult> Edit(int id)
        {
            var ausencia = await _ausenciaRepositorio.GetById(id);
            if (ausencia == null)
            {
                return NotFound();
            }

            ViewBag.Empleados = await _empleadoRepositorio.GetAll();
            return View(ausencia);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Ausencia ausencia)
        {
            if (id != ausencia.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                ausencia.TotalDias = (ausencia.FechaFin - ausencia.FechaInicio).Days;
                await _ausenciaRepositorio.Update(ausencia);
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Empleados = await _empleadoRepositorio.GetAll();
            return View(ausencia);
        }

        // Mostrar el formulario de confirmación de eliminación
        public async Task<IActionResult> Delete(int id)
        {
            var ausencia = await _ausenciaRepositorio.GetById(id);
            if (ausencia == null)
            {
                return NotFound();
            }
            return View(ausencia);
        }

        // Confirmar eliminación
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _ausenciaRepositorio.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "No se puede eliminar la ausencia porque tiene registros dependientes o está en uso.";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
