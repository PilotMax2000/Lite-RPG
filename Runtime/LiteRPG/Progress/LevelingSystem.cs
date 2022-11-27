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
            ExpToNextLevel = _levelingTable.GetExpToLevel(Level);
        }

        public void AddExp(int expPoints)
        {
            TotalExp += expPoints;
            
            CalculateLevelUp();
            ExpToNextLevel = _levelingTable.GetExpToLevel(Level);
            CurrentExp = GetCurrentExp();
        }

        private void CalculateLevelUp()
        {
            if (TotalExp >= _levelingTable.GetExpToLevel(Level))
            {
                Level++;
                OnLevelUp?.Invoke();
                CalculateLevelUp();
            }
        }

        private int GetCurrentExp()
        {
            if(Level > 1)
                return Mathf.FloorToInt(_totalExp - _levelingTable.GetExpToLevel(Level-1));
            
            return TotalExp;
        }
    }
}