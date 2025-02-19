using Microsoft.EntityFrameworkCore;
using ProductosApi.Infrastructure;
using ProductosApi.Models.Context;


var builder = WebApplication.CreateBuilder(args);

// Configurar Entity Framework con SQLite
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.ConfigureServices();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Asegurar que la base de datos se cree automáticamente
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();
    context.Database.Migrate(); // Aplica migraciones automáticamente
}

// Configurar el middleware de la aplicación
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
