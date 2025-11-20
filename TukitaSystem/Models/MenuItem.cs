using System;
using System.Collections.Generic;
using System.Linq;

namespace TukitaSystem
{
    public abstract class MenuItem
    {
        private static List<MenuItem> _extend = new List<MenuItem>();

        private string _name;
        private decimal _price;
        private int _calories;
        private int _preparationTime;

        public MenuItem(string name, decimal price, int calories, int preparationTime)
        {
            Name = name;
            Price = price;
            Calories = calories;
            PreparationTime = preparationTime;
            IsAvailable = true;

            _extend.Add(this);
        }

        public bool IsAvailable { get; set; }
        public string Name
        {
            get => _name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Menu item name cannot be empty.");
                }

                _name = value;
            }
        }

        public decimal Price
        {
            get => _price;
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Price cannot be negative.");
                }

                _price = value;
            }
        }

        public int Calories
        {
            get => _calories;
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Calories cannot be negative.");
                }

                _calories = value;
            }
        }

        public int PreparationTime
        {
            get => _preparationTime;
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("Preparation time must be greater than zero.");
                }

                _preparationTime = value;
            }
        }
        
        public static MenuItem SearchForItem(string name)
        {
            return _extend.FirstOrDefault(item => 
                item.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }
        
        public void AddItemToOrder(List<MenuItem> order)
        {
            if (IsAvailable)
                order.Add(this);
        }
        
        public void AddIngredient(Ingredient ingredient, int quantity)
        {
            if (ingredient == null)
            {
                throw new ArgumentNullException(nameof(ingredient));
            }

            if (quantity <= 0)
            {
                throw new ArgumentException("Quantity must be positive.");
            }
        }

        public static List<MenuItem> GetExtend()
        {
            return new List<MenuItem>(_extend);
        }
    }
}