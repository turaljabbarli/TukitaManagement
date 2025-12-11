using NUnit.Framework;
using System;
using System.Collections.Generic;
using TukitaSystem; // Required to access Menu, MenuItem, Burger, PattyType

namespace TukitaSystem.Tests
{
    public class QualifiedAssociationTests
    {
        private Menu _menu;
        private MenuItem _burger;
        
        [SetUp]
        public void Setup()
        {
            // Reset extent if necessary to prevent state bleeding between tests
            // Menu.ClearExtent(); 
            // MenuItem.ClearExtent();

            _menu = new Menu("Lunch Menu", "12:00-15:00");
            var patties = new List<PattyType> { PattyType.Beef };
            _burger = new Burger("BigMac", 20.0m, 500, "10 min", patties);
        }

        [Test]
        public void AddMenuItem_ShouldAddToDictionary()
        {
            // Act
            _menu.AddMenuItem(_burger);

            // Assert
            // Checks if the dictionary uses the Item Name as the key
            Assert.IsTrue(_menu.QualifiedItems.ContainsKey(_burger.Name));
            Assert.AreEqual(_burger, _menu.QualifiedItems[_burger.Name]);
            
            // Check Reverse Connection (MenuItem should know about Menu)
            Assert.IsTrue(_burger.Menus.Contains(_menu));
        }

        [Test]
        public void AddMenuItem_ReverseConnection_ShouldWork()
        {
            // Act (Start from Item side)
            _burger.AddMenu(_menu);

            // Assert
            Assert.IsTrue(_menu.QualifiedItems.ContainsKey(_burger.Name));
            Assert.IsTrue(_burger.Menus.Contains(_menu));
        }

        [Test]
        public void AddMenuItem_Duplicate_ShouldBeIgnored()
        {
            // Act
            _menu.AddMenuItem(_burger);
            _menu.AddMenuItem(_burger); // Attempt duplicate

            // Assert
            // Should handle recursion guard and not crash or add twice
            Assert.AreEqual(1, _menu.QualifiedItems.Count);
        }

        [Test]
        public void RemoveMenuItem_ShouldRemoveFromDictionary()
        {
            // Arrange
            _menu.AddMenuItem(_burger);

            // Act
            _menu.RemoveMenuItem(_burger);

            // Assert
            Assert.IsFalse(_menu.QualifiedItems.ContainsKey(_burger.Name));
            Assert.IsFalse(_burger.Menus.Contains(_menu));
        }
    }
}