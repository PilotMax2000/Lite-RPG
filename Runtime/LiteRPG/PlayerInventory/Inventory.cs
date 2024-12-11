using System.Collections.Generic;
using LiteRPG.PlayerInventory.InvItem;
using LiteRPG.PlayerInventory.SubMenus.Craft;
using LiteRPG.PlayerInventory.SubMenus.Craft.Recipes;
using LiteRPG.Progress;
using Packages.LiteRPG.Runtime.LiteRPG.Stats;
using UnityEngine;

namespace LiteRPG.PlayerInventory
{
  public class Inventory : ScriptableObject
  {
    public Crafting Crafting { get; private set; }
    public IMoneyProgress MoneyProgress => moneyProgress;
    public InventoryBackpack Backpack => _backpack;
    public EquippedSlots EquippedSlots => _equippedSlots;
    
    [SerializeField] private InventoryBackpack _backpack;
    [SerializeField] private InvItemsDb _invItemsDb;
    [SerializeField] private RecipesBook _recipesBook;
    
    private IMoneyProgress moneyProgress;
    [SerializeField] private EquippedSlots _equippedSlots;

    public void Construct(InventoryBackpack backpack, IMoneyProgress moneyProgress, InvItemsDb itemsDb, RecipesBook recipesBook)
    {
      _recipesBook = recipesBook;
      this.moneyProgress = moneyProgress;
      _backpack = backpack;
      _invItemsDb = itemsDb;
      Crafting = new Crafting(this, itemsDb, recipesBook);
    }

    public void SetupEquipSlots(int maxSlotsNumber, BattleCharStats battleCharStats)
    {
      _equippedSlots = new EquippedSlots(maxSlotsNumber, battleCharStats, this.Backpack);
    }

    public bool TryAddItem(InvItemSlot invItemSlot) => 
      TryAddItem(invItemSlot.ItemData, invItemSlot.Quantity);

    public bool TryAddItem(InvItemData invItemData, int quantity = 1) => 
       TryAddItem(invItemData.Id, quantity);

    //Refactor this (make a separate method for adding many items)
    public void TryAddItem(IEnumerable<InvItemSlot> newItemsSlots)
    {
      foreach (InvItemSlot newItemSlot in newItemsSlots) 
        TryAddItem(newItemSlot);
    }

    public bool CanSellItem(int slotIndex, int quantity, int price)
    {
      if(_backpack.TryGetSlot(slotIndex, out var backpackSlot) == false)
      {
        Debug.LogWarning($"Failed to found slot due to slotIndex {slotIndex}, can't sell item in non-existing slot");
        return false;
      }
      
      if (backpackSlot == null)
      {
        Debug.LogWarning($"Can't sell item because backpack slot {slotIndex} is null");
        return false;
      }

      if (backpackSlot.IsEmpty())
      {
        Debug.LogWarning($"Can't sell item because backpack slot {slotIndex} is empty");
        return false;
      }
      
      if(quantity <= 0)
      {
        Debug.LogWarning($"Can't sell item because backpack slot {slotIndex} required quantity is <= 0");
        return false;
      }
      
      if(backpackSlot.ItemSlot.Quantity < quantity)
      {
        Debug.LogWarning($"Can't sell item because backpack slot {slotIndex} quantity ({backpackSlot.ItemSlot.Quantity}) is less then required ({quantity})");
        return false;
      }

      if (price < 0)
      {
        Debug.LogWarning($"Can't sell item in backpack slot {slotIndex} because the price is negative ({price})");
        return false;
      }

      return true;
    }

    public bool TrySellItem(int slotIndex, int quantity, int price)
    {
      if (CanSellItem(slotIndex, quantity, price))
      {
        SellItem(slotIndex, quantity, price);
        return true;
      }
      return false;
    }

    public void RemoveItems(InvItemData itemData, int quantity)
    {
      var slot = _backpack.GetSlotWithItem(itemData);
      _backpack.RemoveInvItem(slot, quantity);
    }

    public bool HasItemInSlotsOfQuantity(InvItemData invItemData, int quantity) => 
      _backpack.HasItemInSlotsOfQuantity(invItemData, quantity);

    public BackpackSlot GetSlotWithItem(InvItemData recipeInvItem) => 
      _backpack.GetSlotWithItem(recipeInvItem);

    private void SellItem(int slotIndex, int quantity, int price)
    {
      _backpack.RemoveInvItem(slotIndex, quantity);
      moneyProgress.AddMoney(price);
    }

    private bool TryAddItem(int itemId, int quantity = 1)
    {
      var itemData = _invItemsDb.GetData(itemId);
      
      if (IsStackableThanTryToAddToSlot(quantity, itemData)) 
        return true;

      if (_backpack.HasEmptySlot() == false)
        return false;

      return CreateNewSlotWithItem(itemData, quantity);
    }

    private bool CreateNewSlotWithItem(InvItemData itemData, int quantity)
    {
      InvItemSlot newSlot = new InvItemSlot(itemData: itemData, quantity: quantity);
      _backpack.AddInvItemInNewSlot(newSlot);
      return true;
    }

    private bool IsStackableThanTryToAddToSlot(int quantity, InvItemData itemData)
    {
      if (itemData.IsStackable)
      {
        BackpackSlot slot = _backpack.GetSlotWithItem(itemData);
        if (slot != null)
        {
          slot.AddItemQuantity(quantity);
          return true;
        }

        CreateNewSlotWithItem(itemData, quantity);
        return true;
      }

      return false;
    }
  }
}