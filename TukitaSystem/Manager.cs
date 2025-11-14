using System;

namespace TukitaSystem
{
    public class Manager : Employee
    {
        private int _yearsOfExperience;

        public Manager(string name, string surname, DateTime birthDate, decimal baseSalary, DateTime employmentDate, Rank rank, int yearsOfExperience)
            : base(name, surname, birthDate, baseSalary, employmentDate)
        {
            Rank = rank;
            YearsOfExperience = yearsOfExperience;
        }
        public Rank Rank { get; set; }

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