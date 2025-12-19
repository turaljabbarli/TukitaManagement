using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TukitaSystem.Tests
{
    public class AssociationTests
    {
        [Test]
        public void Constructor_Creates_Reverse_Association()
        {
            var burger = new Burger("Cheeseburger", 10m, 800, "10-15 min",
                new List<PattyType> { PattyType.Beef });
            var drink = new Drink("Coke", 2.5m, 150, "10-15 min", true, SizeType.Medium);

            var cookEmployee = EmployeeFactory.CreateCook("Anna", "Smith", "P2",
                DateTime.Now.AddYears(-35),
                3200,
                DateTime.Now.AddYears(-7),
                "PJATK",
                new List<MenuItem> { burger, drink });

            var cookRole = (CookRole)cookEmployee.Role;

            Assert.Contains(cookRole, burger.Cooks.ToList());
            Assert.Contains(cookRole, drink.Cooks.ToList());
        }
        
        [Test]
        public void CookConstructor_Throws_WhenNoSignatureDishesProvided()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                EmployeeFactory.CreateCook("Anna", "Smith", "P2",
                    DateTime.Now.AddYears(-30), 3000, DateTime.Now.AddYears(-5),
                    "CookingSchool", new List<MenuItem>());
            });
        }
        
        [Test]
        public void AddSignatureDish_IgnoresDuplicate()
        {
            var burger = new Burger("Burger", 10m, 500, "10 min", new List<PattyType> { PattyType.Beef });
            
            var cookEmployee = EmployeeFactory.CreateCook("Anna", "Smith", "P2",
                DateTime.Now.AddYears(-30), 3000,
                DateTime.Now.AddYears(-5),
                "School", new List<MenuItem> { burger });

            var cookRole = (CookRole)cookEmployee.Role;

            cookRole.AddSignatureDish(burger);

            Assert.AreEqual(1, cookRole.SignatureDishes.Count);
            Assert.AreEqual(1, burger.Cooks.Count);
        }
        
        [Test]
        public void AddCook_UpdatesBothSides()
        {
            var pasta = new Drink("Pasta Juice", 4.5m, 200, "5 min", true, SizeType.Small);
            
            var cookEmployee = EmployeeFactory.CreateCook("Bob", "Chef", "P3",
                DateTime.Now.AddYears(-40), 3500,
                DateTime.Now.AddYears(-10),
                "School", new List<MenuItem> { pasta });

            var cookRole = (CookRole)cookEmployee.Role;

            var burger = new Burger("Burger", 12m, 700, "10 min",
                new List<PattyType> { PattyType.Beef });

            burger.AddCook(cookRole);

            Assert.Contains(burger, cookRole.SignatureDishes.ToList());
            Assert.Contains(cookRole, burger.Cooks.ToList());
        }
        
        [Test]
        public void RemoveSignatureDish_UpdatesBothSides()
        {
            var burger = new Burger("Burger", 10m, 500, "10 min", new List<PattyType> { PattyType.Beef });
            var drink = new Drink("Cola", 3m, 150, "1 min", true, SizeType.Small);

            var cookEmployee = EmployeeFactory.CreateCook("Anna", "Smith", "P1",
                DateTime.Now.AddYears(-40), 3000,
                DateTime.Now.AddYears(-10),
                "School", new List<MenuItem> { burger, drink });

            var cookRole = (CookRole)cookEmployee.Role;

            cookRole.RemoveSignatureDish(drink);

            Assert.False(cookRole.SignatureDishes.Contains(drink));
            Assert.False(drink.Cooks.Contains(cookRole));
        }
        
        [Test]
        public void RemoveSignatureDish_Throws_WhenLastDish()
        {
            var burger = new Burger("Burger", 10m, 500, "10 min",
                new List<PattyType> { PattyType.Beef });

            var cookEmployee = EmployeeFactory.CreateCook("Anna", "Smith", "P1",
                DateTime.Now.AddYears(-40), 3000,
                DateTime.Now.AddYears(-10),
                "School", new List<MenuItem> { burger });

            var cookRole = (CookRole)cookEmployee.Role;

            Assert.Throws<InvalidOperationException>(() =>
            {
                cookRole.RemoveSignatureDish(burger);
            });
        }
    }
}