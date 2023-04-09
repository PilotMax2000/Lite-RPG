using Packages.LiteRPG.Runtime.LiteRPG.Stats.StatsSystem;
using UnityEngine;

namespace LiteRPG.Stats.StatusEffect
{
    [CreateAssetMenu(fileName = "Status Effect", menuName = "Character/StatusEffectData")]
    public class StatusEffectData : ScriptableObject
    {
        public string Title;
        public StatModifierData StatModifierData;
        public float Duration;
        public Sprite Icon;
    }
}