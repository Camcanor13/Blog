using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic; // Para List y IEnumerable
using System.Data;
using System.Threading.Tasks;
public class MeGustaService
{
    private readonly string _connectionString;

    public MeGustaService(string connectionString)
    {
        _connectionString = connectionString;
    }

    // Método para obtener los me gusta de una publicación
    public async Task<IEnumerable<MeGustaResult>> GetMeGustaByPublication(int publicationId)
    {
        var meGusta = new List<MeGustaResult>();

        try
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = @"
                SELECT u.user
                FROM megusta mg
                JOIN usuarios u ON mg.id_usuario = u.id
                WHERE mg.id_publicacion = @PublicationId";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PublicationId", publicationId);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            meGusta.Add(new MeGustaResult
                            {
                                UserName = reader.GetString("user")
                            });
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al obtener los me gusta: {ex.Message}");
        }

        return meGusta;
    }

    // Método para agregar un me gusta
    public async Task<string> AddMeGusta(int publicationId, int userId)
    {
        try
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                // Verificar si ya existe un me gusta para la publicación y el usuario
                var checkQuery = "SELECT COUNT(*) FROM megusta WHERE id_publicacion = @PublicationId AND id_usuario = @UserId";
                using (var checkCommand = new MySqlCommand(checkQuery, connection))
                {
                    checkCommand.Parameters.AddWithValue("@PublicationId", publicationId);
                    checkCommand.Parameters.AddWithValue("@UserId", userId);

                    var count = Convert.ToInt32(await checkCommand.ExecuteScalarAsync());
                    if (count > 0)
                    {
                        return "El usuario ya ha dado me gusta a esta publicación.";
                    }
                }

                // Insertar el nuevo me gusta
                var insertQuery = "INSERT INTO megusta (id_publicacion, id_usuario) VALUES (@PublicationId, @UserId)";
                using (var insertCommand = new MySqlCommand(insertQuery, connection))
                {
                    insertCommand.Parameters.AddWithValue("@PublicationId", publicationId);
                    insertCommand.Parameters.AddWithValue("@UserId", userId);

                    await insertCommand.ExecuteNonQueryAsync();
                }

                return "Me gusta agregado exitosamente.";
            }
        }
        catch (MySqlException ex)
        {
            return $"Error al procesar la solicitud: {ex.Number} - {ex.Message}";
        }
        catch (Exception ex)
        {
            return $"Error inesperado: {ex.Message}";
        }
    }

    // Método para eliminar un me gusta
    public async Task<string> DeleteMeGusta(int publicationId, int userId)
    {
        try
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                // Verificar si el me gusta existe
                var checkQuery = "SELECT COUNT(*) FROM megusta WHERE id_publicacion = @PublicationId AND id_usuario = @UserId";
                using (var checkCommand = new MySqlCommand(checkQuery, connection))
                {
                    checkCommand.Parameters.AddWithValue("@PublicationId", publicationId);
                    checkCommand.Parameters.AddWithValue("@UserId", userId);

                    var count = Convert.ToInt32(await checkCommand.ExecuteScalarAsync());
                    if (count == 0)
                    {
                        return "El me gusta no existe.";
                    }
                }

                // Eliminar el me gusta
                var deleteQuery = "DELETE FROM megusta WHERE id_publicacion = @PublicationId AND id_usuario = @UserId";
                using (var deleteCommand = new MySqlCommand(deleteQuery, connection))
                {
                    deleteCommand.Parameters.AddWithValue("@PublicationId", publicationId);
                    deleteCommand.Parameters.AddWithValue("@UserId", userId);

                    await deleteCommand.ExecuteNonQueryAsync();
                }

                return "Me gusta eliminado exitosamente.";
            }
        }
        catch (MySqlException ex)
        {
            return $"Error al procesar la solicitud: {ex.Number} - {ex.Message}";
        }
        catch (Exception ex)
        {
            return $"Error inesperado: {ex.Message}";
        }
    }


    public async Task<IEnumerable<PublicacionMeGustaResult>> GetPublicacionesMeGusta(int userId)
    {
        var publicaciones = new List<PublicacionMeGustaResult>();

        try
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = @"
            SELECT p.title, p.body, p.date, u.user as author
            FROM megusta mg
            JOIN publicaciones p ON mg.id_publicacion = p.id
            JOIN usuarios u ON p.author = u.id
            WHERE mg.id_usuario = @UserId";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            publicaciones.Add(new PublicacionMeGustaResult
                            {
                                Title = reader.GetString("title"),
                                Body = reader.GetString("body"),
                                Date = reader.GetDateTime("date"),
                                Author = reader.GetString("author")
                            });
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al obtener las publicaciones: {ex.Message}");
        }

        return publicaciones;
    }
}

public class MeGustaResult
{
    public string UserName { get; set; }
}


public class PublicacionMeGustaResult
{
    public string Title { get; set; }
    public string Body { get; set; }
    public DateTime Date { get; set; }
    public string Author { get; set; }
}