using System;
using System.Collections.Generic;
using LiteRPG.PlayerInventory.InvItem;
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
        _slots.Add(new BackpackSlot(_inventory, _additiveHp));
      }
    }

    public void AddInvItemInNewSlot(InvItemSlot invItemSlot)
    {
      if (HasEmptySlot())
      {
        GetEmptySlot().SetInvItemSlot(invItemSlot);
        OnBackpackChanged?.Invoke();
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
      OnBackpackChanged?.Invoke();
    }

    public void RemoveInvItem(BackpackSlot slot, int quantity)
    {
      //TODO: add test for checking if item could be removed
      slot.AddItemQuantity(-(quantity));
      OnBackpackChanged?.Invoke();
    }

    public BackpackSlot GetSlot(int index)
    {
      return Helper.Helper.ListIndexExist(_slots, index) ? _slots[index] : null;
    }

    public bool TryGetSlotItemData(int index, out InvItemData itemData)
    {
      itemData = null;
      var slot = GetSlot(index);
      if (slot == null) 
        return false;
      
      itemData = slot.ItemSlot.ItemData;
      return true;
    }
    
    public bool TryGetSlotItemData<T>(int index, out T itemData) where T : InvItemData
    {
      itemData = null;
      var slot = GetSlot(index);
      if (slot == null) 
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
        return false;
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

    public void BackpackChanged()
    {
      OnBackpackChanged?.Invoke();
    }
  }
}