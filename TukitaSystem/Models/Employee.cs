using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TukitaSystem
{
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
    [JsonDerivedType(typeof(Cashier), "Cashier")]
    [JsonDerivedType(typeof(Manager), "Manager")]
    [JsonDerivedType(typeof(Cook), "Cook")]
    public abstract class Employee
    {
        private static List<Employee> _extent = new List<Employee>();
        private static readonly string FilePath = "employees.json";

        private string _name;
        private string _surname;
        private string? _peselNumber;
        private string _passportNumber;
        private DateTime _birthDate;
        private decimal _baseSalary;
        private DateTime _employmentDate;
        
        public List<Shift> Shifts { get; private set; } = new();

        public Employee(string name, string surname, string passportNumber, DateTime birthDate, decimal baseSalary, DateTime employmentDate)
        {
            Name = name;
            Surname = surname;
            PassportNumber = passportNumber;
            BirthDate = birthDate;
            BaseSalary = baseSalary;
            EmploymentDate = employmentDate;

            _extent.Add(this);
        }
        
        public string? PeselNumber
        {
            get => _peselNumber;
            set => _peselNumber = value;
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
        
        public string PassportNumber
        {
            get => _passportNumber;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Passport number cannot be empty or whitespace.");
                _passportNumber = value;
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
        
        public static List<Employee> SearchForEmployee(
            string? name = null,
            string? surname = null,
            string? passportNumber = null,
            string? peselNumber = null)
        {
            return _extent.Where(e =>
                (name == null || e.Name.Equals(name, StringComparison.OrdinalIgnoreCase)) &&
                (surname == null || e.Surname.Equals(surname, StringComparison.OrdinalIgnoreCase)) &&
                (passportNumber == null || e.PassportNumber.Equals(passportNumber, StringComparison.OrdinalIgnoreCase)) &&
                (peselNumber == null || e.PeselNumber == peselNumber)
            ).ToList();
        }

        public void AddShift(Shift shift)
        {
            if (!Shifts.Contains(shift)){
                Shifts.Add(shift);
                shift.AddEmployee(this);
            }
        }

        public void RemoveShift(Shift shift)
        {
            Shifts.Remove(shift);
        }

        public static List<Employee> GetExtent()
        {
            return new List<Employee>(_extent);
        }
        
        public static void ClearExtent()
        {
            _extent.Clear();
        }
        
        public static void SaveExtent()
        {
            StorageService.Save(_extent, FilePath);
        }

        public static void LoadExtent()
        {
            _extent = StorageService.Load<Employee>(FilePath);
        }
    }
}