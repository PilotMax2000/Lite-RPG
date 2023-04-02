using System.Collections.Generic;
using Packages.LiteRPG.Runtime.LiteRPG.Stats;

namespace Tests
{
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
}