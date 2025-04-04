﻿using System;
using LiteRPG.PlayerInventory.SubMenus.Craft.Recipes;
using Packages.LiteRPG.Runtime.LiteRPG.Stats.StatsSystem;
using Sirenix.Utilities;
using UnityEngine;

namespace LiteRPG.PlayerInventory.InvItem
{
    public class ScriptableIdData : ScriptableObject
    {
        public int Id = -1;
    }

    [CreateAssetMenu(fileName = "InvItemData", menuName = "Items/InvItemData")]
    public class InvItemData : ScriptableIdData
    {
        public string ItemName = "NoName";
        [TextArea(5, 10)]
        public string Description = "NoDescription";
        public Sprite Icon;
        public InvItemType InvItemType;
        public int BasePrice = 0;
        public bool IsStackable = true;
        public bool CanEquip = false;
        public StatModifierProperty[] StatModifiers;
        public EquipSlotType[] AllowedSlotsToEquip;
        
        [Header("Recipe Item")]
        public bool IsRecipy;
        public RecipeData RecipeData;
        
        public virtual bool Use(Inventory inventory, IAdditiveHp additiveHp)
        {
            if (CanBeUsed() == false)
                return false;
            
            if (IsRecipy)
            {
                if (inventory.Crafting.TryAddRecipe(RecipeData.Id) == false)
                {
                    Debug.LogError("Failed to use and add recipe!");
                    return false;
                }
            }
            return true;
        }

        public bool CanBeUsed() => 
            InvItemType.InvItemTypes().IsUsableType(InvItemType);

        public string GetStatModifiersDescription()
        {
            string res = String.Empty;
            for (var index = 0; index < StatModifiers.Length; index++)
            {
                var modifier = StatModifiers[index];
                res += modifier.GetModifierDescription();
                if (index < StatModifiers.Length - 1)
                    res += "<br>";
            }

            return res;
        }

        public bool HaveStatModifiers() =>
            StatModifiers.IsNullOrEmpty() == false;
    }

    public interface IAdditiveHp
    {
        void AddHp(float hpToAdd);
    }
}
