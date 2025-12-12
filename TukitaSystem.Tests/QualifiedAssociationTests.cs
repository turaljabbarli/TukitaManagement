using NUnit.Framework;
using System;
using System.Collections.Generic;
using TukitaSystem;

namespace TukitaSystem.Tests
{
    public class QualifiedAssociationTests
    {
        private Menu _menu;
        private MenuItem _burger;
        
        [SetUp]
        public void Setup()
        {
            _menu = new Menu("Lunch Menu", "12:00-15:00");
            var patties = new List<PattyType> { PattyType.Beef };
            _burger = new Burger("BigMac", 20.0m, 500, "10 min", patties);
        }

        [Test]
        public void AddMenuItem_ShouldAddToDictionary()
        {
            _menu.AddMenuItem(_burger);

            Assert.IsTrue(_menu.QualifiedItems.ContainsKey(_burger.Name));
            Assert.AreEqual(_burger, _menu.QualifiedItems[_burger.Name]);
            
            Assert.IsTrue(_burger.Menus.Contains(_menu));
        }

        [Test]
        public void AddMenuItem_ReverseConnection_ShouldWork()
        {
            _burger.AddMenu(_menu);

            Assert.IsTrue(_menu.QualifiedItems.ContainsKey(_burger.Name));
            Assert.IsTrue(_burger.Menus.Contains(_menu));
        }

        [Test]
        public void AddMenuItem_Duplicate_ShouldBeIgnored()
        {
            _menu.AddMenuItem(_burger);
            _menu.AddMenuItem(_burger);

            Assert.AreEqual(1, _menu.QualifiedItems.Count);
        }

        [Test]
        public void RemoveMenuItem_ShouldRemoveFromDictionary()
        {
            _menu.AddMenuItem(_burger);

            _menu.RemoveMenuItem(_burger);

            Assert.IsFalse(_menu.QualifiedItems.ContainsKey(_burger.Name));
            Assert.IsFalse(_burger.Menus.Contains(_menu));
        }
    }
}
