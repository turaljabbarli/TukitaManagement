using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TukitaSystem
{
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
    [JsonDerivedType(typeof(Burger), "Burger")]
    [JsonDerivedType(typeof(Dessert), "Dessert")]
    [JsonDerivedType(typeof(Drink), "Drink")]
    public abstract class MenuItem
    {
        private static List<MenuItem> _extent = new List<MenuItem>();
        private static readonly string FilePath = "menuItems.json";

        private string _name;
        private decimal _price;
        private int _calories;
        private string _preparationTime;

        public MenuItem(string name, decimal price, int calories, string preparationTime)
        {
            Name = name;
            Price = price;
            Calories = calories;
            PreparationTime = preparationTime;
            IsAvailable = true;

            _extent.Add(this);
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

        public string PreparationTime
        {
            get => _preparationTime;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Preparation time cannot be empty..");
                }

                _preparationTime = value;
            }
        }
        
        public static MenuItem SearchForItem(string name)
        {
            return _extent.FirstOrDefault(item => 
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
            return new List<MenuItem>(_extent);
        }

        public static void SaveExtent()
        {
            StorageService.Save(_extent, FilePath);
        }

        public static void LoadExtent()
        {
            _extent = StorageService.Load<MenuItem>(FilePath);
        }
    }
}