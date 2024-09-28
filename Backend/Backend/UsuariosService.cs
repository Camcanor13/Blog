//AQUI SE ENCUENTRAN LOS METODOS QUE PERMITEN MANEJAR EL INICIO DE SESION DE LA PAGINA 
//TAMBIEN UN METODO QUE IMPRIME TODOS LOS USUARIOS- EN CASO DE SER NECESARIO NO SE PARA QUE PERO YA LE BUSCAREMOS USO :)

using MySql.Data.MySqlClient;
using System;
using System.Threading.Tasks;
//NOMBRE DEL PROYECTO
namespace Backend
{     //NOMBRE DEL SERVICIO
    public class UsuariosService
    {
        //llama la conexion
        private readonly string _connectionString;
        public UsuariosService(string connectionString)
        {
            _connectionString = connectionString;
        }

        //METODO PARA HACER EL LOGIN
        public async Task<LoginResponse> LoginAsync(string email, string password)
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
                                    var id = Convert.ToInt32(reader["id"]);
                                    var userName = reader["user"].ToString();
                                    var rol = reader["rol"].ToString();

                                    // Crear y retornar el objeto de respuesta
                                    return new LoginResponse
                                    {
                                        Id = id,
                                        UserName = userName,
                                        Rol = rol,
                                        Email = email
                                    };
                                }
                                else
                                {
                                    throw new UnauthorizedAccessException("Contraseña incorrecta.");
                                }
                            }
                            else
                            {
                                throw new UnauthorizedAccessException("El usuario no existe.");
                            }
                        }
                    }
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new UnauthorizedAccessException(ex.Message);
            }
            catch (Exception)
            {
                throw new Exception("Error al procesar la solicitud. Inténtalo de nuevo más tarde.");
            }
        }

        public class LoginResponse
        {
            public int Id { get; set; }

            public string UserName { get; set; }
            public string Rol { get; set; }
            public string Email { get; set; }
        }

        //METODO PARA EL REGISTRO
        public async Task<string> RegisterAsync(string email, string password, string user, string rol)
        {
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    // Verificar si el email ya está registrado
                    var checkQuery = "SELECT COUNT(*) FROM usuarios WHERE email = @Email";
                    using (var checkCommand = new MySqlCommand(checkQuery, connection))
                    {
                        checkCommand.Parameters.AddWithValue("@Email", email);
                        var count = Convert.ToInt32(await checkCommand.ExecuteScalarAsync());

                        if (count > 0)
                        {
                            return "El email ya está registrado.";
                        }
                    }

                    // Insertar el nuevo usuario
                    var insertQuery = "INSERT INTO usuarios (email, password, user, rol) VALUES (@Email, @Password, @User, @Rol)";
                    using (var insertCommand = new MySqlCommand(insertQuery, connection))
                    {
                        insertCommand.Parameters.AddWithValue("@Email", email);
                        insertCommand.Parameters.AddWithValue("@Password", password); // Asegúrate de cifrar la contraseña en producción
                        insertCommand.Parameters.AddWithValue("@User", user);
                        insertCommand.Parameters.AddWithValue("@Rol", rol);

                        await insertCommand.ExecuteNonQueryAsync();
                    }

                    return "Registro exitoso.";
                }
            } //Manejo de expciones en caso de ser necesario
            catch (Exception ex)
            {
                return "Error al procesar la solicitud. Inténtalo de nuevo más tarde.";
            }
        }

        //METODO PARA IMPRIMIR A LOS USUARIOS DE LA BD
        public List<Usuario> GetUsuarios()
        {
            var usuarios = new List<Usuario>();

            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var command = new MySqlCommand("SELECT * FROM usuarios", connection);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var usuario = new Usuario
                        {
                            Id = reader.GetInt32("id"),
                            Nombre = reader.GetString("user"),
                            Contraseña = reader.GetString("password"),
                            Correo = reader.GetString("email")
                            // Mapea el resto de las columnas aquí
                        };
                        usuarios.Add(usuario);
                    }
                }
            }

            return usuarios;
        }
    }

    public class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Contraseña { get; set; }
        public string Correo { get; set; }
        // Agrega más propiedades según tu esquema de la tabla
    }
}


