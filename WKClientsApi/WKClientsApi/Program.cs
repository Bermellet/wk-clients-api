using WKClientsApi.Endpoints;
using WKClientsApi.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddSingleton<IClienteRepository, JsonClienteRepository>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Mapeo de Endpoints
app.MapClienteEndpoints();

app.Run();