using System;
using System.Collections.Generic;

namespace TukitaSystem
{
    public class Customer
    {
        private static List<Customer> _extent = new List<Customer>();

        private string _name;
        private string _email;

        public Customer(string name, string email)
        {
            Name = name;
            Email = email;
            _extent.Add(this);
        }

        public string Name
        {
            get => _name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Customer name cannot be empty.");
                _name = value;
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Email cannot be empty.");
                if (!value.Contains("@"))
                    throw new ArgumentException("Invalid email format.");
                _email = value;
            }
        }

        // Association: 0..1 LoyaltyCard
        public LoyaltyCard? LoyaltyCard { get; set; }

        public void AssignLoyaltyCard(LoyaltyCard card)
        {
            LoyaltyCard = card ?? throw new ArgumentNullException(nameof(card));
        }

        public static List<Customer> GetExtent()
        {
            return new List<Customer>(_extent);
        }
    }
}