

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

        [Test]
        public void CreateTraining_WithLeadManager_ShouldSucceedAndLink()
        {
            
            var leadManager = CreateManager(RankType.Lead);
            
            
            var training = new Training(leadManager, DateTime.Now);

            
            Assert.Contains(training, leadManager.Trainings.ToList()); 
            Assert.AreEqual(leadManager, training.Manager);          
        }

        [Test]
        public void CreateTraining_WithJuniorManager_ShouldThrowException()
        {
           
            var juniorManager = CreateManager(RankType.Junior);

            
            var ex = Assert.Throws<InvalidOperationException>(() => 
                new Training(juniorManager, DateTime.Now)
            );
            
            Assert.That(ex.Message, Does.Contain("Lead"));
        }

        

        [Test]
        public void RemoveTraining_ShouldUnlinkBothSides()
        {
            
            var leadManager = CreateManager(RankType.Lead);
            var training = new Training(leadManager, DateTime.Now);

            
            leadManager.RemoveTraining(training);

            
            Assert.IsFalse(leadManager.Trainings.Contains(training)); 
            Assert.IsNull(training.Manager);
        }

        [Test]
        public void ReassignManager_ShouldUpdateListsCorrectly()
        {
            
            var lead1 = CreateManager(RankType.Lead);
            var lead2 = CreateManager(RankType.Lead);
            var training = new Training(lead1, DateTime.Now);

            
            training.SetManager(lead2);

            
            Assert.IsFalse(lead1.Trainings.Contains(training));
            Assert.IsTrue(lead2.Trainings.Contains(training));
            Assert.AreEqual(lead2, training.Manager);
        }
    }
}