using System;
using LiteRPG.PlayerInventory.InvItem;
using UnityEngine;

namespace LiteRPG.PlayerInventory.SubMenus.Craft.Recipes
{
  [CreateAssetMenu(fileName = "RecipeData", menuName = "Recipes/RecipeData")]
  public class RecipeData : ScriptableIdData
  {
    public InvItemData ItemToCraft;
    public int ResultQuantity = 1;
    public RequiredItems[] RequiredItems;
  }

  [Serializable]
  public class RequiredItems
  {
    public InvItemData InvItemData;
    public int Quantity;
  }
}