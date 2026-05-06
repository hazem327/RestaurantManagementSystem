using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using CoreDine.Models;
using CoreDine.Services;
using CoreDine.ViewModels;

namespace CoreDine.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly ITableService _tableService;
        private readonly IMenuService _menuService;

        public OrderController(
            IOrderService orderService,
            ITableService tableService,
            IMenuService menuService)
        {
            _orderService = orderService;
            _tableService = tableService;
            _menuService = menuService;
        }

        
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index(OrderStatus? status, DateTime? date)
        {
            var orders = await _orderService.GetAllOrdersAsync(status, date);

            var vm = orders.Select(o => new OrderViewModel
            {
                OrderId = o.OrderId,
                OrderName = o.OrderName,
                TableNumber = o.Table.TableNumber,
                DateCreated = o.DateCreated,
                Status = o.Status,
                TotalAmount = o.TotalAmount
            }).ToList();

            return View(vm);
        }

        
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> TodaysOrders()
        {
            var orders = await _orderService.GetTodaysOrdersAsync();

            var vm = orders.Select(o => new TodayOrderViewModel
            {
                OrderId = o.OrderId,
                TableNumber = o.Table.TableNumber,
                Status = o.Status,
                TotalAmount = o.TotalAmount,
                DateCreated = o.DateCreated
            }).ToList();

            return View(vm);
        }

        
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> Details(int id)
        {
            var order = await _orderService.GetOrderWithItemsAsync(id);

            var vm = new OrderViewModel
            {
                OrderId = order.OrderId,
                OrderName = order.OrderName,
                TableNumber = order.Table.TableNumber,
                DateCreated = order.DateCreated,
                Status = order.Status,
                TotalAmount = order.TotalAmount,
                OrderItems = order.OrderItems.Select(oi => new OrderItemViewModel
                {
                    OrderItemId = oi.OrderItemId,
                    MenuItemName = oi.MenuItem.Name,
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice,
                    SubTotal = oi.SubTotal
                }).ToList()
            };

            return View(vm);
        }

        
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> Create()
        {
            var tables = await _tableService.GetAllTablesAsync();

            var vm = new CreateOrderViewModel
            {
                Tables = tables
                    .Where(t => t.Status == TableStatus.Available)
                    .Select(t => new SelectListItem
                    {
                        Value = t.TableId.ToString(),
                        Text = $"Table {t.TableNumber} (Capacity: {t.Capacity})"
                    })
            };

            return View(vm);
        }

        [Authorize(Roles = "Staff")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateOrderViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                var tables = await _tableService.GetAllTablesAsync();
                vm.Tables = tables
                    .Where(t => t.Status == TableStatus.Available)
                    .Select(t => new SelectListItem
                    {
                        Value = t.TableId.ToString(),
                        Text = $"Table {t.TableNumber} (Capacity: {t.Capacity})"
                    });
                return View(vm);
            }

            var order = new Order
            {
                TableId = vm.TableId,
                OrderName = vm.OrderName
            };

            int newOrderId = await _orderService.CreateOrderAsync(order);
            return RedirectToAction(nameof(AddItem), new { id = newOrderId });
        }

        
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> AddItem(int id)
        {
            var items = await _menuService.GetAllItemsAsync();

            var vm = new AddOrderItemViewModel
            {
                OrderId = id,
                AvailableItems = items
                    .Where(m => m.IsAvailable)
                    .Select(m => new SelectListItem
                    {
                        Value = m.MenuItemId.ToString(),
                        Text = $"{m.Name} — ${m.Price:F2}"
                    })
            };

            return View(vm);
        }

        [Authorize(Roles = "Staff")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddItem(AddOrderItemViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                var items = await _menuService.GetAllItemsAsync();
                vm.AvailableItems = items
                    .Where(m => m.IsAvailable)
                    .Select(m => new SelectListItem
                    {
                        Value = m.MenuItemId.ToString(),
                        Text = $"{m.Name} — ${m.Price:F2}"
                    });
                return View(vm);
            }

            var orderItem = new OrderItem
            {
                MenuItemId = vm.MenuItemId,
                Quantity = vm.Quantity
            };

            try
            {
                await _orderService.AddItemToOrderAsync(vm.OrderId, orderItem);
                return RedirectToAction(nameof(Details), new { id = vm.OrderId });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(vm);
            }
        }

        
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> UpdateStatus(int id)
        {
            var order = await _orderService.GetOrderWithItemsAsync(id);

            var vm = new UpdateOrderStatusViewModel
            {
                OrderId = order.OrderId,
                OrderName = order.OrderName,
                Status = order.Status
            };

            return View(vm);
        }

        [Authorize(Roles = "Admin,Staff")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(UpdateOrderStatusViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            await _orderService.UpdateOrderStatusAsync(vm.OrderId, vm.Status);
            return RedirectToAction(nameof(Details), new { id = vm.OrderId });
        }

        
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _orderService.DeleteOrderAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
