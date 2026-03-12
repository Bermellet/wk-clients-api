using System.Text.Json;
using WKClientsApi.Models;

namespace WKClientsApi.Repositories
{
    public class JsonClienteRepository : IClienteRepository
    {
        private readonly string _filePath = "data/clientes_store.json";
        private readonly JsonSerializerOptions _options = new() { PropertyNameCaseInsensitive = true, WriteIndented = true };

        private async Task<List<Cliente>> LoadData()
        {
            var filePath = GetCompleteFilePath(_filePath);
            if (!File.Exists(filePath)) return new List<Cliente>();
            var json = await File.ReadAllTextAsync(filePath);
            return JsonSerializer.Deserialize<List<Cliente>>(json, _options) ?? new List<Cliente>();
        }

        private async Task SaveData(List<Cliente> clientes)
        {
            var json = JsonSerializer.Serialize(clientes, _options);
            var filePath = GetCompleteFilePath(_filePath);
            CreateFolderIfNotExist(_filePath);
            await File.WriteAllTextAsync(filePath, json);
        }

        private void CreateFolderIfNotExist(string path)
        {
            var baseDir = AppDomain.CurrentDomain.BaseDirectory;
            var dir = Path.GetDirectoryName(path);
            var directory = Path.Combine(baseDir, dir);

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        public async Task<IEnumerable<Cliente>> GetAllAsync() => await LoadData();

        public async Task<Cliente?> GetByDniAsync(string dni) =>
            (await LoadData()).FirstOrDefault(c => c.DNI == dni);

        public async Task AddAsync(Cliente cliente)
        {
            var clientes = await LoadData();
            if (clientes.Any(c => c.DNI == cliente.DNI))
            {
                throw new InvalidOperationException("El DNI ya existe");
            }

            clientes.Add(cliente);
            await SaveData(clientes);
        }

        public async Task UpdateAsync(string dni, Cliente cliente)
        {
            var clientes = await LoadData();
            var index = clientes.FindIndex(c => c.DNI == dni);
            if (index == -1)
            {
                throw new KeyNotFoundException("Cliente no encontrado");
            }

            clientes[index] = cliente;
            await SaveData(clientes);
        }

        public async Task DeleteAsync(string dni)
        {
            var clientes = await LoadData();
            clientes.RemoveAll(c => c.DNI == dni);
            await SaveData(clientes);
        }

        private string GetCompleteFilePath(string filePath)
        {
            var baseDir = AppDomain.CurrentDomain.BaseDirectory;
            return Path.Combine(baseDir, filePath);
        }
    }
}
