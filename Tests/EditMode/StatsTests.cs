using FluentAssertions;
using NUnit.Framework;
using Packages.LiteRPG.Runtime.LiteRPG.Stats;

namespace Tests
{
  public class StatsTests
  {
    [Test]
    public void WhenBattleStatsInitialised_AndInitHpStatIsNot0WithOnStartResetOfCurrentValue_ThenCurValueShouldBeAsInit()
    {
      // Arrange.
      BattleCharStats battleCharStats = Create.CreateBattleCharStats();
      CharStatsData charStatsData = Create.LoadCharStatsData("BasePlayerWith10Hp");
      
      // Act.
      battleCharStats.Init(charStatsData);

      // Assert.
      battleCharStats.GetStat("HP").CurValue.Should().NotBe(0f);
      battleCharStats.GetStat("HP").CurValue.Should().Be(10f);
    }
  }
}