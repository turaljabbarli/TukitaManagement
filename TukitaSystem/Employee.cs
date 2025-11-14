using System;
using System.Collections.Generic;
using System.Linq;

namespace TukitaSystem
{
    public abstract class Employee
    {
        private static List<Employee> _extent = new List<Employee>();

        private string _name;
        private string _surname;
        private DateTime _birthDate;
        private decimal _baseSalary;
        private DateTime _employmentDate;

        public Employee(string name, string surname, DateTime birthDate, decimal baseSalary, DateTime employmentDate)
        {
            Name = name;
            Surname = surname;
            BirthDate = birthDate;
            BaseSalary = baseSalary;
            EmploymentDate = employmentDate;

            _extent.Add(this);
        }

        public string Name
        {
            get => _name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Name cannot be empty or whitespace.");
                _name = value;
            }
        }

        public string Surname
        {
            get => _surname;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Surname cannot be empty or whitespace.");
                _surname = value;
            }
        }

        public DateTime BirthDate
        {
            get => _birthDate;
            set
            {
                if (value > DateTime.Now)
                    throw new ArgumentException("Birth date cannot be in the future.");
                _birthDate = value;
            }
        }

        public decimal BaseSalary
        {
            get => _baseSalary;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Salary cannot be negative.");
                _baseSalary = value;
            }
        }

        public DateTime EmploymentDate
        {
            get => _employmentDate;
            set
            {
                if (value > DateTime.Now)
                    throw new ArgumentException("Employment date cannot be in the future.");
                _employmentDate = value;
            }
        }

        public int Age
        {
            get
            {
                var today = DateTime.Today;
                var age = today.Year - BirthDate.Year;
                if (BirthDate.Date > today.AddYears(-age)) age--;
                return age;
            }
        }

        public decimal CurrentSalary
        {
            get
            {
                var yearsWorked = (DateTime.Today.Year - EmploymentDate.Year);
                if (EmploymentDate.Date > DateTime.Today.AddYears(-yearsWorked)) yearsWorked--;

                if (yearsWorked < 0) yearsWorked = 0;

                decimal multiplier = (decimal)Math.Pow(1.03, yearsWorked);
                return Math.Round(_baseSalary * multiplier, 2);
            }
        }

        public static List<Employee> GetExtent()
        {
            return new List<Employee>(_extent);
        }
    }
}