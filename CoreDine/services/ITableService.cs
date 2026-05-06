using CoreDine.Models;

namespace CoreDine.Services
{
    public interface ITableService
    {
        Task<List<DiningTable>> GetAllTablesAsync();
        Task<DiningTable> GetTableByIdAsync(int id);
        Task CreateTableAsync(DiningTable table);
        Task UpdateTableAsync(DiningTable table);
        Task DeleteTableAsync(int id);
    }
}
