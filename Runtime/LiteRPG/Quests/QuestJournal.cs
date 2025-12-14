using System;
using System.Collections.Generic;
using System.Linq;
using LiteRPG.PlayerInventory;
using LiteRPG.PlayerInventory.InvItem;
using Sirenix.Utilities;
using UnityEngine;

namespace LiteRPG.Runtime.LiteRPG.Quests
{
    [Serializable]
    public abstract class QuestJournal<TQuestData, TInvItemData, TInventory> where TQuestData : QuestData
                                                                    where TInvItemData : InvItemData
                                                                    where TInventory : Inventory
    {
        public event Action OnQuestsStateChanged;
        public event Action<TQuestData> OnCurrentActiveQuestChanged;
        public event Action<TQuestData> OnQuestFinished;
        public event Action<TQuestData> OnQuestStarted;
        public event Action<InvItemSlot> OnNewItemRewardAdded; 

        public bool QuestNotificationsEnabled { get; set; }
        public TQuestData CurrentActiveQuest => _currentActiveQuest;

        [SerializeField] private List<TQuestData> _inProgressQuests;
        [SerializeField] private List<TQuestData> _finishedQuests;
        [SerializeField] TQuestData _currentActiveQuest;

        private QuestProgressAnalyzer<TQuestData,TInvItemData, TInventory> _questProgressAnalyzer;
        private List<TQuestData> InProgressQuests => _inProgressQuests;
        private TInventory _inventory;

        public void SetInventory(TInventory inventory)
        {
            _inventory = inventory;
        }

        public List<TQuestData> GetActiveQuests()
        {
            if (_inProgressQuests == null)
            {
                Debug.Log("No active quests");
                return null;
            }

            Debug.Log($"Active quests: {InProgressQuests.Count}");
            foreach (var questInProgress in _inProgressQuests)
            {
                Debug.Log($"Quest: {questInProgress.Title}");
            }
            return _inProgressQuests;
        }

        public bool TryToAddNewQuest(TQuestData questToAdd)
        {
            if (questToAdd == null)
            {
                Debug.LogError("Quest to add is null!");
                return false;
            }
            
            if (_inProgressQuests.Count == 0 || _inProgressQuests.Contains(questToAdd) == false)
            {
                AddNewQuest(questToAdd);
                return true;
            }

            Debug.LogError($"Quest {questToAdd.Title} can't be add as new - it is already added");
            return false;
        }

        public bool IsQuestReadyToComplete(TQuestData quest)
        {
            if (_questProgressAnalyzer.IsQuestCanBeCompleted(quest))
            {
                Debug.Log($"Quest '{quest.Title}' requirements are met. You can complete it now and get your reward!");
                return true;
            }

            return false;
        }

        public bool IsQuestFinished(TQuestData quest)
        {
            if (_finishedQuests.Any(finishedQuest => finishedQuest.Id == quest.Id))
            {
                Debug.Log($"Quest '{quest.Title}' already finished!");
                return true;
            }

            return false;
        }

        public bool TryFinishQuest(TQuestData questToComplete)
        {
            if (IsQuestReadyToComplete(questToComplete) == false)
            {
                Debug.LogError($"Quest '{questToComplete.Title}' is not ready to be completed!");
                return false;
            }
            FinishQuest(questToComplete);
            return true;
        }

        public bool TrySetQuestAsCurrentlyActive(TQuestData activeQuest)
        {
            _currentActiveQuest = activeQuest;
            OnCurrentActiveQuestChanged?.Invoke(activeQuest);
            return true;
        }

        public bool HasCurrentActiveQuest() => 
            _currentActiveQuest;

        public bool IsQuestInProgress(TQuestData questData)
        {
            foreach (var inProgressQuest in _inProgressQuests)
            {
                if (inProgressQuest.Id == questData.Id)
                    return true;
            }
            return false;
        }

        public QuestState GetQuestState(TQuestData[] questData)
        {
            if(questData == null)
                return QuestState.Undefined;
            foreach (var characterQuest in questData)
            {
                if (IsQuestReadyToComplete(characterQuest))
                    return QuestState.ReadyToComplete;
                if (IsQuestInProgress(characterQuest))
                    return QuestState.InProgress;
                if (IsQuestFinished(characterQuest))
                    continue;
                return QuestState.Available;
            }

            return QuestState.Finished;

        }

        public string GetCurrentQuestTextGoalAndProgress()
        {
            return HasCurrentActiveQuest() 
                ? _questProgressAnalyzer.GetQuestTextGoalAndProgress(_currentActiveQuest) 
                : string.Empty;
        }

        protected abstract void AddMoneyAndExpReward(TQuestData quest);

        private void CreateInProgressAndFinishQuestLists()
        {
            _inProgressQuests = new List<TQuestData>();
            _finishedQuests = new List<TQuestData>();
        }

        private void FinishQuest(TQuestData questToComplete)
        {
            Debug.Log($"Quest '{questToComplete.Title}' completed!");
            GetQuestReward(questToComplete);
            _inProgressQuests.Remove(questToComplete);
            _finishedQuests.Add(questToComplete);

            _questProgressAnalyzer.TryRemoveQuestItemsFromPlayer(questToComplete, _inventory);
            
            if (_currentActiveQuest.Id == questToComplete.Id)
                SetNextActiveQuest();
            OnQuestsStateChanged?.Invoke();
            OnQuestFinished?.Invoke(questToComplete);
        }

        private void SetNextActiveQuest()
        {
            if (_inProgressQuests.IsNullOrEmpty())
            {
                _currentActiveQuest = null;
                return;
            }
            _currentActiveQuest = _inProgressQuests.FirstOrDefault();
        }

        private void GetQuestReward(TQuestData quest)
        {
            if (IsQuestFinished(quest))
            {
                Debug.LogError("Quest already finished!");
                return;
            }

            AddMoneyAndExpReward(quest);
            AddRewardItems(quest.ItemsReward);
        }

        private void AddRewardItems(InvItemSlot[] itemsReward)
        {
            if (itemsReward == null || itemsReward.Length == 0)
            {
                Debug.Log("No reward items for quest");
                return;
            }

            foreach (var itemSlot in itemsReward)
            {
                if (_inventory.TryAddItem(itemSlot.ItemData, itemSlot.Quantity))
                {
                    Debug.Log($"Added {itemSlot.Quantity} of {itemSlot.ItemData.ItemName} to inventory as quest reward");
                    OnNewItemRewardAdded?.Invoke(itemSlot);
                }
                else
                    Debug.LogError($"Failed to add {itemSlot.Quantity} of {itemSlot.ItemData.ItemName} to inventory as quest reward");
            }
        }


        private void AddNewQuest(TQuestData newQuest)
        {
            InProgressQuests.Add(newQuest);
            TrySetQuestAsCurrentlyActive(newQuest);
            OnQuestsStateChanged?.Invoke();
            OnQuestStarted?.Invoke(newQuest);
            QuestNotificationsEnabled = true;
            Debug.Log($"<color=green>Quest '{newQuest.Title}' was added as new</color>");
        }
    }
}