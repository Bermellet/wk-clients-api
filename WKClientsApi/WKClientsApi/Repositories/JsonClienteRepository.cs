using System.Text.Json;
using WKClientsApi.Models;

namespace WKClientsApi.Repositories
{
    public class JsonClienteRepository : IClienteRepository
    {
        private readonly string _filePath = "data/clientes.json";
        private readonly JsonSerializerOptions _options = new() { PropertyNameCaseInsensitive = true, WriteIndented = true };

        private async Task<List<Cliente>> LoadData()
        {
            if (!File.Exists(_filePath)) return new List<Cliente>();
            var json = await File.ReadAllTextAsync(_filePath);
            return JsonSerializer.Deserialize<List<Cliente>>(json, _options) ?? new List<Cliente>();
        }

        private async Task SaveData(List<Cliente> clientes)
        {
            var json = JsonSerializer.Serialize(clientes, _options);
            await File.WriteAllTextAsync(_filePath, json);
        }

        public async Task<IEnumerable<Cliente>> GetAllAsync() => await LoadData();

        public async Task<Cliente?> GetByDniAsync(string dni) =>
            (await LoadData()).FirstOrDefault(c => c.DNI == dni);

        public async Task AddAsync(Cliente cliente)
        {
            var clientes = await LoadData();
            clientes.Add(cliente);
            await SaveData(clientes);
        }

        public async Task DeleteAsync(string dni)
        {
            var clientes = await LoadData();
            clientes.RemoveAll(c => c.DNI == dni);
            await SaveData(clientes);
        }
    }
}
