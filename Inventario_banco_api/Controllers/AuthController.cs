using Inventario_banco_api.Models;
using Inventario_banco_api.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inventario_banco_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // aqui tambien sin embargo se añadira una excepcion para el login y el registro de usuarios, ya que no se necesita estar autenticado para realizar estas acciones
    public class AuthController : ControllerBase
    {
        private readonly IUsuarioService _service;

        public AuthController(IUsuarioService service)
        {
            _service = service;
        }

        [AllowAnonymous] // esta anotacion permite que este endpoint sea accesible sin necesidad de autenticacion, lo cual es necesario para el login
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var response = _service.login(request);

            if (response == null)
            {
                return Unauthorized(new { mensaje = "Credenciales incorrectos" });
            }
            return
                Ok(response);
        }

        // REGISTRAR USUARIO
        [HttpPost("registrar")]
        public IActionResult Registrar(
            [FromBody] Usuario usuario)
        {
            try
            {
                _service.registrar(usuario);

                return Ok(new
                {
                    mensaje =
                    "Usuario registrado correctamente"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    mensaje =
                    "Error al registrar usuario",

                    detalle = ex.Message
                });
            }
        }

        // LISTAR USUARIOS
        [HttpGet]
        public IActionResult Listar()
        {
            var lista =
                _service.listar();

            return Ok(lista);
        }
    }
}

