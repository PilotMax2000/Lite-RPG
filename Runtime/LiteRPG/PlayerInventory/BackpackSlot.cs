using LiteRPG.PlayerInventory.InvItem;

namespace LiteRPG.PlayerInventory
{
  public class BackpackSlot
  {
    public InvItemSlot ItemSlot { get; private set; }
    private bool _empty;

    public BackpackSlot() => 
      _empty = true;

    public BackpackSlot(InvItemSlot itemSlot) => 
      SetInvItemSlot(itemSlot);


    public bool IsEmpty() => 
      _empty;

    public void SetInvItemSlot(InvItemSlot itemSlot)
    {
      ItemSlot = itemSlot;
      _empty = itemSlot == null;
    }

    public void AddItemQuantity(int addBy)
    {
      ItemSlot.Quantity += addBy;
      if (ItemSlot.Quantity <= 0)
      {
        MakeSlotEmpty();
      }
    }

    private void MakeSlotEmpty()
    {
      ItemSlot = null;
      _empty = true;
    }
  }
}