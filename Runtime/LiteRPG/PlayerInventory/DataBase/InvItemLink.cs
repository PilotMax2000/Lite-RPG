using System;
using LiteRPG.PlayerInventory.InvItem;

namespace LiteRPG.PlayerInventory.DataBase
{
  [Serializable]
  public class InvItemLink : DataLink<InvItemData>
  {
    public InvItemLink() : base() { }
  }
}