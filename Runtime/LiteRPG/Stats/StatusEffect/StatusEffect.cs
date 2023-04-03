using System;
using LevelGameplay.Generic;

namespace LiteRPG.Stats.StatusEffect
{
    public class StatusEffect
    {
        public StatusEffectData EffectData => _statusEffectData;

        public StatModifier ModifierInstanceInCharacter
        {
            get => _modifierInstanceInCharacter;
            set => _modifierInstanceInCharacter = value;
        }

        public event Action<StatusEffect> OnStatusEffectEnded;

        private readonly StatusEffectData _statusEffectData;
        private CooldownTimer _effectTimer;
        private StatModifier _modifierInstanceInCharacter;


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
            _effectTimer.UpdateByTime(value);
        }

        public void AddReferenceInCharacter(StatModifier addModifier)
        {
            ModifierInstanceInCharacter = addModifier;
        }
    }
}