using TukitaSystem;

class Program
{
    static void Main()
    {
        MenuItem.LoadExtent();
        Menu.LoadExtent();
        Employee.LoadExtent();
        new Menu("Summer menu", new TimeSpan(), new TimeSpan());
        new Cook("John", "Doe", "FS1234567", DateTime.Today.AddYears(-30), 2000, DateTime.Today, "Bachelor - Culinary School");
        new Burger("Big mac", 12, 300, "10 minutes", [PattyType.Chicken, PattyType.Mushroom]);
        new Dessert("Cake", 15, 100, "15-20 mins", false, [FlavorType.Caramel]);
        new Drink("Coka Cola Medium", 7, 150, "Instant", true, SizeType.Medium);
       
        MenuItem.SaveExtent();
        Employee.SaveExtent();
        Menu.SaveExtent();

        foreach (var item in MenuItem.GetExtend())
        {
            Console.WriteLine($"{item.GetType().Name}: {item.Name} - {item.Price}$");
        }
    }
}
