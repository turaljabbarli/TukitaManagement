using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace TukitaSystem.Tests
{
    public class InheritanceTests
    {
        [Test]
        public void DynamicInheritance_EmployeeCanChangeRole()
        {
            var cashierRole = new CashierRole();
            var employee = new Employee("John", "Doe", "AB123456", new DateTime(1990, 1, 1), 3000m, DateTime.Today, cashierRole);

            Assert.IsTrue(employee.Role is CashierRole);
            Assert.IsFalse(employee.Role is ManagerRole);

            var managerRole = new ManagerRole(RankType.Junior, 0);
            employee.ChangeRole(managerRole);

            Assert.IsTrue(employee.Role is ManagerRole);
            var currentRole = employee.Role as ManagerRole;
            Assert.AreEqual(RankType.Junior, currentRole.RankType);
        }

        [Test]
        public void CookRole_EnforcesSignatureDishConstraint()
        {
            Assert.Throws<InvalidOperationException>(() => 
                new CookRole("Culinary School", new List<MenuItem>())
            );
        }
    }
}