using Inventario_banco_api.Models;

namespace Inventario_banco_api.Repository
{
    public interface IUsuarioRepository
    {
        Usuario login(string username);
        List<Usuario> listar();
        void registrar(Usuario usuario);
    }
}
