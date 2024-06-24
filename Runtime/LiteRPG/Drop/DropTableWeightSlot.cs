using System;
using LiteRPG.PlayerInventory.InvItem;
using UnityEngine;

namespace LiteRPG.Drop
{
    [Serializable]
    public class DropTableWeightSlot
    {
        public InvItemData ItemData;
        public int Weight;
        public float DebugPercentage;
    }
    
    [Serializable]
    public class DropTablePercentageSlot
    {
        public InvItemData ItemData;
        [Range(0, 100f)]
        public float Percentage;
        public bool ShouldDropSeveral;
    }
}