using ClosedXML.Excel;
using Inventario_banco_api.Models;
using Inventario_banco_api.Service;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Authorization;

namespace Inventario_banco_api.Controllers
{
    // aqui tuviste un error a partir de ahora usa el MVC como  ruta debido a que components no es un controlador, es una carpeta que contiene componentes de React, entonces el controlador debe ser EquipoController y la ruta api/EquipoController
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // agregado para proteger el endpoint, solo usuarios autenticados pueden acceder a los endpoints de este controlador
    public class EquipoController : ControllerBase
    {
        private readonly IEquipoService _service;
        public EquipoController(IEquipoService service)
        {
            _service = service;
        }

        // es listar p el tipico SELECT * FROM Equipos xd agregar filtros despues 
        [HttpGet]
        public IActionResult listar()
        {
            var equipos = _service.listar();
            return Ok(equipos);
        }


        // endpoint para registrar un nuevo equipo, el cuerpo de la solicitud debe contener los datos del equipo a registrar(ojo en codigo patrimonial se ingresa de manera automatica)
        [HttpPost]
        public IActionResult registrar([FromBody] Equipo equipo)
        {
            try
            {
                _service.registrar(equipo);

                return Ok(new
                {
                    mensaje = "Registrado correctamente"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    mensaje = ex.Message,
                    detalle = ex.ToString()
                });
            }
        }




        // Controller para importar de excel 20:03:c

        //Controller mejorado con la ia 20:48

        [HttpPost("importar")]
        public async Task<IActionResult> importarExcel(
        IFormFile archivo)
        {
            // VALIDAR EXISTENCIA
            if (archivo == null || archivo.Length == 0)
            {
                return BadRequest(
                    "No se seleccionó ningún archivo."
                );
            }

            // VALIDAR TAMAÑO
            // 100 MB
            if (archivo.Length > 100 * 1024 * 1024)
            {
                return BadRequest(
                    "El archivo supera el límite permitido de 100 MB."
                );
            }

            // VALIDAR EXTENSIÓN
            var extension =
                Path.GetExtension(
                    archivo.FileName
                ).ToLower();

            if (extension != ".xlsx")
            {
                return BadRequest(
                    "Solo se permiten archivos Excel .xlsx"
                );
            }

            // VALIDAR MIME TYPE
            if (archivo.ContentType !=
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                return BadRequest(
                    "Tipo de archivo inválido."
                );
            }

            int filasProcesadas = 0;
            int filasInsertadas = 0;
            int filasError = 0;

            List<string> errores =
                new List<string>();

            try
            {
                using (var stream =
                    new MemoryStream())
                {
                    await archivo.CopyToAsync(stream);

                    using (var workbook =
                        new XLWorkbook(stream))
                    {
                        var hoja =
                            workbook.Worksheet(1);

                        var filas =
                            hoja.RowsUsed().Skip(1);

                        // LIMITAR FILAS
                        if (filas.Count() > 10000)
                        {
                            return BadRequest(
                                "Máximo permitido: 10,000 filas."
                            );
                        }

                        foreach (var fila in filas)
                        {
                            filasProcesadas++;

                            try
                            {
                                // VALIDAR FILA VACÍA
                                if (string.IsNullOrWhiteSpace(
                                    fila.Cell(2)
                                    .GetValue<string>()))
                                {
                                    continue;
                                }

                                Equipo equipo =
                                    new Equipo()
                                    {
                                        CodigoPatrimonial =
                                            fila.Cell(1)
                                            .GetValue<string>()
                                            .Trim(),

                                        TipoEquipo =
                                            fila.Cell(2)
                                            .GetValue<string>()
                                            .Trim(),

                                        Marca =
                                            fila.Cell(3)
                                            .GetValue<string>()
                                            .Trim(),

                                        Modelo =
                                            fila.Cell(4)
                                            .GetValue<string>()
                                            .Trim(),

                                        NumeroSerie =
                                            fila.Cell(5)
                                            .GetValue<string>()
                                            .Trim(),

                                        EstadoEquipo =
                                            fila.Cell(6)
                                            .GetValue<string>()
                                            .Trim(),

                                        Ubicacion =
                                            fila.Cell(7)
                                            .GetValue<string>()
                                            .Trim(),

                                        UsuarioAsignado =
                                            fila.Cell(8)
                                            .GetValue<string>()
                                            .Trim()
                                    };

                                // VALIDACIONES
                                if (string.IsNullOrWhiteSpace(
                                    equipo.TipoEquipo))
                                {
                                    filasError++;

                                    errores.Add(
                                        $"Fila {filasProcesadas}: TipoEquipo vacío."
                                    );

                                    continue;
                                }

                                if (equipo.TipoEquipo.Length > 100)
                                {
                                    filasError++;

                                    errores.Add(
                                        $"Fila {filasProcesadas}: TipoEquipo demasiado largo."
                                    );

                                    continue;
                                }

                                if (equipo.Marca.Length > 100)
                                {
                                    filasError++;

                                    errores.Add(
                                        $"Fila {filasProcesadas}: Marca demasiado larga."
                                    );

                                    continue;
                                }

                                if (equipo.NumeroSerie.Length > 100)
                                {
                                    filasError++;

                                    errores.Add(
                                        $"Fila {filasProcesadas}: Número de serie demasiado largo."
                                    );

                                    continue;
                                }

                                // REGISTRAR
                                _service.registrar(equipo);

                                filasInsertadas++;
                            }
                            catch (Exception exFila)
                            {
                                filasError++;

                                errores.Add(
                                    $"Fila {filasProcesadas}: {exFila.Message}"
                                );
                            }
                        }
                    }
                }

                return Ok(new
                {
                    mensaje = "Importación completada.",

                    filasProcesadas =
                        filasProcesadas,

                    filasInsertadas =
                        filasInsertadas,

                    filasError =
                        filasError,

                    errores = errores
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    mensaje =
                        "Error al procesar el Excel.",

                    detalle =
                        ex.ToString()
                });
            }
        }


        // endpoint de lo de buscar por id 
        [HttpGet("{id}")]
        public IActionResult buscarPorId(int id)
        {
            var equipo = _service.buscarPorId(id);

            if (equipo == null)
            {
                return NotFound();
            }

            return Ok(equipo);
        }



        // endpoint para actualizar el equipo mediante el id y el cuerpo de la solicitud con los datos actualizados del equipo
        [HttpPut("{id}")]
        public IActionResult actualizar(int id,[FromBody] Equipo equipo)
        {
            equipo.Id = id;

            _service.actualizar(equipo);

            return Ok(new
            {
                mensaje = "Equipo actualizado correctamente"
            });
        }


        // endpoint para eliminar un equipo mediante su id
        [HttpDelete("{id}")]
        public IActionResult eliminar(int id)
        {
            _service.eliminar(id);

            return Ok(new
            {
                mensaje = "Equipo eliminado correctamente"
            });
        }




    }
}
