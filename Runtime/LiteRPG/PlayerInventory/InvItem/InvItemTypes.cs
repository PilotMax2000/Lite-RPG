using UnityEngine;

namespace LiteRPG.PlayerInventory.InvItem
{
 [CreateAssetMenu(fileName = "InvItemTypesList", menuName = "Items/InvItemTypesList")]
  public class InvItemTypes : ObjectTypesSO
  {
    [Header("Other Types")]
    public InvItemType[] UsableTypes;

    public bool IsUsableType(InvItemType invItemType)
    {
      foreach (var usableType in UsableTypes)
      {
        if (usableType.SameTypeOf(invItemType)) 
          return true;
      }
      return false;
    }
  }
}