using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic; // Para List y IEnumerable
using System.Data;
using System.Threading.Tasks;

namespace Backend
{
    public class CalificacionesService
    {
        private readonly string _connectionString;

        public CalificacionesService(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Método para obtener calificaciones por id de publicación
        public async Task<IEnumerable<CalificationResult>> GetCalificationsByPublication(int postId)
        {
            var califications = new List<CalificationResult>();

            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    // Consulta SQL para obtener las calificaciones filtradas por postId
                    var query = @"
            SELECT c.id, c.calificacion, u.user
            FROM calificaciones c
            JOIN usuarios u ON c.id_usuario = u.id
            WHERE c.id_publicacion = @PostId";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@PostId", postId);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                califications.Add(new CalificationResult
                                {
                                    Id = reader.GetInt32("id"), // Obtener el ID
                                    Calification = reader.GetInt32("calificacion"),
                                    UserName = reader.GetString("user")
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores
                throw new Exception($"Error al obtener las calificaciones: {ex.Message}");
            }

            return califications;
        }


        // Método para agregar una nueva calificación
        public async Task<string> AddCalification(int postId, int userId, int calification)
        {
            if (calification < 0 || calification > 5)
            {
                return "Parámetros de calificación inválidos.";
            }

            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    // Verificar si ya existe una calificación para la publicación y el usuario
                    var checkQuery = "SELECT COUNT(*) FROM calificaciones WHERE id_publicacion = @PostId AND id_usuario = @UserId";
                    using (var checkCommand = new MySqlCommand(checkQuery, connection))
                    {
                        checkCommand.Parameters.AddWithValue("@PostId", postId);
                        checkCommand.Parameters.AddWithValue("@UserId", userId);

                        var count = Convert.ToInt32(await checkCommand.ExecuteScalarAsync());
                        if (count > 0)
                        {
                            return "El usuario ya ha calificado esta publicación.";
                        }
                    }

                    // Insertar la nueva calificación
                    var insertQuery = "INSERT INTO calificaciones (id_publicacion, id_usuario, calificacion) VALUES (@PostId, @UserId, @Calification)";
                    using (var insertCommand = new MySqlCommand(insertQuery, connection))
                    {
                        insertCommand.Parameters.AddWithValue("@PostId", postId);
                        insertCommand.Parameters.AddWithValue("@UserId", userId);
                        insertCommand.Parameters.AddWithValue("@Calification", calification);

                        await insertCommand.ExecuteNonQueryAsync();
                    }

                    return "Calificación agregada exitosamente.";
                }
            }
            catch (MySqlException ex)
            {
                // Manejo de errores específicos de MySQL
                return $"Error al procesar la solicitud: {ex.Number} - {ex.Message}";
            }
            catch (Exception ex)
            {
                // Manejo de otros errores
                return $"Error inesperado: {ex.Message}";
            }
        }

        // Método para eliminar una calificación por id
        public async Task<string> DeleteCalification(int id)
        {
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    // Verificar si la calificación existe
                    var checkQuery = "SELECT COUNT(*) FROM calificaciones WHERE id = @Id";
                    using (var checkCommand = new MySqlCommand(checkQuery, connection))
                    {
                        checkCommand.Parameters.AddWithValue("@Id", id);
                        var count = Convert.ToInt32(await checkCommand.ExecuteScalarAsync());

                        if (count == 0)
                        {
                            return "La calificación no existe.";
                        }
                    }

                    // Eliminar la calificación
                    var deleteQuery = "DELETE FROM calificaciones WHERE id = @Id";
                    using (var deleteCommand = new MySqlCommand(deleteQuery, connection))
                    {
                        deleteCommand.Parameters.AddWithValue("@Id", id);
                        await deleteCommand.ExecuteNonQueryAsync();
                    }

                    return "Calificación eliminada exitosamente.";
                }
            }
            catch (MySqlException ex)
            {
                // Manejo de errores específicos de MySQL
                return $"Error al procesar la solicitud: {ex.Number} - {ex.Message}";
            }
            catch (Exception ex)
            {
                // Manejo de otros errores
                return $"Error inesperado: {ex.Message}";
            }
        }
    }

    // Clase para almacenar el resultado de las calificaciones
    public class CalificationResult
    {
        public int Id { get; set; }
        public int Calification { get; set; }
        public string UserName { get; set; }
    }
}
