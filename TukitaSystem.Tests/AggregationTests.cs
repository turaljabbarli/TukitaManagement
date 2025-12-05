namespace TukitaSystem.Tests;

public class AggregationTests
{
    [Test]
    public void AddMenuItem_ShouldAddReference_AndSetReverseConnection()
    {
        var menu = new Menu("Lunch", new TimeSpan(12, 0, 0), new TimeSpan(15, 0, 0));
        var patties = new List<PattyType> { PattyType.Beef };
        var burger = new Burger("Hamburger", 10m, 500, "10 mins", patties);
        
        menu.AddMenuItem(burger);
        
        Assert.Contains(burger, menu.QualifiedItems.Values.ToList());
        Assert.Contains(menu, burger.Menus.ToList());
    }

    [Test]
    public void RemoveMenuItem_ShouldRemoveReference_OnBothSides()
    {
        var menu = new Menu("Dinner", new TimeSpan(18, 0, 0), new TimeSpan(22, 0, 0));
        var patties = new List<PattyType> { PattyType.Bean };
        var burger = new Burger("Veggie Burger", 12m, 400, "8 mins", patties);
        menu.AddMenuItem(burger);

        menu.RemoveMenuItem(burger);
        
        Assert.IsFalse(menu.GetItem("Veggie Burger", out var result));
        Assert.IsFalse(burger.Menus.Contains(menu));
    }
}