using FluentAssertions;
using LiteRPG.PlayerInventory;
using LiteRPG.PlayerInventory.InvItem;
using NUnit.Framework;
using Packages.LiteRPG.Runtime.LiteRPG.Stats;
using Packages.LiteRPG.Runtime.LiteRPG.Stats.StatsSystem;

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
            var slotTypeToEquip = EquipSlotType.S0;

            // Act.
      
            // Assert.
            inventory.EquippedSlots.IsBackpackSlotEquipped(inventory.Backpack.GetSlot(0)).Should().BeFalse();
        }

        [Test]
        public void WhenSlot0IsEmpty_AndWeTryToEquipIt_ThenSlot0StillShouldBeNonEquipped()
        {
            // Arrange.
            Inventory inventory = Create.InventoryWithCharStatsAndItemsDb(MAX_ITEMS_SLOTS);
            inventory.SetupEquipSlots(MAX_EQUIP_SLOTS);
            var slotTypeToEquip = EquipSlotType.S0;

            // Act.
            var emptyInvSlot = inventory.Backpack.GetSlot(0);
            inventory.EquippedSlots.TryEquipSlot(slotTypeToEquip, emptyInvSlot);
      
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
            inventory.SetupEquipSlots(MAX_EQUIP_SLOTS);
            var slotTypeToEquip = EquipSlotType.S0;
            InvItemData swordItem = Create.LoadInvItem(GameDesign.Items.SwordNonStackable);

            // Act.
            inventory.AddItem(swordItem);
            inventory.EquippedSlots.TryEquipSlot(slotTypeToEquip, inventory.Backpack.GetSlot(0));
      
            // Assert.
            //inventory.Backpack.GetSlot(0).IsEquipped.Should().BeTrue();
            var slotWasFound = inventory.EquippedSlots.TryGetSlotByType(slotTypeToEquip, out var equippedSlot);
            slotWasFound.Should().BeTrue();
            equippedSlot.IsEquipped.Should().BeTrue();
        }
        
        [Test]
        public void WhenSwordItemWasEquippedOnSlotS0_AndWasUnequiped_ThenS0ShouldNotBeEquipped()
        {
            // Arrange.
            Inventory inventory = Create.InventoryWithCharStatsAndItemsDb(MAX_ITEMS_SLOTS);
            inventory.SetupEquipSlots(MAX_EQUIP_SLOTS);
            var slotTypeToEquip = EquipSlotType.S0;
            InvItemData swordItem = Create.LoadInvItem(GameDesign.Items.SwordNonStackable);

            // Act.
            inventory.AddItem(swordItem);
            inventory.EquippedSlots.TryEquipSlot(slotTypeToEquip, inventory.Backpack.GetSlot(0));
            inventory.EquippedSlots.TryUnequipSlot(EquipSlotType.S0);
      
            // Assert.
            //inventory.Backpack.GetSlot(0).IsEquipped.Should().BeTrue();
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
            inventory.SetupEquipSlots(MAX_EQUIP_SLOTS);
            var slotTypeToEquip = EquipSlotType.S0;
            InvItemData swordItem1Atk = Create.LoadInvItem(GameDesign.Items.SwordNonStackable);
            BattleCharStats battleCharStats = Create.FullBattleCharStatsWith1AtkAnd10Hp();

            // Act.
            inventory.AddItem(swordItem1Atk);
            inventory.EquippedSlots.TryEquipSlot(slotTypeToEquip, inventory.Backpack.GetSlot(0));
            battleCharStats.AddModifierFromObject(swordItem1Atk.StatModifiers[0], swordItem1Atk);
      
            // Assert.
            battleCharStats.GetStat(ATK_STAT_ID).Value.Should().Be(2);
        }
        
        [Test]
        public void WhenSwordWith1BaseAttackWasAdded_AndWasEquippedAndUnequippedOnSlotS0_ThenTotalBaseAttackShouldBe1()
        {
            // Arrange.
            Inventory inventory = Create.InventoryWithCharStatsAndItemsDb(MAX_ITEMS_SLOTS);
            inventory.SetupEquipSlots(MAX_EQUIP_SLOTS);
            var slotTypeToEquip = EquipSlotType.S0;
            InvItemData swordItem1Atk = Create.LoadInvItem(GameDesign.Items.SwordNonStackable);
            BattleCharStats battleCharStats = Create.FullBattleCharStatsWith1AtkAnd10Hp();

            // Act.
            inventory.AddItem(swordItem1Atk);
            inventory.EquippedSlots.TryEquipSlot(slotTypeToEquip, inventory.Backpack.GetSlot(0));
            battleCharStats.AddModifierFromObject(swordItem1Atk.StatModifiers[0], swordItem1Atk);
            battleCharStats.RemoveAllModifiersFromObject(swordItem1Atk);
      
            // Assert.
            battleCharStats.GetStat(ATK_STAT_ID).Value.Should().Be(1);
        }
        
        [Test]
        public void WhenSwordWith1AtkAnd1MaxHp_AndWasEquippedOnSlotS0_ThenShouldBe2Atk11MaxHp()
        {
            // Arrange.
            Inventory inventory = Create.InventoryWithCharStatsAndItemsDb(MAX_ITEMS_SLOTS);
            inventory.SetupEquipSlots(MAX_EQUIP_SLOTS);
            var slotTypeToEquip = EquipSlotType.S0;
            InvItemData swordItem1Atk1MaxHp = Create.LoadInvItem(GameDesign.Items.SwordNonStackable);
            BattleCharStats battleCharStats = Create.FullBattleCharStatsWith1AtkAnd10Hp();

            // Act.
            inventory.AddItem(swordItem1Atk1MaxHp);
            inventory.EquippedSlots.TryEquipSlot(slotTypeToEquip, inventory.Backpack.GetSlot(0));
            battleCharStats.AddModifierFromObject(swordItem1Atk1MaxHp.StatModifiers, swordItem1Atk1MaxHp);
      
            // Assert.
            battleCharStats.GetStat(ATK_STAT_ID).Value.Should().Be(2);
            battleCharStats.GetStat(HP_STAT_ID).Value.Should().Be(11);
        }
        
        [Test]
        public void WhenSwordWith1AtkAnd1MaxHp_AndWasEquippedAndRemovedOnSlotS0_ThenShouldBe1Atk10MaxHp()
        {
            // Arrange.
            Inventory inventory = Create.InventoryWithCharStatsAndItemsDb(MAX_ITEMS_SLOTS);
            inventory.SetupEquipSlots(MAX_EQUIP_SLOTS);
            var slotTypeToEquip = EquipSlotType.S0;
            InvItemData swordItem1Atk1MaxHp = Create.LoadInvItem(GameDesign.Items.SwordNonStackable);
            BattleCharStats battleCharStats = Create.FullBattleCharStatsWith1AtkAnd10Hp();

            // Act.
            inventory.AddItem(swordItem1Atk1MaxHp);
            inventory.EquippedSlots.TryEquipSlot(slotTypeToEquip, inventory.Backpack.GetSlot(0));
            battleCharStats.AddModifierFromObject(swordItem1Atk1MaxHp.StatModifiers, swordItem1Atk1MaxHp);
            battleCharStats.RemoveAllModifiersFromObject(swordItem1Atk1MaxHp);
      
            // Assert.
            battleCharStats.GetStat(ATK_STAT_ID).Value.Should().Be(1);
            battleCharStats.GetStat(HP_STAT_ID).Value.Should().Be(10);
        }
    }
}