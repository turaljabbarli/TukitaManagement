using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace TukitaSystem
{
    // Removed [JsonDerivedType] because Employee is now a single concrete class
    public class Employee
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
        
        // COMPOSITION: Employee HAS A Role
        // This property allows the behavior to change dynamically
        public EmployeeRole Role { get; private set; }

        private readonly List<ShiftAssignment> _assignments = new();
        public IReadOnlyList<ShiftAssignment> Assignments => _assignments.AsReadOnly();

        // Constructor now requires a Role
        public Employee(string name, string surname, string passportNumber, DateTime birthDate, decimal baseSalary, DateTime employmentDate, EmployeeRole role)
        {
            Name = name;
            Surname = surname;
            PassportNumber = passportNumber;
            BirthDate = birthDate;
            BaseSalary = baseSalary;
            EmploymentDate = employmentDate;
            
            // Assign the initial role (e.g., new CashierRole())
            Role = role ?? throw new ArgumentNullException(nameof(role));

            _extent.Add(this);
        }

        // DYNAMIC INHERITANCE: The method to change roles at runtime
        public void ChangeRole(EmployeeRole newRole)
        {
            if (newRole == null) throw new ArgumentNullException(nameof(newRole));
            
            // Optional: Add logic to clean up old role (e.g., remove from trainings)
            // if (Role is ManagerRole oldManager) { ... cleanup ... }

            Role = newRole;
        }

        // --- Standard Properties (Unchanged) ---
        public string? PeselNumber
        {
            get => _peselNumber;
            set
            {
                if (!IsValidPesel(value)) throw new ArgumentException($"Invalid PESEL number: {value}");
                _peselNumber = value;
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Name cannot be empty or whitespace");
                _name = value;
            }
        }

        public string Surname
        {
            get => _surname;
            set
            {
                if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Surname cannot be empty or whitespace");
                _surname = value;
            }
        }
        
        public string PassportNumber
        {
            get => _passportNumber;
            set
            {
                if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Passport number cannot be empty or whitespace");
                _passportNumber = value;
            }
        }

        public DateTime BirthDate
        {
            get => _birthDate;
            set
            {
                if (value > DateTime.Now) throw new ArgumentException("Birth date cannot be in the future");
                _birthDate = value;
            }
        }

        public decimal BaseSalary
        {
            get => _baseSalary;
            set
            {
                if (value < 0) throw new ArgumentException("Salary cannot be negative");
                _baseSalary = value;
            }
        }

        public DateTime EmploymentDate
        {
            get => _employmentDate;
            set
            {
                if (value > DateTime.Now) throw new ArgumentException("Employment date cannot be in the future");
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
        
        // --- Persistence & Helper Methods (Unchanged) ---
        public static List<Employee> SearchForEmployee(string? name = null, string? surname = null, string? passportNumber = null, string? peselNumber = null)
        {
            return _extent.Where(e =>
                (name == null || e.Name.Equals(name, StringComparison.OrdinalIgnoreCase)) &&
                (surname == null || e.Surname.Equals(surname, StringComparison.OrdinalIgnoreCase)) &&
                (passportNumber == null || e.PassportNumber.Equals(passportNumber, StringComparison.OrdinalIgnoreCase)) &&
                (peselNumber == null || e.PeselNumber == peselNumber)
            ).ToList();
        }

        public void AddAssignment(ShiftAssignment assignment)
        {
            if (_assignments.Contains(assignment)) throw new InvalidOperationException("Duplicate ShiftAssignment");
            _assignments.Add(assignment);
        }

        public void RemoveAssignment(ShiftAssignment assignment)
        {
            if (!_assignments.Contains(assignment)) throw new InvalidOperationException("Assignment not found");
            _assignments.Remove(assignment);
        }

        public static List<Employee> GetExtent() => new List<Employee>(_extent);
        public static void ClearExtent() => _extent.Clear();
        public static void SaveExtent() => StorageService.Save(_extent, FilePath);
        public static void LoadExtent() => _extent = StorageService.Load<Employee>(FilePath);

        private bool IsValidPesel(string? pesel)
        {
            if (pesel == null) return true;
            if (pesel.Length != 11) return false;
            if (!pesel.All(char.IsDigit)) return false;
            int[] weights = { 1, 3, 7, 9, 1, 3, 7, 9, 1, 3 };
            int sum = 0;
            for (int i = 0; i < 10; i++) sum += (pesel[i] - '0') * weights[i];
            int lastDigit = sum % 10;
            int checkDigit = (10 - lastDigit) % 10;
            return checkDigit == (pesel[10] - '0');
        }
    }
}