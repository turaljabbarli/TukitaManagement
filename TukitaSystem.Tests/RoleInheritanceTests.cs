namespace TukitaSystem.Tests;

[TestFixture]
    public class RoleInheritanceTests
    {
        private Employee _baseEmployee;
        private Burger _signatureBurger;

        [SetUp]
        public void Setup()
        {
            Employee.ClearExtent();
            _baseEmployee = new Employee("Ivan", "Ivanov", "PASSPORT123", 
                DateTime.Today.AddYears(-30), 5000m, DateTime.Today.AddYears(-2));
            
            _signatureBurger = new Burger("Classic Burger", 15.0m, 600, "10 min", 
                new List<PattyType> { PattyType.Beef });
        }

        [Test]
        public void MultiAspect_Employee_CanHaveMultipleRoles()
        {
            var cashier = new Cashier(_baseEmployee);
            
            var cook = new Cook(_baseEmployee, "Culinary Academy", _signatureBurger);

            Assert.IsNotNull(_baseEmployee.GetRole<Cashier>());
            Assert.IsNotNull(_baseEmployee.GetRole<Cook>());
            Assert.AreSame(cashier, _baseEmployee.GetRole<Cashier>());
            Assert.AreSame(cook, _baseEmployee.GetRole<Cook>());
        }

        [Test]
        public void CookRole_LinksToSignatureDish_Correctly()
        {
            var cook = new Cook(_baseEmployee, "Chef School", _signatureBurger);

            Assert.AreEqual(_signatureBurger, cook.SignatureDish);
            
            Assert.Contains(cook, _signatureBurger.Cooks.ToList());
        }

        [Test]
        public void Manager_LeadRank_CanAddTraining()
        {
            var manager = new Manager(_baseEmployee, RankType.Lead, 10);
            var studentEmp = new Employee("Petr", "Petrov", "P999", 
                DateTime.Today.AddYears(-20), 2000m, DateTime.Today);
            var studentManager = new Manager(studentEmp, RankType.Junior, 0);

            var training = new Training(studentManager, manager, DateTime.Now);
            
            Assert.Contains(training, manager.LeadingTrainings.ToList());
            Assert.AreEqual(manager, training.ManagerTeacher);
        }

        [Test]
        public void Manager_JuniorRank_ThrowsWhenAddingTraining()
        {
            var juniorManager = new Manager(_baseEmployee, RankType.Junior, 1);
            var training = new Training(null, null, DateTime.Now);

            Assert.Throws<InvalidOperationException>(() => juniorManager.AddLeadingTraining(training));
        }

        [Test]
        public void Salary_CalculatesWithAnnualIncrease()
        {
            decimal expectedSalary = 5304.5m;
            Assert.AreEqual(expectedSalary, _baseEmployee.CurrentSalary);
        }

        [Test]
        public void Cook_Constructor_CreatesNewEmployee_WhenDataProvided()
        {
            var cook = new Cook("Anna", "Smith", "PASS456", 
                DateTime.Today.AddYears(-25), 4000m, DateTime.Today, 
                "Academy", _signatureBurger);

            Assert.IsNotNull(cook.BaseEmployee);
            Assert.AreEqual("Anna", cook.BaseEmployee.Name);
            Assert.IsNotNull(cook.BaseEmployee.GetRole<Cook>());
            Assert.IsTrue(Employee.GetExtent().Contains(cook.BaseEmployee));
        }
    }