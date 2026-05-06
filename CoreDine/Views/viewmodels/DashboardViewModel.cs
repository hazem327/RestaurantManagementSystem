namespace CoreDine.ViewModels
{
    public class DashboardViewModel
    {
        public int TodayOrderCount { get; set; }
        public decimal TodayRevenue { get; set; }
        public int ActiveTableCount { get; set; }
        public int AvailableMenuItemCount { get; set; }
    }
}
