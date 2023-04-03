using System;
using System.Collections.Generic;
using Packages.LiteRPG.Runtime.LiteRPG.Stats;
using UnityEngine;

namespace LiteRPG.Stats.StatusEffect
{
    [Serializable]
    public class StatusEffectHandler
    {
        [SerializeField] private BattleCharStats _battleCharStats;
        [SerializeField] private List<StatusEffect> _activeEffects = new();
        private event System.Action<StatusEffect> OnStatusEffectEnded;

        public StatusEffectHandler(BattleCharStats battleCharStats)
        {
            _battleCharStats = battleCharStats;
        }

        public void AddStatusEffect(StatusEffectData statusEffectData)
        {
            var statusEffect = new StatusEffect(statusEffectData);
            statusEffect.OnStatusEffectEnded += RemoveStatusEffect;
            statusEffect.AddReferenceInCharacter(_battleCharStats.AddModifier(statusEffectData.StatModifierData));
            _activeEffects.Add(statusEffect);
        }

        public void UpdateByTime(float f)
        {
            for (int i = 0; i < _activeEffects.Count; i++)
            {
                _activeEffects[i].UpdateByTime(f);
            }
        }

        public int GetNumberOfStatusEffects() => 
            _activeEffects.Count;
        
        private void RemoveStatusEffect(StatusEffect statusEffect)
        {
            statusEffect.OnStatusEffectEnded -= RemoveStatusEffect;

            _battleCharStats.RemoveModifier(statusEffect.EffectData.StatModifierData, statusEffect.ModifierInstanceInCharacter);
            _activeEffects.Remove(statusEffect);
        }
    }
}