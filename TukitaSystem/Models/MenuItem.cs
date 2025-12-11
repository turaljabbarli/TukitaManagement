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
        public bool IsAvailable { get; set; }
        
        // Association: Menu (Many-to-Many)
        private HashSet<Menu> _menus = new HashSet<Menu>();
        public IReadOnlyCollection<Menu> Menus => _menus.ToList();
        
        // Composition/Aggregation: Ingredient (1..* / 0..*)
        private readonly HashSet<Ingredient> _ingredients = new HashSet<Ingredient>();
        public IReadOnlyCollection<Ingredient> Ingredients => _ingredients;

        public MenuItem(string name, decimal price, int calories, string preparationTime)
        {
            Name = name;
            Price = price;
            Calories = calories;
            PreparationTime = preparationTime;
            IsAvailable = true;

            _extent.Add(this);
        }

        public string Name
        {
            get => _name;
            set
            {
                if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Menu item name cannot be empty.");
                _name = value;
            }
        }

        public decimal Price
        {
            get => _price;
            set
            {
                if (value < 0) throw new ArgumentException("Price cannot be negative.");
                _price = value;
            }
        }

        public int Calories
        {
            get => _calories;
            set
            {
                if (value < 0) throw new ArgumentException("Calories cannot be negative.");
                _calories = value;
            }
        }

        public string PreparationTime
        {
            get => _preparationTime;
            set
            {
                if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Preparation time cannot be empty.");
                _preparationTime = value;
            }
        }
        
        public static MenuItem? SearchForItem(string name)
        {
            return _extent.FirstOrDefault(item => 
                item.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        // --- Menu Association (Qualified, Strict Recursion Guard) ---
        
        public void AddMenu(Menu menu)
        {
            if (menu == null) throw new ArgumentNullException(nameof(menu));
            
            // 1. THE CRITICAL CHECK (Recursion Guard)
            if (_menus.Contains(menu))
            {
                return; // Stop the loop
            }

            // 2. Add locally
            _menus.Add(menu);
            
            // 3. Trigger the reverse connection
            // We call the counter-method. Menu.AddMenuItem has its own guard to prevent infinite looping.
            menu.AddMenuItem(this);
        }

        public void RemoveMenu(Menu menu)
        {
            if (menu == null) return;

            // 1. THE CRITICAL CHECK (Guard)
            if (!_menus.Contains(menu))
            {
                return; // Nothing to remove
            }

            // 2. Remove locally
            _menus.Remove(menu);

            // 3. Trigger the reverse connection
            menu.RemoveMenuItem(this);
        }

        // --- Ingredient Aggregation (Fixed for Tests) ---
        
        public void AddIngredient(Ingredient ingredient)
        {
            if (ingredient == null) throw new ArgumentNullException(nameof(ingredient));

            // 1. Guard: Prevent duplicates
            if (_ingredients.Contains(ingredient))
            {
                return;
            }
            
            // 2. Add Locally
            _ingredients.Add(ingredient);
            
            // No reverse connection for Ingredient (as per your current model)
        }
        
        public bool RemoveIngredient(Ingredient ingredient)
        {
            if (ingredient == null) return false;
            
            // 1. Guard / Check existence
            if (!_ingredients.Contains(ingredient))
            {
                return false; // Item not found
            }
            
            // 2. Remove Locally
            _ingredients.Remove(ingredient);
            
            // No reverse connection for Ingredient
            
            return true; // Item successfully removed
        }

        public static List<MenuItem> GetExtent() => new List<MenuItem>(_extent);
        public static void SaveExtent() => StorageService.Save(_extent, FilePath);
        public static void LoadExtent() => _extent = StorageService.Load<MenuItem>(FilePath);
    }
}