using System;
using System.Collections.Generic;

namespace TukitaSystem
{
    public class LoyaltyCard
    {
        private static List<LoyaltyCard> _extent = new List<LoyaltyCard>();
        private static readonly string FilePath = "loyaltyCards.json";

        private string _cardNumber;
        private int _loyaltyPoints;
        private Customer _customer;
        
        public Customer Customer => _customer;
        public DateTime CreationDate { get; set; }
        public DateTime ExpiryDate { get; set; }

        public LoyaltyCard(Customer customer, string cardNumber, DateTime creationDate, DateTime expiryDate)
        {
            if (customer == null) throw new ArgumentNullException(nameof(customer));
            if (expiryDate <= creationDate)
                throw new ArgumentException("Expiry date must be after creation date.");

            _customer = customer;
            
            _customer.SetLoyaltyCard(this);

            CardNumber = cardNumber;
            CreationDate = creationDate;
            ExpiryDate = expiryDate;
            LoyaltyPoints = 0;

            _extent.Add(this);
        }

        public string CardNumber
        {
            get => _cardNumber;
            set
            {
                if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Card number cannot be empty.");
                _cardNumber = value;
            }
        }

        public int LoyaltyPoints
        {
            get => _loyaltyPoints;
            set
            {
                if (value < 0) throw new ArgumentException("Loyalty points cannot be negative.");
                _loyaltyPoints = value;
            }
        }

        public void AddPoints(int points)
        {
            if (points < 0) throw new ArgumentException("Cannot add negative points.");
            LoyaltyPoints += points;
        }

        public static List<LoyaltyCard> GetExtent() => new List<LoyaltyCard>(_extent);
        
        public void Destroy()
        {
            if (_customer != null)
            {
                // Unlink from customer side
                _customer.SetLoyaltyCard(null);
            }
            
            _customer = null!; // Mark as null
            _extent.Remove(this);
        }
        
        public static void SaveExtent() => StorageService.Save(_extent, FilePath);
        public static void LoadExtent() => _extent = StorageService.Load<LoyaltyCard>(FilePath);
    }
}