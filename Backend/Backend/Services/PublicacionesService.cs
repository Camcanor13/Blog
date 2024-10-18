//AQUI SE ENCUENTRAN LOS METODOS PARA EL MANEJO DE LA TABLA DE PUBLICACIONES
//POR AHORA SOLO ESTA PARA IMPRIMIR TODAS LAS PUBLICACIONES EN UN JSON 
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.Services
{
    public class PublicacionesService
    {
        private readonly string _connectionString;

        public PublicacionesService(string connectionString)
        {
            _connectionString = connectionString;
        }
        //Get publicaciones
        public async Task<List<Publication>> GetPublicationsAsync()
        {
            var publications = new List<Publication>();

            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    var query = @"
                SELECT p.id, p.title, p.body, p.date, 
                       (SELECT COUNT(*) FROM comentarios c WHERE c.id_publicacion = p.id) AS coments, 
                       p.qualification, p.estado, u.user AS author
                FROM publicaciones p
                JOIN usuarios u ON p.author = u.id;";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var publication = new Publication
                                {
                                    Id = Convert.ToInt32(reader["id"]),
                                    Title = reader["title"].ToString(),
                                    Body = reader["body"].ToString(),
                                    Date = Convert.ToDateTime(reader["date"]),
                                    Comments = Convert.ToInt32(reader["coments"]),
                                    Qualification = Convert.ToInt32(reader["qualification"]),
                                    Status = reader["estado"].ToString(),
                                    Author = reader["author"].ToString()
                                };

                                publications.Add(publication);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener publicaciones", ex);
            }

            return publications;
        }
        //Add publicacion
        public async Task<string> AddPublication(string title, string body, DateTime date, int comments, int qualification, string status, int author)
        {
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                   
                    var checkQuery = "SELECT COUNT(*) FROM publicaciones WHERE title = @Title";
                    using (var checkCommand = new MySqlCommand(checkQuery, connection))
                    {
                        checkCommand.Parameters.AddWithValue("@Title", title);
                        var count = Convert.ToInt32(await checkCommand.ExecuteScalarAsync());

                        if (count > 0)
                        {
                            return "Ya existe una publicación con ese título.";
                        }
                    }

                    var insertQuery = "INSERT INTO publicaciones (title, body, date, coments, qualification, estado, author) VALUES (@Title, @Body, @Date, @Comments, @Qualification, @Status, @Author)";
                    using (var insertCommand = new MySqlCommand(insertQuery, connection))
                    {
                        insertCommand.Parameters.AddWithValue("@Title", title);
                        insertCommand.Parameters.AddWithValue("@Body", body);
                        insertCommand.Parameters.AddWithValue("@Date", date);
                        insertCommand.Parameters.AddWithValue("@Comments", comments);
                        insertCommand.Parameters.AddWithValue("@Qualification", qualification);
                        insertCommand.Parameters.AddWithValue("@Status", status);
                        insertCommand.Parameters.AddWithValue("@Author", author);

                        await insertCommand.ExecuteNonQueryAsync();
                    }

                    return "Registro exitoso.";
                }
            }
            catch (Exception ex)
            {
            
                return "Error al procesar la solicitud. Inténtalo de nuevo más tarde.";
            }
        }
        //Actualizar publicacion
        public async Task<string> UpdatePublication(int id, string title, string body, DateTime date, int comments, int qualification, string status)
        {
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                   
                    var checkQuery = "SELECT COUNT(*) FROM publicaciones WHERE title = @Title AND id != @Id";
                    using (var checkCommand = new MySqlCommand(checkQuery, connection))
                    {
                        checkCommand.Parameters.AddWithValue("@Title", title);
                        checkCommand.Parameters.AddWithValue("@Id", id);
                        var count = Convert.ToInt32(await checkCommand.ExecuteScalarAsync());

                        if (count > 0)
                        {
                            return "Ya existe una publicación con ese título.";
                        }
                    }

                    //Consulta para actualizacion
                    var updateQuery = @"
                        UPDATE publicaciones
                        SET title = @Title, body = @Body, date = @Date, coments = @Comments, qualification = @Qualification, estado = @Status
                        WHERE id = @Id";
                    using (var updateCommand = new MySqlCommand(updateQuery, connection))
                    {
                        updateCommand.Parameters.AddWithValue("@Title", title);
                        updateCommand.Parameters.AddWithValue("@Body", body);
                        updateCommand.Parameters.AddWithValue("@Date", date);
                        updateCommand.Parameters.AddWithValue("@Comments", comments);
                        updateCommand.Parameters.AddWithValue("@Qualification", qualification);
                        updateCommand.Parameters.AddWithValue("@Status", status);
                        updateCommand.Parameters.AddWithValue("@Id", id);

                        var rowsAffected = await updateCommand.ExecuteNonQueryAsync();
                        if (rowsAffected > 0)
                        {
                            return "Actualización exitosa.";
                        }
                        else
                        {
                            return "No se encontró la publicación para actualizar.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return "Error al procesar la solicitud. Inténtalo de nuevo más tarde.";
            }
        }
        //Eliminar publicacion
        public async Task<string> DeletePublication(int id)
        {
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    // Verificar si hay comentarios asociados con la publicación
                    var checkCommentsQuery = "SELECT COUNT(*) FROM comentarios WHERE id_publicacion = @Id";
                    using (var checkCommentsCommand = new MySqlCommand(checkCommentsQuery, connection))
                    {
                        checkCommentsCommand.Parameters.AddWithValue("@Id", id);
                        var count = Convert.ToInt32(await checkCommentsCommand.ExecuteScalarAsync());

                        if (count > 0)
                        {
                            return "No se puede eliminar la publicación porque tiene comentarios asociados.";
                        }
                    }

                    // Si no hay comentarios, proceder a eliminar la publicación
                    var deleteQuery = "DELETE FROM publicaciones WHERE id = @Id";
                    using (var deleteCommand = new MySqlCommand(deleteQuery, connection))
                    {
                        deleteCommand.Parameters.AddWithValue("@Id", id);

                        var rowsAffected = await deleteCommand.ExecuteNonQueryAsync();
                        if (rowsAffected > 0)
                        {
                            return "Eliminación exitosa.";
                        }
                        else
                        {
                            return "No se encontró la publicación para eliminar.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return "Error al procesar la solicitud. Inténtalo de nuevo más tarde.";
            }
        }

        public async Task<string> UpdatePublicationStatus(int id, string newStatus)
        {
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    // Actualizar solo el estado de la publicación
                    var updateQuery = "UPDATE publicaciones SET estado = @Status WHERE id = @Id";
                    using (var updateCommand = new MySqlCommand(updateQuery, connection))
                    {
                        updateCommand.Parameters.AddWithValue("@Status", newStatus);
                        updateCommand.Parameters.AddWithValue("@Id", id);

                        var rowsAffected = await updateCommand.ExecuteNonQueryAsync();
                        if (rowsAffected > 0)
                        {
                            return "Estado actualizado exitosamente.";
                        }
                        else
                        {
                            return "No se encontró la publicación para actualizar.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores
                return "Error al procesar la solicitud. Inténtalo de nuevo más tarde.";
            }
        }




    }

    public class Publication
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime Date { get; set; }
        public int Comments { get; set; }
        public int Qualification { get; set; }
        public string Status { get; set; }
        public string Author { get; set; }
    }
}

