using CoreDine.Models;

namespace CoreDine.Services
{
    public interface IMenuService
    {
        Task<List<MenuItem>> GetAllItemsAsync();
        Task<MenuItem> GetItemByIdAsync(int id);
        Task<List<Category>> GetAllCategoriesAsync();
        Task CreateItemAsync(MenuItem item);
        Task UpdateItemAsync(MenuItem item);
        Task DeleteItemAsync(int id);
    }
}
