using NUnit.Framework;
using System;
using System.Linq;
using System.Collections.Generic;
using TukitaSystem;

namespace TukitaSystem.Tests
{
    public class EmployeeTests
    {
        [Test]
        public void CreateCook_ValidData_AddsToExtent()
        {
            var cook = new Cook("John", "Doe", "FS1234567", DateTime.Today.AddYears(-30), 2000, DateTime.Today, "Bachelor - Culinary School");

            var allEmployees = Employee.GetExtent();

            Assert.Contains(cook, allEmployees);
        }

        [Test]
        public void CreateEmployee_EmptyName_ThrowsException()
        {

            Assert.Throws<ArgumentException>(() => new Cook("", "Doe", "FS1234567", DateTime.Today.AddYears(-25), 2000, DateTime.Today, "Bachelor - Culinary School"));
        }

        [Test]
        public void CreateEmployee_FutureBirthDate_ThrowsException()
        {

            Assert.Throws<ArgumentException>(() => new Cook("John", "Doe", "FS1234567", DateTime.Today.AddYears(1), 2000, DateTime.Today, "Bachelor - Culinary School"));
        }

        [Test]
        public void CreateEmployee_NegativeSalary_ThrowsException()
        {

            Assert.Throws<ArgumentException>(() => new Cook("John", "Doe", "FS1234567", DateTime.Today.AddYears(-25), -100, DateTime.Today, "Bachelor - Culinary School"));
        }

        [Test]
        public void Age_CalculatedCorrectly()
        {
            var birthDate = DateTime.Today.AddYears(-25);
            var cook = new Cook("John", "Doe", "FS1234567", birthDate, 2000, DateTime.Today, "Bachelor - Culinary School");

            Assert.AreEqual(25, cook.Age);
        }

        [Test]
        public void CurrentSalary_OneYearExperience_CalculatesIncrease()
        {
            var employmentDate = DateTime.Today.AddYears(-1);
            var baseSalary = 1000m;

            var cook = new Cook("John", "Doe", "FS1234567", DateTime.Today.AddYears(-30), baseSalary, employmentDate, "Bachelor - Culinary School");
            
            Assert.AreEqual(1030m, cook.CurrentSalary);
        }

        /*[Test]
        public void Education_Null_ThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => new Cook("John", "Doe", "FS1234567", DateTime.Today.AddYears(-25), 2000, DateTime.Today, null));
        }*/

        [Test]
        public void KnownDishes_AddDish_UpdatesList()
        {
            var cook = new Cook("John", "Doe", "FS1234567", DateTime.Today.AddYears(-30), 2000, DateTime.Today, "Bachelor - Culinary School");

            cook.AddDish("Pasta");
            cook.AddDish("Pizza");

            Assert.AreEqual(2, cook.KnownDishes.Count);
            Assert.IsTrue(cook.KnownDishes.Contains("Pasta"));
        }

        [Test]
        public void SignatureDish_Optional_CanBeNull()
        {
            var cook = new Cook("John", "Doe", "FS1234567", DateTime.Today.AddYears(-30), 2000, DateTime.Today, "Bachelor - Culinary School");

            cook.SignatureDish = null;

            Assert.IsNull(cook.SignatureDish);
        }

        [Test]
        public void SignatureDish_EmptyString_ThrowsException()
        {
            var cook = new Cook("John", "Doe", "FS1234567", DateTime.Today.AddYears(-30), 2000, DateTime.Today, "Bachelor - Culinary School");

            Assert.Throws<ArgumentException>(() => cook.SignatureDish = "");
        }
    }
}