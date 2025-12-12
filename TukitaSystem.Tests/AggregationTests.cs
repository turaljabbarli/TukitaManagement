namespace TukitaSystem.Tests;

public class AggregationTests
{
    
    private MenuItem item;
    private Ingredient tomato;
    private Ingredient cheese;
    
    [SetUp]
    public void Setup()
    {
        var patties = new List<PattyType> { PattyType.Beef };
        item = new Burger("Beef King", 15.0m, 800, "10-15 minutes", patties);
        cheese = new Ingredient("Cheese", 10, false, false);
        tomato = new Ingredient("Tomato", 20, true, false);
    }
    
    [Test]
    public void AddIngredient_ShouldAdd()
    {
        item.AddIngredient(cheese);

        Assert.AreEqual(1, item.Ingredients.Count);
        Assert.IsTrue(item.Ingredients.Contains(cheese));
    }

    [Test]
    public void AddIngredient_ShouldIgnoreDuplicate()
    {
        item.AddIngredient(cheese);
        item.AddIngredient(cheese);

        Assert.AreEqual(1, item.Ingredients.Count);
    }

    [Test]
    public void AddIngredient_Null_ShouldThrow()
    {
        Assert.Throws<ArgumentNullException>(() => item.AddIngredient(null));
    }

    [Test]
    public void RemoveIngredient_ShouldRemove()
    {
        item.AddIngredient(cheese);

        bool removed = item.RemoveIngredient(cheese);

        Assert.IsTrue(removed);
        Assert.AreEqual(0, item.Ingredients.Count);
    }

    [Test]
    public void RemoveIngredient_NotPresent_ShouldReturnFalse()
    {
        bool result = item.RemoveIngredient(tomato);

        Assert.IsFalse(result);
    }
    
    [Test]
    public void AddIngredient_ShouldSetReverseConnection()
    {
        item.AddIngredient(cheese);
    
        Assert.IsTrue(cheese.UsedInItems.Contains(item));
    }

    [Test]
    public void RemoveIngredient_ShouldRemoveReverseConnection()
    {
        item.AddIngredient(cheese);
        item.RemoveIngredient(cheese);

        Assert.AreEqual(0, item.Ingredients.Count);
        Assert.AreEqual(0, cheese.UsedInItems.Count);
    }
    
}
