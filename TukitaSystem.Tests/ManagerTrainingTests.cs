

namespace TukitaSystem.Tests
{
    public class ManagerTrainingTests
    {
        private Manager CreateManager(RankType rank)
        {
            return new Manager(
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

        // --- Тесты для роли Учителя (Teacher / Leading) ---

        [Test]
        public void CreateTraining_WithLeadManagerAsTeacher_ShouldSucceedAndLink()
        {
            var leadManager = CreateManager(RankType.Lead);
            
            // Создаем тренинг: Студент = null, Учитель = leadManager
            var training = new Training(null, leadManager, DateTime.Now);

            // Проверяем, что тренинг попал в список LeadingTrainings учителя
            Assert.Contains(training, leadManager.LeadingTrainings.ToList()); 
            
            // Проверяем обратную связь
            Assert.AreEqual(leadManager, training.ManagerTeacher);          
        }

        [Test]
        public void CreateTraining_WithJuniorManagerAsTeacher_ShouldThrowException()
        {
            var juniorManager = CreateManager(RankType.Junior);

            // Попытка назначить Junior учителем должна вызвать ошибку
            var ex = Assert.Throws<InvalidOperationException>(() => 
                new Training(null, juniorManager, DateTime.Now)
            );
            
            // Ожидаем сообщение про ранг Lead (или общее исключение из Manager.AddLeadingTraining)
            Assert.That(ex.Message, Does.Contain("Lead").Or.Contain("rank"));
        }

        [Test]
        public void RemoveLeadingTraining_ShouldUnlinkBothSides()
        {
            var leadManager = CreateManager(RankType.Lead);
            var training = new Training(null, leadManager, DateTime.Now);

            // Учитель отказывается вести тренинг
            leadManager.RemoveLeadingTraining(training);

            Assert.IsFalse(leadManager.LeadingTrainings.Contains(training)); 
            Assert.IsNull(training.ManagerTeacher);
        }

        [Test]
        public void ReassignTeacher_ShouldUpdateListsCorrectly()
        {
            var lead1 = CreateManager(RankType.Lead);
            var lead2 = CreateManager(RankType.Lead);
            var training = new Training(null, lead1, DateTime.Now);

            // Меняем учителя через метод тренинга
            training.SetManagerTeacher(lead2);

            // Проверяем, что у первого учителя тренинг пропал
            Assert.IsFalse(lead1.LeadingTrainings.Contains(training));
            
            // Проверяем, что у второго появился
            Assert.IsTrue(lead2.LeadingTrainings.Contains(training));
            Assert.AreEqual(lead2, training.ManagerTeacher);
        }

        // --- Тесты для роли Студента (Student) ---

        [Test]
        public void AssignStudent_ShouldLinkCorrectly()
        {
            var studentManager = CreateManager(RankType.Junior); // Джуниор может быть студентом
            var training = new Training(studentManager, null, DateTime.Now);

            Assert.AreEqual(training, studentManager.StudentTraining);
            Assert.AreEqual(studentManager, training.ManagerStudent);
        }

        [Test]
        public void Student_CanOnlyHaveOneTrainingAtATime()
        {
            var student = CreateManager(RankType.Senior);
            
            var training1 = new Training(student, null, DateTime.Now);
            var training2 = new Training(null, null, DateTime.Now); // Создаем пока без студентов

            // Назначаем студенту второй тренинг
            student.SetStudentTraining(training2);

            // Проверяем, что ссылка обновилась
            Assert.AreEqual(training2, student.StudentTraining);
            Assert.AreEqual(student, training2.ManagerStudent);

            // Проверяем, что старый тренинг "забыл" этого студента
            Assert.IsNull(training1.ManagerStudent);
        }

        [Test]
        public void FullTrainingSetup_TeacherAndStudent()
        {
            var teacher = CreateManager(RankType.Lead);
            var student = CreateManager(RankType.Junior);

            // Создаем тренинг сразу с учителем и учеником
            var training = new Training(student, teacher, DateTime.Now);

            // Проверки учителя
            Assert.Contains(training, teacher.LeadingTrainings.ToList());
            Assert.AreEqual(teacher, training.ManagerTeacher);

            // Проверки студента
            Assert.AreEqual(training, student.StudentTraining);
            Assert.AreEqual(student, training.ManagerStudent);
        }
    }
}