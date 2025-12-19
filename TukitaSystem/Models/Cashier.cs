using System;

namespace TukitaSystem
{
    public class Cashier
    {
        public readonly Employee _employee;

        public Cashier(string name, string surname, string passportNumber, DateTime birthDate, decimal baseSalary, DateTime employmentDate)
            : this(new Employee(name, surname, passportNumber, birthDate, baseSalary, employmentDate))
        {
        }

        public Cashier(Employee employee)
        {
            _employee = employee ?? throw new ArgumentNullException(nameof(employee));
            _employee.AddRole(this);
        }

        public Employee BaseEmployee => _employee;
    }
}