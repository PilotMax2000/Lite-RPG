using System;
using LiteRPG.PlayerInventory.InvItem;
using UnityEngine.Serialization;

namespace LiteRPG.Drop
{
    [Serializable]
    public class DropTableSingle
    {
        public DropTableWeightSlot[] DropItems;

        public InvItemData GetRandomItem()
        {
            int totalWeight = 0;
            foreach (var item in DropItems)
            {
                totalWeight += item.Weight;
            }

            int randomValue = UnityEngine.Random.Range(0, totalWeight);
            int cumulativeWeight = 0;

            foreach (var item in DropItems)
            {
                cumulativeWeight += item.Weight;
                if (randomValue <= cumulativeWeight)
                {
                    return item.ItemData;
                }
            }

            return null; // Just in case something goes wrong
        }
    }
    
    
}