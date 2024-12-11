using System;
using System.Collections.Generic;
using LiteRPG.Helper;
using LiteRPG.PlayerInventory.InvItem;
using Sirenix.Utilities;
using UnityEngine;

namespace LiteRPG.PlayerInventory
{
  [Serializable]
  public class InventoryBackpack
  {
    public const int DefaultLimit = 10;
    public const int DefaultMinLimit = 1;
    public event Action OnBackpackChanged;
    
    [SerializeField] private List<BackpackSlot> _slots;
    
    private readonly int _slotLimit;
    private Inventory _inventory;
    private IAdditiveHp _additiveHp;

    public InventoryBackpack(Inventory inventory, IAdditiveHp additiveHp)
    {
      _additiveHp = additiveHp;
      _inventory = inventory;
      _slots = new List<BackpackSlot>();
      _slotLimit = DefaultLimit;
      CreateSlots(_slotLimit);
    }

    public InventoryBackpack(int slotLimit, Inventory inventory, IAdditiveHp additiveHp)
    {
      _slots = new List<BackpackSlot>();
      _additiveHp = additiveHp;
      _inventory = inventory;
      _slotLimit = slotLimit > DefaultMinLimit ? slotLimit : DefaultMinLimit;
      CreateSlots(_slotLimit);
    }

    ~InventoryBackpack() => 
      UnsubscribeSlots();

    public List<BackpackSlot> GetAllSlots() => 
      _slots;

    public bool HasEmptySlot()
    {
      foreach (var slot in _slots)
      {
        if (slot.IsEmpty())
          return true;
      }
      return false;
    }

    public List<BackpackSlot> GetEmptySlots()
    {
      List<BackpackSlot> emptySlots = new List<BackpackSlot>();
      foreach (var slot in _slots)
      {
        if(slot.IsEmpty())
          emptySlots.Add(slot);
      }
      return emptySlots;
    }

    public List<BackpackSlot> GetNonEmptySlots()
    {
      List<BackpackSlot> nonEmptySlots = new List<BackpackSlot>();
      foreach (var slot in _slots)
      {
        if(slot.IsEmpty() == false)
          nonEmptySlots.Add(slot);
      }
      return nonEmptySlots;
    }

    public BackpackSlot GetEmptySlot()
    {
      foreach (var slot in _slots)
      {
        if (slot.IsEmpty())
          return slot;
      }
      return null;
    }

    private void CreateSlots(int maxSlots)
    {
      for (int i = 0; i < maxSlots; i++)
      {
        var backpackSlot = new BackpackSlot(_inventory, _additiveHp, i);
        _slots.Add(backpackSlot);
        backpackSlot.OnSlotChanged += OnSlotChanged;
      }
    }

    private void UnsubscribeSlots()
    {
      foreach (var slot in _slots) 
        slot.OnSlotChanged -= OnSlotChanged;
    }

    private void OnSlotChanged()
    {
      OnBackpackChanged?.Invoke();
    }

    public void AddInvItemInNewSlot(InvItemSlot invItemSlot)
    {
      if (HasEmptySlot())
      {
        GetEmptySlot().AddItemSlot(invItemSlot);
      }
    }

    public void RemoveInvItem(int slotId, int quantity = -1)
    {
      if (Helper.Helper.ListIndexExist(_slots, slotId) == false)
      {
        return;
      }

      int resQuantityToRemove = quantity == -1 ? _slots[slotId].ItemSlot.Quantity : quantity;
      RemoveInvItem(_slots[slotId], resQuantityToRemove);
    }

    public void RemoveInvItem(BackpackSlot slot, int quantity)
    {
      //TODO: add test for checking if item could be removed
      slot.AddItemQuantity(-(quantity));
    }

    // public BackpackSlot GetSlot(int index)
    // {
    //   return _slots.IsIndexExist(index) ? _slots[index] : null;
    // }

    public bool TryGetSlot(int index, out BackpackSlot slot)
    {
      slot = null;
      if(_slots.IsIndexExist(index) == false)
        return false;

      slot = _slots[index];
      return true;
    }

    public bool TryGetSlotItemData(int index, out InvItemData itemData)
    {
      itemData = null;
      if (TryGetSlot(index, out BackpackSlot slot) == false)
        return false;
      
      itemData = slot.ItemSlot.ItemData;
      return true;
    }
    
    public bool TryGetSlotItemData<T>(int index, out T itemData) where T : InvItemData
    {
      itemData = null;
      if (TryGetSlot(index, out BackpackSlot slot) == false)
        return false;
      
      itemData = slot.ItemSlot.ItemData as T;
      return true;
    }

    public BackpackSlot GetSlotWithItem(InvItemData itemData)
    {
      foreach (var slot in _slots)
      {
        if (slot.IsEmpty())
          continue;
        if (slot.ItemSlot.ItemId == itemData.Id)
          return slot;
      }
      return null;
    }

    public bool HasItemInSlotsOfQuantity(InvItemData itemData, int quantity)
    {
      if (HasItemInSlots(itemData) == false)
        return quantity == 0;
      BackpackSlot slot = GetSlotWithItem(itemData);
      if (slot == null)
        return false;
      return slot.ItemSlot.Quantity >= quantity;
    }

    public bool HasItemInSlots(InvItemData itemData)
    {
      foreach (var slot in _slots)
      {
        if (slot.IsEmpty())
          continue;
        if (slot.ItemSlot.ItemId == itemData.Id)
          return true;
      }
      return false;
    }
  }
}