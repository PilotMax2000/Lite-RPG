using System;
using System.Collections.Generic;
using Packages.LiteRPG.Runtime.LiteRPG.Stats;
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

        public EquippedSlots(int totalNumberOfSlots, BattleCharStats battleCharStats)
        {
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
            if (equippedSlot.IsEquipped)
                return false;
      
            equippedSlot.Equip(slotToEquip);
            var itemData = slotToEquip.ItemSlot.ItemData;
            _battleCharStats.AddModifierFromObject(itemData.StatModifiers, itemData);
            return true;
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
    }
}