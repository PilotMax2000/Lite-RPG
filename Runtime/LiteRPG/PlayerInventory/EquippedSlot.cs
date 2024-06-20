using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace LiteRPG.PlayerInventory
{
    public partial class EquippedSlots
    {
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
                _backpackSlot.Equip();
            }
      
            public void Unequip()
            {
                var savedReferenceForUnequipCall = _backpackSlot;
                _isEquipped = false;
                _backpackSlot = null;
                savedReferenceForUnequipCall.Unequip();
            } 

            public bool CanEquip() => 
                _isEquipped == false;
        }
    }
}