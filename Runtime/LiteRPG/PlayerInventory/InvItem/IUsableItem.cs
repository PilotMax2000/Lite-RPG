namespace LiteRPG.PlayerInventory.InvItem
{
  public interface IUsableItem
  {
    void Use(Inventory inventory);
    bool CanBeUsed();
  }
}