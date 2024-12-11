using System;
using System.Collections.Generic;
using System.Linq;
using LiteRPG.PlayerInventory.InvItem;
using Packages.LiteRPG.Runtime.LiteRPG.Stats;
using Sirenix.Utilities;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LiteRPG.PlayerInventory
{
    [Serializable]
    public class EquippedSlots
    {
        [SerializeField] private EquippedSlot[] _equippedSlots;
        private Dictionary<EquipSlotType, EquippedSlot> _cachedSlots;
        private BattleCharStats _battleCharStats;
        private InventoryBackpack _backpack;

        public EquippedSlots(int totalNumberOfSlots, BattleCharStats battleCharStats, InventoryBackpack backpack)
        {
            _backpack = backpack;
            _battleCharStats = battleCharStats;
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

        public bool TryGetEquipSlotTypeByBackpackSlot(BackpackSlot backpackSlot, out EquipSlotType equipSlotType)
        {
            equipSlotType = EquipSlotType.None;
            bool res = false;
            if (backpackSlot == null)
                return false;
            
            foreach (var equippedSlot in _equippedSlots)
            {
                if (equippedSlot.BackpackSlot == backpackSlot)
                {
                    equipSlotType = equippedSlot.EquipSlotType;
                    return true;
                }
            }

            return false;
        }

        public bool TryEquipSlot(EquipSlotType slotType, int backpackSlotIndex)
        {
            if (_backpack.TryGetSlot(backpackSlotIndex, out var backpackSlot) == false)
                return false;
            return TryEquipSlot(slotType, backpackSlot);
        }

        public bool TryEquipSlot(EquipSlotType slotType, BackpackSlot slotToEquip)
        {
            var slotExists = TryGetSlotByType(slotType, out var equippedSlot);
            if (slotExists == false)
                return false;
            if (slotToEquip.IsEmpty())
            {
                Debug.LogWarning("Can't equip empty slot!");
                return false;
            }
            
            var itemDataItemName = slotToEquip.ItemSlot.ItemData.ItemName;
            if (slotToEquip.ItemSlot.ItemData.CanEquip == false)
            {
                Debug.LogWarning($"Item {itemDataItemName} can't be equipped due to its InvItemData!");
                return false;
            }

            if (equippedSlot.IsEquipped)
            {
                Debug.LogWarning($"Target slot {slotType} is already equipped");
                return false;
            }

            var itemData = slotToEquip.ItemSlot.ItemData;
            if (IsSlotAllowed(slotType, itemData) == false)
            {
                Debug.LogWarning($"You are trying to put item '{itemDataItemName}' in slot {slotType} which is not allowed for this item");
                return false;
            }

            _battleCharStats.AddModifierFromObject(itemData.StatModifiers, itemData);
            equippedSlot.Equip(slotToEquip);
            return true;
        }

        public bool TryEquipSlot(BackpackSlot slotToEquip) => 
            _equippedSlots.Any(equippedSlot => TryEquipSlot(equippedSlot.EquipSlotType, slotToEquip));

        private bool IsSlotAllowed(EquipSlotType slotTypeToEquip, InvItemData itemData)
        {
            if (itemData.AllowedSlotsToEquip.IsNullOrEmpty())
                return false;
            return itemData.AllowedSlotsToEquip.Any(allowedSlotType => slotTypeToEquip == allowedSlotType);
        }

        public bool TryUnequipSlot(EquipSlotType slotType)
        {
            var slotExists = TryGetSlotByType(slotType, out EquippedSlot equippedSlot);
            if (slotExists == false)
                return false;
            if (equippedSlot.IsEquipped == false)
                return false;
      
            _battleCharStats.RemoveAllModifiersFromObject(equippedSlot.BackpackSlot.ItemSlot.ItemData);
            equippedSlot.Unequip();
            return true;
        }

        public bool IsBackpackSlotEquipped(BackpackSlot backpackSlot)
        {
            foreach (var equippedSlot in _equippedSlots)
            {
                if(equippedSlot.IsEquipped == false)
                    continue;
                if (equippedSlot.BackpackSlot == backpackSlot)
                    return true;
            }

            return false;
        }
        
        public bool IsEquipSlotUsed(EquipSlotType slotType) => 
            _equippedSlots.Any(equippedSlot => equippedSlot.EquipSlotType == slotType && equippedSlot.IsEquipped);
    }
}