using Inventario_banco_api.Service;
using Microsoft.AspNetCore.Mvc;

namespace Inventario_banco_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnalisisController : ControllerBase
    {
        private readonly PythonService _python;

        public AnalisisController(PythonService python)
        {
            _python = python;
        }

        [HttpGet]
        public async Task<IActionResult> Obtener()
        {
            var data = await _python.ObtenerAnalisis();
            return Ok(data);
        }

        [HttpGet("exportar-pdf")]
        public async Task<IActionResult> ExportarPdf()
        {
            var bytes = await _python.ExportarPdf();
            return File(bytes, "application/pdf", "inventario_equipos.pdf");
        }

        [HttpGet("exportar-excel")]
        public async Task<IActionResult> ExportarExcel()
        {
            var bytes = await _python.ExportarExcel();
            // los bytes se envían como un archivo descargable con el tipo MIME adecuado para Excel
            return File(
                bytes,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "inventario_equipos.xlsx"
            );
        }
    }
}