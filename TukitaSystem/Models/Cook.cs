using System;
using System.Collections.Generic;

namespace TukitaSystem
{
    public class Cook
    {
        public readonly Employee _employee;
        private MenuItem _signatureDish;
        private string _education;
        

        public Cook(string name, string surname, string passportNumber, DateTime birthDate, decimal baseSalary, DateTime employmentDate, string education, MenuItem signatureDish)
            : this(new Employee(name, surname, passportNumber, birthDate, baseSalary, employmentDate), education, signatureDish)
        {
        }

        public Cook(Employee employee, string education, MenuItem signatureDish)
        {
            _employee = employee ?? throw new ArgumentNullException(nameof(employee));

            AddSignatureDish(signatureDish);
            
            _education = education;
            

            _employee.AddRole(this);
        }

        public void AddSignatureDish(MenuItem signatureDish)
        {
            _signatureDish =  signatureDish;
            if (!signatureDish.Cooks.Contains(this))
            {
                signatureDish.AddCook(this);
            }
        }

        public void RemoveSignatureDish(MenuItem signatureDish)
        {
            if (_signatureDish == null)return;

            _signatureDish = null;
            signatureDish.RemoveCook(this);

        }
        
        public string Education
        {
            get => _education;
            set => _education = value ?? throw new ArgumentException("Education cannot be null");
        }
        
        

        public MenuItem SignatureDish => _signatureDish;
        public Employee BaseEmployee => _employee;
    }
}