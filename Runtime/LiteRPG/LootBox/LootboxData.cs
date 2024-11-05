using System;
using System.Collections.Generic;
using System.Linq;
using LiteRPG.PlayerInventory.InvItem;
using Sirenix.Utilities;
using Unity.Collections;
using UnityEngine;

namespace LiteRPG.LootBox
{
  [CreateAssetMenu(fileName = "LootboxData", menuName = "Items/LootboxData")]
  public class LootboxData : ScriptableObject
  {
    public InvItemSlot[] GuaranteedLoot;
    public LootEntry[] RandomLootEntry;

    public List<InvItemSlot> GetLoot()
    {
      List<InvItemSlot> loot = new List<InvItemSlot>();
      if(GuaranteedLoot.Length > 0)
        loot.AddRange(GuaranteedLoot);
      if (RandomLootEntry.Length <= 0) 
        return loot;
      if (CanGetRandomItem(out var resultItemSlot)) 
        loot.Add(resultItemSlot);
      return loot;
    }

    public bool CanGetRandomItem(out InvItemSlot resultItemSlot)
    {
      resultItemSlot = null;
      int totalWeight = 0;
      foreach (var entry in RandomLootEntry)
      {
        totalWeight += entry.Weight;
      }

      int randomWeight = UnityEngine.Random.Range(0, totalWeight);
      int currentWeight = 0;
      foreach (var entry in RandomLootEntry)
      {
        currentWeight += entry.Weight;
        if (randomWeight < currentWeight)
        {
          resultItemSlot = entry.ItemSlot;
          return true;
        }
      }
      return false;
    }

    private void OnValidate()
    {
      if (RandomLootEntry.IsNullOrEmpty())
        return;

      int totalWeight = CalculateTotalWeight();
      UpdateEditorChances(totalWeight);
    }

    private void UpdateEditorChances(int totalWeight)
    {
      foreach (var lootEntry in RandomLootEntry) 
        lootEntry.UpdateEditorChance(totalWeight);
    }

    private int CalculateTotalWeight() => 
      RandomLootEntry.Sum(lootEntry => lootEntry.Weight);

    [Serializable]
    public class LootEntry
    {
      public InvItemSlot ItemSlot;
      public int Weight;
      [SerializeField] [ReadOnly] private string _editorDropChance;

      public void UpdateEditorChance(float totalWeight) => 
        _editorDropChance = $"{Weight / totalWeight * 100}%";
    }
  }
}
