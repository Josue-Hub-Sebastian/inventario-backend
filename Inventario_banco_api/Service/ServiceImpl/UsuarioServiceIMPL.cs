using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Inventario_banco_api.Models;
using Inventario_banco_api.Repository;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;

namespace Inventario_banco_api.Service.ServiceImpl
{
    public class UsuarioServiceIMPL : IUsuarioService
    {
        private readonly IUsuarioRepository _repo;
        private readonly byte[] _jwtKeyBytes;

        public UsuarioServiceIMPL(IUsuarioRepository repo, IConfiguration config)
        {
            _repo = repo;
            var key = config["Jwt:Key"] ?? throw new ArgumentException("Jwt:Key no configurada");
            _jwtKeyBytes = Encoding.UTF8.GetBytes(key);
        }

        public List<Usuario> listar()
        {
            return _repo.listar();
        }
        /*
        public LoginResponse login(
            LoginRequest request)
        {
            var usuario =
                _repo.login(request.Username);

            if (usuario == null)
            {
                return null;
            }

            bool passwordCorrecto =
                BCrypt.Net.BCrypt.Verify(
                    request.Password,
                    usuario.PasswordHash);

            if (!passwordCorrecto)
            {
                return null;
            }

            return new LoginResponse()
            {
                Username = usuario.Username,
                Rol = usuario.Rol,
                Token = "TOKEN_TEMPORAL"
            };
        }
        */



        public LoginResponse login(LoginRequest request)
        {
            var usuario = _repo.login(request.Username);
            if (usuario == null) return null;
            bool passwordCorrecto = BCrypt.Net.BCrypt.Verify(request.Password, usuario.PasswordHash);
            if (!passwordCorrecto) return null;
            var claims = new[]
            {
        new Claim(ClaimTypes.Name, usuario.Username),
        new Claim(ClaimTypes.Role, usuario.Rol)
    };
            var key = new SymmetricSecurityKey(_jwtKeyBytes);
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: "InventarioBanco",
                audience: "InventarioBancoUsers",
                claims: claims,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: creds
            );
            string jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return new LoginResponse()
            {
                Username = usuario.Username,
                Rol = usuario.Rol,
                Token = jwt
            };
        }
        public void registrar(Usuario usuario)
        {
            usuario.PasswordHash = BCrypt.Net.BCrypt.HashPassword(usuario.PasswordHash);
            _repo.registrar(usuario);
        }
    }
}
