using FluentAssertions;
using NUnit.Framework;
using Packages.LiteRPG.Runtime.LiteRPG.Stats;
using Packages.LiteRPG.Runtime.LiteRPG.Stats.StatsSystem;

namespace Tests
{
  public class StatsTests
  {
    [Test]
    public void WhenBattlePlayerStatsInitialised_AndInitHpStatIsNot0WithBaseValue10_ThenFinalValueShouldBe10()
    {
      // Arrange.
      BattleCharStats battleCharStats = Create.CreateBattleCharStats();
      CharStatsData charStatsData = Create.LoadCharStatsData("BasePlayerWith10Hp");
      
      // Act.
      battleCharStats.Init(charStatsData);

      // Assert.
      battleCharStats.GetStat("HP").BaseValue.Should().NotBe(0f);
      battleCharStats.GetStat("HP").Value.Should().Be(10f);
    }
    
    [Test]
    public void WhenPlayerWithBase10HpStatsInitialised_AndGet5HpAddModifier_ThenFinalValueShouldBe15()
    {
      // Arrange.
      BattleCharStats battleCharStats = Create.CreateBattleCharStats();
      CharStatsData charStatsData = Create.LoadCharStatsData("BasePlayerWith10Hp");
      StatModifierData add5HpMod = Create.LoadStatModifier("FlatAdd5HpMod");
      
      // Act.
      battleCharStats.Init(charStatsData);
      battleCharStats.AddModifier(add5HpMod);

      // Assert.
      battleCharStats.GetStat("HP").BaseValue.Should().Be(10f);
      battleCharStats.GetStat("HP").Value.Should().Be(15f);
    }
    
    [Test]
    public void WhenPlayerWithBase10HpStatsInitialised_AndGet10PercentAddModifier_ThenFinalValueShouldBe12()
    {
      // Arrange.
      BattleCharStats battleCharStats = Create.CreateBattleCharStats();
      CharStatsData charStatsData = Create.LoadCharStatsData("BasePlayerWith10Hp");
      StatModifierData add20PercentHp = Create.LoadStatModifier("20PercentAddHpMod");
      
      // Act.
      battleCharStats.Init(charStatsData);
      battleCharStats.AddModifier(add20PercentHp);

      // Assert.
      battleCharStats.GetStat("HP").BaseValue.Should().Be(10f);
      battleCharStats.GetStat("HP").Value.Should().Be(12f);
    }
    
    [Test]
    public void WhenPlayerWithBase10HpStatsInitialised_AndGet50PercentAddAnd50PercentMultModifier_ThenFinalValueShouldBe12()
    {
      // Arrange.
      BattleCharStats battleCharStats = Create.CreateBattleCharStats();
      CharStatsData charStatsData = Create.LoadCharStatsData("BasePlayerWith10Hp");
      StatModifierData add20Percent = Create.LoadStatModifier("20PercentAddHpMod");
      StatModifierData multiply50PercentHp = Create.LoadStatModifier("50PercentMultHpMod");
      
      // Act.
      battleCharStats.Init(charStatsData);
      battleCharStats.AddModifier(add20Percent);
      battleCharStats.AddModifier(multiply50PercentHp);

      // Assert.
      battleCharStats.GetStat("HP").BaseValue.Should().Be(10f);
      battleCharStats.GetStat("HP").Value.Should().Be(18f);
    }
  }
}