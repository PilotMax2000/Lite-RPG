using FluentAssertions;
using LiteRPG.PlayerInventory;
using LiteRPG.PlayerInventory.InvItem;
using NUnit.Framework;
using Packages.LiteRPG.Runtime.LiteRPG.Stats;  

namespace Tests.EditMode
{
    public class InventoryEquippedItemsTests
    {
        private const int MAX_ITEMS_SLOTS = 10;
        private const int MAX_EQUIP_SLOTS = 10;
        private const int HP_STAT_ID = 0;
        private const int ATK_STAT_ID = 3;
        
        [Test]
        public void WhenInventoryIsCreated_ThenEquipSlot0IsEmpty()
        {
            // Arrange.
            Inventory inventory = Create.InventoryWithCharStatsAndItemsDb(MAX_ITEMS_SLOTS);
            BattleCharStats battleCharStats = Create.FullBattleCharStatsWith1AtkAnd10Hp();
            inventory.SetupEquipSlots(MAX_EQUIP_SLOTS, battleCharStats);
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
            BattleCharStats battleCharStats = Create.FullBattleCharStatsWith1AtkAnd10Hp();
            inventory.SetupEquipSlots(MAX_EQUIP_SLOTS, battleCharStats);
            var slotTypeToEquip = EquipSlotType.S0;

            // Act.
      
            // Assert.
            bool slotWasFound = inventory.Backpack.TryGetSlot(0, out var slot);
            slotWasFound.Should().BeTrue();
            inventory.EquippedSlots.IsBackpackSlotEquipped(slot).Should().BeFalse();
        }

        [Test]
        public void WhenSlot0IsEmpty_AndWeTryToEquipIt_ThenSlot0StillShouldBeNonEquipped()
        {
            // Arrange.
            Inventory inventory = Create.InventoryWithCharStatsAndItemsDb(MAX_ITEMS_SLOTS);
            BattleCharStats battleCharStats = Create.FullBattleCharStatsWith1AtkAnd10Hp();
            inventory.SetupEquipSlots(MAX_EQUIP_SLOTS, battleCharStats);
            var slotTypeToEquip = EquipSlotType.S0;

            // Act.
            bool slotFound = inventory.Backpack.TryGetSlot(0, out var backpackSlot);
            inventory.EquippedSlots.TryEquipSlot(slotTypeToEquip, backpackSlot);
      
            // Assert.
            var slotWasFound = inventory.EquippedSlots.TryGetSlotByType(slotTypeToEquip, out var equippedSlot);
            slotWasFound.Should().BeTrue();
            equippedSlot.IsEquipped.Should().BeFalse();
        }

        [Test]
        public void WhenSwordItemWasAdded_AndWasEquippedOnSlotS0_ThenS0ShouldBeEquipped()
        {
            // Arrange.
            Inventory inventory = Create.InventoryWithCharStatsAndItemsDb(MAX_ITEMS_SLOTS);
            BattleCharStats battleCharStats = Create.FullBattleCharStatsWith1AtkAnd10Hp();
            inventory.SetupEquipSlots(MAX_EQUIP_SLOTS, battleCharStats);
            var slotTypeToEquip = EquipSlotType.S0;
            InvItemData swordItem = Create.LoadInvItem(GameDesign.Items.SwordNonStackable);

            // Act.
            inventory.TryAddItem(swordItem);
            inventory.EquippedSlots.TryEquipSlot(slotTypeToEquip, 0);
      
            // Assert.
            var slotWasFound = inventory.EquippedSlots.TryGetSlotByType(slotTypeToEquip, out var equippedSlot);
            slotWasFound.Should().BeTrue();
            equippedSlot.IsEquipped.Should().BeTrue();
        }
        
        [Test]
        public void WhenSwordItemWasEquippedOnSlotS0_AndWasUnequiped_ThenS0ShouldNotBeEquipped()
        {
            // Arrange.
            Inventory inventory = Create.InventoryWithCharStatsAndItemsDb(MAX_ITEMS_SLOTS);
            BattleCharStats battleCharStats = Create.FullBattleCharStatsWith1AtkAnd10Hp();
            inventory.SetupEquipSlots(MAX_EQUIP_SLOTS, battleCharStats);
            var slotTypeToEquip = EquipSlotType.S0;
            InvItemData swordItem = Create.LoadInvItem(GameDesign.Items.SwordNonStackable);

            // Act.
            inventory.TryAddItem(swordItem);
            inventory.EquippedSlots.TryEquipSlot(slotTypeToEquip, 0);
            inventory.EquippedSlots.TryUnequipSlot(EquipSlotType.S0);
      
            // Assert.
            var slotWasFound = inventory.EquippedSlots.TryGetSlotByType(slotTypeToEquip, out var equippedSlot);
            slotWasFound.Should().BeTrue();
            equippedSlot.IsEquipped.Should().BeFalse();
        }

