using System;
using System.Collections.Generic;
using System.Linq;

namespace TukitaSystem
{
    public class Order
    {
        private static List<Order> _extent = new List<Order>();
        
        // Association with Attribute: OrderDetail
        private List<OrderDetail> _orderDetails;
        
        private Cashier _cashier;
        private Customer _customer;
        
        public DateTime TimeAdded { get; private set; }
        public OrderStatusType StatusType { get; set; }
        public Cashier Cashier { get => _cashier; set => _cashier = value; }
        public Customer Customer { get => _customer; set => _customer = value; }
        
        public IReadOnlyCollection<OrderDetail> OrderDetails => _orderDetails.AsReadOnly();

        public Order(Cashier cashier, Customer customer)
        {
            if (cashier == null) throw new ArgumentNullException(nameof(cashier));
            if (customer == null) throw new ArgumentNullException(nameof(customer));

            Cashier = cashier;
            Customer = customer;
            TimeAdded = DateTime.Now;
            StatusType = OrderStatusType.Confirmed;
            _orderDetails = new List<OrderDetail>();

            _extent.Add(this);
        }
        
        // High-level method for business logic
        public void AddItem(MenuItem item, int quantity)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            if (quantity <= 0) throw new ArgumentException("Quantity must be positive.");

            var existingDetail = _orderDetails.FirstOrDefault(od => od.MenuItem == item);
            if (existingDetail != null)
            {
                existingDetail.Quantity += quantity;
            }
            else
            {
                // This constructor triggers the association logic
                new OrderDetail(this, item, quantity);
            }
        }
        
        // --- STRICT RECURSION GUARD PATTERN (Association with Attribute) ---

        public void AddOrderDetail(OrderDetail detail)
        {
            if (detail == null) throw new ArgumentNullException(nameof(detail));

            // 1. Recursion Guard
            if (_orderDetails.Contains(detail))
            {
                return;
            }

            // 2. Add Locally
            _orderDetails.Add(detail);

            // 3. Trigger Reverse Connection (OrderDetail is the 'Link' class)
            // In Association Class patterns, the Link object usually manages the pointers in its constructor/properties.
            // If OrderDetail needed to set 'Order = this', it would happen here, but OrderDetail is immutable regarding its Order parent usually.
            if (detail.Order != this)
            {
                // This implies a logic error if we try to add an OrderDetail belonging to another order
                throw new InvalidOperationException("Cannot add an OrderDetail that belongs to a different Order.");
            }
        }

        public void RemoveOrderDetail(OrderDetail detail)
        {
            if (detail == null) throw new ArgumentNullException(nameof(detail));

            // 1. Guard
            if (!_orderDetails.Contains(detail))
            {
                return;
            }

            // 2. Remove Locally
            _orderDetails.Remove(detail);

            // 3. Reverse Connection
            // Since OrderDetail cannot exist without an Order, removing it often implies deletion/disposal.
            // We usually don't need to call back to detail because detail.Order is fixed.
            // However, if we wanted to nullify it, we'd need a SetOrder method on OrderDetail (which breaks immutability).
            // For now, removing it from the list is sufficient for this side.
        }

        public decimal FinalPrice => _orderDetails.Sum(od => od.MenuItem.Price * od.Quantity);
        
        public static List<Order> GetExtent() => new List<Order>(_extent);
    }
}