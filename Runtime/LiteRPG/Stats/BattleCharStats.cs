using System.Collections.Generic;
using Packages.LiteRPG.Runtime.LiteRPG.Stats.StatsSystem;
using UnityEngine;

namespace Packages.LiteRPG.Runtime.LiteRPG.Stats
{
  public class BattleCharStats : MonoBehaviour
  {
    [SerializeField] private List<CharStatUnit> _charStatsUnits;
    private CharStatsTypesSO _charStatsTypes;
    
    public void Init(CharStatsData charStatsInitData)
    {
      _charStatsUnits = Instantiate(charStatsInitData).CharStats;
      foreach (var statUnit in  _charStatsUnits)
      {
        statUnit.Init();
      }
    }

    public CharacterStat GetStat(string statName)
    {
      var resId =_charStatsUnits[0].CharStatType.FromString(statName).Id;

      return GetStat(resId);
    }

    private CharacterStat GetStat(CharStatTypeSO statType) => GetStat(statType.Id);

    private CharacterStat GetStat(int statId)
    {
      //TODO: cache the list search
      foreach (var statUnit in _charStatsUnits)
      {
        if (statUnit.CharStatType.Id == statId)
          return statUnit.CharacterStat;
      }

      return null;
    }

    public void AddModifier(StatModifierData modifier)
    {
      GetStat(modifier.StatType).AddModifier(modifier.Create());
    }
  }
}