using LiteRPG.PlayerInventory;
using LiteRPG.PlayerInventory.SubMenus.Craft.Recipes;
using UnityEngine;

namespace Tests
{
  public class Setup : MonoBehaviour
  {
    public static void AddToInventoryNeededItemsForCraftingRecipe(RecipeData hammerRecipe, Inventory inventory)
    {
      foreach (var requiredItem in hammerRecipe.RequiredItems)
      {
        inventory.AddItem(requiredItem.InvItemData, requiredItem.Quantity);
      }
    }

    public static void AddRecipeToBookAndEnoughItemsForCraftingIt(Inventory inventory, RecipeData hammerRecipe)
    {
      Setup.AddToInventoryNeededItemsForCraftingRecipe(hammerRecipe, inventory);
      inventory.Crafting.TryAddRecipe(hammerRecipe.Id);
    }
  }
}