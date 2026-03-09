using WKClientsApi.Endpoints;
using WKClientsApi.Repositories;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddSingleton<IClienteRepository, JsonClienteRepository>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "WK Clients API", Version = "v1" });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WK Clients API V1"));
}

// Mapeo de Endpoints
app.MapClienteEndpoints();

app.Run();