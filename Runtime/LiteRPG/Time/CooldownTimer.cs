using System;
using UnityEngine;
using Math = System.Math;

namespace LevelGameplay.Generic
{
    [Serializable]
    public class CooldownTimer
    {
        public Action<float> OnTimerValueChanged;
        public Action<bool> OnIsOverChanged;
        public Action<float> OnTimerTick;
        public float TimeLeft => _timeLeft;
        public bool IsOver { get; private set; }

        [SerializeField] private float _timeLeft;
        [SerializeField] private bool _timerIsActive;
        private float _cooldownTime;

        public CooldownTimer(float cooldownTime, bool isOverAtBeginning = false)
        {
            _cooldownTime = cooldownTime;
            if (isOverAtBeginning)
            {
                _timeLeft = 0;
                IsOver = true;
            }
            else
            {
                _timeLeft = cooldownTime;
                IsOver = false;
            }
        }

        public CooldownTimer()
        {
            _cooldownTime = 0;
            _timeLeft = 0;
            _timerIsActive = false;
        }

        public void UpdateByTime(float value)
        {
            if(_timerIsActive == false)
                return;
            
            if(_timeLeft <= 0)
                return;

            if (IsTimeChangeSignificant() == false)
                return;

            _timeLeft = Mathf.Clamp(_timeLeft - value, 0f, _cooldownTime);
            OnTimerValueChanged?.Invoke(_timeLeft);
            OnTimerTick?.Invoke(value);
                
            if(_timeLeft <= 0)
            {
                IsOver = true;
                OnIsOverChanged?.Invoke(true);
            }

            bool IsTimeChangeSignificant() =>
                Math.Abs(_timeLeft - (_timeLeft - value))> Constants.Epsilon;
        }
        
        public void SetTimerAsActive(bool isActive)
        {
            _timerIsActive = isActive;
        }

        public void ResetCooldown()
        {
            _timeLeft = _cooldownTime;
            IsOver = false;
            OnIsOverChanged?.Invoke(false);
            OnTimerValueChanged?.Invoke(_timeLeft);
        }
        
        public void InstantlySetCooldownAsOver()
        {
            _timeLeft = 0;
            IsOver = true;
            OnIsOverChanged?.Invoke(true);
            OnTimerValueChanged?.Invoke(_timeLeft);
        }
        
        public void SetNewCooldownTime(float newCooldownTime)
        {
            _cooldownTime = newCooldownTime;
            _timeLeft = _cooldownTime;
            IsOver = false;
            OnIsOverChanged?.Invoke(false);
            OnTimerValueChanged?.Invoke(_timeLeft);
        }
        
        public float GetTimeLeftPercentage() =>
            _timeLeft / _cooldownTime;

        public void AddTime(float time)
        {
            _timeLeft = Mathf.Clamp(_timeLeft + time, 0f, _timeLeft + time);
            OnTimerValueChanged?.Invoke(_timeLeft);
            if(_timeLeft <= 0)
            {
                IsOver = true;
                OnIsOverChanged?.Invoke(true);
            }
        }
    }
}