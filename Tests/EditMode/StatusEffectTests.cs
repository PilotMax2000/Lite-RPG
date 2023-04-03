using FluentAssertions;
using NUnit.Framework;
using Packages.LiteRPG.Runtime.LiteRPG.Stats;

namespace Tests
{

    public class StatusEffectTests
    {
        [Test]
        public void WhenCharacterHas1BaseAttackWithStatusEffectThatGives1BaseAttackFor1Sec_AndNoTimePasses_ThenTotalAttackIs2()
        {
            // Arrange.
            BattleCharStats battleCharStats = Create.CreateBattleCharStats();
            CharStatsData charStatsData = Create.LoadCharStatsData("T_PlayerWith1BaseAttack10Hp");
            StatusEffectData statusEffectData = Create.LoadStatusEffectData("T_StatusEffect_1BaseAttackFor1Sec");
            StatusEffectHandler statusEffectHandler = new StatusEffectHandler(battleCharStats);

            // Act.
            battleCharStats.Init(charStatsData);
            statusEffectHandler.AddStatusEffect(statusEffectData);

            // Assert.
            battleCharStats.GetStat("Base Attack").Value.Should().Be(2);
        }

        [Test]
        public void WhenCharacterHasNoStatusEffect_ThenNumberOfStatusEffectsShouldBe0()
        {
            // Arrange.
            BattleCharStats battleCharStats = Create.CreateBattleCharStats();
            CharStatsData charStatsData = Create.LoadCharStatsData("T_PlayerWith1BaseAttack10Hp");
            StatusEffectData statusEffectData = Create.LoadStatusEffectData("T_StatusEffect_1BaseAttackFor1Sec");
            StatusEffectHandler statusEffectHandler = new StatusEffectHandler(battleCharStats);

            // Act.
            battleCharStats.Init(charStatsData);

            // Assert.
            statusEffectHandler.GetNumberOfStatusEffects().Should().Be(0);
        }

        [Test]
        public void WhenCharacterHasNoStatusEffect_AndEffect1BaseAttackFor1SecondWasAdded_ThenNumberOfStatusEffectsShouldBe1()
        {
            // Arrange.
            BattleCharStats battleCharStats = Create.CreateBattleCharStats();
            CharStatsData charStatsData = Create.LoadCharStatsData("T_PlayerWith1BaseAttack10Hp");
            StatusEffectData statusEffectData = Create.LoadStatusEffectData("T_StatusEffect_1BaseAttackFor1Sec");
            StatusEffectHandler statusEffectHandler = new StatusEffectHandler(battleCharStats);

            // Act.
            battleCharStats.Init(charStatsData);
            statusEffectHandler.AddStatusEffect(statusEffectData);

            // Assert.
            statusEffectHandler.GetNumberOfStatusEffects().Should().Be(1);
        }

        [Test]
        public void WhenCharacterHasStatusEffect1BaseAttackFor1Second_And1SecondPassed_ThenNumberOfEffectsShouldBe0()
        {
            // Arrange.
            BattleCharStats battleCharStats = Create.CreateBattleCharStats();
            CharStatsData charStatsData = Create.LoadCharStatsData("T_PlayerWith1BaseAttack10Hp");
            StatusEffectData statusEffectData = Create.LoadStatusEffectData("T_StatusEffect_1BaseAttackFor1Sec");
            StatusEffectHandler statusEffectHandler = new StatusEffectHandler(battleCharStats);

            // Act.
            battleCharStats.Init(charStatsData);
            statusEffectHandler.AddStatusEffect(statusEffectData);
            statusEffectHandler.UpdateByTime(1f);

            // Assert.
            statusEffectHandler.GetNumberOfStatusEffects().Should().Be(0);
        }
        [Test]
        public void WhenCharacterHasStatusEffect1BaseAttackFor1Second_AndHalfSecondPassed_ThenNumberOfEffectsShouldBe1()
        {
            // Arrange.
            BattleCharStats battleCharStats = Create.CreateBattleCharStats();
            CharStatsData charStatsData = Create.LoadCharStatsData("T_PlayerWith1BaseAttack10Hp");
            StatusEffectData statusEffectData = Create.LoadStatusEffectData("T_StatusEffect_1BaseAttackFor1Sec");
            StatusEffectHandler statusEffectHandler = new StatusEffectHandler(battleCharStats);

            // Act.
            battleCharStats.Init(charStatsData);
            statusEffectHandler.AddStatusEffect(statusEffectData);
            statusEffectHandler.UpdateByTime(.5f);

            // Assert.
            statusEffectHandler.GetNumberOfStatusEffects().Should().Be(1);
        }
        
        [Test]
        public void WhenCharacterHas1BaseAttackWithStatusEffectThatGives1BaseAttackFor1Sec_And1SecondPassed_ThenTotalAttackIs1()
        {
            // Arrange.
            BattleCharStats battleCharStats = Create.CreateBattleCharStats();
            CharStatsData charStatsData = Create.LoadCharStatsData("T_PlayerWith1BaseAttack10Hp");
            StatusEffectData statusEffectData = Create.LoadStatusEffectData("T_StatusEffect_1BaseAttackFor1Sec");
            StatusEffectHandler statusEffectHandler = new StatusEffectHandler(battleCharStats);

            // Act.
            battleCharStats.Init(charStatsData);
            statusEffectHandler.AddStatusEffect(statusEffectData);
            statusEffectHandler.UpdateByTime(1f);

            // Assert.
            battleCharStats.GetStat("Base Attack").Value.Should().Be(1);
        }
        
        [Test]
        public void WhenCharacterHas1BaseAttackWithStatusEffectThatGives1BaseAttackFor1Sec_And05SecondPassed_ThenTotalAttackIs2()
        {
            // Arrange.
            BattleCharStats battleCharStats = Create.CreateBattleCharStats();
            CharStatsData charStatsData = Create.LoadCharStatsData("T_PlayerWith1BaseAttack10Hp");
            StatusEffectData statusEffectData = Create.LoadStatusEffectData("T_StatusEffect_1BaseAttackFor1Sec");
            StatusEffectHandler statusEffectHandler = new StatusEffectHandler(battleCharStats);

            // Act.
            battleCharStats.Init(charStatsData);
            statusEffectHandler.AddStatusEffect(statusEffectData);
            statusEffectHandler.UpdateByTime(.5f);

            // Assert.
            battleCharStats.GetStat("Base Attack").Value.Should().Be(2);
        }
    }
}