using LiteRPG.PlayerInventory.InvItem;
using UnityEngine;

namespace LiteRPG.PlayerInventory.DataBase
{
  [CreateAssetMenu(fileName = "FILENAME", menuName = "MENUNAME", order = 0)]
  public class TestDataLinkRealization : TestDataLink<InvItemData>
  {
    
  }
  
 // [CreateAssetMenu(fileName = "FILENAMEBaswe", menuName = "MENUNAMEBase", order = 0)]
  public class TestDataLink<TData> : ScriptableObject where TData : ScriptableIdData
  {
    public TData Data;
    public int Id => Data.Id;
  }
}