using System;

namespace Packages.LiteRPG.Runtime.LiteRPG.Stats
{
  [Serializable]
  public class CharStatUnit
  {
    public CharStatTypeSO CharStatType;
    public CharacterStat CharacterStat;

    public void Init()
    {
      CharacterStat = new CharacterStat(CharacterStat.BaseValue);
    }
  }
}