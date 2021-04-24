using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LiteRPG.PlayerInventory.InvItem
{
  [CreateAssetMenu(fileName = "InvItemType", menuName = "Items/InvItemType")]
  public class InvItemType : ObjectTypeSO
  {
    public InvItemTypes InvItemTypes()
    {
      return (InvItemTypes) ObjectTypes;
    }
  }
}