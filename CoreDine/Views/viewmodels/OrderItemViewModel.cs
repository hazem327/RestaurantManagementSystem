namespace CoreDine.ViewModels
{
    public class OrderItemViewModel
    {
        public int OrderItemId { get; set; }
        public string MenuItemName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal SubTotal { get; set; }
    }
}
