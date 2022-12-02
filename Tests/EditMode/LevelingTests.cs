using FluentAssertions;
using LiteRPG.Progress;
using NUnit.Framework;

namespace Tests
{
    public class LevelingTests
    {
        [Test]
        public void WhenLevelingSystemIsInitialized_AndNothingHappened_ThenLevelShouldBe1()
        {
            // Arrange.
            var levelingSystem = new LevelingSystem(Create.LoadLevelingTable());
        
            // Act.
        
            // Assert.
            levelingSystem.Level.Should().Be(1);
        }
        
        [Test]
        public void WhenLevelingSystemIsInitialized_AndNothingHappened_ThenCurrentExpShouldBe0()
        {
            // Arrange.
            var levelingSystem = new LevelingSystem(Create.LoadLevelingTable());
        
            // Act.
        
            // Assert.
            levelingSystem.CurrentExp.Should().Be(0);
        }
        
        [Test]
        public void WhenLevelingSystemIsInitialized_AndNothingHappened_ThenTotalExpShouldBe0()
        {
            // Arrange.
            var levelingSystem = new LevelingSystem(Create.LoadLevelingTable());
        
            // Act.
        
            // Assert.
            levelingSystem.TotalExp.Should().Be(0);
        }
        
        [Test]
        public void WhenLevelingSystemIsInitialized_And100ExpPointsWasAdded_ThenTotalExpShouldBe100()
        {
            // Arrange.
            var levelingSystem = new LevelingSystem(Create.LoadLevelingTable());
        
            // Act.
            levelingSystem.AddExp(100);
        
            // Assert.
            levelingSystem.TotalExp.Should().Be(100);
        }
        
        [Test]
        public void WhenLevelingSystemIsInitialized_AndMinus100ExpPointsWasAdded_ThenTotalExpShouldBe0()
        {
            // Arrange.
            var levelingSystem = new LevelingSystem(Create.LoadLevelingTable());
        
            // Act.
            levelingSystem.AddExp(-100);
        
            // Assert.
            levelingSystem.TotalExp.Should().Be(0);
        }
        
        [Test]
        public void WhenLevelingSystemIsInitializedWith100ExpToNextLevel_And50ExpPointsWasAdded_ThenCurrentExpShouldBe50()
        {
            // Arrange.
            var levelingSystem = new LevelingSystem(Create.LoadLevelingTable());
        
            // Act.
            levelingSystem.AddExp(50);
        
            // Assert.
            levelingSystem.CurrentExp.Should().Be(50);
        }
        
        [Test]
        public void WhenLevelingSystemRequiresTotal100ExpTo2ndLevelAnd300To3d_And150ExpPointsWasAddedFrom0_ThenTheRequiredExpOnNewLevelShouldBe200()
        {
            // Arrange.
            var levelingSystem = new LevelingSystem(Create.LoadLevelingTable());
        
            // Act.
            levelingSystem.AddExp(150);
        
            // Assert.
            levelingSystem.ExpToNextLevel.Should().Be(200);
        }

        [Test]
        public void WhenLevelingSystemIsInitializedWith100ExpToNextLevel_And150ExpPointsWasAdded_ThenCurrentExpShouldBe50()
        {
            // Arrange.
            var levelingSystem = new LevelingSystem(Create.LoadLevelingTable());
        
            // Act.
            levelingSystem.AddExp(150);
        
            // Assert.
            levelingSystem.CurrentExp.Should().Be(50);
        }
        
        [Test]
        public void WhenLevelingSystemIsInitializedWith1LevelAnd100ExpToNextLevel_And150ExpPointsWasAdded_ThenCurrentLevelShouldBe2()
        {
            // Arrange.
            var levelingSystem = new LevelingSystem(Create.LoadLevelingTable());
        
            // Act.
            levelingSystem.AddExp(150);
        
            // Assert.
            levelingSystem.Level.Should().Be(2);
        }
        
        [Test]
        public void WhenLevelingSystemIsInitializedWith1LevelAnd100ExpToNextLevel_And150ExpPointsWasAdded_ThenOnLevelUpEventShouldBeTriggered()
        {
            // Arrange.
            var levelingSystem = new LevelingSystem(Create.LoadLevelingTable());
        
            // Act.
            var isLevelUpTriggered = false;
            levelingSystem.OnLevelUp += () => isLevelUpTriggered = true;
            levelingSystem.AddExp(150);
        
            // Assert.
            isLevelUpTriggered.Should().BeTrue();
        }
        
        [Test]
        public void WhenLoadingExpIs0_AndDoNothing_ThenLevelShouldBe1WithCurrentExp0()
        {
            // Arrange.
            int totalExpToLoad = 0;
            var levelingSystem = new LevelingSystem(Create.LoadLevelingTable(), totalExpToLoad);
        
            // Act.

            // Assert.
            levelingSystem.Level.Should().Be(1);
            levelingSystem.CurrentExp.Should().Be(0);
        }
        
        [Test]
        public void WhenLoadingExpIs150_AndNeeds100ExpToLevel2_ThenLevelShouldBe2WithCurrentExp50()
        {
            // Arrange.
            const int totalExpToLoad = 150;
            var levelingSystem = new LevelingSystem(Create.LoadLevelingTable(), totalExpToLoad);
        
            // Act.

            // Assert.
            levelingSystem.Level.Should().Be(2);
            levelingSystem.CurrentExp.Should().Be(50);
        }




        // [Test]
        // public void When_And_Then()
        // {
        //     // Arrange.
        //
        //
        //     // Act.
        //
        //     // Assert.
        // }
    }
}