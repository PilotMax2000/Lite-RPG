using System;
using System.Collections.Generic;
using Packages.LiteRPG.Runtime.LiteRPG.Stats;
using UnityEngine;

namespace LiteRPG.Stats.StatusEffect
{
    [Serializable]
    public class StatusEffectsHandler
    {
        [SerializeField] private BattleCharStats _battleCharStats;
        [SerializeField] private List<StatusEffect> _activeEffects = new();
        public event Action<StatusEffect> OnStatusEffectEnded;
        public event Action<StatusEffect> OnStatusEffectAdded;

        public StatusEffectsHandler(BattleCharStats battleCharStats)
        {
            _battleCharStats = battleCharStats;
        }

        public void AddStatusEffect(StatusEffectData statusEffectData)
        {
            var statusEffect = new StatusEffect(statusEffectData);
            statusEffect.OnStatusEffectEnded += RemoveStatusEffect;
            statusEffect.AddReferenceInCharacter(_battleCharStats.AddModifier(statusEffectData.StatModifierData));
            _activeEffects.Add(statusEffect);
            Debug.Log($"<color=blue>[Status Effect]</color> Added \"{statusEffectData.Title}\" for {statusEffectData.Duration} seconds");
            
            OnStatusEffectAdded?.Invoke(statusEffect);
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
            OnStatusEffectEnded?.Invoke(statusEffect);
            statusEffect.OnStatusEffectEnded -= RemoveStatusEffect;

            _battleCharStats.RemoveModifier(statusEffect.EffectData.StatModifierData, statusEffect.ModifierInstanceInCharacter);
            _activeEffects.Remove(statusEffect);
            Debug.Log($"<color=blue>[Status Effect]</color> Removed \"{statusEffect.EffectData.Title}\"");
        }
    }
}