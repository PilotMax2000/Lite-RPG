using System.Collections;
using FluentAssertions;
using LevelGameplay.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEngine.TestTools;

namespace AstralRPG
{
    public class CooldownTimerTests
    {
        [Test]
        public void WhenCooldownIs3s_AndTimerIsActiveWithNoTimeHasPast_ThenLeftCooldownTimerIsOverFalseWithValue3()
        {
            // Arrange.
            float cooldownTime = 3f;
            var cooldownTimer = new CooldownTimer(cooldownTime);

            // Act.
            cooldownTimer.SetTimerAsActive(true);

            // Assert.
            cooldownTimer.TimeLeft.Should().Be(3);
            cooldownTimer.IsOver.Should().Be(false);
        }
        
        [Test]
        public void WhenCooldownIs3s_AndTimerIsActivedWith1SecPast_ThenLeftCooldownTimeShouldBe2()
        {
            // Arrange.
            const float cooldownTime = 3f;
            var cooldownTimer = new CooldownTimer(cooldownTime);

            // Act.
            cooldownTimer.SetTimerAsActive(true);
            cooldownTimer.UpdateTickTime(1f);

            // Assert.
            cooldownTimer.TimeLeft.Should().Be(2);
        }
        [Test]
        public void WhenCooldownIs3s_AndTimerIsNotActivedWith1SecPast_ThenLeftCooldownTimeShouldBe3()
        {
            // Arrange.
            const float cooldownTime = 3f;
            var cooldownTimer = new CooldownTimer(cooldownTime);

            // Act.
            cooldownTimer.SetTimerAsActive(false);
            cooldownTimer.UpdateTickTime(1f);

            // Assert.
            cooldownTimer.TimeLeft.Should().Be(3);
        }
        
        [Test]
        public void WhenCooldownIs3s_AndTimerIsActiveWith4SecPast_ThenLeftCooldownTimeShouldBe0()
        {
            // Arrange.
            float cooldownTime = 3f;
            var cooldownTimer = new CooldownTimer(cooldownTime);

            // Act.
            cooldownTimer.SetTimerAsActive(true);
            cooldownTimer.UpdateTickTime(4f);

            // Assert.
            cooldownTimer.TimeLeft.Should().Be(0);
        }
        
        [Test]
        public void WhenCooldownIs3s_AndTimerWith4SecPast_ThenCooldownIsOver()
        {
            // Arrange.
            float cooldownTime = 3f;
            var cooldownTimer = new CooldownTimer(cooldownTime);

            // Act.
            cooldownTimer.SetTimerAsActive(true);
            cooldownTimer.UpdateTickTime(4f);

            // Assert.
            cooldownTimer.IsOver.Should().Be(true);
        }
        
        [Test]
        public void WhenCooldownIs3s_AndTimerIsActiveWithPast3SecondsAndReset_ThenCooldownLeftTimeIs3WithIsOverFalse()
        {
            // Arrange.
            float cooldownTime = 3f;
            var cooldownTimer = new CooldownTimer(cooldownTime);

            // Act.
            cooldownTimer.SetTimerAsActive(true);
            cooldownTimer.UpdateTickTime(3f);
            cooldownTimer.ResetCooldown();

            // Assert.
            cooldownTimer.TimeLeft.Should().Be(3);
            cooldownTimer.IsOver.Should().Be(false);
        }
        
        [Test]
        public void WhenCooldownIs3s_AndTimerIsActiveWithPast1Second_ThenOnTimerValueChangedTriggersWithValue2()
        {
            // Arrange.
            float cooldownTime = 3f;
            float onTimerLeftValue = -1;
            var cooldownTimer = new CooldownTimer(cooldownTime);

            // Act.
            cooldownTimer.SetTimerAsActive(true);
            cooldownTimer.OnTimerValueChanged += (value) => {onTimerLeftValue = value;};
            cooldownTimer.UpdateTickTime(1f);

            // Assert.
            onTimerLeftValue.Should().Be(2);
        }
        
        [Test]
        public void WhenCooldownIs3s_AndTimerIsNotActiveWithPast1Second_ThenOnTimerValueChangedValueShouldLeftMinus1()
        {
            // Arrange.
            float cooldownTime = 3f;
            float onTimerLeftValue = -1;
            var cooldownTimer = new CooldownTimer(cooldownTime);

            // Act.
            cooldownTimer.SetTimerAsActive(false);
            cooldownTimer.OnTimerValueChanged += (value) => {onTimerLeftValue = value;};
            cooldownTimer.UpdateTickTime(1f);

            // Assert.
            onTimerLeftValue.Should().Be(-1);
        }
    }
}