using CoreDine.Data;
using CoreDine.Models;
using Microsoft.EntityFrameworkCore;

namespace CoreDine.Services
{
    public class TableService : ITableService
    {
        private readonly ApplicationDbContext _context;

        public TableService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<DiningTable>> GetAllTablesAsync()
        {
            return await _context.DiningTables.ToListAsync();
        }

        public async Task<DiningTable> GetTableByIdAsync(int id)
        {
            var table = await _context.DiningTables.FindAsync(id);
            if (table == null) throw new Exception("Table not found");
            return table;
        }

        public async Task CreateTableAsync(DiningTable table)
        {
            bool exists = await _context.DiningTables
                .AnyAsync(t => t.TableNumber == table.TableNumber);

            if (exists)
                throw new Exception($"Table number {table.TableNumber} is already in use.");

            _context.DiningTables.Add(table);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTableAsync(DiningTable table)
        {
            var existing = await _context.DiningTables.FindAsync(table.TableId);
            if (existing == null) throw new Exception("Table not found");

            bool numberTaken = await _context.DiningTables
                .AnyAsync(t => t.TableNumber == table.TableNumber && t.TableId != table.TableId);

            if (numberTaken)
                throw new Exception($"Table number {table.TableNumber} is already in use.");

            existing.TableNumber = table.TableNumber;
            existing.Capacity = table.Capacity;
            existing.Status = table.Status;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteTableAsync(int id)
        {
            var table = await _context.DiningTables.FindAsync(id);
            if (table == null) throw new Exception("Table not found");

            _context.DiningTables.Remove(table);
            await _context.SaveChangesAsync();
        }
    }
}
