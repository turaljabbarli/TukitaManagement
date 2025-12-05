namespace TukitaSystem.Tests;

public class AssociationTests
{
    [Test]
    public void AddItemToOrder_ShouldCreateOrderDetail_AndLinkReversely()
    {
        var cashier = new Cashier("Alice", "Smith", "PASS123", DateTime.Today.AddYears(-25), 2500m, DateTime.Today);
        var customer = new Customer("Test Cust", "test@c.com");
        var order = new Order(cashier, customer);
        var drink = new Drink("Cola", 2.5m, 150, "1 min", true, SizeType.Medium);

        order.AddItem(drink, 3);

        var detail = order.OrderDetails.First();
        Assert.AreEqual(drink, detail.MenuItem);
        Assert.AreEqual(order, detail.Order);
        Assert.AreEqual(3, detail.Quantity);
        
        Assert.Contains(detail, drink.OrderDetails.ToList());
    }

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
}