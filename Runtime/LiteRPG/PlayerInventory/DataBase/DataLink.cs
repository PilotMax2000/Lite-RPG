using System;
using LiteRPG.PlayerInventory.InvItem;

namespace LiteRPG.PlayerInventory.DataBase
{
  [Serializable]
  public class DataLink<TData> where TData : ScriptableIdData 
  {
    public TData Data;
    public int Id => Data.Id;

  }
  
  // public class DataLink<TData> where TData : ScriptableIdData
  // {
  //   public int Id { get; private set; }
  //   public TData Data { get; private set; }
  //
  //   public DataLink(TData data)
  //   {
  //     Id = data.Id;
  //     Data = data;
  //   }
  // }
}