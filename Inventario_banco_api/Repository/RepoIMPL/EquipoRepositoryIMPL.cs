using System.Data;
using Inventario_banco_api.Models;
using Microsoft.Data.SqlClient;

namespace Inventario_banco_api.Repository.RepoIMPL
{
    // Esto se pasa de la cara de IEquipoRepository, es decir, se implementa la interfaz IEquipoRepository en esta clase EquipoRepositoryIMPL.
    // despues se manda al service y luego al controller , seguridad y orden de capas 
    public class EquipoRepositoryIMPL : IEquipoRepository
    {

        private readonly IConfiguration _config;

        public EquipoRepositoryIMPL(IConfiguration config)
        {
            _config = config;
        }

        // actualizar el tipico PUT en los endpoints
        public void actualizar(Equipo equipo)
        {
            using (SqlConnection cn = new SqlConnection(
                _config.GetConnectionString("cadenaSQL")))
            {
                SqlCommand cmd = new SqlCommand("sp_ActualizarEquipo", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Id", equipo.Id);
                cmd.Parameters.AddWithValue("@CodigoPatrimonial", equipo.CodigoPatrimonial);
                cmd.Parameters.AddWithValue("@TipoEquipo", equipo.TipoEquipo);
                cmd.Parameters.AddWithValue("@Marca", equipo.Marca);
                cmd.Parameters.AddWithValue("@Modelo", equipo.Modelo);
                cmd.Parameters.AddWithValue("@NumeroSerie", equipo.NumeroSerie);
                cmd.Parameters.AddWithValue("@EstadoEquipo", equipo.EstadoEquipo);
                cmd.Parameters.AddWithValue("@Ubicacion", equipo.Ubicacion);
                cmd.Parameters.AddWithValue("@UsuarioAsignado", equipo.UsuarioAsignado);

                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        //buscar por id metodo
        public Equipo buscarPorId(int id)
        {
            Equipo equipo = null;

            using (SqlConnection cn = new SqlConnection(_config.GetConnectionString("cadenaSQL")))
            {
                SqlCommand cmd = new SqlCommand("sp_BuscarEquipoPorId", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);

                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    equipo = new Equipo()
                    {
                        Id = Convert.ToInt32(dr["Id"]),
                        CodigoPatrimonial = dr["CodigoPatrimonial"].ToString(),
                        TipoEquipo = dr["TipoEquipo"].ToString(),
                        Marca = dr["Marca"].ToString(),
                        Modelo = dr["Modelo"].ToString(),
                        NumeroSerie = dr["NumeroSerie"].ToString(),
                        EstadoEquipo = dr["EstadoEquipo"].ToString(),
                        Ubicacion = dr["Ubicacion"].ToString(),
                        UsuarioAsignado = dr["UsuarioAsignado"].ToString(),
                    };
                }
            }
            return equipo;
        }


        public void eliminar(int id)
        {
            using (SqlConnection cn = new SqlConnection(
                _config.GetConnectionString("cadenaSQL")))
            {
                SqlCommand cmd = new SqlCommand("sp_EliminarEquipo", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Id", id);

                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }



        // Metodo de listar equipos desde la base de datos utilizando un procedimiento almacenado llamado "sp_ListarEquipos".
        public List<Equipo> listar()
        {
            var x = 0;

            List<Equipo> lista = new List<Equipo>();
            using (SqlConnection cn = new SqlConnection(_config.GetConnectionString("cadenaSQL")))
            {
                SqlCommand cmd = new SqlCommand("sp_ListarEquipos", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cn.Open();

                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    lista.Add(new Equipo()
                    {
                        Id = Convert.ToInt32(dr["Id"]),
                        CodigoPatrimonial = dr["CodigoPatrimonial"].ToString(),
                        TipoEquipo = dr["TipoEquipo"].ToString(),
                        Marca = dr["Marca"].ToString(),
                        Modelo = dr["Modelo"].ToString(),
                        NumeroSerie = dr["NumeroSerie"].ToString(),
                        EstadoEquipo = dr["EstadoEquipo"].ToString(),
                        Ubicacion = dr["Ubicacion"].ToString(),
                        UsuarioAsignado = dr["UsuarioAsignado"].ToString(),
                    });
                }
            }
            return lista;
        }


        // Metodo de registrar un nuevo equipo en la base de datos utilizando un procedimiento almacenado llamado "sp_RegistrarEquipo".
        public void registrar(Equipo equipo)
        {
            using (SqlConnection cn = new SqlConnection(
                _config.GetConnectionString("cadenaSQL")))
            {
                SqlCommand cmd = new SqlCommand(
                    "sp_RegistrarEquipo",
                    cn
                );

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue(
                    "@TipoEquipo",
                    equipo.TipoEquipo
                );

                cmd.Parameters.AddWithValue(
                    "@Marca",
                    equipo.Marca
                );

                cmd.Parameters.AddWithValue(
                    "@Modelo",
                    equipo.Modelo
                );

                cmd.Parameters.AddWithValue(
                    "@NumeroSerie",
                    equipo.NumeroSerie
                );

                cmd.Parameters.AddWithValue(
                    "@EstadoEquipo",
                    equipo.EstadoEquipo
                );

                cmd.Parameters.AddWithValue(
                    "@Ubicacion",
                    equipo.Ubicacion
                );

                cmd.Parameters.AddWithValue(
                    "@UsuarioAsignado",
                    equipo.UsuarioAsignado
                );

                cn.Open();

                cmd.ExecuteNonQuery();

            }
        }
    }
}
