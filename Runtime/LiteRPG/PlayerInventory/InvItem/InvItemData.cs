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
        [TextArea(5, 10)]
        public string Description = "NoDescription";
        public Sprite Icon;
        public InvItemType InvItemType;
        public int BasePrice = 0;
        public virtual bool Use(Inventory inventory, IAdditiveHp additiveHp)
        {
            return false;
        }

        public bool CanBeUsed() => 
            InvItemType.InvItemTypes().IsUsableType(InvItemType);
    }

    public interface IAdditiveHp
    {
        void AddHp(float hpToAdd);
    }
}
