using System;

public enum StatModType
{
  Flat = 100,
  PercentAdd = 200,
  PercentMul = 300,
}

[Serializable]
public class StatModifier
{
  public object Source;
  public float Value;
  public StatModType Type;
  public int Order;
  
  public StatModifier(float value, StatModType type, int order, object source)
  {
    Value = value;
    Type = type;
    Order = order;
    Source = source;
  }

  public StatModifier(float value, StatModType type) : this(value, type, (int)type, null) { }
  public StatModifier(float value, StatModType type, int order) : this(value, type, order, null) { }
  public StatModifier(float value, StatModType type, object source) : this(value, type, (int)type, source) { }
}
