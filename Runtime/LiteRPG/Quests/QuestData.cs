using LiteRPG.PlayerInventory.InvItem;
using UnityEngine;

namespace LiteRPG.Runtime.LiteRPG.Quests
{
    public abstract class QuestData : ScriptableObject
    {
        public int Id;
        public string Title;
        public Sprite QuestIcon;
        public int MoneyReward;
        public int ExpReward;
        public InvItemSlot[]  ItemsReward; 
        [TextArea(3, 10)]
        public string QuestDescription;
        public InvItemSlot[] RequiredItems;
        public bool RemoveRequiredItemsAfterQuestFinished;
    }
}