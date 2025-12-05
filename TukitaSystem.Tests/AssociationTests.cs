namespace TukitaSystem.Tests;

public class AssociationTests
{

    [Test]
    public void Order_FinalPrice_ShouldCalculateCorrectly()
    {
        var cashier = new Cashier("Alice", "Smith", "FF31213123", DateTime.Today.AddYears(-25), 2500, DateTime.Today);
        var customer = new Customer("Bob", "bob@example.com");
        var order = new Order(cashier, customer);

        var patties = new List<PattyType> { PattyType.Beef };
        var burger = new Burger("Cheeseburger", 10.0m, 800, "10-15 minutes", patties);
        var drink = new Drink("Coke", 2.5m, 150, "10-15 minutes", true, SizeType.Medium);
        
        order.AddItem(burger, 2);
        order.AddItem(drink, 1);
        
        Assert.AreEqual(22.5m, order.FinalPrice);
    }
    
    [Test]
    public void ShiftMustHaveAtLeastOneEmployee()
    {
        Assert.Throws<ArgumentException>(() =>
        {
            var invalid = new Shift(
                ShiftType.Morning,
                DateTime.Today,
                TimeSpan.FromHours(8),
                TimeSpan.FromHours(16),
                new List<Employee>()
            );
        });
    }

    [Test]
    public void ShiftWithMultipleEmployeesIsValid()
    {
        var emp1 = new Cashier("John", "Doe", "P1", DateTime.Now.AddYears(-30), 3000, DateTime.Now.AddYears(-5));
        var emp2 = new Cook("Anna", "Smith", "P2", DateTime.Now.AddYears(-35), 3200, DateTime.Now.AddYears(-7), "PJATK");

        var shift = new Shift(
            ShiftType.Morning,
            DateTime.Today,
            TimeSpan.FromHours(8),
            TimeSpan.FromHours(16),
            new List<Employee> { emp1, emp2 }
        );

        Assert.AreEqual(2, shift.Employees.Count);
        Assert.AreEqual(1, emp1.Shifts.Count);
        Assert.AreEqual(1, emp2.Shifts.Count);
    }

    [Test]
    public void CannotRemoveLastEmployeeFromShift()
    {
        var emp = new Cashier("John", "Doe", "P1", DateTime.Now.AddYears(-30), 3000, DateTime.Now.AddYears(-5));

        var shift = new Shift(
            ShiftType.Morning,
            DateTime.Today,
            TimeSpan.FromHours(8),
            TimeSpan.FromHours(16),
            new List<Employee> { emp }
        );

        Assert.Throws<InvalidOperationException>(() =>
            shift.RemoveEmployee(emp)
        );
    }
}