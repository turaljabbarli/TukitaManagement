

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
            var cook = new Cook("Anna", "Smith", "P2", DateTime.Now.AddYears(-35), 3200, DateTime.Now.AddYears(-7),
                "PJATK", new List<MenuItem> { burger, drink });

            var allEmployees = Employee.GetExtent();

            Assert.Contains(cook, allEmployees);
        }

        /*[Test]
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
        }*/

        [Test]
        public void Age_CalculatedCorrectly()
        {
            var birthDate = DateTime.Today.AddYears(-25);
            var burger = new Burger("Cheeseburger", 10.0m, 800, "10-15 minutes",
                new List<PattyType> { PattyType.Beef });
            var drink = new Drink("Coke", 2.5m, 150, "10-15 minutes", true, SizeType.Medium);
            var cook = new Cook("Anna", "Smith", "P2", birthDate, 3200, DateTime.Now.AddYears(-7), "PJATK",
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
            var cook = new Cook("Anna", "Smith", "P2", DateTime.Today.AddYears(-30), baseSalary, employmentDate,
                "PJATK", new List<MenuItem> { burger, drink });

            Assert.AreEqual(1030m, cook.CurrentSalary);
        }
    }
}

/*[Test]
public void Education_Null_ThrowsException()
{
    Assert.Throws<ArgumentNullException>(() => new Cook("John", "Doe", "FS1234567", DateTime.Today.AddYears(-25), 2000, DateTime.Today, null));
}*/

        /*[Test]
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
        
        [Test]
        public void SearchByName_ShouldReturnCorrectEmployee()
        {
            var e1 = new Cook("John", "Doe", "FS1234567", DateTime.Today.AddYears(-30), 2000, DateTime.Today, "Bachelor - Culinary School");
            var e2 = new Cook("John 2", "Doe", "FS1234567", DateTime.Today.AddYears(-30), 2000, DateTime.Today, "Bachelor - Culinary School");

            var results = Employee.SearchForEmployee(name: "John");

            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(e1, results[0]);
        }

        [Test]
        public void SearchBySurname_ShouldReturnCorrectEmployee()
        {
            var e1 = new Cook("John", "Doe", "FS1234567", DateTime.Today.AddYears(-30), 2000, DateTime.Today, "Bachelor - Culinary School");
            var e2 = new Cook("John", "Smith", "FS1234567", DateTime.Today.AddYears(-30), 2000, DateTime.Today, "Bachelor - Culinary School");

            var results = Employee.SearchForEmployee(surname: "Smith");

            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(e2, results[0]);
        }
    }
}*/