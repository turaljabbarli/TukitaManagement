namespace TukitaSystem.Tests;

public class QualifiedAssociationTests
{
    [Test]
    public void AddMenuItem_ShouldStoreItemByName()
    {
        var menu = new Menu("Breakfast", TimeSpan.FromHours(6), TimeSpan.FromHours(11));
        var patties = new List<PattyType> { PattyType.Beef };
        var burger = new Burger("Burger", 10.0m, 800, "10-15 minutes", patties);

        menu.AddMenuItem(burger);

        Assert.IsTrue(menu.QualifiedItems.ContainsKey("Burger"));
        Assert.AreEqual(burger, menu.QualifiedItems["Burger"]);
    }

    [Test]
    public void AddMenuItem_DuplicateName_ShouldThrow()
    {
        var menu = new Menu("Lunch", TimeSpan.FromHours(12), TimeSpan.FromHours(17));
        var patties = new List<PattyType> { PattyType.Beef };
        menu.AddMenuItem(new Burger("Cheeseburger", 10.0m, 800, "10-15 minutes", patties));

        Assert.Throws<InvalidOperationException>(() =>
            menu.AddMenuItem(new Burger("Cheeseburger", 10.0m, 800, "10-15 minutes", patties))
        );
    }

    [Test]
    public void TryGetItem_ShouldReturnItem()
    {
        var menu = new Menu("Dinner", TimeSpan.FromHours(17), TimeSpan.FromHours(22));
        var item = new Drink("Coke", 2.5m, 150, "10-15 minutes", true, SizeType.Medium);

        menu.AddMenuItem(item);

        Assert.IsTrue(menu.GetItem("Coke", out var result));
        Assert.AreEqual(item, result);
    }

    [Test]
    public void RemoveMenuItem_ShouldRemoveBothSides()
    {
        var menu = new Menu("Evening", TimeSpan.FromHours(18), TimeSpan.FromHours(23));
        var item = new Drink("Fanta", 2.5m, 150, "10-15 minutes", true, SizeType.Medium);

        menu.AddMenuItem(item);
        menu.RemoveMenuItem(item);

        Assert.IsFalse(menu.QualifiedItems.ContainsKey("Fanta"));
        Assert.IsFalse(item.Menus.Contains(menu));
    }
}