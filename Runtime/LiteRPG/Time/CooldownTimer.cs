using System;
using UnityEngine;

namespace LevelGameplay.Generic
{
    public class CooldownTimer
    {
        public float TimeLeft => _timeLeft;
        public bool IsOver { get; private set; }

        private readonly float _cooldownTime;
        private float _timeLeft;
        private bool _timerIsActive;

        public Action<float> OnTimerValueChanged;
        public Action<bool> OnIsOverChanged;
        
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

        public void UpdateByTime(float value)
        {
            if(_timerIsActive == false)
                return;
            
            if(_timeLeft <= 0)
                return;

            if (Math.Abs(_timeLeft - (_timeLeft - value))> Constants.Epsilon)
            {
                _timeLeft = Mathf.Clamp(_timeLeft - value, 0f, _cooldownTime);
                OnTimerValueChanged?.Invoke(_timeLeft);
                
                if(_timeLeft <= 0)
                {
                    IsOver = true;
                    OnIsOverChanged?.Invoke(true);
                }
            }
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

    }
}