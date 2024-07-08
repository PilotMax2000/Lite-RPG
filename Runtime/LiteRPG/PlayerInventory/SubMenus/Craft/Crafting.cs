using LiteRPG.PlayerInventory.InvItem;
using LiteRPG.PlayerInventory.SubMenus.Craft.Recipes;
using UnityEngine;

namespace LiteRPG.PlayerInventory.SubMenus.Craft
{
  public class Crafting
  {
    private InvItemsDb _itemsDb;
    private Inventory _inventory;
    private RecipesBook _recipesBook;

    public Crafting(Inventory inventory, InvItemsDb itemsDb, RecipesBook recipesBook)
    {
      _recipesBook = recipesBook;
      _inventory = inventory;
      _itemsDb = itemsDb;
    }

    public bool CraftItemFromRecipe(int recipeId)
    {
      if (CanFindRecipeInBook() == false)
      {
        Debug.LogWarning($"Failed to found recipe with id {recipeId}");
        return false;
      }
      
      RecipeData recipe = _recipesBook.GetData(recipeId);
      if (NotEnoughItemsForCrafting())
      {
        Debug.LogWarning("Not enough items for crafting");
        return false;
      }
      
      TakeRequiredItemsFromInventory(recipe);
      if (AddCraftedItemInSlot(recipe) == false)
      {
        Debug.LogWarning("Failed to add crafted item to inventory slot");
        return false;
      }
      
      return true;
      
      bool CanFindRecipeInBook() => 
        _recipesBook.DataExists(recipeId);
      
      bool NotEnoughItemsForCrafting() => 
        EnoughItemsForCrafting(recipe) == false;
    }

    private bool AddCraftedItemInSlot(RecipeData recipe) => 
      _inventory.AddItem(recipe.ItemToCraft, recipe.ResultQuantity);

    private void TakeRequiredItemsFromInventory(RecipeData recipe)
    {
      foreach (RequiredItems requiredItem in recipe.RequiredItems)
      {
        _inventory.RemoveItems(requiredItem.InvItemData, requiredItem.Quantity);
      }
    }

    public bool EnoughItemsForCrafting(RecipeData recipe)
    {
      foreach (RequiredItems requiredItem in recipe.RequiredItems)
      {
        if (NotEnoughItemsFromCrafting(requiredItem))
          return false;
      }
      return true;

      bool NotEnoughItemsFromCrafting(RequiredItems requiredItem) => 
        _inventory.HasItemInSlotsOfQuantity(requiredItem.InvItemData, requiredItem.Quantity) == false;
    }

    public bool EnoughItemsForCrafting(int recipeId)
    {
      if (_recipesBook.DataExists(recipeId) == false)
        return false;

      return EnoughItemsForCrafting(_recipesBook.GetData(recipeId));
    }
    
    
  }
}