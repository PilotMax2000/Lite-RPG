﻿using System.Collections.Generic;
using LiteRPG.PlayerInventory.InvItem;

namespace LiteRPG.PlayerInventory.DataBase
{
  public class DataDbAddable<TData>: DataDb<TData> where TData : ScriptableIdData
  {
    protected bool AddData(TData data)
    {
      DataLinks = DataLinks ?? new List<TData>();
      
      if(_dbCache == null)
        InitDbCache();
      else
      {
        if(DataExists(data.Id))
          return false;
      }

      
      DataLinks.Add(data);
      UpdateDbCache();
      return true;
    }
    
    public List<TData> GetAllData() => DataLinks;
  }
}