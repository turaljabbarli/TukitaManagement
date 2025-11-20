using System;
using System.Collections.Generic;
using System.Linq;

namespace TukitaSystem
{
    public class Order
    {
        private static List<Order> _extent = new List<Order>();

        private Dictionary<MenuItem, int> _items;
        private Cashier _cashier;
        private Customer _customer;

        public Order(Cashier cashier, Customer customer)
        {
            if (cashier == null) throw new ArgumentNullException(nameof(cashier));
            if (customer == null) throw new ArgumentNullException(nameof(customer));

            Cashier = cashier;
            Customer = customer;
            TimeAdded = DateTime.Now;
            StatusType = OrderStatusType.Confirmed;
            _items = new Dictionary<MenuItem, int>();

            _extent.Add(this);
        }

        public DateTime TimeAdded { get; private set; }
        public OrderStatusType StatusType { get; set; }

        public Cashier Cashier
        {
            get => _cashier;
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                _cashier = value;
            }
        }

        public Customer Customer
        {
            get => _customer;
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                _customer = value;
            }
        }

        public Dictionary<MenuItem, int> Items => new Dictionary<MenuItem, int>(_items);

        public void AddItem(MenuItem item, int quantity)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            if (quantity <= 0) throw new ArgumentException("Quantity must be positive.");

            if (_items.ContainsKey(item))
            {
                _items[item] += quantity;
            }
            else
            {
                _items.Add(item, quantity);
            }
        }

        public decimal FinalPrice
        {
            get
            {
                decimal total = 0;
                foreach (var entry in _items)
                {
                    total += entry.Key.Price * entry.Value;
                }
                return total;
            }
        }

        public static List<Order> GetExtent()
        {
            return new List<Order>(_extent);
        }
    }
}