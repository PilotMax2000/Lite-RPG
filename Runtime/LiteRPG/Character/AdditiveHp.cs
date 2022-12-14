using LiteRPG.PlayerInventory.InvItem;

namespace LiteRPG.Character
{
    public class AdditiveHp : IAdditiveHp
    {
        public float Hp { get; set; }
        
        public AdditiveHp()
        {
            Hp = 1;
        }
        
        public void AddHp(float hpToAdd)
        {
            Hp += hpToAdd;
        }
    }
}