using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Packages.LiteRPG.Runtime.LiteRPG.Stats
{
  [CreateAssetMenu(fileName = "CharStats", menuName = "Character/CharStatsData")]
  public class CharStatsData : ScriptableObject
  {
    public List<CharStatUnit> CharStats;
    
    public bool TryToGetStatById(int statId, out CharacterStat stat)
    {
      stat = (from statUnit in CharStats
        where statUnit.CharStatType.Id == statId 
        select statUnit.CharacterStat).FirstOrDefault();
      return stat != null;
    }
  }
}