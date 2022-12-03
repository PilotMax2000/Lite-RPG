using System;
using UnityEngine;

namespace LiteRPG.Progress
{
    [CreateAssetMenu(fileName = "LevelingTable", menuName = "LightRPG/Character/LevelingTable")]
    public class LevelingTableData : ScriptableObject
    {
        public ExpToLevel[] ExpToLevels;

        public int GetTotalExpToLevelUpgradeOnLevel(int level)
        {
            if (level < 0)
            {
                Debug.LogError("Level can't be less than 0");
                return 0;
            }
            
            if (level == 0)
                return 0;

            if (level == GetMaxLevel())
                return 0;

            for (int i = 0; i < ExpToLevels.Length; i++)
            {
                if(ExpToLevels[i].Level == level)
                    return ExpToLevels[i].ExpPointsToLevel;
            }
            
            Debug.LogError($"Level {level} not found");
            return 0;
        }
        
        public int GetMaxLevel() => 
            ExpToLevels.Length + 1;

        [Serializable]
        public class ExpToLevel
        {
            public int Level;
            public int ExpPointsToLevel;
        }
    }
}