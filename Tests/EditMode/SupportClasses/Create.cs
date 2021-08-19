﻿using System.IO;
using LiteRPG.LootBox;
using LiteRPG.PlayerInventory;
using LiteRPG.PlayerInventory.InvItem;
using LiteRPG.PlayerInventory.SubMenus.Craft.Recipes;
using LiteRPG.Progress;
using Packages.LiteRPG.Runtime.LiteRPG.Stats;
using Packages.LiteRPG.Runtime.LiteRPG.Stats.StatsSystem;
using UnityEngine;

namespace Tests
{
  public class Create : MonoBehaviour
  {
    public static T LoadSOFromResources<T>(string path) where T : ScriptableObject
    {
      return Resources.Load(path, typeof(T)) as T;
    }

    public static RecipesBook LoadRecipesBook()
    {
      var book =  Resources.Load(GetPathFromTestingFolder("Recipes/RecipesBook")) as RecipesBook;
      var recipesDb = Create.LoadSOFromResources<RecipesDb>(GetPathFromTestingFolder("Recipes/RecipesDb"));
      book.Construct(recipesDb);
      return book;
    }

    public static string GetPathFromTestingFolder(string path)
    {
      return Path.Combine(GameDesign.TestingFolder, path);
    }

    public static InvItemsDb LoadInvItemsDbFromResources()
    {
      return Resources.Load(GetPathFromTestingFolder("InvItems/InvItemsDB")) as InvItemsDb;
    }

    public static RecipeData LoadRecipe(string recipeName)
    {
      return Create.LoadSOFromResources<RecipeData>(GetPathFromTestingFolder("Recipes/" + recipeName));
    }

    public static Inventory CreateInventory(IMoneyStats moneyStats, InvItemsDb itemsDb)
    {
      var gameObject = new GameObject("Inventory");
      InventoryBackpack backpack = new InventoryBackpack();
      Inventory inventory = gameObject.AddComponent<Inventory>();
      inventory.Construct(backpack, moneyStats, itemsDb, Create.LoadRecipesBook());
      return inventory;
    }

    public static InvItemData CreateInvItemData(int id, string name = "NoName", int price = 0)
    {
      var invItemData = ScriptableObject.CreateInstance<InvItemData>();
      invItemData.ItemName = name;
      invItemData.Id = id;
      invItemData.BasePrice = price;
      return invItemData;
    }

    public static InvItemSlot CreateItemSlot(string name, int id, int price, int quantity = 1)
    {
      InvItemData invItemData = Create.CreateInvItemData(id, name, price);
      InvItemSlot slot = new InvItemSlot(invItemData, quantity);
      return slot;
    }

    public static LootboxData LoadLootbox(string lootBoxName) =>
      Create.LoadSOFromResources<LootboxData>(GetPathFromTestingFolder("Lootboxes/" + lootBoxName));

    public static InvItemData LoadInvItem(string invItemName) =>
      Create.LoadSOFromResources<InvItemData>(GetPathFromTestingFolder("InvItems/" + invItemName));

    public static Inventory InventoryWithCharStatsAndItemsDb()
    {
      IMoneyStats moneyStats = new CharStats();
      InvItemsDb itemsDb = Create.LoadInvItemsDbFromResources();
      Inventory inventory = Create.CreateInventory(moneyStats, itemsDb);
      return inventory;
    }

    public static CharStatsData CrateCharStatsData()
    {
      var statsData = ScriptableObject.CreateInstance<CharStatsData>();
      return statsData;
    }

    public static CharStatsData LoadCharStatsData(string charName)
    {
      return LoadSOFromResources<CharStatsData>(GetPathFromTestingFolder("CharStats/CharsData/" + charName));
    }
    public static StatModifierData LoadStatModifier(string modifierName)
    {
      return LoadSOFromResources<StatModifierData>(GetPathFromTestingFolder("CharStats/Modifiers/" + modifierName));
    }

    public static BattleCharStats CreateBattleCharStats()
    {
      GameObject go = new GameObject();
      return go.AddComponent<BattleCharStats>();
    }
  }
}