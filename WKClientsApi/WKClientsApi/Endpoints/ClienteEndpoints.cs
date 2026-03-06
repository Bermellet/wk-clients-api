using WKClientsApi.Models;
using WKClientsApi.Repositories;

namespace WKClientsApi.Endpoints
{
    public static class ClienteEndpoints
    {
        public static void MapClienteEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/clientes");

            group.MapGet("/", async (IClienteRepository repo) =>
                Results.Ok(await repo.GetAllAsync()));

            group.MapGet("/{dni}", async (string dni, IClienteRepository repo) =>
                await repo.GetByDniAsync(dni) is Cliente c ? Results.Ok(c) : Results.NotFound());

            group.MapPost("/", async (Cliente cliente, IClienteRepository repo) =>
            {
                if (string.IsNullOrWhiteSpace(cliente.Dni)) return Results.BadRequest("DNI inválido"); // TODO: Validations DNI

                var existing = await repo.GetByDniAsync(cliente.Dni);
                if (existing is not null) return Results.BadRequest("El DNI ya existe");

                await repo.AddAsync(cliente);
                return Results.Created($"/clientes/{cliente.Dni}", cliente);
            });

            group.MapDelete("/{dni}", async (string dni, IClienteRepository repo) =>
            {
                var existing = await repo.GetByDniAsync(dni);
                if (existing is null) return Results.NotFound();

                await repo.DeleteAsync(dni);
                return Results.NoContent();
            });
        }
    }
}
