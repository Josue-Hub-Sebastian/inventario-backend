using Inventario_banco_api.Models;

namespace Inventario_banco_api.Repository
{
    public interface IEquipoRepository
    {
        List<Equipo> listar();
        void registrar(Equipo equipo);
        Equipo buscarPorId(int id);
        void actualizar (Equipo equipo);
        void eliminar (int id);

    }
}
