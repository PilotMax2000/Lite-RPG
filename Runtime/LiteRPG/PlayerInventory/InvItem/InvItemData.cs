using UnityEngine;

namespace LiteRPG.PlayerInventory.InvItem
{
    public class ScriptableIdData : ScriptableObject
    {
        public int Id = -1;
    }

    [CreateAssetMenu(fileName = "InvItemData", menuName = "Items/InvItemData")]
    public class InvItemData : ScriptableIdData
    {
        public string ItemName = "NoName";
        public InvItemType InvItemType;
        public int BasePrice = 0;
        public virtual bool Use(Inventory inventory)
        {
            return false;
        }

        public bool CanBeUsed() => 
            InvItemType.InvItemTypes().IsUsableType(InvItemType);
    }
}
