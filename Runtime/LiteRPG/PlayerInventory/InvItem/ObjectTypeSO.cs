using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LiteRPG.PlayerInventory.InvItem
{
  [Serializable]
  public abstract class ObjectTypeSO : ScriptableObject
  {
    public string Name;
    public int Id;
    public ObjectTypesSO ObjectTypes;
    
    public IEnumerable<ObjectTypeSO> GetTypes()
    {
      // alternately, use a dictionary keyed by value
      return ObjectTypes.Types;
    }

    public ObjectTypeSO FromString(string roleString)
    {
      return GetTypes().Single(r => String.Equals(r.Name, roleString, StringComparison.OrdinalIgnoreCase));
    }

    public ObjectTypeSO FromId(int id)
    {
      return GetTypes().Single(r => r.Id == id);
    }
  }

  public static class ObjectTypeHelper
  {
    public static bool SameTypeOf(this ObjectTypeSO thisType, ObjectTypeSO itemTypeToCompare) =>
      thisType.Id == itemTypeToCompare.Id;
  }
}

