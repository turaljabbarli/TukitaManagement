using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TukitaSystem.Tests
{
    public class ManagerTrainingTests
    {
        private ManagerRole GetManagerRole(Employee emp)
        {
            return (ManagerRole)emp.Role;
        }

        private Employee CreateManagerEmployee(RankType rank)
        {
            return EmployeeFactory.CreateManager(
                "Test", 
                "Manager", 
                "PASS123", 
                DateTime.Now.AddYears(-30), 
                5000, 
                DateTime.Now.AddYears(-5), 
                rank, 
                5
            );
        }

        [Test]
        public void CreateTraining_WithLeadManagerAsTeacher_ShouldSucceedAndLink()
        {
            var leadEmp = CreateManagerEmployee(RankType.Lead);
            var leadRole = GetManagerRole(leadEmp);
            
            var training = new Training(null, leadRole, DateTime.Now);

            Assert.Contains(training, leadRole.LeadingTrainings.ToList()); 
            
            Assert.AreEqual(leadRole, training.ManagerTeacher);          
        }

        [Test]
        public void CreateTraining_WithJuniorManagerAsTeacher_ShouldThrowException()
        {
            var juniorEmp = CreateManagerEmployee(RankType.Junior);
            var juniorRole = GetManagerRole(juniorEmp);

            var ex = Assert.Throws<InvalidOperationException>(() => 
                new Training(null, juniorRole, DateTime.Now)
            );
            
            Assert.That(ex.Message, Does.Contain("Lead").Or.Contain("rank"));
        }

        [Test]
        public void RemoveLeadingTraining_ShouldUnlinkBothSides()
        {
            var leadEmp = CreateManagerEmployee(RankType.Lead);
            var leadRole = GetManagerRole(leadEmp);
            
            var training = new Training(null, leadRole, DateTime.Now);

            leadRole.RemoveLeadingTraining(training);

            Assert.IsFalse(leadRole.LeadingTrainings.Contains(training)); 
            Assert.IsNull(training.ManagerTeacher);
        }

        [Test]
        public void ReassignTeacher_ShouldUpdateListsCorrectly()
        {
            var leadEmp1 = CreateManagerEmployee(RankType.Lead);
            var leadRole1 = GetManagerRole(leadEmp1);

            var leadEmp2 = CreateManagerEmployee(RankType.Lead);
            var leadRole2 = GetManagerRole(leadEmp2);
            
            var training = new Training(null, leadRole1, DateTime.Now);

            training.SetManagerTeacher(leadRole2);

            Assert.IsFalse(leadRole1.LeadingTrainings.Contains(training));
            
            Assert.IsTrue(leadRole2.LeadingTrainings.Contains(training));
            Assert.AreEqual(leadRole2, training.ManagerTeacher);
        }

        [Test]
        public void AssignStudent_ShouldLinkCorrectly()
        {
            var juniorEmp = CreateManagerEmployee(RankType.Junior);
            var juniorRole = GetManagerRole(juniorEmp);
            
            var training = new Training(juniorRole, null, DateTime.Now);

            Assert.AreEqual(training, juniorRole.StudentTraining);
            Assert.AreEqual(juniorRole, training.ManagerStudent);
        }

        [Test]
        public void Student_CanOnlyHaveOneTrainingAtATime()
        {
            var studentEmp = CreateManagerEmployee(RankType.Senior);
            var studentRole = GetManagerRole(studentEmp);
            
            var training1 = new Training(studentRole, null, DateTime.Now);
            var training2 = new Training(null, null, DateTime.Now); 

            studentRole.SetStudentTraining(training2);

            Assert.AreEqual(training2, studentRole.StudentTraining);
            Assert.AreEqual(studentRole, training2.ManagerStudent);

            Assert.IsNull(training1.ManagerStudent);
        }

        [Test]
        public void FullTrainingSetup_TeacherAndStudent()
        {
            var teacherEmp = CreateManagerEmployee(RankType.Lead);
            var teacherRole = GetManagerRole(teacherEmp);
            
            var studentEmp = CreateManagerEmployee(RankType.Junior);
            var studentRole = GetManagerRole(studentEmp);

            var training = new Training(studentRole, teacherRole, DateTime.Now);

            Assert.Contains(training, teacherRole.LeadingTrainings.ToList());
            Assert.AreEqual(teacherRole, training.ManagerTeacher);

            Assert.AreEqual(training, studentRole.StudentTraining);
            Assert.AreEqual(studentRole, training.ManagerStudent);
        }
    }
}