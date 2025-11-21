using System;
using System.Collections.Generic;

namespace TukitaSystem
{
    public class Dessert : MenuItem
    {
        private List<FlavorType> _flavors;

        public Dessert(string name, decimal price, int calories, string preparationTime, bool isFrozen, List<FlavorType> flavors)
            : base(name, price, calories, preparationTime)
        {
            IsFrozen = isFrozen;
            Flavors = flavors;
        }

        public bool IsFrozen { get; set; }

        public List<FlavorType> Flavors
        {
            get => _flavors;
            set
            {
                if (value == null || value.Count == 0)
                    throw new ArgumentException("Dessert must have at least one flavor.");
                _flavors = value;
            }
        }
    }
}