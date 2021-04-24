using LiteRPG.PlayerInventory.SubMenus.Craft.Recipes;
using UnityEngine;

namespace LiteRPG.PlayerInventory.InvItem
{
  [CreateAssetMenu(fileName = "InvItemRecipeData", menuName = "Items/InvItemRecipeData")]
  public class InvItemRecipeData : InvItemData
  {
    [Header("Recipe Item")]
    public RecipeData RecipeData;

    public override bool Use(Inventory inventory)
    {
      if (CanBeUsed() == false)
        return false;
      
      inventory.RecipesBook.AddRecipe(RecipeData.Id);
      return true;
    }
  }
}