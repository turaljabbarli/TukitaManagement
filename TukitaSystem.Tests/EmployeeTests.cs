using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace TukitaSystem.Tests
{
    public class EmployeeTests
    {
        [SetUp]
        public void Setup()
        {
            Employee.ClearExtent();
        }

        [Test]
        public void CreateCook_ValidData_AddsToExtent()
        {
            var burger = new Burger("Cheeseburger", 10.0m, 800, "10-15 minutes",
                new List<PattyType> { PattyType.Beef });
            var drink = new Drink("Coke", 2.5m, 150, "10-15 minutes", true, SizeType.Medium);
            
            var cook = EmployeeFactory.CreateCook("Anna", "Smith", "P2", DateTime.Now.AddYears(-35), 3200, DateTime.Now.AddYears(-7),
                "PJATK", new List<MenuItem> { burger, drink });

            var allEmployees = Employee.GetExtent();

            Assert.Contains(cook, allEmployees);
        }

        [Test]
        public void Age_CalculatedCorrectly()
        {
            var birthDate = DateTime.Today.AddYears(-25);
            var burger = new Burger("Cheeseburger", 10.0m, 800, "10-15 minutes",
                new List<PattyType> { PattyType.Beef });
            var drink = new Drink("Coke", 2.5m, 150, "10-15 minutes", true, SizeType.Medium);
            
            var cook = EmployeeFactory.CreateCook("Anna", "Smith", "P2", birthDate, 3200, DateTime.Now.AddYears(-7), "PJATK",
                new List<MenuItem> { burger, drink });

            Assert.AreEqual(25, cook.Age);
        }

        [Test]
        public void CurrentSalary_OneYearExperience_CalculatesIncrease()
        {
            var employmentDate = DateTime.Today.AddYears(-1);
            var baseSalary = 1000m;

            var burger = new Burger("Cheeseburger", 10.0m, 800, "10-15 minutes",
                new List<PattyType> { PattyType.Beef });
            var drink = new Drink("Coke", 2.5m, 150, "10-15 minutes", true, SizeType.Medium);
            
            var cook = EmployeeFactory.CreateCook("Anna", "Smith", "P2", DateTime.Today.AddYears(-30), baseSalary, employmentDate,
                "PJATK", new List<MenuItem> { burger, drink });

            Assert.AreEqual(1030m, cook.CurrentSalary);
        }
    }
}