using System.IO;
using LiteRPG.Character;
using LiteRPG.LootBox;
using LiteRPG.PlayerInventory;
using LiteRPG.PlayerInventory.InvItem;
using LiteRPG.PlayerInventory.SubMenus.Craft.Recipes;
using LiteRPG.Progress;
using LiteRPG.Stats.StatusEffect;
using Packages.LiteRPG.Runtime.LiteRPG.Stats;
using Packages.LiteRPG.Runtime.LiteRPG.Stats.StatsSystem;
using UnityEngine;

namespace Tests
{
  public class Create : MonoBehaviour
  {
    public static T LoadSOFromResources<T>(string path) where T : ScriptableObject
    {
      T scriptableObject = Resources.Load(path, typeof(T)) as T;
      if (scriptableObject == null)
        Debug.LogError("Could not load scriptable object from path: " + path);
      else
        Debug.Log("Loaded from the pass " + path);
      return scriptableObject;
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
      var resPath = Path.Combine(GameDesign.TestingFolder, path);
      Debug.Log($"Trying to load test item using path {resPath}");
      return resPath;
    }

    public static InvItemsDb LoadInvItemsDbFromResources()
    {
      return Resources.Load(GetPathFromTestingFolder("InvItems/InvItemsDB")) as InvItemsDb;
    }

    public static RecipeData LoadRecipe(string recipeName)
    {
      return Create.LoadSOFromResources<RecipeData>(GetPathFromTestingFolder("Recipes/" + recipeName));
    }

    public static Inventory CreateInventory(IMoneyProgress moneyProgress, InvItemsDb itemsDb)
    {
      AdditiveHp additiveHp = new AdditiveHp();
      Inventory inventory = ScriptableObject.CreateInstance<Inventory>();
      InventoryBackpack backpack = new InventoryBackpack(inventory, additiveHp);
      inventory.Construct(backpack, moneyProgress, itemsDb, Create.LoadRecipesBook());
      return inventory;
    }
    
    public static Inventory CreateInventory(int maxSlots, IMoneyProgress moneyProgress, InvItemsDb itemsDb)
    {
      AdditiveHp additiveHp = new AdditiveHp();
      Inventory inventory = ScriptableObject.CreateInstance<Inventory>();
      InventoryBackpack backpack = new InventoryBackpack(maxSlots, inventory, additiveHp);
      inventory.Construct(backpack, moneyProgress, itemsDb, Create.LoadRecipesBook());
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
      IMoneyProgress moneyProgress = new TestGameProgress();
      InvItemsDb itemsDb = Create.LoadInvItemsDbFromResources();
      Inventory inventory = Create.CreateInventory(moneyProgress, itemsDb);
      return inventory;
    }
    
    public static Inventory InventoryWithCharStatsAndItemsDb(int maxSlots)
    {
      IMoneyProgress moneyProgress = new TestGameProgress();
      InvItemsDb itemsDb = Create.LoadInvItemsDbFromResources();
      Inventory inventory = Create.CreateInventory(maxSlots, moneyProgress, itemsDb);
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

    public static StatusEffectData LoadStatusEffectData(string statusEffectName)
    {
      return LoadSOFromResources<StatusEffectData>(GetPathFromTestingFolder("CharStats/StatusEffects/" + statusEffectName));
    }

    public static LevelingTableData LoadLevelingTable()
    {
      return LoadSOFromResources<LevelingTableData>(GetPathFromTestingFolder("CharStats/LevelingTables/BaseLevelingTable"));
    }

    public static BattleCharStats CreateBattleCharStats()
    {
      GameObject go = new GameObject();
      return go.AddComponent<BattleCharStats>();
    }

    public static BattleCharStats FullBattleCharStatsWith1AtkAnd10Hp()
    {
      BattleCharStats battleCharStats = CreateBattleCharStats();
      CharStatsData charStatsData = Create.LoadCharStatsData("T_PlayerWith1BaseAttack10Hp");
      battleCharStats.Init(charStatsData);
      return battleCharStats;
    }
  }
}