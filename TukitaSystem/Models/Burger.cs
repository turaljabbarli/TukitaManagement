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
            Patties = patties;
            Sauces = new List<SauceType>();
        }

        public List<PattyType> Patties
        {
            get => _patties;
            set
            {
                if (value == null || value.Count == 0)
                    throw new ArgumentException("A burger must have at least one patty.", nameof(Patties));

                _patties = value;
            }
        }

        public List<SauceType> Sauces
        {
            get => _sauces;
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(Sauces), "Sauces list cannot be null.");

                _sauces = value;
            }
        }

        public bool IsVegetarian
        {
            get
            {
                bool pattiesAreVeg = _patties.All(p => p == PattyType.Bean || p == PattyType.Mushroom);
                return pattiesAreVeg;
            }
        }

        public void AddSauce(SauceType sauce)
        {
            _sauces.Add(sauce);
        }
    }
}