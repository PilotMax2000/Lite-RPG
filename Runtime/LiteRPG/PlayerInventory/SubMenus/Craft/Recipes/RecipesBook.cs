using LiteRPG.PlayerInventory.DataBase;
using UnityEngine;

namespace LiteRPG.PlayerInventory.SubMenus.Craft.Recipes
{
  [CreateAssetMenu(fileName = "RecipesBook", menuName = "Recipes/RecipesBook")]
  public class RecipesBook : DataDbAddable<RecipeData>
  {
    private RecipesDb _recipesDb;

    public void Construct(RecipesDb recipesDb)
    {
      _recipesDb = recipesDb;
    }

    public bool AddRecipe(int recipeId)
    {
      return AddData(_recipesDb.GetData(recipeId));
    }
  }
}