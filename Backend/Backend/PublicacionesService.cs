//AQUI SE ENCUENTRAN LOS METODOS PARA EL MANEJO DE LA TABLA DE PUBLICACIONES
//POR AHORA SOLO ESTA PARA IMPRIMIR TODAS LAS PUBLICACIONES EN UN JSON 
using MySql.Data.MySqlClient;
using System;
using System.Threading.Tasks;
namespace Backend
{
    public class PublicacionesService
    {
        private readonly string _connectionString;

        public PublicacionesService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<List<Publication>> GetPublicationsAsync()
        {
            var publications = new List<Publication>();

            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    var query = @"
                        SELECT p.id, p.title, p.body, p.date, p.coments, p.qualification, p.estado, u.user AS author
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
                                    Comments = reader["coments"].ToString(),
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
                // Manejo de errores
                throw new Exception("Error al obtener publicaciones", ex);
            }

            return publications;
        }
    }
    public class Publication
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime Date { get; set; }
        public string Comments { get; set; }
        public int Qualification { get; set; }
        public string Status { get; set; }
        public string Author { get; set; }
    }

}


