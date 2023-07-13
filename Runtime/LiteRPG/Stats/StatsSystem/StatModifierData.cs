using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Packages.LiteRPG.Runtime.LiteRPG.Stats.StatsSystem
{
  [CreateAssetMenu(fileName = "StatModifier", menuName = "Character/StatModifier")]
  public class StatModifierData : ScriptableObject
  {
    public CharStatTypeSO StatType;
    public float Value;
    public StatModType Type;
    
    [Header("Rewrite Order")]
    public bool RewriteOrder;
    public int Order;

    public StatModifier Create() => 
      RewriteOrder ? new StatModifier(Value, Type, Order) : new StatModifier(Value, Type);
    
    public StatModifier Create(Object source) => 
      RewriteOrder ? new StatModifier(Value, Type, Order, source) : new StatModifier(Value, Type, source);
  }

  [Serializable]
  public class StatModifierInfo
  {
    public CharStatTypeSO StatType;
    public float Value;
    public StatModType Type;
    
    [Header("Rewrite Order")]
    public bool RewriteOrder;
    public int Order;

    public StatModifier Create() => 
      RewriteOrder ? new StatModifier(Value, Type, Order) : new StatModifier(Value, Type);
    
    public StatModifier Create(Object source) => 
      RewriteOrder ? new StatModifier(Value, Type, Order, source) : new StatModifier(Value, Type, source);
  }
}