using System;
using System.Collections.Generic;

namespace TukitaSystem
{
    public class Manager : Employee
    {
        private int _yearsOfExperience;
        
        // Association: Training (0..*)
        private List<Training> _trainings = new List<Training>();

        public Manager(
            string name, 
            string surname, 
            string passportNumber, 
            DateTime birthDate, 
            decimal baseSalary, 
            DateTime employmentDate, 
            RankType rankType, 
            int yearsOfExperience)
            : base(name, surname, passportNumber, birthDate, baseSalary, employmentDate)
        {
            RankType = rankType;
            YearsOfExperience = yearsOfExperience;
        }

        public RankType RankType { get; set; }

        public int YearsOfExperience
        {
            get => _yearsOfExperience;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Years of experience cannot be negative.");
                _yearsOfExperience = value;
            }
        }
        
        public IReadOnlyCollection<Training> Trainings => _trainings.AsReadOnly();

        // --- STRICT RECURSION GUARD PATTERN ---

        public void AddTraining(Training training)
        {
            if (training == null) throw new ArgumentNullException(nameof(training));

            // Business Logic: Only Lead managers can train
            if (this.RankType != RankType.Lead)
            {
                throw new InvalidOperationException("This manager does not have the 'Lead' rank and cannot conduct training.");
            }

            // 1. THE CRITICAL CHECK (Recursion Guard)
            if (_trainings.Contains(training))
            {
                return; // Stop loop
            }

            // 2. Add locally
            _trainings.Add(training);
            
            // 3. Trigger reverse connection
            // For a reference type (0..1), we use the Setter
            training.SetManager(this);
        }

        public void RemoveTraining(Training training)
        {
            if (training == null) return;

            // 1. THE CRITICAL CHECK (Guard)
            if (!_trainings.Contains(training))
            {
                return; // Nothing to remove
            }

            // 2. Remove locally
            _trainings.Remove(training);

            // 3. Trigger reverse connection
            // We set the reference on the other side to null
            training.SetManager(null);
        }
    }
}