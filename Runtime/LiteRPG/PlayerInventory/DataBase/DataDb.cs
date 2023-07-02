using System;
using System.Collections.Generic;
using LiteRPG.PlayerInventory.InvItem;
using UnityEngine;

namespace LiteRPG.PlayerInventory.DataBase
{
  [Serializable]
  public abstract class DataDb<TData> : ScriptableObject where TData : ScriptableIdData
  {
    public List<TData> DataLinks;
    protected Dictionary<int, TData> _dbCache;
    protected bool _dbWasCached;

    public TData GetData(int id)
    {
      if (DataLinks.Count == 0)
      {
        Debug.LogWarning("Database is empty. Please, add data to database.");
        return null;
      }
      
      CheckCacheInit();

      return _dbCache[id];
    }

    public void ResetCache()
    {
      _dbCache = null;
      _dbWasCached = false;
    }

    protected void CheckCacheInit()
    {
      if (_dbWasCached == false)
        CacheDb();
    }

    public bool DataExists(int id)
    {
      if (DataLinks == null || DataLinks.Count == 0)
        return false;
      
      return _dbCache.TryGetValue(id, out TData _);
    }

    protected void CacheDb()
    {
      if (_dbCache != null && _dbCache.Count != 0) 
        return;
      
      _dbCache = new Dictionary<int, TData >();
      foreach (var link in DataLinks)
      {
        if (DataExists(link.Id))
          continue;
        _dbCache.Add(link.Id, link);
      }
      
      _dbWasCached = true;
    }
  }
}