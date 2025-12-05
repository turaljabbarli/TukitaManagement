namespace TukitaSystem.Tests;

public class CompositionTests
{
    [Test]
    public void CreateLoyaltyCard_ShouldLinkToCustomer_AndSetReverseConnection()
    {
        var customer = new Customer("John Doe", "john@example.com");
        customer.CreateLoyaltyCard("123456", DateTime.Now.AddYears(1));
        
        Assert.IsNotNull(customer.LoyaltyCard);
        Assert.AreEqual("123456", customer.LoyaltyCard.CardNumber);
        Assert.AreEqual(customer, customer.LoyaltyCard.Customer);
    }

    [Test]
    public void RemoveLoyaltyCard_ShouldDestroyLink_OnBothSides()
    {
        var customer = new Customer("Jane Doe", "jane@example.com");
        customer.CreateLoyaltyCard("987654", DateTime.Now.AddYears(1));
        var cardRef = customer.LoyaltyCard;
        customer.RemoveLoyaltyCard();

        Assert.IsNull(customer.LoyaltyCard);
        var customerField = cardRef.GetType().GetField("_customer", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        Assert.IsNull(customerField.GetValue(cardRef));
    }

    [Test]
    public void CreateLoyaltyCard_AlreadyExists_ShouldThrowException()
    {
        var customer = new Customer("Double Dipper", "double@test.com");
        customer.CreateLoyaltyCard("11111", DateTime.Now.AddYears(1));

        Assert.Throws<InvalidOperationException>(() => customer.CreateLoyaltyCard("22222", DateTime.Now.AddYears(1)));
    }
}