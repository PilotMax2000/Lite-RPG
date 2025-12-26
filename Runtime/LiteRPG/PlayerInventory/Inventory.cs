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
    public IMoneyProgress MoneyProgress => _moneyProgress;
    public InventoryBackpack Backpack => _backpack;
    public EquippedSlots EquippedSlots => _equippedSlots;
    
    [SerializeField] private InventoryBackpack _backpack;
    [SerializeField] private InvItemsDb _invItemsDb;
    [SerializeField] private RecipesBook _recipesBook;
    
    private IMoneyProgress _moneyProgress;
    [SerializeField] private EquippedSlots _equippedSlots;

    public void Construct(InventoryBackpack backpack, IMoneyProgress moneyProgress, InvItemsDb itemsDb, RecipesBook recipesBook)
    {
      _recipesBook = recipesBook;
      _moneyProgress = moneyProgress;
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

    public void RemoveItem(InvItemData itemData, int quantity)
    {
      var slot = _backpack.GetSlotWithItem(itemData);
      _backpack.RemoveInvItem(slot, quantity);
    }

    public bool TryRemoveItem(InvItemData itemData, int quantity)
    {
      var backpackSlot = _backpack.GetSlotWithItem(itemData);
      if (backpackSlot == null)
      {
        Debug.LogWarning($"Failed to remove item: {itemData.ItemName} not found in inventory");
        return false;
      }

      if (backpackSlot.ItemSlot.Quantity < quantity)
      {
        Debug.LogWarning(
          $"Failed to remove item: {itemData.ItemName} because there are not enough items ({backpackSlot.ItemSlot.Quantity} of {quantity})");
        return false;
      }

      _backpack.RemoveInvItem(backpackSlot, quantity);
      return true;
    }

    public bool TryRemoveItem(int slotIndex, int quantity)
    {
      if (_backpack.TryGetSlot(slotIndex, out BackpackSlot backpackSlot) == false)
      {
        Debug.LogWarning($"Failed to remove item in slot {slotIndex}");
        return false;
      }

      if (backpackSlot.ItemSlot.Quantity < quantity)
      {
        Debug.LogWarning($"Failed to remove item in slot {slotIndex} because in slot is not enough items ({backpackSlot.ItemSlot.Quantity} of {quantity})");
        return false;
      }
      
      _backpack.RemoveInvItem(backpackSlot, quantity);
      return true;
    }
    

    public bool HasItemInSlotsOfQuantity(InvItemData invItemData, int quantity) => 
      _backpack.HasItemInSlotsOfQuantity(invItemData, quantity);

    public BackpackSlot GetSlotWithItem(InvItemData recipeInvItem) => 
      _backpack.GetSlotWithItem(recipeInvItem);

    private void SellItem(int slotIndex, int quantity, int price)
    {
      _backpack.RemoveInvItem(slotIndex, quantity);
      _moneyProgress.AddMoney(price);
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

    public bool CanAddItem(int itemId, int quantity = 1)
    {
      var itemData = _invItemsDb.GetData(itemId);
      
      if (CanStackItemToExistingSlot(quantity, itemData)) 
        return true;

      if (_backpack.HasEmptySlot())
        return true;

      return false;
    }

    public bool CanAddItem(InvItemData itemData, int quantity = 1) => 
      CanAddItem(itemData.Id, quantity);

    private bool CreateNewSlotWithItem(InvItemData itemData, int quantity)
    {
      InvItemSlot newSlot = new InvItemSlot(itemData: itemData, quantity: quantity);
      _backpack.AddInvItemInNewSlot(newSlot);
      return true;
    }

    public bool TryBuyAndTransferItem(Inventory sellerInventory, int sellSlotIndex, int quantity, int calculatedItemPrice)
    {
      if (CanBuyAndTransferItem(sellerInventory, sellSlotIndex, quantity, calculatedItemPrice) == false)
        return false;
      
      int totalPriceForItems = calculatedItemPrice * quantity;
      
      //On item transferred successfully 
      if (sellerInventory.Backpack.TryGetSlotItemData(sellSlotIndex, out var sellerItemData) == false)
      {
        Debug.LogWarning($"Failed to find slot by index! (index: {sellSlotIndex})");
        return false;
      }
      
      if (TryAddItem(sellerItemData, quantity) == false)
      {
        Debug.LogWarning($"Failed to add item! (index: {sellSlotIndex})");
        return false;
      }
      
      if (sellerInventory.TryRemoveItem(sellSlotIndex, quantity) == false)
      {
        Debug.LogWarning($"Failed to remove item from seller slot! (index {sellSlotIndex}");
        return false;
      }

      MoneyProgress.AddMoney(-totalPriceForItems);
      sellerInventory.MoneyProgress.AddMoney(totalPriceForItems);
      
      return true;
    }

    public bool CanBuyAndTransferItem(Inventory sellerInventory, int sellSlotIndex, int quantity, int calculatedItemPrice)
    {
      int totalPriceForItems = calculatedItemPrice * quantity;
      if (MoneyProgress.Money < totalPriceForItems)
      {
        Debug.LogWarning($"[Market] Can't buy item, not enough money! Required : {totalPriceForItems}, current : {MoneyProgress.Money}");
        return false;
      }

      if (sellerInventory.CanSellItem(sellSlotIndex, quantity, 0) == false)
        return false;

      if (sellerInventory.Backpack.TryGetSlot(sellSlotIndex, out BackpackSlot itemBackpackSlot) == false)
      {
        Debug.LogWarning("Failed to find slot, something is really wrong!");
        return false;
      }

      if (CanAddItem(itemBackpackSlot.ItemSlot.ItemData, quantity) == false)
      {
        Debug.LogWarning($"Failed to add item to the buying inventory");
        return false;
      }

      return true;
    }
    
    public List<BackpackSlot> GetNonEmptyAndNonEquippedSlots() => 
      _backpack.GetNonEmptyAndNonEquippedSlots(_equippedSlots);

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
    
    private bool CanStackItemToExistingSlot(int quantity, InvItemData itemData)
    {
      if (itemData.IsStackable)
      {
        BackpackSlot slot = _backpack.GetSlotWithItem(itemData);
        return slot != null;
      }

      return false;
    }
  }
}