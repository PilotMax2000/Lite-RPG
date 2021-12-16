using System;
using LiteRPG.PlayerInventory.InvItem;
using UnityEngine;

namespace Packages.LiteRPG.Runtime.LiteRPG.Stats
{
  [CreateAssetMenu(fileName = "CharStat", menuName = "Character/CharStat")]
  [Serializable]
  public class CharStatTypeSO : ObjectTypeSO
  {
    public Sprite Icon;
  }
}