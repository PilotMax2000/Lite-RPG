using System.Collections.Generic;
using LiteRPG.PlayerInventory.InvItem;
using UnityEngine;

namespace LiteRPG.LootBox
{
  [CreateAssetMenu(fileName = "LootboxData", menuName = "Items/LootboxData")]
  public class LootboxData : ScriptableObject
  {
    public InvItemSlot[] GuaranteedLoot;
    public InvItemSlot[] RandomLoot;

    public List<InvItemSlot> GetLoot()
    {
      List<InvItemSlot> loot = new List<InvItemSlot>();
      loot.AddRange(GuaranteedLoot);
      //TODO: GEtRandomLoot
      return loot;
    }
  }
}
