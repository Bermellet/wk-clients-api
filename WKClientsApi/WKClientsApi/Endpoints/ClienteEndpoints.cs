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
                if (!ClienteValidator.TryValidate(cliente, out var errors))
                {
                    return Results.BadRequest(errors);
                }

                if (string.IsNullOrWhiteSpace(cliente.DNI)) return Results.BadRequest("DNI inválido");

                var existing = await repo.GetByDniAsync(cliente.DNI);
                if (existing is not null) return Results.BadRequest("El DNI ya existe");

                try
                {
                    await repo.AddAsync(cliente);
                }
                catch (InvalidOperationException ex)
                {
                    return Results.BadRequest(ex.Message);
                }

                return Results.Created($"/clientes/{cliente.DNI}", cliente);
            });

            group.MapPut("/{dni}", async (string dni, Cliente cliente, IClienteRepository repo) =>
            {
                if (!ClienteValidator.TryValidate(cliente, out var errors))
                {
                    return Results.BadRequest(errors);
                }

                if (string.IsNullOrWhiteSpace(dni) || string.IsNullOrWhiteSpace(cliente.DNI))
                {
                    return Results.BadRequest("DNI inválido");
                }

                if (!string.Equals(dni, cliente.DNI, StringComparison.Ordinal))
                {
                    return Results.BadRequest("El DNI de la ruta debe coincidir con el DNI del cliente");
                }

                var existing = await repo.GetByDniAsync(dni);
                if (existing is null) return Results.NotFound();

                await repo.UpdateAsync(dni, cliente);
                return Results.NoContent();
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
