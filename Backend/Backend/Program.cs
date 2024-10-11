using Backend.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Agregar la cadena de conexi�n realizada en el JSON y registrar el servicio UsuariosService
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
builder.Services.AddSingleton<CalificacionesService>(provider =>
{
    var connectionString = builder.Configuration.GetConnectionString("MySqlConnection");
    return new CalificacionesService(connectionString);
});

builder.Services.AddSingleton<MeGustaService>(provider =>
{
    var connectionString = builder.Configuration.GetConnectionString("MySqlConnection");
    return new MeGustaService(connectionString);
});

// Configurar Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configurar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.WithOrigins("*") // Permitir solicitudes desde el origen especificado
                   .AllowAnyHeader()                    // Permitir cualquier encabezado
                   .AllowAnyMethod();                   // Permitir cualquier m�todo HTTP
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

// Aplicar la pol�tica de CORS
app.UseCors("AllowAllOrigins");

app.UseAuthorization();

app.MapControllers();

app.Run();