        [Test]
        public void WhenPlayerWithHpAndAttackStartingStats_AndNotingIsEquipped_ThenAttackShouldBe1()
        {
            // Arrange.
            BattleCharStats battleCharStats = Create.FullBattleCharStatsWith1AtkAnd10Hp();
      
            // Assert.
            battleCharStats.GetStat(ATK_STAT_ID).Value.Should().Be(1);
        }

        [Test]
        public void WhenSwordWith1BaseAttackWasAdded_AndWasEquippedOnSlotS0_ThenTotalBaseAttackShouldBe2()
        {
            // Arrange.
            Inventory inventory = Create.InventoryWithCharStatsAndItemsDb(MAX_ITEMS_SLOTS);
            BattleCharStats battleCharStats = Create.FullBattleCharStatsWith1AtkAnd10Hp();
            inventory.SetupEquipSlots(MAX_EQUIP_SLOTS, battleCharStats);
            var slotTypeToEquip = EquipSlotType.S0;
            InvItemData swordItem1Atk = Create.LoadInvItem(GameDesign.Items.SwordNonStackable);

            // Act.
            inventory.TryAddItem(swordItem1Atk);
            inventory.EquippedSlots.TryEquipSlot(slotTypeToEquip, 0);
      
            // Assert.
            battleCharStats.GetStat(ATK_STAT_ID).Value.Should().Be(2);
        }
        
        [Test]
        public void WhenSwordWith1BaseAttackWasAdded_AndWasEquippedAndUnequippedOnSlotS0_ThenTotalBaseAttackShouldBe1()
        {
            // Arrange.
            Inventory inventory = Create.InventoryWithCharStatsAndItemsDb(MAX_ITEMS_SLOTS);
            BattleCharStats battleCharStats = Create.FullBattleCharStatsWith1AtkAnd10Hp();
            inventory.SetupEquipSlots(MAX_EQUIP_SLOTS, battleCharStats);
            var slotTypeToEquip = EquipSlotType.S0;
            InvItemData swordItem1Atk = Create.LoadInvItem(GameDesign.Items.SwordNonStackable);

            // Act.
            inventory.TryAddItem(swordItem1Atk);
            inventory.EquippedSlots.TryEquipSlot(slotTypeToEquip, 0);
            inventory.EquippedSlots.TryUnequipSlot(slotTypeToEquip);
      
            // Assert.
            battleCharStats.GetStat(ATK_STAT_ID).Value.Should().Be(1);
        }
        
        [Test]
        public void WhenSwordWith1AtkAnd1MaxHp_AndWasEquippedOnSlotS0_ThenShouldBe2Atk11MaxHp()
        {
            // Arrange.
            Inventory inventory = Create.InventoryWithCharStatsAndItemsDb(MAX_ITEMS_SLOTS);
            BattleCharStats battleCharStats = Create.FullBattleCharStatsWith1AtkAnd10Hp();
            inventory.SetupEquipSlots(MAX_EQUIP_SLOTS, battleCharStats);
            var slotTypeToEquip = EquipSlotType.S0;
            InvItemData swordItem1Atk1MaxHp = Create.LoadInvItem(GameDesign.Items.SwordNonStackable);

            // Act.
            inventory.TryAddItem(swordItem1Atk1MaxHp);
            inventory.EquippedSlots.TryEquipSlot(slotTypeToEquip, 0);
      
            // Assert.
            battleCharStats.GetStat(ATK_STAT_ID).Value.Should().Be(2);
            battleCharStats.GetStat(HP_STAT_ID).Value.Should().Be(11);
        }
        
