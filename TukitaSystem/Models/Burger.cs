using System;
using System.Collections.Generic;
using System.Linq;

namespace TukitaSystem
{
    public class Burger : MenuItem
    {
        private List<PattyType> _patties;
        private List<SauceType> _sauces;

        public Burger(
            string name, 
            decimal price, 
            int calories, 
            string preparationTime, 
            List<PattyType> patties)
            : base(name, price, calories, preparationTime)
        {
            // Validate patties immediately upon creation
            if (patties == null || patties.Count == 0)
                throw new ArgumentException("A burger must have at least one patty.", nameof(patties));

            _patties = patties;
            _sauces = new List<SauceType>(); // Initialize empty sauce list
        }

        // Multi-Value Attribute: Patties
        public List<PattyType> Patties
        {
            get => new List<PattyType>(_patties); // Return a copy to protect internal state
            private set 
            {
                if (value == null || value.Count == 0)
                    throw new ArgumentException("A burger must have at least one patty.", nameof(Patties));
                _patties = value;
            }
        }

        public void AddPatty(PattyType patty)
        {
            _patties.Add(patty);
        }

        public void RemovePatty(PattyType patty)
        {
            if (_patties.Count <= 1)
                throw new InvalidOperationException("Cannot remove patty. A burger must have at least one patty.");
            
            if (!_patties.Contains(patty))
                throw new ArgumentException("The specified patty is not in this burger.");

            _patties.Remove(patty);
        }

        // Multi-Value Attribute: Sauces
        public List<SauceType> Sauces
        {
            get => new List<SauceType>(_sauces); // Return a copy
            private set
            {
                // We allow empty list, but not null list to ensure stability
                _sauces = value ?? new List<SauceType>();
            }
        }

        public void AddSauce(SauceType sauce)
        {
            _sauces.Add(sauce);
        }

        public void RemoveSauce(SauceType sauce)
        {
            if (_sauces.Contains(sauce))
            {
                _sauces.Remove(sauce);
            }
            // Optional: throw exception if sauce not found, or just ignore. 
            // Given "Stoic" approach, we perform the action if possible.
        }

        public bool IsVegetarian
        {
            get
            {
                // Check if all patties are vegetarian options
                return _patties.All(p => p == PattyType.Bean || p == PattyType.Mushroom);
            }
        }
    }
}