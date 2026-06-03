using Inventario_banco_api.Models;
using Inventario_banco_api.Repository;

namespace Inventario_banco_api.Service.ServiceImpl
{
    public class EquipoServiceIMPL : IEquipoService
    {

        private readonly IEquipoRepository _repository;

        public EquipoServiceIMPL(IEquipoRepository repository)
        {
            _repository = repository;
        }

        public void actualizar(Equipo equipo)
        {
            _repository.actualizar(equipo);
        }

        public Equipo buscarPorId(int id)
        {
            return _repository.buscarPorId(id);
        }

        public void eliminar(int id)
        {
            _repository.eliminar(id);
        }

        public List<Equipo> listar()
        {
            return _repository.listar();
        }

        public void registrar(Equipo equipo)
        {
            _repository.registrar(equipo);
        }

        public List<Equipo> buscar(string texto, string estado, string ubicacion)
        {
            return _repository.buscar(texto, estado, ubicacion);
        }
    }
}
