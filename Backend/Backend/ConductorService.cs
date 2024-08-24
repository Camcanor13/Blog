using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;

namespace Backend
{
    public class ConductorService
    {
        private readonly string _connectionString;

        public ConductorService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Conductor> GetConductores()
        {
            var conductores = new List<Conductor>();

            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var command = new MySqlCommand("SELECT * FROM conductores", connection);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var conductor = new Conductor
                        {
                            Id = reader.GetInt32("id"),
                            Nombre = reader.GetString("nombre"),
                            Password = reader.GetString("password"),
                            Programa = reader.GetString("programa")
                            // Mapea el resto de las columnas aquí
                        };
                        conductores.Add(conductor);
                    }
                }
            }

            return conductores;
        }
    }

    public class Conductor
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Password { get; set; }
        public string Programa { get; set; }
        // Agrega más propiedades según tu esquema de la tabla
    }
}
