using System;
using System.Collections.Generic;

namespace TukitaSystem
{
    public class Manager : Employee
    {
        private int _yearsOfExperience;

        private Training? _studentTraining;

        private List<Training> _leadingTrainings = new List<Training>();

        public Manager(string name, string surname, string passportNumber, DateTime birthDate, decimal baseSalary, DateTime employmentDate, RankType rankType, int yearsOfExperience)
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


        public Training? StudentTraining => _studentTraining;

        public void SetStudentTraining(Training? training)
        {
            if (_studentTraining == training) return;

            if (_studentTraining != null)
            {
                var oldTraining = _studentTraining;
                _studentTraining = null;
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

            if (this.RankType != RankType.Lead)
            {
                throw new InvalidOperationException("This manager does not have the 'Lead' rank and cannot conduct training.");
            }

            if (!_leadingTrainings.Contains(training))
            {
                _leadingTrainings.Add(training);

                if (training.ManagerTeacher != this)
                {
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
}