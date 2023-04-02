using System.Collections.Generic;
using FluentAssertions;
using LevelGameplay.Generic;
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
            //Add Status effect Data to the handler

            // Assert.
            battleCharStats.GetStat("Base Attack").Value.Should().Be(2);
        }
    }

    public class StatusEffectHandler
    {
        private readonly BattleCharStats _battleCharStats;
        private readonly List<StatusEffect> _statusEffects = new List<StatusEffect>();

        public StatusEffectHandler(BattleCharStats battleCharStats)
        {
            _battleCharStats = battleCharStats;
        }

        public void AddStatusEffect(StatusEffectData statusEffectData)
        {
            //Add Status effect to queue
            //Add Status effect to the handler
            var statusEffect = new StatusEffect(statusEffectData);
            _statusEffects.Add(statusEffect);
            _battleCharStats.AddModifier(statusEffectData.StatModifierData);
        }
    }

    public class StatusEffect
    {
        private readonly StatusEffectData _statusEffectData;
        private CooldownTimer _effectTimer;

        public StatusEffect(StatusEffectData statusEffectData)
        {
            _statusEffectData = statusEffectData;
            _effectTimer = new CooldownTimer(_statusEffectData.Duration);
        }
    }
}