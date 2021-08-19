using System.Collections.Generic;
using UnityEngine;

namespace Packages.LiteRPG.Runtime.LiteRPG.Stats
{
  [CreateAssetMenu(fileName = "CharStats", menuName = "Character/CharStatsData")]
  public class CharStatsData : ScriptableObject
  {
    public List<CharStatUnit> CharStats;
  }
}