using LiteRPG.PlayerInventory.DataBase;
using Sirenix.Utilities;
using UnityEngine;

namespace LiteRPG.PlayerInventory.SubMenus.Craft.Recipes
{
  //Should be instantiated, not loaded from folder!
  public class RecipesBook : DataDbAddable<RecipeData>
  {
    private RecipesDb _recipesDb;

    public void Construct(RecipesDb recipesDb)
    {
      if (recipesDb.DataLinks.IsNullOrEmpty())
      {
        Debug.LogError("RecipesDB is empty! Fill it with some data!");
        return;
      }
      _recipesDb = recipesDb;
    }

    public bool AddRecipe(int recipeId)
    {
      return AddData(_recipesDb.GetData(recipeId));
    }
  }
}