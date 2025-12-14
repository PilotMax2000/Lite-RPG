using System.Text;
using LiteRPG.PlayerInventory;
using LiteRPG.PlayerInventory.InvItem;
using UnityEngine;

namespace LiteRPG.Runtime.LiteRPG.Quests
{
    public abstract class QuestProgressAnalyzer<TQuestData, TInvItemData, TInventory> where TInvItemData : InvItemData 
                                                                                        where TQuestData : QuestData 
                                                                                        where TInventory : Inventory
    {
        public abstract string GetQuestTextGoalAndProgress(TQuestData questData);
        public abstract bool IsQuestCanBeCompleted(TQuestData quest);

        public bool TryRemoveQuestItemsFromPlayer(TQuestData questData, TInventory inventory)
        {
            if (questData.RemoveRequiredItemsAfterQuestFinished == false)
                return false;

            var questItems = questData.RequiredItems;
            foreach (var invItemSlot in questItems)
            {
                if (inventory.TryRemoveItem(invItemSlot.ItemData, invItemSlot.Quantity) == false)
                    return false;
            }

            return true;
        }

        private string GetQuestRequiredItemsProgress(TQuestData questData, TInventory inventory)
        {
            StringBuilder result = new StringBuilder();
            foreach (var requiredItem in questData.RequiredItems)
            {
                var currentQuantity = 0;
                if (inventory.Backpack.TryGetBackpackSlot(requiredItem.ItemData, out var backpackSlot))
                    currentQuantity = backpackSlot.ItemSlot.Quantity;
                result.Append($"{requiredItem.ItemData.ItemName} : {currentQuantity}/{requiredItem.Quantity}\n");
            }

            return result.ToString();
        }

        private bool IsItemBriningQuestCanBeCompleted(TQuestData quest, TInventory inventory)
        {
            if (quest.RequiredItems == null)
            {
                Debug.LogError("Required items not set!");
                return false;
            }

            foreach (var requiredItem in quest.RequiredItems)
            {
                if (inventory.Backpack.HasItemInSlotsOfQuantity(requiredItem.ItemData, requiredItem.Quantity) == false)
                    return false;
            }

            return true;
        }

    }
}