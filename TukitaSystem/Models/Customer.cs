using System;
using System.Collections.Generic;

namespace TukitaSystem
{
    public class Customer
    {
        private static List<Customer> _extent = new List<Customer>();
        private static readonly string FilePath = "customers.json";

        private string _name;
        private string _email;
        private LoyaltyCard? _loyaltyCard;
        
        // Свойство только для чтения извне, изменение управляется методами связи
        public LoyaltyCard? LoyaltyCard => _loyaltyCard;

        public Customer(string name, string email)
        {
            Name = name;
            Email = email;
            _extent.Add(this);
        }

        // ... Свойства Name и Email без изменений ...
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

        public void CreateLoyaltyCard(string cardNumber, DateTime expiryDate)
        {
            if (_loyaltyCard != null)
                throw new InvalidOperationException("Customer already has a loyalty card.");
            
            // Мы просто вызываем конструктор Карты.
            // Благодаря Reverse Connection в конструкторе Карты,
            // поле _loyaltyCard у этого клиента заполнится АВТОМАТИЧЕСКИ.
            //new LoyaltyCard(this, cardNumber, DateTime.Now, expiryDate);//----------------------------------------------------!!!!!!!!!!!!!
        }

        public void RemoveLoyaltyCard()
        {
            if (_loyaltyCard != null)
            {
                _loyaltyCard.Destroy(); // Карта сама обнулит ссылку в Customer
                // _loyaltyCard = null; // Эту строку можно убрать, так как Destroy вызовет SetLoyaltyCard(null)
            }
        }

        // === МЕТОД ДЛЯ REVERSE CONNECTION ===
        // Этот метод вызывается ТОЛЬКО из LoyaltyCard, чтобы синхронизировать связь
        public void SetLoyaltyCard(LoyaltyCard? card)
        {
            // Если пытаемся присвоить карту, а она уже есть (и это не та же самая), кидаем ошибку
            if (card != null && _loyaltyCard != null && _loyaltyCard != card)
            {
                throw new InvalidOperationException("Customer already has a loyalty card assigned.");
            }
            
            _loyaltyCard = card;
        }
        
        public static void RemoveCustomer(Customer customer)
        {
            if (customer == null) throw new ArgumentNullException(nameof(customer));
            
            if (customer._loyaltyCard != null)
            {
                customer.RemoveLoyaltyCard();
            }
            _extent.Remove(customer);
        }

        // ... GetExtent, SaveExtent, LoadExtent без изменений ...
        public static List<Customer> GetExtent() => new List<Customer>(_extent);
        public static void SaveExtent() => StorageService.Save(_extent, FilePath);
        public static void LoadExtent() => _extent = StorageService.Load<Customer>(FilePath);
    }
}