using System;
using System.Collections.Generic;
using Packages.LiteRPG.Runtime.LiteRPG.Stats;
using Packages.LiteRPG.Runtime.LiteRPG.Stats.StatsSystem;
using UnityEngine;
using UnityEngine.Serialization;

namespace LiteRPG.SpellsAndSkills
{
    [CreateAssetMenu(fileName = "BaseSpellData", menuName = "LiteRPG/Character/BaseSpell")]
    public class SpellData : ScriptableObject
    {
        public string SpellName = "Spell";
        public Sprite Icon;
        [TextArea(3,10)] public string Description = "Description";

        [Header("Stats")]
        public List<StatModifierAndValue> SpellStats;

        [Header("Audio")]
        public AudioClip CastSound;
    }
    
    [Serializable]
    public class StatModifierAndValue
    {
        [FormerlySerializedAs("ModifierInfo")] public StatModifierProperty ModifierProperty;
        public float Value;
    }
}