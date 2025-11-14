using System;

namespace TukitaSystem
{
    public class Cashier : Employee
    {
        public Cashier(string name, string surname, DateTime birthDate, decimal baseSalary, DateTime employmentDate)
            : base(name, surname, birthDate, baseSalary, employmentDate)
        {
        }
    }
}