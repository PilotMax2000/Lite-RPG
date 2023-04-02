using LevelGameplay.Generic;

namespace Tests
{
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