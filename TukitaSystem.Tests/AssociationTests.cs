namespace TukitaSystem.Tests;

public class AssociationTests
{

    /*[Test]
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
*/
    
    [Test]
    public void Constructor_Creates_Reverse_Association()
    {
        var burger = new Burger("Cheeseburger", 10m, 800, "10-15 min",
            new List<PattyType> { PattyType.Beef });
        var drink = new Drink("Coke", 2.5m, 150, "10-15 min", true, SizeType.Medium);

        var cook = new Cook("Anna", "Smith", "P2",
            DateTime.Now.AddYears(-35),
            3200,
            DateTime.Now.AddYears(-7),
            "PJATK",
            burger);

        Assert.Contains(cook, burger.Cooks.ToList());
        Assert.Contains(cook, drink.Cooks.ToList());
    }
    
    [Test]
    public void CookConstructor_Throws_WhenNoSignatureDishesProvided()
    {
        Assert.Throws<InvalidOperationException>(() =>
        {
            var cook = new Cook("Anna", "Smith", "P2",
                DateTime.Now.AddYears(-30), 3000, DateTime.Now.AddYears(-5),
                "CookingSchool", new Burger("Burger", 10m, 500, "10 min", new List<PattyType> { PattyType.Beef }));
        });
    }
    
    [Test]
    public void AddSignatureDish_IgnoresDuplicate()
    {
        var burger = new Burger("Burger", 10m, 500, "10 min", new List<PattyType> { PattyType.Beef });
        var cook = new Cook("Anna", "Smith", "P2",
            DateTime.Now.AddYears(-30), 3000,
            DateTime.Now.AddYears(-5),
            "School", burger);

        cook.AddSignatureDish(burger);
        
        Assert.AreEqual(1, burger.Cooks.Count);
    }
    
    [Test]
    public void AddCook_UpdatesBothSides()
    {
        var pasta = new Drink("Pasta Juice", 4.5m, 200, "5 min", true, SizeType.Small);
        var cook = new Cook("Bob", "Chef", "P3",
            DateTime.Now.AddYears(-40), 3500,
            DateTime.Now.AddYears(-10),
            "School", pasta);

        var burger = new Burger("Burger", 12m, 700, "10 min",
            new List<PattyType> { PattyType.Beef });

        burger.AddCook(cook);

        Assert.Equals(burger, cook.SignatureDish);
        Assert.Contains(cook, burger.Cooks.ToList());
    }
    
    [Test]
    public void RemoveSignatureDish_UpdatesBothSides()
    {
        var burger = new Burger("Burger", 10m, 500, "10 min", new List<PattyType> { PattyType.Beef });
        var drink = new Drink("Cola", 3m, 150, "1 min", true, SizeType.Small);

        var cook = new Cook("Anna", "Smith", "P1",
            DateTime.Now.AddYears(-40), 3000,
            DateTime.Now.AddYears(-10),
            "School", burger);

        cook.RemoveSignatureDish(drink);
        
        Assert.False(drink.Cooks.Contains(cook));
    }
    
    [Test]
    public void RemoveSignatureDish_Throws_WhenLastDish()
    {
        var burger = new Burger("Burger", 10m, 500, "10 min",
            new List<PattyType> { PattyType.Beef });

        var cook = new Cook("Anna", "Smith", "P1",
            DateTime.Now.AddYears(-40), 3000,
            DateTime.Now.AddYears(-10),
            "School", burger);

        Assert.Throws<InvalidOperationException>(() =>
        {
            cook.RemoveSignatureDish(burger);
        });
    }
}