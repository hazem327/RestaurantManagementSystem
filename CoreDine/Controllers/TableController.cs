using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CoreDine.Models;
using CoreDine.Services;
using CoreDine.ViewModels;

namespace CoreDine.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TableController : Controller
    {
        private readonly ITableService _tableService;

        public TableController(ITableService tableService)
        {
            _tableService = tableService;
        }

        public async Task<IActionResult> Index()
        {
            var tables = await _tableService.GetAllTablesAsync();
            return View("~/Views/Table/Index.cshtml", tables);
        }

        public IActionResult Create()
        {
            var vm = new TableViewModel();
            return View("~/Views/Table/Create.cshtml", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TableViewModel vm)
        {
            if (!ModelState.IsValid)
                return View("~/Views/Table/Create.cshtml", vm);

            var table = new DiningTable
            {
                TableNumber = vm.TableNumber,
                Capacity = vm.Capacity,
                Status = vm.Status
            };

            try
            {
                await _tableService.CreateTableAsync(table);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View("~/Views/Table/Create.cshtml", vm);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            var table = await _tableService.GetTableByIdAsync(id);

            var vm = new TableViewModel
            {
                TableId = table.TableId,
                TableNumber = table.TableNumber,
                Capacity = table.Capacity,
                Status = table.Status
            };

            return View("~/Views/Table/Edit.cshtml", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TableViewModel vm)
        {
            if (!ModelState.IsValid)
                return View("~/Views/Table/Edit.cshtml", vm);

            var table = new DiningTable
            {
                TableId = vm.TableId,
                TableNumber = vm.TableNumber,
                Capacity = vm.Capacity,
                Status = vm.Status
            };

            try
            {
                await _tableService.UpdateTableAsync(table);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View("~/Views/Table/Edit.cshtml", vm);
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            var table = await _tableService.GetTableByIdAsync(id);

            var vm = new TableViewModel
            {
                TableId = table.TableId,
                TableNumber = table.TableNumber,
                Capacity = table.Capacity,
                Status = table.Status
            };

            return View("~/Views/Table/Delete.cshtml", vm);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _tableService.DeleteTableAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
