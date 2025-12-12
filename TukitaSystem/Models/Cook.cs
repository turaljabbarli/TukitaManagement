using System;
using System.Collections.Generic;

namespace TukitaSystem
{
    public class Cook : Employee
    {
        private List<string> _knownDishes;
        private string _education;
        
        private readonly List<MenuItem> _signatureDishes = new();
        public IReadOnlyCollection<MenuItem> SignatureDishes => _signatureDishes.AsReadOnly();

        public Cook(string name, string surname, string passportNumber, DateTime birthDate, decimal baseSalary, DateTime employmentDate, string education, List<MenuItem> signatureDishes)
            : base(name, surname, passportNumber, birthDate, baseSalary, employmentDate)
        {

            if (signatureDishes == null || signatureDishes.Count == 0)
            {
                throw new InvalidOperationException("Cook must have at least one signature dish");
            }
            
            Education = education;
            _knownDishes = new List<string>();
            
            foreach (var dish in signatureDishes)
                AddSignatureDish(dish);
        }

        public string Education
        {
            get => _education;
            set
            {
                if (value == null)
                {
                    throw new ArgumentException("Education cannot be null");
                }
                _education = value;
            }
        }

        public List<string> KnownDishes
        {
            get => _knownDishes;
            set
            {
                if (value == null)
                    throw new ArgumentNullException("Known dishes list cannot be null");
                _knownDishes = value;
            }
        }

        public void AddSignatureDish(MenuItem item)
        {
            if (_signatureDishes.Contains(item))
                return;

            _signatureDishes.Add(item);
            item.AddCook(this);
        }
        
        public void RemoveSignatureDish(MenuItem item)
        {
            if (!_signatureDishes.Contains(item))
                return;

            if (_signatureDishes.Count == 1)
                throw new InvalidOperationException("Cannot remove the last signature dish. A cook must have at least one");

            _signatureDishes.Remove(item);
            item.RemoveCook(this);
        }

        public void AddDish(string dish)
        {
            if (string.IsNullOrWhiteSpace(dish))
                throw new ArgumentException("Dish name cannot be empty");
            _knownDishes.Add(dish);
        }
    }
}