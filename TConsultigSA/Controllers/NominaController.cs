using Microsoft.AspNetCore.Mvc;
using TConsultigSA.Services;
using TConsultigSA.Repositories;
using System.IO; // Necesario para MemoryStream
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using SelectPdf;

namespace TConsultigSA.Controllers
{
    public class NominaController : Controller
    {
        private readonly NominaService _nominaService;
        private readonly EmpleadoRepositorio _empleadoRepositorio;
        private readonly ICompositeViewEngine _viewEngine;

        // Modificamos el constructor para inyectar ICompositeViewEngine
        public NominaController(NominaService nominaService, EmpleadoRepositorio empleadoRepositorio, ICompositeViewEngine viewEngine)
        {
            _nominaService = nominaService;
            _empleadoRepositorio = empleadoRepositorio;
            _viewEngine = viewEngine;
        }

        public async Task<IActionResult> CalcularNomina()
        {
            var empleados = await _empleadoRepositorio.GetAll();
            ViewBag.Empleados = empleados;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CalcularNomina(int idEmpleado, int mes, int año)
        {
            try
            {
                var resultado = await _nominaService.CalcularNominaParaEmpleado(idEmpleado, mes, año);
                return View("ResultadoNomina", resultado);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                var empleados = await _empleadoRepositorio.GetAll();
                ViewBag.Empleados = empleados;
                return View();
            }
        }

        // Nuevo método para generar el PDF
        [HttpPost]
        public async Task<IActionResult> GenerarReciboNomina(int IdEmpleado, int Mes, int Año)
        {
            try
            {
                var resultado = await _nominaService.CalcularNominaParaEmpleado(IdEmpleado, Mes, Año);

                // Renderizar la vista como HTML
                string htmlString = await RenderViewAsString("ReciboNominaPDF", resultado);

                // Configurar SelectPdf para convertir HTML a PDF
                HtmlToPdf converter = new HtmlToPdf();
                PdfDocument doc = converter.ConvertHtmlString(htmlString);

                // Guardar el documento en un MemoryStream
                MemoryStream pdfStream = new MemoryStream();
                doc.Save(pdfStream);
                doc.Close();

                pdfStream.Position = 0;

                // Devolver el PDF como un archivo descargable
                return File(pdfStream, "application/pdf", $"ReciboNomina_{resultado.NombreEmpleado}_{Mes}_{Año}.pdf");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                var empleados = await _empleadoRepositorio.GetAll();
                ViewBag.Empleados = empleados;
                return View("CalcularNomina");
            }
        }

        // Método auxiliar para renderizar la vista como string
        private async Task<string> RenderViewAsString(string viewName, object model)
        {
            ViewData.Model = model;

            using (var sw = new StringWriter())
            {
                ViewEngineResult viewResult = _viewEngine.FindView(ControllerContext, viewName, false);

                if (viewResult.View == null)
                {
                    throw new ArgumentNullException($"{viewName} no se encontró.");
                }

                ViewContext viewContext = new ViewContext(
                    ControllerContext,
                    viewResult.View,
                    ViewData,
                    TempData,
                    sw,
                    new HtmlHelperOptions()
                );

                await viewResult.View.RenderAsync(viewContext);
                return sw.GetStringBuilder().ToString();
            }
        }
    }
}
