using Backend.Controllers;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Threading.Tasks;

public class ComentariosService
{
    private readonly string _connectionString;

    public ComentariosService(string connectionString)
    {
        _connectionString = connectionString;
    }

    //Add comentario
    public async Task<string> AddComment(int postId, int userId, string comment)
    {
        try
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var insertQuery = "INSERT INTO comentarios (id_publicacion, id_usuario, comentario) VALUES (@PostId, @UserId, @Comment)";
                using (var insertCommand = new MySqlCommand(insertQuery, connection))
                {
                    insertCommand.Parameters.AddWithValue("@PostId", postId);
                    insertCommand.Parameters.AddWithValue("@UserId", userId);
                    insertCommand.Parameters.AddWithValue("@Comment", comment);

                    await insertCommand.ExecuteNonQueryAsync();
                }

                return "Comentario agregado exitosamente.";
            }
        }
        catch (Exception ex)
        {
            return $"Error al procesar la solicitud: {ex.Message}";
        }
    }
    //obtener comentarios por publicacion
    public async Task<IEnumerable<CommentResult>> GetCommentsByPublication(int postId)
    {
        var comments = new List<CommentResult>();

        try
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var query = @"
                    SELECT c.comentario, u.user
                    FROM comentarios c
                    JOIN usuarios u ON c.id_usuario = u.id
                    WHERE c.id_publicacion = @PostId";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PostId", postId);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            comments.Add(new CommentResult
                            {
                                Comment = reader.GetString("comentario"),
                                UserName = reader.GetString("user")
                            });
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al obtener los comentarios: {ex.Message}");
        }

        return comments;
    }
}
public class CommentResult
{
    public string Comment { get; set; }
    public string UserName { get; set; }
}