using FluentAssertions;
using LiteRPG.PlayerInventory;
using LiteRPG.PlayerInventory.InvItem;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace Tests
{
    public class InventoryBackpackTests
    {
        [Test]
        public void InventoryBackpackDefault10LimitOnCreationPasses()
        {
            // Use the Assert class to test conditions
            InventoryBackpack backpack = new InventoryBackpack();
            Assert.AreEqual(10, backpack.GetAllSlots().Count);
        }

        [Test]
        public void InventoryBackpackDefault10EmptySlotOnCreationPasses()
        {
            // Use the Assert class to test conditions
            InventoryBackpack backpack = new InventoryBackpack();
            Assert.AreEqual(10, backpack.GetEmptySlots().Count);
        }

        [Test]
        public void InventoryBackpack25LimitOnCreationPasses()
        {
            // Use the Assert class to test conditions
            InventoryBackpack backpack = new InventoryBackpack(25);
            Assert.AreEqual(25, backpack.GetAllSlots().Count);
        }

        [Test]
        public void InventoryBackpack25EmptySlotsOnCreationPasses()
        {
            // Use the Assert class to test conditions
            InventoryBackpack backpack = new InventoryBackpack(25);
            Assert.AreEqual(25, backpack.GetEmptySlots().Count);
        }

        [Test]
        public void InventoryBackpack1SlotWithSet0OnCreationPasses()
        {
            // Use the Assert class to test conditions
            InventoryBackpack backpack = new InventoryBackpack(0);
            Assert.AreEqual(1, backpack.GetAllSlots().Count);
        }

        [Test]
        public void InventoryBackpack1EmptySlotWithSet0OnCreationPasses()
        {
            // Use the Assert class to test conditions
            InventoryBackpack backpack = new InventoryBackpack(0);
            Assert.AreEqual(1, backpack.GetEmptySlots().Count);
        }
        
        [Test]
        public void InventoryBackpackHasEmptySlotWith1LimitSlotPasses()
        {
            // Use the Assert class to test conditions
            InventoryBackpack backpack = new InventoryBackpack(1);
            Assert.AreEqual(true, backpack.HasEmptySlot());
        }
        
        [Test]
        public void InventoryBackpackNoEmptySlotWithLimit1And1Item()
        {
            // Use the Assert class to test conditions
            InventoryBackpack backpack = new InventoryBackpack(1);
            var invItemData = ScriptableObject.CreateInstance<InvItemData>();
            invItemData.ItemName = "Sword";
            invItemData.Id = 1;
            InvItemSlot invItemSlot= new InvItemSlot(invItemData);
            backpack.AddInvItem(invItemSlot);
            Assert.AreEqual(0, backpack.GetEmptySlots().Count);
        }
        
        [Test]
        public void InventoryBackpackAddAndRemove1ItemOf2Passes()
        {
            // Use the Assert class to test conditions
            InventoryBackpack backpack = new InventoryBackpack(1);
            var invItemData = ScriptableObject.CreateInstance<InvItemData>();
            invItemData.ItemName = "Sword";
            invItemData.Id = 1;
            InvItemSlot invItemSlot= new InvItemSlot(invItemData,2);
            backpack.AddInvItem(invItemSlot);
            backpack.RemoveInvItem(0,1);
            Assert.AreEqual(1, backpack.GetSlot(0).ItemSlot.Quantity);
        }
        
        [Test]
        public void InventoryBackpackAddAndRemove1ItemOf1Passes()
        {
            // Arrange.
            InventoryBackpack backpack = new InventoryBackpack(1);
            var invItemData = ScriptableObject.CreateInstance<InvItemData>();
            invItemData.ItemName = "Sword";
            invItemData.Id = 1;
            InvItemSlot invItemSlot= new InvItemSlot(invItemData,1);
            
            // Act
            backpack.AddInvItem(invItemSlot);
            backpack.RemoveInvItem(0,1);
            
            // Assert
            backpack.GetSlot(0).IsEmpty().Should().BeTrue();
        }
        
        

        // // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // // `yield return null;` to skip a frame.
        // [UnityTest]
        // public IEnumerator InventoryTestsWithEnumeratorPasses()
        // {
        //     // Use the Assert class to test conditions.
        //     // Use yield to skip a frame.
        //     yield return null;
        // }
    }
}
