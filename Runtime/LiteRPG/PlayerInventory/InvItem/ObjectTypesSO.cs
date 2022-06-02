using System;
using System.Linq;
using UnityEngine;

namespace LiteRPG.PlayerInventory.InvItem
{
  [Serializable]
  public class ObjectTypesSO : ScriptableObject
  {
    public ObjectTypeSO[] Types;
    
    public ObjectTypeSO GetStatById(int statId)
    {
      return (from stat in Types
              where stat.Id == statId
              select stat)
              .FirstOrDefault();
    }
  }
}