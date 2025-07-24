
using ApiFinanzas.Servicios;
using FluentValidation;
using FluentValidation.AspNetCore;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.EntityFrameworkCore;
using Persistencia;
using System.Text.Json.Serialization;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

Console.WriteLine(connectionString);
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddScoped<IProgramacionService, ProgramacionService>();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.PropertyNamingPolicy = null; // ← si quieres PascalCase en respuesta
    });

if (builder.Environment.IsProduction())
{
    var envConn = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection");
    if (!string.IsNullOrEmpty(envConn))
    {
        Console.WriteLine("✔ Variable de entorno encontrada: " + envConn);
        builder.Configuration["ConnectionStrings:DefaultConnection"] = envConn;
    }
    else
    {
        Console.WriteLine("⚠ No se encontró la variable ConnectionStrings__DefaultConnection en entorno.");
    }
}

builder.Services.AddHangfire(config =>
    config.UsePostgreSqlStorage(connectionString)); // Usa tu connectionString real

builder.Services.AddHangfireServer();
builder.Services.AddTransient<EmailService>();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});


var app = builder.Build();
app.UseCors();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
// Después de app.UseAuthorization()
app.UseHangfireDashboard("/hangfire");
using (var scope = app.Services.CreateScope())
{
    var programador = scope.ServiceProvider.GetRequiredService<IProgramacionService>();
    await programador.RegistrarProgramacionesActivas();
}


// Trabajo ejemplo
BackgroundJob.Enqueue(() => Console.WriteLine("🔥 Trabajo en segundo plano ejecutado."));
app.MapControllers();

app.Run();
