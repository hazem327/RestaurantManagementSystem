using CoreDine.Data;
using CoreDine.Models;
using Microsoft.EntityFrameworkCore;
using CoreDine.Services;
namespace CoreDine.Services
{
    public class MenuService : IMenuService
    {
        private readonly ApplicationDbContext _context;

        public MenuService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<MenuItem>> GetAllItemsAsync()
        {
            return await _context.MenuItems
                .Include(m => m.Category)
                .ToListAsync();
        }

        public async Task<MenuItem> GetItemByIdAsync(int id)
        {
            var item = await _context.MenuItems
                .Include(m => m.Category)
                .FirstOrDefaultAsync(m => m.MenuItemId == id);

            if (item == null) throw new Exception("Menu item not found");
            return item;
        }

        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task CreateItemAsync(MenuItem item)
        {
            item.CreatedDate = DateTime.Now;
            _context.MenuItems.Add(item);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateItemAsync(MenuItem item)
        {
            var existing = await _context.MenuItems.FindAsync(item.MenuItemId);
            if (existing == null) throw new Exception("Menu item not found");

            existing.Name = item.Name;
            existing.Description = item.Description;
            existing.Price = item.Price;
            existing.IsAvailable = item.IsAvailable;
            existing.CategoryId = item.CategoryId;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteItemAsync(int id)
        {
            var item = await _context.MenuItems.FindAsync(id);
            if (item == null) throw new Exception("Menu item not found");

            _context.MenuItems.Remove(item);
            await _context.SaveChangesAsync();
        }
    }
}
