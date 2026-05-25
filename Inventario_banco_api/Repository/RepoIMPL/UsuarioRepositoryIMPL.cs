using Inventario_banco_api.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Inventario_banco_api.Repository.RepoIMPL
{
    public class UsuarioRepositoryIMPL : IUsuarioRepository
    {
        private readonly IConfiguration _config;

        public UsuarioRepositoryIMPL(IConfiguration config)
        {
            _config = config;
        }


        public List<Usuario> listar()
        {
            List<Usuario> lista = new();
            using (SqlConnection cn = new SqlConnection(_config.GetConnectionString("cadenaSQL")))
            {
                SqlCommand cmd = new SqlCommand("sp_ListarUsuarios", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    lista.Add(new Usuario()
                    {
                        Id = Convert.ToInt32(dr["Id"]),
                        Username = dr["Username"].ToString(),
                        PasswordHash = dr["PasswordHash"].ToString(),
                        Rol = dr["Rol"].ToString(),
                        Estado = Convert.ToBoolean(dr["Estado"])
                    });
                }
            }
            return lista;
        }

        public Usuario login(string username)
        {
            Usuario usuario = null;
            using (SqlConnection cn = new SqlConnection(_config.GetConnectionString("cadenaSQL")))
            {
                SqlCommand cmd = new SqlCommand("sp_loginUsuario", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Username", username);
                cn.Open();

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    usuario = new Usuario()
                    {
                        Id = Convert.ToInt32(dr["Id"]),
                        Username = dr["Username"].ToString(),
                        PasswordHash = dr["PasswordHash"].ToString(),
                        Rol = dr["Rol"].ToString(),
                        Estado = Convert.ToBoolean(dr["Estado"])
                    };
                }
            }
            return usuario;

        }

        public void registrar(Usuario usuario)
        {
            using (SqlConnection cn = new SqlConnection(_config.GetConnectionString("cadenaSQL")))
            {
                SqlCommand cmd = new SqlCommand("sp_RegistrarUsuario", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Username", usuario.Username);
                cmd.Parameters.AddWithValue("@PasswordHash", usuario.PasswordHash);
                cmd.Parameters.AddWithValue("@Rol", usuario.Rol);

                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
