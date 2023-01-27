using api;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHealthChecks()
    .AddNpgSql(
        builder.Configuration.GetConnectionString("DefaultConnection"), name: "postgresql", tags: new[] { "db", "sql", "postgresql" }
    )
    .AddRedis(builder.Configuration["RedisConnectionString"], name: "redis", tags: new[] { "cache", "redis" });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

app.UseAuthorization();


app.MapControllers();
app.MapHealthChecks("/healthz", new HealthCheckOptions
{
    ResponseWriter = CustomHealthCheckWriter.WriteResponse
});

app.Run();

