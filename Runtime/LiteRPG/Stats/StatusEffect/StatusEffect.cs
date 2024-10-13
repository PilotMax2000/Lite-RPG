using System;
using LevelGameplay.Generic;
using UnityEngine;

namespace LiteRPG.Stats.StatusEffect
{
    [Serializable]
    public class StatusEffect
    {
        public StatusEffectData EffectData => _statusEffectData;

        public StatModifier ModifierInstanceInCharacter
        {
            get => _modifierInstanceInCharacter;
            set => _modifierInstanceInCharacter = value;
        }

        public event Action<StatusEffect> OnStatusEffectEnded;
        public event Action<float> OnTimerUpdated;

        [SerializeField] private StatusEffectData _statusEffectData;
        [SerializeField] private CooldownTimer _effectTimer;
        [SerializeField] private StatModifier _modifierInstanceInCharacter;


        public StatusEffect(StatusEffectData statusEffectData)
        {
            _statusEffectData = statusEffectData;
            _effectTimer = new CooldownTimer(EffectData.Duration);
            _effectTimer.OnIsOverChanged += TriggerStatusEffectEnded;
            _effectTimer.SetTimerAsActive(true);
        }

        private void TriggerStatusEffectEnded(bool obj)
        {
            _effectTimer.OnIsOverChanged -= TriggerStatusEffectEnded;
            OnStatusEffectEnded?.Invoke(this);
        }

        public void UpdateByTime(float value)
        {
            _effectTimer.UpdateTickTime(value);
            OnTimerUpdated?.Invoke(_effectTimer.TimeLeft);
        }

        public void AddReferenceInCharacter(StatModifier addModifier)
        {
            ModifierInstanceInCharacter = addModifier;
        }
    }
}