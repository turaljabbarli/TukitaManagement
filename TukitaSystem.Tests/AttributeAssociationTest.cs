
namespace TukitaSystem.Tests;

public class AttributeAssociationTest
{
    private Order CreateTestOrder()
    {
        var cashier = new Cashier("Test", "Cashier", "PASS123", DateTime.Today.AddYears(-20), 2000, DateTime.Today);
        var customer = new Customer("Test Customer", "test@test.com");
        return new Order(cashier, customer);
    }

    [Test]
    public void OrderDetail_ShouldLinkOrderAndMenuItem_WithQuantityAttribute()
    {
        var order = CreateTestOrder();
        var patties = new List<PattyType> { PattyType.Beef };
        var burger = new Burger("Test Burger", 20.0m, 500, "10 min", patties);
        int quantityAttribute = 3;

        
        var detail = new OrderDetail(order, burger, quantityAttribute);

        
        Assert.AreEqual(order, detail.Order);
        Assert.AreEqual(burger, detail.MenuItem);
        Assert.AreEqual(quantityAttribute, detail.Quantity);

        
        Assert.IsTrue(order.OrderDetails.Contains(detail));
        Assert.AreEqual(1, order.OrderDetails.Count);
    }

    [Test]
    public void FinalPrice_ShouldRecalculate_WhenAttributeChanges()
    {
        
        var order = CreateTestOrder();
        var burger = new Burger("Expensive Burger", 50.0m, 1000, "20 min", new List<PattyType> { PattyType.Beef });
        
        
        order.AddItem(burger, 1);
        Assert.AreEqual(50.0m, order.FinalPrice);

        
        var detail = order.OrderDetails.First();
        detail.Quantity = 2;

        
        Assert.AreEqual(100.0m, order.FinalPrice);
    }

    [Test]
    public void OrderDetail_QuantityAttribute_MustBePositive()
    {
        
        var order = CreateTestOrder();
        var drink = new Drink("Cola", 5.0m, 100, "1 min", true, SizeType.Medium);

        
        Assert.Throws<ArgumentException>(() =>
        {
            new OrderDetail(order, drink, -5);
        });

        
        order.AddItem(drink, 1);
        var detail = order.OrderDetails.First();
        
        Assert.Throws<ArgumentException>(() =>
        {
            detail.Quantity = 0;
        });
    }
}