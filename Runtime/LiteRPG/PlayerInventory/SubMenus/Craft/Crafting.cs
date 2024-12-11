using System;
using LiteRPG.PlayerInventory.InvItem;
using LiteRPG.PlayerInventory.SubMenus.Craft.Recipes;
using UnityEngine;

namespace LiteRPG.PlayerInventory.SubMenus.Craft
{
  public class Crafting
  {
    public RecipesBook RecipesBook => _recipesBook;
    public event Action<RecipeData> OnRecipesBookChanged;
    public event Action<RecipeData> OnTryAddLearnedRecipe;

    private Inventory _inventory;
    private RecipesBook _recipesBook;

    public Crafting(Inventory inventory, InvItemsDb itemsDb, RecipesBook recipesBook)
    {
      _recipesBook = recipesBook;
      _inventory = inventory;
    }

    public bool TryAddRecipe(RecipeData recipeData)
    {
      return _recipesBook.AddRecipe(recipeData.Id);
    }

    public bool TryAddRecipe(int recipeId)
    {
      if (_recipesBook.DataExists(recipeId))
      {
        OnTryAddLearnedRecipe?.Invoke(_recipesBook.GetData(recipeId));
        Debug.LogWarning($"Failed to add recipe with id {recipeId} because it was already learned");
        return false;
      }
      
      var recipeAddedSuccessfuly = _recipesBook.AddRecipe(recipeId);
      if(recipeAddedSuccessfuly)
        OnRecipesBookChanged?.Invoke(_recipesBook.GetData(recipeId));
      return recipeAddedSuccessfuly;
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

    public bool IsRecipeAddedToBook(int recipeId)
    {
      return _recipesBook.DataExists(recipeId);
    }

    private bool AddCraftedItemInSlot(RecipeData recipe) => 
      _inventory.TryAddItem(recipe.ItemToCraft, recipe.ResultQuantity);

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