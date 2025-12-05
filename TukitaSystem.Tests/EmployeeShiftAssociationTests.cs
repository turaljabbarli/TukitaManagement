namespace TukitaSystem.Tests;

public class EmployeeShiftAssociationTests
{

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