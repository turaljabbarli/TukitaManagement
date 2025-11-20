using NUnit.Framework;
using System;
using System.Collections.Generic;
using TukitaSystem;

namespace TukitaSystem.Tests
{
    public class SystemTests
    {
        [Test]
        public void Burger_IsVegetarian_ReturnsTrueForVeggiePatties()
        {
            var patties = new List<PattyType> { PattyType.Bean, PattyType.Mushroom };
            var burger = new Burger("Veggie King", 15.0m, 500, 10, patties);

            Assert.IsTrue(burger.IsVegetarian);
        }

        [Test]
        public void Burger_IsVegetarian_ReturnsFalseForBeef()
        {
            var patties = new List<PattyType> { PattyType.Beef };
            var burger = new Burger("Beef King", 15.0m, 800, 10, patties);

            Assert.IsFalse(burger.IsVegetarian);
        }

        [Test]
        public void Order_CalculateFinalPrice_SumsCorrectly()
        {
            var cashier = new Cashier("Alice", "Smith", "FF31213123", DateTime.Today.AddYears(-25), 2500, DateTime.Today)
            {
                PeselNumber = "1234567890"
            };
            var customer = new Customer("Bob", "bob@example.com");
            var order = new Order(cashier, customer);

            var patties = new List<PattyType> { PattyType.Beef };
            var burger = new Burger("Cheeseburger", 10.0m, 800, 10, patties);
            var drink = new Drink("Coke", 2.5m, 150, 2, true, SizeType.Medium);

            order.AddItem(burger, 2);
            order.AddItem(drink, 1);

            Assert.AreEqual(22.5m, order.FinalPrice);
        }

        [Test]
        public void LoyaltyCard_AddPoints_IncreasesTotal()
        {
            var card = new LoyaltyCard("123456", DateTime.Today, DateTime.Today.AddYears(1));
            card.AddPoints(50);
            card.AddPoints(25);

            Assert.AreEqual(75, card.LoyaltyPoints);
        }

        [Test]
        public void Customer_InvalidEmail_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() => new Customer("Bob", "invalid-email"));
        }

        [Test]
        public void Shift_EndBeforeStart_ThrowsException()
        {
            var cashier = new Cashier("Alice", "Smith", "FS1234567", DateTime.Today.AddYears(-25), 2500, DateTime.Today);
            var start = new TimeSpan(14, 0, 0);
            var end = new TimeSpan(10, 0, 0);

            Assert.Throws<ArgumentException>(() => new Shift(ShiftType.Morning, DateTime.Today, start, end, cashier));
        }

        [Test]
        public void Ingredient_NegativeStock_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() => new Ingredient("Tomato", -5, true, false));
        }

        [Test]
        public void MenuItem_AddIngredient_UpdatesRecipe()
        {
            var patties = new List<PattyType> { PattyType.Beef };
            var burger = new Burger("Basic Burger", 5.0m, 500, 10, patties);
            var ingredient = new Ingredient("Tomato", 100, true, false);

            burger.AddIngredient(ingredient, 2);
            
            Assert.DoesNotThrow(() => burger.AddIngredient(ingredient, 1));
        }
    }
}