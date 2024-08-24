using MySql.Data.MySqlClient;
using System;
using System.Threading.Tasks;

namespace Backend
{
    public class UsuariosService
    {
        private readonly string _connectionString;

        public UsuariosService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<string> LoginAsync(string email, string password)
        {
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    var query = "SELECT * FROM usuarios WHERE email = @Email";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Email", email);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                // Email existe, verificar la contraseña
                                var storedPassword = reader["password"].ToString();
                                if (storedPassword == password)
                                {
                                    var userName = reader["user"].ToString();
                                    var rol = reader["rol"].ToString();
                                    return $"Concedido acceso. Bienvenido {userName}, Su rol es: {rol}";

                                }
                                else
                                {
                                    return "Contraseña incorrecta.";
                                }
                            }
                            else
                            {
                                return "El usuario no existe.";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return "Error al procesar la solicitud. Inténtalo de nuevo más tarde.";
            }
        }
    }
}