        [Test]
        public void WhenSwordWith1AtkAnd1MaxHp_AndWasEquippedAndRemovedOnSlotS0_ThenShouldBe1Atk10MaxHp()
        {
            // Arrange.
            Inventory inventory = Create.InventoryWithCharStatsAndItemsDb(MAX_ITEMS_SLOTS);
            BattleCharStats battleCharStats = Create.FullBattleCharStatsWith1AtkAnd10Hp();
            inventory.SetupEquipSlots(MAX_EQUIP_SLOTS, battleCharStats);
            var slotTypeToEquip = EquipSlotType.S0;
            InvItemData swordItem1Atk1MaxHp = Create.LoadInvItem(GameDesign.Items.SwordNonStackable);

            // Act.
            inventory.TryAddItem(swordItem1Atk1MaxHp);
            inventory.EquippedSlots.TryEquipSlot(slotTypeToEquip, 0);
            inventory.EquippedSlots.TryUnequipSlot(slotTypeToEquip);
      
            // Assert.
            battleCharStats.GetStat(ATK_STAT_ID).Value.Should().Be(1);
            battleCharStats.GetStat(HP_STAT_ID).Value.Should().Be(10);
        }
        
        [Test]
        public void WhenWoodNonWearableItemWasAdded_AndItWasEquippedOnSlotS0_ThenS0ShouldBeEmpty()
        {
            // Arrange.
            Inventory inventory = Create.InventoryWithCharStatsAndItemsDb(MAX_ITEMS_SLOTS);
            BattleCharStats battleCharStats = Create.FullBattleCharStatsWith1AtkAnd10Hp();
            inventory.SetupEquipSlots(MAX_EQUIP_SLOTS, battleCharStats);
            var slotTypeToEquip = EquipSlotType.S0;
            InvItemData woodNonWearable = Create.LoadInvItem(GameDesign.Items.Wood);

            // Act.
            inventory.TryAddItem(woodNonWearable);
            bool equippedSuccessfully = inventory.EquippedSlots.TryEquipSlot(slotTypeToEquip, 0);
      
            // Assert.
            equippedSuccessfully.Should().BeFalse();
            var slotWasFound = inventory.EquippedSlots.TryGetSlotByType(slotTypeToEquip, out var equippedSlot);
            slotWasFound.Should().BeTrue();
            equippedSlot.IsEquipped.Should().BeFalse();
        }
        
        [Test]
        public void WhenSwordItemForSlotS0WasAdded_AndItWasEquippedOnSlotS1_ThenS1ShouldBeEmpty()
        {
            // Arrange.
            Inventory inventory = Create.InventoryWithCharStatsAndItemsDb(MAX_ITEMS_SLOTS);
            BattleCharStats battleCharStats = Create.FullBattleCharStatsWith1AtkAnd10Hp();
            inventory.SetupEquipSlots(MAX_EQUIP_SLOTS, battleCharStats);
            var slotTypeToEquip = EquipSlotType.S1;
            InvItemData sword = Create.LoadInvItem(GameDesign.Items.SwordNonStackable);

            // Act.
            inventory.TryAddItem(sword);
            bool equippedSuccessfully = inventory.EquippedSlots.TryEquipSlot(slotTypeToEquip, 0);
      
            // Assert.
            equippedSuccessfully.Should().BeFalse();
            var slotWasFound = inventory.EquippedSlots.TryGetSlotByType(slotTypeToEquip, out var equippedSlot);
            slotWasFound.Should().BeTrue();
            equippedSlot.IsEquipped.Should().BeFalse();
        } 
        
        [Test]
        public void WhenSwordItemForSlotS0WasAdded_AndItWasEquippedOnAnySlotByBackpackSlot_ThenS0ShouldBeEquipped()
        {
            // Arrange.
            Inventory inventory = Create.InventoryWithCharStatsAndItemsDb(MAX_ITEMS_SLOTS);
            BattleCharStats battleCharStats = Create.FullBattleCharStatsWith1AtkAnd10Hp();
            inventory.SetupEquipSlots(MAX_EQUIP_SLOTS, battleCharStats);
            InvItemData sword = Create.LoadInvItem(GameDesign.Items.SwordNonStackable);

            // Act.
            inventory.TryAddItem(sword);
            bool slot0Found = inventory.Backpack.TryGetSlot(0, out var swordBackpackSlot);
            bool equippedSuccessfully = inventory.EquippedSlots.TryEquipSlot(swordBackpackSlot);
      
            // Assert.
            slot0Found.Should().BeTrue();
            equippedSuccessfully.Should().BeTrue();
            inventory.EquippedSlots.TryGetEquipSlotTypeByBackpackSlot(swordBackpackSlot, out var equipSlotType);
            var slotWasFound = inventory.EquippedSlots.TryGetSlotByType(equipSlotType, out var equippedSlot);
            slotWasFound.Should().BeTrue();
            equippedSlot.IsEquipped.Should().BeTrue();
        }
    }
}