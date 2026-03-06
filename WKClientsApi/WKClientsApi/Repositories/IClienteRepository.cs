using WKClientsApi.Models;

namespace WKClientsApi.Repositories
{
    public interface IClienteRepository
    {
        Task<IEnumerable<Cliente>> GetAllAsync();
        Task<Cliente?> GetByDniAsync(string dni);
        Task AddAsync(Cliente client);
        Task DeleteAsync(string dni);
    }
}
