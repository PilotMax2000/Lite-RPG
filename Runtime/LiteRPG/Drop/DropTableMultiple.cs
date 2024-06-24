using System;
using System.Collections.Generic;
using LiteRPG.PlayerInventory.InvItem;

namespace LiteRPG.Drop
{
    [Serializable]
    public class DropTableMultiple
    {
        public DropTablePercentageSlot[] DropItems;

        public List<InvItemSlot> GetRandomItems()
        {
            var res = new List<InvItemSlot>();
            foreach (var dropItem in DropItems)
            {
                if (CanDrop(dropItem) == false)
                    continue;

                int quantity = dropItem.ShouldDropSeveral ? 10 : 1;
                res.Add(new InvItemSlot(dropItem.ItemData, quantity));
            }

            return res;
        }

        private static bool CanDrop(DropTablePercentageSlot dropItem) => 
            dropItem.Percentage >= UnityEngine.Random.Range(0,100f);
    }
}