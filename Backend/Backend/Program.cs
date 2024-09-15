using Backend;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Agregar la cadena de conexión realizada en el JSON y registrar el servicio UsuariosService
builder.Services.AddSingleton<UsuariosService>(provider =>
{
    var connectionString = builder.Configuration.GetConnectionString("MySqlConnection");
    return new UsuariosService(connectionString);
});

// Registrar el servicio PublicacionesService
builder.Services.AddSingleton<PublicacionesService>(provider =>
{
    var connectionString = builder.Configuration.GetConnectionString("MySqlConnection");
    return new PublicacionesService(connectionString);
});

builder.Services.AddSingleton<ComentariosService>(provider =>
{
    var connectionString = builder.Configuration.GetConnectionString("MySqlConnection");
    return new ComentariosService(connectionString);
});

// Configurar Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configurar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder =>
        {
            builder.WithOrigins("http://localhost:5173") // Permitir solicitudes desde el origen especificado
                   .AllowAnyHeader()                    // Permitir cualquier encabezado
                   .AllowAnyMethod();                   // Permitir cualquier método HTTP
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Aplicar la política de CORS
app.UseCors("AllowSpecificOrigin");

app.UseAuthorization();

app.MapControllers();

app.Run();
