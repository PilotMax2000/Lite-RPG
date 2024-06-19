using System;
using System.Collections.Generic;
using LiteRPG.PlayerInventory.InvItem;
using LiteRPG.PlayerInventory.SubMenus.Craft;
using LiteRPG.PlayerInventory.SubMenus.Craft.Recipes;
using LiteRPG.Progress;
using UnityEditor.Graphs;
using UnityEngine;
using UnityEngine.Serialization;

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
          _backpack.BackpackChanged();
          return true;
        }

        CreateNewSlotWithItem(itemData, quantity);
        return true;
      }

      return false;
    }
  }

  [Serializable]
  public class EquippedSlots
  {
    [SerializeField] private EquippedSlot[] _equippedSlots;
    private Dictionary<EquipSlotType, EquippedSlot> _cachedSlots;

    public EquippedSlots(int totalNumberOfSlots)
    {
      if (IsNotEnoughSlotsToInitialize(totalNumberOfSlots))
        return;

      CreateEquippedSlots(totalNumberOfSlots);
      CacheSlots();
    }

    private void CacheSlots()
    {
      _cachedSlots = new Dictionary<EquipSlotType, EquippedSlot>();
      foreach (EquippedSlot slot in _equippedSlots) 
        _cachedSlots.Add(slot.EquipSlotType, slot);
    }

    private void CreateEquippedSlots(int totalNumberOfSlots)
    {

      _equippedSlots = new EquippedSlot[totalNumberOfSlots];
      for (int i = 0; i < _equippedSlots.Length; i++)
      {
        _equippedSlots[i] = new EquippedSlot((EquipSlotType)i + 1);
      }
    }

    private static bool IsNotEnoughSlotsToInitialize(int totalNumberOfSlots)
    {
      if (totalNumberOfSlots < 1)
      {
        Debug.LogError("Wrong number of equipable slots in inventory. It should be 1 or more!");
        return true;
      }

      return false;
    }

    public bool TryGetSlotByType(EquipSlotType slotType, out EquippedSlot equippedSlot)
    {
      equippedSlot = null;
      bool slotExists = _cachedSlots.TryGetValue(slotType, out var foundSlot);
      if (slotExists == false)
        return false;
      
      equippedSlot = foundSlot;
      return true;
    }
    
    [Serializable]
    public class EquippedSlot
    {
      public EquipSlotType EquipSlotType => equipSlotType;
      public bool IsEquipped => _isEquipped;
      public BackpackSlot BackpackSlot  => _backpackSlot;
      
      [FormerlySerializedAs("_equipSlot")] [SerializeField] private EquipSlotType equipSlotType;
      [SerializeField] private bool _isEquipped;
      [SerializeField] private BackpackSlot _backpackSlot;

      public EquippedSlot(EquipSlotType equipSlotType) => 
        this.equipSlotType = equipSlotType;

      public void Equip(BackpackSlot slotToEquip)
      {
        _isEquipped = true;
        _backpackSlot = slotToEquip;
        _backpackSlot.Equip(EquipSlotType);
      }
      
      public void Unequip()
      {
        _isEquipped = false;
        _backpackSlot.Unequip();
        _backpackSlot = null;
      } 

      public bool CanEquip() => 
        _isEquipped == false;
    }
  }
}