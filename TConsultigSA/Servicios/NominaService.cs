using TConsultigSA.Models;
using TConsultigSA.Repositories;
using TConsultingSA.Repositories;

namespace TConsultigSA.Services
{
    public class NominaService
    {
        private readonly EmpleadoRepositorio _empleadoRepositorio;
        private readonly IHorasTrabajoRepositorio _horasTrabajoRepositorio;
        private readonly AusenciaRepositorio _ausenciaRepositorio;
        private readonly PrestamoRepositorio _prestamoRepositorio;

        public NominaService(
            EmpleadoRepositorio empleadoRepositorio,
            IHorasTrabajoRepositorio horasTrabajoRepositorio,
            AusenciaRepositorio ausenciaRepositorio,
            PrestamoRepositorio prestamoRepositorio)
        {
            _empleadoRepositorio = empleadoRepositorio;
            _horasTrabajoRepositorio = horasTrabajoRepositorio;
            _ausenciaRepositorio = ausenciaRepositorio;
            _prestamoRepositorio = prestamoRepositorio;
        }

        public async Task<NominaResultado> CalcularNominaParaEmpleado(int idEmpleado, int mes, int año)
        {
            // Paso 1: Obtener datos básicos del empleado
            var empleado = await _empleadoRepositorio.GetById(idEmpleado);
            if (empleado == null)
            {
                throw new Exception("Empleado no encontrado.");
            }

            // Salario base
            decimal salarioBase = empleado.Salario;

            // Paso 2: Obtener horas trabajadas y calcular horas extras
            var horasTrabajadas = await _horasTrabajoRepositorio.ObtenerHorasPorEmpleadoYMes(idEmpleado, mes, año);
            decimal totalHorasTrabajadas = horasTrabajadas.Sum(h => h.TotalHoras);

            // Calcular horas extras (máximo 2 horas extras por día)
            decimal horasExtras = CalcularHorasExtras(horasTrabajadas);

            // Paso 3: Calcular descuentos por ausencias no autorizadas
            var ausencias = await _ausenciaRepositorio.ObtenerAusenciasPorEmpleadoYMes(idEmpleado, mes, año);
            int diasAusentes = ausencias.Where(a => !a.Autorizado).Sum(a => a.TotalDias);

            // Calcular descuento por ausencias
            decimal descuentoAusencias = (salarioBase / 30) * diasAusentes;

            // Paso 4: Calcular deducciones por préstamos
            var prestamos = await _prestamoRepositorio.ObtenerPrestamosActivosPorEmpleado(idEmpleado);
            decimal totalDeduccionesPrestamos = prestamos.Sum(p => p.Total / p.CuotasPendientes);

            // Paso 5: Calcular salario final
            decimal salarioBruto = salarioBase + (horasExtras * (empleado.Salario / 160) * 1.5m) + 250m;
            decimal salarioNeto = salarioBruto - descuentoAusencias - totalDeduccionesPrestamos;

            // Crear objeto de resultado
            var resultado = new NominaResultado
            {
                IdEmpleado = idEmpleado,
                NombreEmpleado = empleado.Nombre,
                Mes = mes,
                Año = año,
                SalarioBase = salarioBase,
                HorasTrabajadas = totalHorasTrabajadas,
                HorasExtras = horasExtras,
                Bonificacion = 250m,
                DescuentoAusencias = descuentoAusencias,
                DeduccionesPrestamos = totalDeduccionesPrestamos,
                SalarioBruto = salarioBruto,
                SalarioNeto = salarioNeto
            };

            return resultado;
        }

        private decimal CalcularHorasExtras(IEnumerable<HorasTrabajo> horasTrabajadas)
        {
            decimal horasExtrasTotales = 0;
            var horasPorDia = horasTrabajadas.GroupBy(h => h.Fecha.Date);

            foreach (var dia in horasPorDia)
            {
                decimal horasDelDia = dia.Sum(h => h.TotalHoras);
                if (horasDelDia > 8)
                {
                    // Máximo 2 horas extras por día
                    decimal horasExtrasDia = Math.Min(horasDelDia - 8, 2);
                    horasExtrasTotales += horasExtrasDia;
                }
            }

            return horasExtrasTotales;
        }
    }
}
