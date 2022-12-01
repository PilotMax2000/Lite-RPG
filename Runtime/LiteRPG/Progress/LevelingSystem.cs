using System;
using UnityEngine;

namespace LiteRPG.Progress
{
    [Serializable]
    public class LevelingSystem
    {
        [field: SerializeField] public int Level { get; private set; }
        [field: SerializeField] public int CurrentExp { get; set; }
        [field: SerializeField] public int ExpToNextLevel { get; private set; }

        public event Action OnLevelUp;

        private readonly LevelingTableData _levelingTable;
        private int _totalExp;

        public int TotalExp
        {
            get => _totalExp;
            private set { _totalExp = value < 0 ? 0 : value; }
        }

        public LevelingSystem(LevelingTableData levelingTable)
        {
            _levelingTable = levelingTable;
            
            Level = 1;
            CurrentExp = 0;
            ExpToNextLevel = GetExpDifferenceToNextLevel();
        }

        public void AddExp(int expPoints)
        {
            TotalExp += expPoints;
            
            CalculateLevelUp();
            CurrentExp = GetCurrentExp();
        }

        private int GetExpDifferenceToNextLevel() => 
            _levelingTable.GetTotalExpToLevel(Level) - _levelingTable.GetTotalExpToLevel(Level-1);

        private void CalculateLevelUp()
        {
            if (TotalExp >= _levelingTable.GetTotalExpToLevel(Level))
            {
                Level++;
                ExpToNextLevel = GetExpDifferenceToNextLevel();
                OnLevelUp?.Invoke();
                CalculateLevelUp();
            }
        }

        private int GetCurrentExp()
        {
            if(Level > 1)
                return Mathf.RoundToInt(_totalExp - _levelingTable.GetTotalExpToLevel(Level-1));
            
            return TotalExp;
        }
    }
}