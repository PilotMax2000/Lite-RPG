using System.Collections.Generic;
using LiteRPG.PlayerInventory.InvItem;
using LiteRPG.PlayerInventory.SubMenus.Craft;
using LiteRPG.PlayerInventory.SubMenus.Craft.Recipes;
using LiteRPG.Progress;
using UnityEditor.Graphs;
using UnityEngine;

namespace LiteRPG.PlayerInventory
{
  public class Inventory : ScriptableObject
  {
    public Crafting Crafting { get; private set; }
    public RecipesBook RecipesBook => _recipesBook;
    public IMoneyStats MoneyStats => _moneyStats;
    public InventoryBackpack Backpack => _backpack;
    public EquippedSlots EquippedSlots => _equippedSlots;
    
    [SerializeField] private InventoryBackpack _backpack;
    [SerializeField] private InvItemsDb _invItemsDb;
    [SerializeField] private RecipesBook _recipesBook;
    
    private IMoneyStats _moneyStats;
    [SerializeField] private EquippedSlots _equippedSlots;

    public void Construct(InventoryBackpack backpack, IMoneyStats moneyStats, InvItemsDb itemsDb, RecipesBook recipesBook)
    {
      _recipesBook = recipesBook;
      _moneyStats = moneyStats;
      _backpack = backpack;
      _invItemsDb = itemsDb;
      Crafting = new Crafting(this, itemsDb, recipesBook);
    }

    public void SetupEquipSlots(int maxSlotsNumber)
    {
      _equippedSlots = new EquippedSlots(maxSlotsNumber);
    }

    public bool AddItem(InvItemSlot invItemSlot) => 
      AddItem(invItemSlot.ItemData, invItemSlot.Quantity);

    public bool AddItem(InvItemData invItemData, int quantity = 1) => 
       AddItem(invItemData.Id, quantity);

    public void AddItem(IEnumerable<InvItemSlot> newItemsSlots)
    {
      foreach (InvItemSlot newItemSlot in newItemsSlots) 
        AddItem(newItemSlot);
    }

    public bool SellItem(int slotIndex, int quantity, int price)
    {
      var backpackSlot = _backpack.GetSlot(slotIndex);
      if (backpackSlot == null)
        return false;
      quantity = quantity == -1 ? backpackSlot.ItemSlot.Quantity : quantity;
      _backpack.RemoveInvItem(slotIndex, quantity);
      _moneyStats.AddMoney(quantity);
      return true;
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

    private bool AddItem(int itemId, int quantity = 1)
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