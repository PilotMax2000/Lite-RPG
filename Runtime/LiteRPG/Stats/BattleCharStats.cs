using System.Collections.Generic;
using Packages.LiteRPG.Runtime.LiteRPG.Stats.StatsSystem;
using UnityEngine;

namespace Packages.LiteRPG.Runtime.LiteRPG.Stats
{
  public class BattleCharStats : MonoBehaviour
  {
    [SerializeField] protected List<CharStatUnit> _charStatsUnits;
    private CharStatsTypesSO _charStatsTypes;

    public List<CharStatUnit> GetCharStatsUnits()
    {
      return _charStatsUnits;
    }
    public void Init(CharStatsData charStatsInitData)
    {
      _charStatsUnits = Instantiate(charStatsInitData).CharStats;
      foreach (var statUnit in  _charStatsUnits)
      {
        statUnit.Init();
      }
    }
    
    public void Init(List<CharStatUnit> charStatsUnits)
    {
      _charStatsUnits = charStatsUnits;
    }
    

    public CharacterStat GetStat(string statName)
    {
      var resId =_charStatsUnits[0].CharStatType.FromString(statName).Id;

      return GetStat(resId);
    }

    public CharacterStat GetStat(CharStatTypeSO statType) => GetStat(statType.Id);

    public CharacterStat GetStat(int statId)
    {
      //TODO: cache the list search
      foreach (var statUnit in _charStatsUnits)
      {
        if (statUnit.CharStatType.Id == statId)
          return statUnit.CharacterStat;
      }

      return null;
    }

    public StatModifier AddModifier(StatModifierData modifier)
    {
      var newModifier = modifier.Create();
      GetStat(modifier.StatType).AddModifier(newModifier);
      return newModifier;
    }
    
    public void LogCurrentStats()
    {
      //Improve look with console pro
      Debug.Log("Player Stats:");
      string resStats = "";
      foreach (var statUnit in _charStatsUnits)
      {
        resStats += $"<color=green>{statUnit.CharStatType.Name}: {statUnit.CharacterStat.Value}</color>\n";
      }
      Debug.Log(resStats);
    }

    public void RemoveModifier(StatModifierData modifierData, StatModifier statModifier)
    {
      GetStat(modifierData.StatType).RemoveModifier(statModifier);
    }
  }
}