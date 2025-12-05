using System;
using System.Collections.Generic;

namespace TukitaSystem
{
    public class Ingredient
    {
        private static List<Ingredient> _extend = new List<Ingredient>();

        private string _name;
        private int _currentStock;
        
        public bool IsVegetarian { get; set; }
        public bool IsAllergic { get; set; }

        public Ingredient(string name, int currentStock, bool isVegetarian, bool isAllergic)
        {
            Name = name;
            CurrentStock = currentStock;
            IsVegetarian = isVegetarian;
            IsAllergic = isAllergic;

            _extend.Add(this);
        }

        public string Name
        {
            get => _name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Ingredient name cannot be empty.");
                _name = value;
            }
        }

        public int CurrentStock
        {
            get => _currentStock;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Stock cannot be negative.");
                _currentStock = value;
            }
        }

        public static List<Ingredient> GetExtend()
        {
            return new List<Ingredient>(_extend);
        }
    }
}