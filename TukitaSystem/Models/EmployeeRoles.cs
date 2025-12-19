using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TukitaSystem
{
    // 1. The Abstract Base Role
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
    [JsonDerivedType(typeof(ManagerRole), "ManagerRole")]
    [JsonDerivedType(typeof(CookRole), "CookRole")]
    [JsonDerivedType(typeof(CashierRole), "CashierRole")]
    public abstract class EmployeeRole
    {
        // This makes sure we can link the role back to the employee if needed
        // But for serialization, we often keep it simple.
    }

    // 2. The Manager Role
    public class ManagerRole : EmployeeRole
    {
        private int _yearsOfExperience;
        private Training? _studentTraining;
        private List<Training> _leadingTrainings = new List<Training>();

        public RankType RankType { get; set; }

        public int YearsOfExperience
        {
            get => _yearsOfExperience;
            set
            {
                if (value < 0) throw new ArgumentException("Years of experience cannot be negative.");
                _yearsOfExperience = value;
            }
        }

        public ManagerRole(RankType rankType, int yearsOfExperience)
        {
            RankType = rankType;
            YearsOfExperience = yearsOfExperience;
        }

        // NOTE: You will need to update your Training class to accept 'ManagerRole' instead of 'Manager'
        public Training? StudentTraining => _studentTraining;

        public void SetStudentTraining(Training? training)
        {
            if (_studentTraining == training) return;

            if (_studentTraining != null)
            {
                var oldTraining = _studentTraining;
                _studentTraining = null;
                // Assuming Training.SetManagerStudent now accepts ManagerRole
                oldTraining.SetManagerStudent(null);
            }

            _studentTraining = training;

            if (_studentTraining != null && _studentTraining.ManagerStudent != this)
            {
                _studentTraining.SetManagerStudent(this);
            }
        }

        public IReadOnlyCollection<Training> LeadingTrainings => _leadingTrainings.AsReadOnly();

        public void AddLeadingTraining(Training training)
        {
            if (training == null) throw new ArgumentNullException(nameof(training));
            if (this.RankType != RankType.Lead) throw new InvalidOperationException("Only Lead managers can train.");

            if (!_leadingTrainings.Contains(training))
            {
                _leadingTrainings.Add(training);
                if (training.ManagerTeacher != this)
                {
                    // Assuming Training.SetManagerTeacher now accepts ManagerRole
                    training.SetManagerTeacher(this);
                }
            }
        }

        public void RemoveLeadingTraining(Training training)
        {
            if (training == null) return;
            if (_leadingTrainings.Contains(training))
            {
                _leadingTrainings.Remove(training);
                if (training.ManagerTeacher == this)
                {
                    training.SetManagerTeacher(null);
                }
            }
        }
    }

    // 3. The Cook Role
    public class CookRole : EmployeeRole
    {
        private List<string> _knownDishes;
        private string _education;
        private readonly List<MenuItem> _signatureDishes = new();

        public string Education
        {
            get => _education;
            set
            {
                if (value == null) throw new ArgumentException("Education cannot be null");
                _education = value;
            }
        }

        public List<string> KnownDishes
        {
            get => _knownDishes;
            set
            {
                if (value == null) throw new ArgumentNullException("Known dishes list cannot be null");
                _knownDishes = value;
            }
        }

        public IReadOnlyCollection<MenuItem> SignatureDishes => _signatureDishes.AsReadOnly();

        // Constructor enforces the "Must have 1 signature dish" rule
        public CookRole(string education, List<MenuItem> signatureDishes)
        {
            if (signatureDishes == null || signatureDishes.Count == 0)
                throw new InvalidOperationException("Cook must have at least one signature dish");

            Education = education;
            _knownDishes = new List<string>();

            foreach (var dish in signatureDishes)
                AddSignatureDish(dish);
        }

        public void AddSignatureDish(MenuItem item)
        {
            if (_signatureDishes.Contains(item)) return;

            _signatureDishes.Add(item);
            // NOTE: You must update MenuItem.AddCook to accept 'CookRole' instead of 'Cook'
            item.AddCook(this);
        }

        public void RemoveSignatureDish(MenuItem item)
        {
            if (!_signatureDishes.Contains(item)) return;
            if (_signatureDishes.Count == 1)
                throw new InvalidOperationException("Cannot remove the last signature dish.");

            _signatureDishes.Remove(item);
            item.RemoveCook(this);
        }

        public void AddDish(string dish)
        {
            if (string.IsNullOrWhiteSpace(dish)) throw new ArgumentException("Dish name cannot be empty");
            _knownDishes.Add(dish);
        }
    }

    // 4. The Cashier Role
    public class CashierRole : EmployeeRole
    {
        // Cashier specific logic can go here later
        public CashierRole() { }
    }
}