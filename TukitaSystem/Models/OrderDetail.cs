using System;

namespace TukitaSystem
{
    public class OrderDetail
    {
        private Order _order;
        private MenuItem _menuItem;
        private int _quantity;

        // The Association Class links Order and MenuItem
        public OrderDetail(Order order, MenuItem menuItem, int quantity)
        {
            if (order == null) throw new ArgumentNullException(nameof(order));
            if (menuItem == null) throw new ArgumentNullException(nameof(menuItem));
            if (quantity <= 0) throw new ArgumentException("Quantity must be positive.");

            _order = order;
            _menuItem = menuItem;
            _quantity = quantity;
            
            // Trigger the connection on the Order side
            _order.AddOrderDetail(this);
            
            // Note: If MenuItem needed a list of OrderDetails (to track sales), we would call:
            // _menuItem.AddOrderDetail(this);
            // But MenuItem.cs provided doesn't have that list, so we skip it.
        }

        public Order Order => _order;
        public MenuItem MenuItem => _menuItem;

        public int Quantity
        {
            get => _quantity;
            set
            {
                if (value <= 0) throw new ArgumentException("Quantity must be positive.");
                _quantity = value;
            }
        }
    }
}