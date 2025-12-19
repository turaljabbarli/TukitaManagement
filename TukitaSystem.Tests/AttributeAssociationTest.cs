using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TukitaSystem.Tests
{
    public class AttributeAssociationTest
    {
        private Employee emp1;
        private Employee emp2;

        [SetUp]
        public void Setup()
        {
            emp1 = EmployeeFactory.CreateCashier("John", "Smith", "P123", 
                new DateTime(1990, 1, 1), 3000, new DateTime(2020, 1, 1));

            emp2 = EmployeeFactory.CreateCashier("Anna", "Johnson", "P222",
                new DateTime(1985, 5, 10), 3200, new DateTime(2018, 1, 1));
        }

        private Order CreateTestOrder()
        {
            var cashier = EmployeeFactory.CreateCashier("Test", "Cashier", "PASS123", DateTime.Today.AddYears(-20), 2000, DateTime.Today);
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
        
        [Test]
        public void Shift_IsCreated_WithAssignmentsForAllEmployees()
        {
            var shift = new Shift(
                ShiftType.Morning,
                DateTime.Today,
                TimeSpan.FromHours(8),
                TimeSpan.FromHours(16),
                new List<Employee> { emp1, emp2 }
            );

            Assert.AreEqual(2, shift.Assignments.Count);
            Assert.AreEqual(1, emp1.Assignments.Count);
            Assert.AreEqual(1, emp2.Assignments.Count);
        }
        
        [Test]
        public void Assignment_CanBeUpdated()
        {
            var shift = new Shift(
                ShiftType.Evening,
                DateTime.Today,
                TimeSpan.FromHours(8),
                TimeSpan.FromHours(16),
                new List<Employee> { emp1 }
            );

            var assignment = shift.Assignments[0];
            assignment.UpdateAttendance(true, 8);

            Assert.IsTrue(assignment.IsPresent);
            Assert.AreEqual(8, assignment.HoursWorked);
        }
        
        [Test]
        public void CannotCreateDuplicateAssignment()
        {
            var shift = new Shift(
                ShiftType.Evening,
                DateTime.Today,
                TimeSpan.FromHours(8),
                TimeSpan.FromHours(16),
                new List<Employee> { emp1 }
            );

            Assert.Throws<InvalidOperationException>(() =>
            {
                new ShiftAssignment(emp1, shift, DateTime.Today, false, 0);
            });
        }
        
        [Test]
        public void ReverseLink_IsCreatedCorrectly()
        {
            var shift = new Shift(
                ShiftType.Morning,
                DateTime.Today,
                TimeSpan.FromHours(8),
                TimeSpan.FromHours(16),
                new List<Employee> { emp1, emp2 }
            );

            Assert.AreSame(shift, emp1.Assignments[0].Shift);
            Assert.AreSame(emp1, shift.Assignments[0].Employee);

            Assert.AreSame(shift, emp2.Assignments[0].Shift);
            Assert.AreSame(emp2, shift.Assignments[1].Employee);
        }
        
        [Test]
        public void CannotRemoveLastAssigmentFromShift()
        {
            var shift = new Shift(
                ShiftType.Morning,
                DateTime.Today,
                TimeSpan.FromHours(8),
                TimeSpan.FromHours(16),
                new List<Employee> { emp1 }
                );

            var assignment = emp1.Assignments[0];

            Assert.Throws<InvalidOperationException>(() =>
            {
                assignment.Remove();
            });
        }
        
        [Test]
        public void RemoveAssignment_RemovesFromBothEnds()
        {
            var shift = new Shift(
                ShiftType.Morning,
                DateTime.Today,
                TimeSpan.FromHours(8),
                TimeSpan.FromHours(16),
                new List<Employee> { emp1 }
            );

            var assignment = emp1.Assignments[0];

            Assert.Throws<InvalidOperationException>(() => assignment.Remove(),
                "Should not be allowed because it is the only assignment for the shift");
            
            var assignment2 = new ShiftAssignment(emp2, shift, DateTime.Today, false, 0);

            assignment.Remove();

            Assert.AreEqual(1, shift.Assignments.Count);
            Assert.AreEqual(1, emp2.Assignments.Count);
            Assert.AreEqual(0, emp1.Assignments.Count);
        }
    }
}