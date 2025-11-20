using System;

namespace TukitaSystem
{
    public class Manager : Employee
    {
        private int _yearsOfExperience;

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
    }
}