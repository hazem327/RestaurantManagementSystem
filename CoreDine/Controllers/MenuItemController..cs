using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using CoreDine.Models;
using CoreDine.Services;
using CoreDine.ViewModels;

namespace CoreDine.Controllers
{
    [Authorize(Roles = "Admin")]
    public class MenuItemController : Controller
    {
        private readonly IMenuService _menuService;

        public MenuItemController(IMenuService menuService)
        {
            _menuService = menuService;
        }

        public async Task<IActionResult> Index()
        {
            var items = await _menuService.GetAllItemsAsync();
            return View("~/Views/MenuItem/Index.cshtml", items);
        }

        public async Task<IActionResult> Create()
        {
            var vm = new MenuItemViewModel
            {
                Categories = await GetCategorySelectList()
            };
            return View("~/Views/MenuItem/Create.cshtml", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MenuItemViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Categories = await GetCategorySelectList();
                return View("~/Views/MenuItem/Create.cshtml", vm);
            }

            var item = new MenuItem
            {
                Name = vm.Name,
                Description = vm.Description,
                Price = vm.Price,
                IsAvailable = vm.IsAvailable,
                CategoryId = vm.CategoryId
            };

            await _menuService.CreateItemAsync(item);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var item = await _menuService.GetItemByIdAsync(id);

            var vm = new MenuItemViewModel
            {
                MenuItemId = item.MenuItemId,
                Name = item.Name,
                Description = item.Description,
                Price = item.Price,
                IsAvailable = item.IsAvailable,
                CategoryId = item.CategoryId,
                Categories = await GetCategorySelectList()
            };

            return View("~/Views/MenuItem/Edit.cshtml", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MenuItemViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Categories = await GetCategorySelectList();
                return View("~/Views/MenuItem/Edit.cshtml", vm);
            }

            var item = new MenuItem
            {
                MenuItemId = vm.MenuItemId,
                Name = vm.Name,
                Description = vm.Description,
                Price = vm.Price,
                IsAvailable = vm.IsAvailable,
                CategoryId = vm.CategoryId
            };

            await _menuService.UpdateItemAsync(item);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var item = await _menuService.GetItemByIdAsync(id);

            var vm = new MenuItemViewModel
            {
                MenuItemId = item.MenuItemId,
                Name = item.Name,
                Description = item.Description,
                Price = item.Price,
                IsAvailable = item.IsAvailable
            };

            return View("~/Views/MenuItem/Delete.cshtml", vm);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _menuService.DeleteItemAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task<IEnumerable<SelectListItem>> GetCategorySelectList()
        {
            var categories = await _menuService.GetAllCategoriesAsync();
            return categories.Select(c => new SelectListItem
            {
                Value = c.CategoryId.ToString(),
                Text = c.Name
            });
        }
    }
}
