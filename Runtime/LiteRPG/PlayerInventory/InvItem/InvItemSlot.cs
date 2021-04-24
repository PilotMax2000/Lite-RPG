using System;

namespace LiteRPG.PlayerInventory.InvItem
{
  [Serializable]
  public class InvItemSlot
  {
    public int ItemId => ItemData.Id;
    public int Quantity;
    public InvItemData ItemData;

    // public InvItemSlot(int itemId, int quantity = 0)
    // {
    //   ItemID = itemId;
    //   Quantity = quantity;
    //   //TODO: Get From DB
    // }

    public InvItemSlot(InvItemData itemData, int quantity = 1)
    {
      Quantity = quantity;
      ItemData = itemData;
    }
  }
}