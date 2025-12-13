using System.Collections.Generic;
using LiteRPG.Character;
using LiteRPG.Helper;
using LiteRPG.PlayerInventory.InvItem;
using LiteRPG.PlayerInventory.SubMenus.Craft.Recipes;
using LiteRPG.Progress;
using Packages.LiteRPG.Runtime.LiteRPG.Stats;
using UnityEngine;

namespace LiteRPG.PlayerInventory
{
    public static class InventoryCreator<TInventory> where TInventory: Inventory 
    {
        public static Inventory CreateInventory(IMoneyProgress moneyProgress, BattleCharStats battleCharStats, int inventorySlotsLimit, int equipSlotsLimit)
        {
            AdditiveHp additiveHp = new AdditiveHp();
            var inventory = ScriptableObject.CreateInstance<Inventory>();
            InventoryBackpack backpack = new InventoryBackpack(inventorySlotsLimit, inventory, additiveHp);
            inventory.Construct(backpack, moneyProgress, LoadInvItemsDbFromResources(), LoadAndInitializeRecipesBook());
            inventory.SetupEquipSlots(equipSlotsLimit, battleCharStats);
            
            return inventory;
        }

        public static TInventory CreateCustomInventory(IMoneyProgress moneyProgress, BattleCharStats battleCharStats,
            int inventorySlotsLimit, int equipSlotsLimit)
        {
            AdditiveHp additiveHp = new AdditiveHp();
            var inventory = ScriptableObject.CreateInstance<TInventory>();
            InventoryBackpack backpack = new InventoryBackpack(inventorySlotsLimit, inventory, additiveHp);
            inventory.Construct(backpack, moneyProgress, LoadInvItemsDbFromResources(), LoadAndInitializeRecipesBook());
            inventory.SetupEquipSlots(equipSlotsLimit, battleCharStats);
            
            return inventory;
        }

        public static void GiveStartingItems(IReadOnlyList<InvItemSlot> startingItemSlots, Inventory inventory)
        {
            if (startingItemSlots.Count > 0) 
                inventory.TryAddItem(startingItemSlots);
        }
        
        private static RecipesBook LoadAndInitializeRecipesBook()
        {
            RecipesBook book =  CreateRecipesBook();
            RecipesDb recipesDb = LoadRecipesDb();
            book.Construct(recipesDb);
            return book;
        }
        
        private static InvItemsDb LoadInvItemsDbFromResources() => 
            SOHelper.LoadAndInstantiateSOFromResources<InvItemsDb>("Inventory/InvItemsDb");

        private static RecipesDb LoadRecipesDb() =>
            SOHelper.LoadAndInstantiateSOFromResources<RecipesDb>("Inventory/Recipes/RecipesDb");
        
        private static RecipesBook CreateRecipesBook() => 
            ScriptableObject.CreateInstance<RecipesBook>();

    }
}