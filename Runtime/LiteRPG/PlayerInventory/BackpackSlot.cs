using System;
using LiteRPG.PlayerInventory.InvItem;
using UnityEngine;

namespace LiteRPG.PlayerInventory
{
  [Serializable]
  public class BackpackSlot
  {
    [field: SerializeField] public InvItemSlot ItemSlot { get; private set; }
    [field: SerializeField] public bool IsEquipped {get; private set;}
    [field: SerializeField] public EquipSlot EquipSlot {get; private set;}
    
    private bool _empty;
    private Inventory _inventory;
    private IAdditiveHp _additiveHp;

    public BackpackSlot(Inventory inventory, IAdditiveHp additiveHp)
    {
      _additiveHp = additiveHp;
      _inventory = inventory;
      _empty = true;
    }

    public BackpackSlot(InvItemSlot itemSlot, Inventory inventory, IAdditiveHp additiveHp)
    {
      _additiveHp = additiveHp;
      _inventory = inventory;
      SetInvItemSlot(itemSlot);
    }


    public bool IsEmpty() => 
      _empty;

    public void SetInvItemSlot(InvItemSlot itemSlot)
    {
      ItemSlot = itemSlot;
      _empty = itemSlot == null;
    }

    public void AddItemQuantity(int addBy)
    {
      ItemSlot.Quantity += addBy;
      if (ItemSlot.Quantity <= 0)
      {
        MakeSlotEmpty();
      }
    }

    private void MakeSlotEmpty()
    {
      ItemSlot = null;
      _empty = true;
    }

    public void Use()
    {
      if(_empty) 
        return;
      
      if(ItemSlot.ItemData.CanBeUsed() == false)
        return;
      
      if(ItemSlot.ItemData.Use(_inventory, _additiveHp))
        AddItemQuantity(-1);
    }

    public void Equip(EquipSlot s0)
    {
      if (s0 == EquipSlot.None)
        return;

      IsEquipped = true;
      EquipSlot = s0;
    } 
    
    public void Unequip()
    {
      IsEquipped = false;
      EquipSlot = EquipSlot.None;
    }
  }
}