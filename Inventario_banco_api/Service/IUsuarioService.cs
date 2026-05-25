using Inventario_banco_api.Models;

namespace Inventario_banco_api.Service
{
    public interface IUsuarioService
    {
        LoginResponse login(LoginRequest request);

        List<Usuario> listar();

        void registrar(Usuario usuario);
    }
}
