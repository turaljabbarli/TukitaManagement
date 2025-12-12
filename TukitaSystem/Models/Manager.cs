using System;
using System.Collections.Generic;

namespace TukitaSystem
{
    public class Manager : Employee
    {
        private int _yearsOfExperience;

        // 1. Переменная для обучения, где менеджер является СТУДЕНТОМ (только одна за раз)
        private Training? _studentTraining;

        // 2. Список обучений, где менеджер является УЧИТЕЛЕМ (много трейнингов)
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

        // --- Логика Студента ---

        public Training? StudentTraining => _studentTraining;

        public void SetStudentTraining(Training? training)
        {
            // Если тренинг тот же самый, ничего не делаем
            if (_studentTraining == training) return;

            // Если уже есть активный тренинг, убираем себя из него (разрываем старую связь)
            if (_studentTraining != null)
            {
                var oldTraining = _studentTraining;
                _studentTraining = null;
                oldTraining.SetManagerStudent(null);
            }

            _studentTraining = training;

            // Настраиваем обратную связь в классе Training
            if (_studentTraining != null && _studentTraining.ManagerStudent != this)
            {
                _studentTraining.SetManagerStudent(this);
            }
        }

        // --- Логика Учителя (Lead) ---

        public IReadOnlyCollection<Training> LeadingTrainings => _leadingTrainings.AsReadOnly();

        public void AddLeadingTraining(Training training)
        {
            if (training == null) throw new ArgumentNullException(nameof(training));

            // Проверка ранга: только Lead может вести тренинги
            if (this.RankType != RankType.Lead)
            {
                throw new InvalidOperationException("This manager does not have the 'Lead' rank and cannot conduct training.");
            }

            if (!_leadingTrainings.Contains(training))
            {
                _leadingTrainings.Add(training);

                // Обратная связь: говорим тренингу, кто его учитель
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

                // Обратная связь: убираем учителя у тренинга
                if (training.ManagerTeacher == this)
                {
                    training.SetManagerTeacher(null);
                }
            }
        }
    }
}