using FluentAssertions;
using LiteRPG.PlayerInventory;
using LiteRPG.PlayerInventory.InvItem;
using NUnit.Framework;

namespace Tests.EditMode
{
    public class InventoryEquippedItemsTests
    {
        private const int MAX_ITEMS_SLOTS = 10;
        private const int MAX_EQUIP_SLOTS = 10;
        
        [Test]
        public void WhenInventoryIsCreated_ThenEquipSlot0IsEmpty()
        {
            // Arrange.
            Inventory inventory = Create.InventoryWithCharStatsAndItemsDb(MAX_ITEMS_SLOTS);
            inventory.SetupEquipSlots(MAX_EQUIP_SLOTS);
            var slotTypeToEquip = EquipSlotType.S0;

            // Act.
      
            // Assert.
            bool slotWasFound = inventory.EquippedSlots.TryGetSlotByType(slotTypeToEquip, out var foundSlot);
            slotWasFound.Should().BeTrue();
            foundSlot.IsEquipped.Should().BeFalse();
        }
        
        [Test]
        public void WhenInventoryIsCreated_AndSwordItemAdded_ThenItIsNotEquipped()
        {
            // Arrange.
            Inventory inventory = Create.InventoryWithCharStatsAndItemsDb(MAX_ITEMS_SLOTS);
            inventory.SetupEquipSlots(MAX_EQUIP_SLOTS);

            // Act.
      
            // Assert.
            inventory.Backpack.GetSlot(0).IsEquipped.Should().BeFalse();
        }
        
        [Test]
        public void WhenSwordItemWasAdded_AndWasEquippedOnSlotS0_ThenItsSlotEquippedStateShouldBeTrue()
        {
            // Arrange.
            Inventory inventory = Create.InventoryWithCharStatsAndItemsDb(MAX_ITEMS_SLOTS);
            inventory.SetupEquipSlots(MAX_EQUIP_SLOTS);
            var slotTypeToEquip = EquipSlotType.S0;

            // Act.
            inventory.Backpack.GetSlot(0).Equip(slotTypeToEquip);
      
            // Assert.
            inventory.Backpack.GetSlot(0).IsEquipped.Should().BeTrue();
        }
        
        [Test]
        public void WhenSwordItemWasAdded_AndWasEquippedOnSlotS0_ThenEquipSlotS0IsEquippedStateShouldBeTrue()
        {
            // Arrange.
            Inventory inventory = Create.InventoryWithCharStatsAndItemsDb(MAX_ITEMS_SLOTS);
            inventory.SetupEquipSlots(MAX_EQUIP_SLOTS);
            var slotTypeToEquip = EquipSlotType.S0;

            // Act.
            inventory.Backpack.GetSlot(0).Equip(slotTypeToEquip);
      
            // Assert.
            inventory.Backpack.GetSlot(0).IsEquipped.Should().BeTrue();
        }
        
        /*[Test]
        public void When_And_Then()
        {
            // Arrange.
            Inventory inventory = Create.InventoryWithCharStatsAndItemsDb(MAX_ITEMS_SLOTS);
            inventory.SetupEquipSlots(MAX_EQUIP_SLOTS);
            InvItemData swordItem = Create.LoadInvItem(GameDesign.Items.SwordNonStackable);

            // Act.
            bool firstItemWasSuccessfullyAdded = inventory.AddItem(swordItem);
            bool secondItemWasSuccessfullyAdded = inventory.AddItem(swordItem);
      
            // Assert.
            firstItemWasSuccessfullyAdded.Should().BeTrue();
            secondItemWasSuccessfullyAdded.Should().BeTrue();
            inventory.GetSlotWithItem(swordItem).ItemSlot.Quantity.Should().Be(2);
            inventory.Backpack.GetAllSlots().Count.Should().Be(1);
        }*/
    }
}