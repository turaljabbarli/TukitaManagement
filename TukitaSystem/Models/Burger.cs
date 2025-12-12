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
            
            if (patties == null || patties.Count == 0)
                throw new ArgumentException("A burger must have at least one patty.", nameof(patties));

            _patties = patties;
            _sauces = new List<SauceType>();
        }

        
        public List<PattyType> Patties
        {
            get => new List<PattyType>(_patties);
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

        
        public List<SauceType> Sauces
        {
            get => new List<SauceType>(_sauces); 
            private set
            {
                
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
            
        }

        public bool IsVegetarian
        {
            get
            {
                
                return _patties.All(p => p == PattyType.Bean || p == PattyType.Mushroom);
            }
        }
    }
}
