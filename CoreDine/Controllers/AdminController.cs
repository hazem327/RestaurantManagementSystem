
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CoreDine.Services;
using CoreDine.ViewModels;


namespace CoreDine.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly UserManager<IdentityUser> _userManager;

        // ONE constructor only — both dependencies injected here
        public AdminController(
            IOrderService orderService,
            UserManager<IdentityUser> userManager)
        {
            _orderService = orderService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Dashboard()
        {
            var stats = await _orderService.GetDashboardStatsAsync();
            return View(stats);
        }

        public async Task<IActionResult> Staff()
        {
            var staffUsers = await _userManager.GetUsersInRoleAsync("Staff");
            return View(staffUsers);
        }

        public IActionResult CreateStaff()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateStaff(CreateStaffViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var user = new IdentityUser
            {
                UserName = vm.Email,
                Email = vm.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, vm.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Staff");
                return RedirectToAction(nameof(Staff));
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            return View(vm);
        }
    }
}
