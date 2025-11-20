using System;

namespace TukitaSystem
{
    public class Cashier : Employee
    {
        public Cashier(string name, string surname, string passportNumber, DateTime birthDate, decimal baseSalary, DateTime employmentDate)
            : base(name, surname, passportNumber, birthDate, baseSalary, employmentDate)
        {
        }
    }
}