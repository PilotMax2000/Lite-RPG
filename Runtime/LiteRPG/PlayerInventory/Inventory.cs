using System.Collections.Generic;
using LiteRPG.PlayerInventory.InvItem;
using LiteRPG.PlayerInventory.SubMenus.Craft;
using LiteRPG.PlayerInventory.SubMenus.Craft.Recipes;
using LiteRPG.Progress;
using UnityEngine;

namespace LiteRPG.PlayerInventory
{
  public class Inventory : MonoBehaviour
  {
    public Crafting Crafting { get; private set; }
    public RecipesBook RecipesBook => _recipesBook;
    public IMoneyStats MoneyStats => _moneyStats;
    private InventoryBackpack _backpack;
    private IMoneyStats _moneyStats;
    private InvItemsDb _invItemsDb;
    private RecipesBook _recipesBook;

    public void Construct(InventoryBackpack backpack, IMoneyStats moneyStats, InvItemsDb itemsDb, RecipesBook recipesBook)
    {
      _recipesBook = recipesBook;
      _moneyStats = moneyStats;
      _backpack = backpack;
      _invItemsDb = itemsDb;
      Crafting = new Crafting(this, itemsDb, recipesBook);
    }

    public bool AddItem(InvItemSlot invItemSlot)
    {
      if (_backpack.HasEmptySlot() == false)
        return false;
      _backpack.AddInvItem(invItemSlot);
      return true;
    }

    public bool AddItem(int itemId, int quantity = 1)
    {
      if (_backpack.HasEmptySlot() == false)
        return false;
      InvItemSlot newSlot = new InvItemSlot(itemData: _invItemsDb.GetData(itemId), quantity: quantity);
      _backpack.AddInvItem(newSlot);
      return true;
    }

    public bool AddItem(InvItemData invItemData, int quantity = 1) => 
      AddItem(invItemData.Id, quantity);

    public void AddItem(IEnumerable<InvItemSlot> newItemsSlots)
    {
      foreach (InvItemSlot newItemSlot in newItemsSlots)
      {
        AddItem(newItemSlot);
      }
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
      InventoryBackpack.RemoveInvItem(slot, quantity);
    }

    public bool HasItemInSlotsOfQuantity(InvItemData invItemData, int quantity) => 
      _backpack.HasItemInSlotsOfQuantity(invItemData, quantity);

    public BackpackSlot GetSlotWithItem(InvItemData recipeInvItem) => 
      _backpack.GetSlotWithItem(recipeInvItem);
  }
}