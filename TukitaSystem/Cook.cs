using System;
using System.Collections.Generic;

namespace TukitaSystem
{
    public class Cook : Employee
    {
        private string? _signatureDish;
        private List<string> _knownDishes;
        private Education _education;

        public Cook(string name, string surname, DateTime birthDate, decimal baseSalary, DateTime employmentDate, Education education)
            : base(name, surname, birthDate, baseSalary, employmentDate)
        {
            Education = education;
            _knownDishes = new List<string>();
        }

        public Education Education
        {
            get => _education;
            set
            {
                if (value == null)
                    throw new ArgumentNullException("Education cannot be null.");
                _education = value;
            }
        }

        public List<string> KnownDishes
        {
            get => _knownDishes;
            set
            {
                if (value == null)
                    throw new ArgumentNullException("Known dishes list cannot be null.");
                _knownDishes = value;
            }
        }

        public string? SignatureDish
        {
            get => _signatureDish;
            set
            {
                if (value != null && string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Signature dish cannot be empty if provided.");
                _signatureDish = value;
            }
        }

        public void AddDish(string dish)
        {
            if (string.IsNullOrWhiteSpace(dish))
                throw new ArgumentException("Dish name cannot be empty.");
            _knownDishes.Add(dish);
        }
    }
}