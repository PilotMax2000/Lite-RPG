using System;
using System.Collections.Generic;
using LiteRPG.PlayerInventory.InvItem;
using UnityEngine;

namespace LiteRPG.PlayerInventory.DataBase
{
  [Serializable]
  public abstract class DataDb<TData> : ScriptableObject where TData : ScriptableIdData
  {
    [field: SerializeField]
    public List<TData> DataLinks { get; private set; }
    protected Dictionary<int, TData> _dbCache;
    protected bool _dbWasCached;
    protected bool _dataLinksAreEmpty;

    public TData GetData(int id)
    {
      CheckCacheInit();
      if (_dataLinksAreEmpty)
        return null;
      
      return _dbCache[id];
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
      if (DataLinks == null || DataLinks.Count == 0)
      {
        _dataLinksAreEmpty = true;
        _dbWasCached = true;
        return;
      }
      
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