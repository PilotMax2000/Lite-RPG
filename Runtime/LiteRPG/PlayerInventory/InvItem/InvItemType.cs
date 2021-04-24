using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LiteRPG.PlayerInventory.InvItem
{
  [CreateAssetMenu(fileName = "InvItemType", menuName = "Items/InvItemType")]
  public class InvItemType : ScriptableObject
  {
    public string Name;
    public int Value;
    public InvItemTypes InvItemTypes;

    public IEnumerable<InvItemType> List()
    {
      // alternately, use a dictionary keyed by value
      return InvItemTypes.List;
    }

    public InvItemType FromString(string roleString)
    {
      return List().Single(r => String.Equals(r.Name, roleString, StringComparison.OrdinalIgnoreCase));
    }

    public InvItemType FromValue(int value)
    {
      return List().Single(r => r.Value == value);
    }
  }

  public static class ItemTypeHelper
  {
    public static bool SameTypeOf(this InvItemType thisType, InvItemType itemTypeToCompare) => 
      thisType.Value == itemTypeToCompare.Value;
  }
}