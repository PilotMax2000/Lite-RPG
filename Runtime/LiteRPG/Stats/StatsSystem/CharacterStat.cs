using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

[Serializable]
public class CharacterStat
{
  public float BaseValue;
  public event Action ValueWasChanged;
  public float Value
  {
    get
    {
      if (_isDirty == false && Mathf.Abs(BaseValue - _lastBaseValue) < Mathf.Epsilon)
        return _value;

      _lastBaseValue = BaseValue;
      _value = CalculateFinalValue();
      _isDirty = false;
      return _value;
    }
  }

  public List<StatModifier> StatModifiers => _statModifiers;
  
  [SerializeField] protected List<StatModifier> _statModifiers;
  
  protected float _value;
  protected bool _isDirty;
  protected float _lastBaseValue = float.MinValue;


  public CharacterStat()
  {
    _statModifiers = new List<StatModifier>();
  }
  
  public CharacterStat(float baseValue) : this()
  {
    BaseValue = baseValue;
  }

  public void AddModifier(StatModifier mod)
  {
    _isDirty = true;
    _statModifiers.Add(mod);
    _statModifiers.Sort(CompareModifierOrder);
    ValueWasChanged?.Invoke();
  }

  protected int CompareModifierOrder(StatModifier a, StatModifier b)
  {
    if (a.Order < b.Order)
      return -1;
    else if (a.Order > b.Order)
      return 1;
    return 0; //if (a.Order == b.Order
  }

  public bool RemoveModifier(StatModifier mod)
  {
    if (_statModifiers.Remove(mod) == false) 
      return false;
    
    _isDirty = true;
    ValueWasChanged?.Invoke();
    return _isDirty;
  }

  public bool RemoveAllModifiersFromSource(object source)
  {
    bool didRemoved = false;
    for (int i = _statModifiers.Count - 1; i >= 0; i--)
    {
      if (_statModifiers[i].Source == source)
      {
        _isDirty = true;
        didRemoved = true;
        _statModifiers.RemoveAt(i);
      }
    }
    
    if(_isDirty)
      ValueWasChanged?.Invoke();
    
    return didRemoved;
  }

  public bool IsUsingModifiersFromItem()
  {
    return _statModifiers.Any(modifier => modifier.Source != null);
  }

  protected float CalculateFinalValue()
  {
    float finalValue = BaseValue;
    float sumPercentAdd = 0;
    
    for (int i = 0; i < _statModifiers.Count; i++)
    {
      StatModifier mod = _statModifiers[i];

      if (mod.Type == StatModType.Flat)
      {
        finalValue += mod.Value;
      }
      else if (mod.Type == StatModType.PercentAdd)
      {
        sumPercentAdd += mod.Value;
        if (i + 1 >= _statModifiers.Count || _statModifiers[i + 1].Type != StatModType.PercentAdd)
        {
          finalValue *= 1 + sumPercentAdd / 100f;
          sumPercentAdd = 0;
        }
      }
      else if(mod.Type == StatModType.PercentMul)
      {
        finalValue *= 1 + mod.Value / 100f;
      }
    }
    
    return (float)Math.Round(finalValue, 4);
  }

}
