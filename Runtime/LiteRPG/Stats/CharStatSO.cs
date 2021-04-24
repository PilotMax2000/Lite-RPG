using System;
using System.Collections.Generic;
using LiteRPG.PlayerInventory.InvItem;
using UnityEngine;

namespace Packages.LiteRPG.Runtime.LiteRPG.Stats
{
  [CreateAssetMenu(fileName = "CharStat", menuName = "Character/CharStat")]
  public class CharStatSO : ObjectTypeSO
  {
  }
  
  [CreateAssetMenu(fileName = "CharStatTypes", menuName = "Character/StatTypes")]
  public class CharStatsTypesSO : ObjectTypesSO
  {
    
  }
  
  [CreateAssetMenu(fileName = "CharStats", menuName = "Character/CharStatsData")]
  public class CharStatsData : ScriptableObject
  {
    public List<CharStatValues> CharStats;
  }

  [Serializable]
  public class CharStatValues
  {
    public CharStatSO CharStat;
    public float InitValue;
    public float CurValue;
    public bool OnResetCopyInitValueToCur;
    
    public void ResetCurValue() => 
      CurValue = OnResetCopyInitValueToCur ? InitValue : 0;
  }

  public class BattleCharStats : MonoBehaviour
  {
    [SerializeField] private List<CharStatValues> _charStats;
    private CharStatsTypesSO _charStatsTypes;
    
    public void Init(CharStatsData charStatsInitData)
    {
      _charStats = Instantiate(charStatsInitData).CharStats;
      foreach (var stat in  _charStats)
      {
        stat.ResetCurValue();
      }
    }

    public CharStatValues GetStat(string statName)
    {
      var resId =_charStats[0].CharStat.FromString(statName).Id;

      foreach (var stat in _charStats)
      {
        if (stat.CharStat.Id == resId)
          return stat;
      }

      return null;
    }
  }
}