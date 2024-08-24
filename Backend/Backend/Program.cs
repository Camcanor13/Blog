using Backend;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Agregar la cadena de conexi�n y registrar el servicio UsuariosService
builder.Services.AddSingleton<UsuariosService>(provider =>
{
    var connectionString = builder.Configuration.GetConnectionString("MySqlConnection");
    return new UsuariosService(connectionString);
});

// Configurar Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
