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
            var edu = new Education("Bachelor", "Culinary School");
            var cook = new Cook("John", "Doe", DateTime.Today.AddYears(-30), 2000, DateTime.Today, edu);

            var allEmployees = Employee.GetExtent();

            Assert.Contains(cook, allEmployees);
        }

        [Test]
        public void CreateEmployee_EmptyName_ThrowsException()
        {
            var edu = new Education("Bachelor", "Culinary School");

            Assert.Throws<ArgumentException>(() => new Cook("", "Doe", DateTime.Today.AddYears(-25), 2000, DateTime.Today, edu));
        }

        [Test]
        public void CreateEmployee_FutureBirthDate_ThrowsException()
        {
            var edu = new Education("Bachelor", "Culinary School");

            Assert.Throws<ArgumentException>(() => new Cook("John", "Doe", DateTime.Today.AddDays(1), 2000, DateTime.Today, edu));
        }

        [Test]
        public void CreateEmployee_NegativeSalary_ThrowsException()
        {
            var edu = new Education("Bachelor", "Culinary School");

            Assert.Throws<ArgumentException>(() => new Cook("John", "Doe", DateTime.Today.AddYears(-25), -100, DateTime.Today, edu));
        }

        [Test]
        public void Age_CalculatedCorrectly()
        {
            var edu = new Education("Bachelor", "Culinary School");
            var birthDate = DateTime.Today.AddYears(-25);
            var cook = new Cook("John", "Doe", birthDate, 2000, DateTime.Today, edu);

            Assert.AreEqual(25, cook.Age);
        }

        [Test]
        public void CurrentSalary_OneYearExperience_CalculatesIncrease()
        {
            var edu = new Education("Bachelor", "Culinary School");
            var employmentDate = DateTime.Today.AddYears(-1);
            var baseSalary = 1000m;

            var cook = new Cook("John", "Doe", DateTime.Today.AddYears(-30), baseSalary, employmentDate, edu);

            // 1000 * 1.03 = 1030
            Assert.AreEqual(1030m, cook.CurrentSalary);
        }

        [Test]
        public void Education_Null_ThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => new Cook("John", "Doe", DateTime.Today.AddYears(-25), 2000, DateTime.Today, null));
        }

        [Test]
        public void KnownDishes_AddDish_UpdatesList()
        {
            var edu = new Education("Bachelor", "Culinary School");
            var cook = new Cook("John", "Doe", DateTime.Today.AddYears(-30), 2000, DateTime.Today, edu);

            cook.AddDish("Pasta");
            cook.AddDish("Pizza");

            Assert.AreEqual(2, cook.KnownDishes.Count);
            Assert.IsTrue(cook.KnownDishes.Contains("Pasta"));
        }

        [Test]
        public void SignatureDish_Optional_CanBeNull()
        {
            var edu = new Education("Bachelor", "Culinary School");
            var cook = new Cook("John", "Doe", DateTime.Today.AddYears(-30), 2000, DateTime.Today, edu);

            cook.SignatureDish = null;

            Assert.IsNull(cook.SignatureDish);
        }

        [Test]
        public void SignatureDish_EmptyString_ThrowsException()
        {
            var edu = new Education("Bachelor", "Culinary School");
            var cook = new Cook("John", "Doe", DateTime.Today.AddYears(-30), 2000, DateTime.Today, edu);

            Assert.Throws<ArgumentException>(() => cook.SignatureDish = "");
        }
    }
}