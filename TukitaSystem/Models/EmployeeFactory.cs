using System;
using System.Collections.Generic;

namespace TukitaSystem
{
    public static class EmployeeFactory
    {
        public static Employee CreateCashier(string name, string surname, string passportNumber, DateTime birthDate, decimal baseSalary, DateTime employmentDate)
        {
            var role = new CashierRole();
            return new Employee(name, surname, passportNumber, birthDate, baseSalary, employmentDate, role);
        }

        public static Employee CreateManager(string name, string surname, string passportNumber, DateTime birthDate, decimal baseSalary, DateTime employmentDate, RankType rankType, int yearsOfExperience)
        {
            var role = new ManagerRole(rankType, yearsOfExperience);
            return new Employee(name, surname, passportNumber, birthDate, baseSalary, employmentDate, role);
        }

        public static Employee CreateCook(string name, string surname, string passportNumber, DateTime birthDate, decimal baseSalary, DateTime employmentDate, string education, List<MenuItem> signatureDishes)
        {
            var role = new CookRole(education, signatureDishes);
            return new Employee(name, surname, passportNumber, birthDate, baseSalary, employmentDate, role);
        }
    }
}