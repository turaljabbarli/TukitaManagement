using System;
using System.Collections.Generic;
using System.Linq;

namespace TukitaSystem
{
    public class Order
    {
        private static List<Order> _extent = new List<Order>();
        private List<OrderDetail> _orderDetails;
        
        // CHANGED: Cashier -> Employee
        private Employee _cashier;
        private Customer _customer;
        
        public DateTime TimeAdded { get; private set; }
        public OrderStatusType StatusType { get; set; }

        // CHANGED: Property type is Employee
        public Employee Cashier 
        { 
            get => _cashier; 
            set 
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                // VALIDATION: Ensure the employee is actually a cashier
                if (!(value.Role is CashierRole))
                {
                    throw new ArgumentException("Only an employee with the Cashier role can be assigned to an order.");
                }
                _cashier = value; 
            }
        }

        public Customer Customer { get => _customer; set => _customer = value; }
        public IReadOnlyCollection<OrderDetail> OrderDetails => _orderDetails.AsReadOnly();

        // CHANGED: Constructor accepts Employee
        public Order(Employee cashier, Customer customer)
        {
            if (cashier == null) throw new ArgumentNullException(nameof(cashier));
            if (customer == null) throw new ArgumentNullException(nameof(customer));

            // VALIDATION: Check role immediately upon creation
            if (!(cashier.Role is CashierRole))
            {
                throw new ArgumentException("The employee creating the order must be a Cashier.");
            }

            _cashier = cashier;
            Customer = customer;
            TimeAdded = DateTime.Now;
            StatusType = OrderStatusType.Confirmed;
            _orderDetails = new List<OrderDetail>();

            _extent.Add(this);
        }
        
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
                new OrderDetail(this, item, quantity);
            }
        }
        
        public void AddOrderDetail(OrderDetail detail)
        {
            if (detail != null && !_orderDetails.Contains(detail))
            {
                _orderDetails.Add(detail);
            }
        }

        public void RemoveOrderDetail(OrderDetail detail)
        {
            if (detail != null && _orderDetails.Contains(detail))
            {
                _orderDetails.Remove(detail);
            }
        }

        public decimal FinalPrice => _orderDetails.Sum(od => od.MenuItem.Price * od.Quantity);
        public static List<Order> GetExtent() => new List<Order>(_extent);
    }
}