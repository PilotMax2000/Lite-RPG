using System;
using UnityEngine;

namespace LiteRPG.Progress
{
    [Serializable]
    public class LevelingSystem
    {
        [field: SerializeField] public int Level { get; private set; }
        [field: SerializeField] public int CurrentExp { get; private set; }
        [field: SerializeField] public int ExpToNextLevel { get; private set; }
        [field: SerializeField] public int MaxLevel { get; private set; }

        public event Action OnLevelUp;

        private const int StartingLevel = 1;
        private readonly LevelingTableData _levelingTable;
        private int _totalExp;

        public LevelingSystem(LevelingTableData levelingTable)
        {
            _levelingTable = levelingTable;
            
            Level = StartingLevel;
            CurrentExp = 0;
            ExpToNextLevel = GetExpDifferenceToNextLevel();
            MaxLevel = _levelingTable.ExpToLevels.Length + StartingLevel;
        }

        public int TotalExp
        {
            get => _totalExp;
            private set => _totalExp = value < 0 ? 0 : value;
        }

        public LevelingSystem(LevelingTableData levelingTable, int totalExpToLoad)
        {
            _levelingTable = levelingTable;

            Vector2 convertToLevelAndLeftExp = ConvertTotalExpToLevel(totalExpToLoad);
            TotalExp = totalExpToLoad;
            Level = (int)convertToLevelAndLeftExp.x;
            CurrentExp = (int)convertToLevelAndLeftExp.y;
            ExpToNextLevel = GetExpDifferenceToNextLevel();
            MaxLevel = _levelingTable.ExpToLevels.Length + 1;
        }

        public void AddExp(int expPoints)
        {
            if(IsMaxLevelReached()) 
                return;
            var newTotalExp = TotalExp + expPoints;
            TotalExp = newTotalExp > _levelingTable.GetTotalExpToLevelUpgradeOnLevel(_levelingTable.GetMaxLevel()-1) 
                ? _levelingTable.GetTotalExpToLevelUpgradeOnLevel(_levelingTable.GetMaxLevel()-1) 
                : newTotalExp;
            
            CalculateLevelUp();
            CurrentExp = GetCurrentExp();
        }

        private bool IsMaxLevelReached() => 
            Level == MaxLevel;

        private int GetExpDifferenceToNextLevel()
        {
            if (Level >= _levelingTable.GetMaxLevel())
                return 0;
            return _levelingTable.GetTotalExpToLevelUpgradeOnLevel(Level) -
                   _levelingTable.GetTotalExpToLevelUpgradeOnLevel(Level - 1);
        }

        private Vector2 ConvertTotalExpToLevel(int totalExpToLoad)
        {
            int resultLevel = 1;
            while (totalExpToLoad - _levelingTable.GetTotalExpToLevelUpgradeOnLevel(resultLevel) >= 0)
            {
                resultLevel++;
                if (resultLevel >= _levelingTable.GetMaxLevel())
                    return new Vector2(resultLevel, 0);
            }

            return new Vector2(resultLevel, totalExpToLoad - _levelingTable.GetTotalExpToLevelUpgradeOnLevel(resultLevel-1));
        }

        private void CalculateLevelUp()
        {
            while (true)
            {
                if (TotalExp >= _levelingTable.GetTotalExpToLevelUpgradeOnLevel(Level))
                {
                    Level++;
                    ExpToNextLevel = GetExpDifferenceToNextLevel();
                    OnLevelUp?.Invoke();

                    if (Level < MaxLevel) 
                        continue;
                }

                break;
            }
        }

        private int GetCurrentExp() => 
            Level > 1 ? Mathf.RoundToInt(_totalExp - _levelingTable.GetTotalExpToLevelUpgradeOnLevel(Level-1)) : TotalExp;
    }
}