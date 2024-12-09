using System.Collections.Generic;
using NUnit.Framework;
using FluentAssertions;
using LiteRPG.Character;
using LiteRPG.PlayerInventory;
using LiteRPG.PlayerInventory.InvItem;
using LiteRPG.PlayerInventory.SubMenus.Craft.Recipes;
using LiteRPG.Progress;
using UnityEngine;

namespace Tests
{
  public class InventoryTests
  {
    [Test]
    public void WhenStartMoneyIs0_AndItemWithPrice1WasSold_ThenMoneyShouldBeEqualTo1()
    {
      // Arrange.
      Inventory inventory = Create.InventoryWithCharStatsAndItemsDb();
      int priceToSell = 1;
      inventory.AddItem(Create.CreateItemSlot("Sword", 0, priceToSell, 1));
      
      // Act.
      bool itemWasSoldSuccessfully = inventory.TrySellItem(0, 1,1);

      // Assert.
      itemWasSoldSuccessfully.Should().BeTrue();
      inventory.MoneyProgress.Money.Should().Be(1);
    }

    [Test]
    public void WhenHaveHammerRecipe_AndHaveEnoughResources_ThenAdd1HammerToInventory()
    {
      // Arrange.
      Inventory inventory = Create.InventoryWithCharStatsAndItemsDb();
      RecipeData hammerRecipe = Create.LoadRecipe(GameDesign.Recipes.Hammer);
      Setup.AddRecipeToBookAndEnoughItemsForCraftingIt(inventory, hammerRecipe);

      // Act.
      inventory.Crafting.CraftItemFromRecipe(hammerRecipe.Id);

      // Assert.
      hammerRecipe.RequiredItems.Should().NotBeEmpty();
      bool oneHammerWasCrafted = inventory.HasItemInSlotsOfQuantity(hammerRecipe.ItemToCraft, 1);
      oneHammerWasCrafted.Should().BeTrue();
    }
    
    [Test]
    public void WhenDontHaveHammerRecipe_AndHaveEnoughResources_ThenShouldNotGetTheHammer()
    {
      // Arrange.
      Inventory inventory = Create.InventoryWithCharStatsAndItemsDb();
      RecipeData hammerRecipe = Create.LoadRecipe(GameDesign.Recipes.Hammer);

      // Act.
      inventory.Crafting.CraftItemFromRecipe(hammerRecipe.Id);

      // Assert.
      hammerRecipe.RequiredItems.Should().NotBeEmpty();
      bool oneHammerWasCrafted = inventory.HasItemInSlotsOfQuantity(hammerRecipe.ItemToCraft, 1);
      oneHammerWasCrafted.Should().BeFalse();
    }

    [Test]
    public void WhenHaveHammerRecipe_AndInventoryIsEmpty_ThenShouldNotGetTheHammer()
    {
      // Arrange.
      Inventory inventory = Create.InventoryWithCharStatsAndItemsDb();
      RecipeData hammerRecipe = Create.LoadRecipe(GameDesign.Recipes.Hammer);
      inventory.Crafting.TryAddRecipe(hammerRecipe.Id);
      
      // Act.
      inventory.Crafting.CraftItemFromRecipe(hammerRecipe.Id);

      // Assert.
      hammerRecipe.RequiredItems.Should().NotBeEmpty();
      inventory.Crafting.EnoughItemsForCrafting(hammerRecipe.Id).Should().BeFalse();
      bool oneHammerWasCrafted = inventory.HasItemInSlotsOfQuantity(hammerRecipe.ItemToCraft, 1);
      oneHammerWasCrafted.Should().BeFalse();
    }
    
    [Test]
    public void WhenHaveHammerRecipe_AndHaveNotEnoughResources_ThenShouldNotGetTheHammer()
    {
      // Arrange.
      Inventory inventory = Create.InventoryWithCharStatsAndItemsDb();
      RecipeData hammerRecipe = Create.LoadRecipe(GameDesign.Recipes.Hammer);
      inventory.Crafting.TryAddRecipe(hammerRecipe.Id);
      
      // Act.
      inventory.Crafting.CraftItemFromRecipe(hammerRecipe.Id);
      inventory.AddItem(Create.LoadInvItem(GameDesign.Items.Wood));

      // Assert.
      hammerRecipe.RequiredItems.Should().NotBeEmpty();
      inventory.Crafting.EnoughItemsForCrafting(hammerRecipe.Id).Should().BeFalse();
      bool oneHammerWasCrafted = inventory.HasItemInSlotsOfQuantity(hammerRecipe.ItemToCraft, 1);
      oneHammerWasCrafted.Should().BeFalse();
    }

    [Test]
    public void WhenInventoryDbWasLoadedFromResources_ThenItShouldNotBeNull()
    {
      // Arrange.
      // Act.
      InvItemsDb itemsDb = Create.LoadInvItemsDbFromResources();
      
      // Assert.
      itemsDb.Should().NotBeNull();
    }
    
    [Test]
    public void WhenGettingGuaranteed1WoodFromLootbox_AndInvIsEmpty_ThenShouldGet1WoodToInventory()
    {
      // Arrange.
      Inventory inventory = Create.InventoryWithCharStatsAndItemsDb();
      
      // Act.
      inventory.AddItem(Create.LoadLootbox(GameDesign.Lootboxes.Wood1LB).GetLoot());
      bool oneWoodWasGetFromLootBox = inventory.HasItemInSlotsOfQuantity(Create.LoadInvItem(GameDesign.Items.Wood), 1);
      
      // Assert.
      oneWoodWasGetFromLootBox.Should().BeTrue();
    }
    
    [Test]
    public void WhenUsingHammerRecipeItem_AndBookHasNoHammerRecipe_ThenBookShouldGetHammerRecipe()
    {
      // Arrange.
      Inventory inventory = Create.InventoryWithCharStatsAndItemsDb();
      InvItemData recipeInvItem = Create.LoadInvItem(GameDesign.Items.HammerRecipe);
      List<InvItemSlot> openedLoot = Create.LoadLootbox(GameDesign.Lootboxes.HammerRecipe).GetLoot();
      int hammerRecipeId = Create.LoadRecipe(GameDesign.Recipes.Hammer).Id;
      
      // Act.
      inventory.AddItem(openedLoot);
      Assert.IsTrue(inventory.HasItemInSlotsOfQuantity(recipeInvItem, 1));
      BackpackSlot slotWithRecipe = inventory.GetSlotWithItem(recipeInvItem);
      bool recipeWasUsedSuccessfully = slotWithRecipe.ItemSlot.ItemData.Use(inventory, null);
      
      // Assert.
      recipeWasUsedSuccessfully.Should().BeTrue();
      inventory.Crafting.IsRecipeAddedToBook(hammerRecipeId).Should().BeTrue();
      //new {recipeItemToCraft = resRecipeInBook.ItemToCraft.Id}.Should().Be(new {recipeItemToCraft = 1});
    }
    
    [Test]
    public void WhenHavingOneHealthPotion_AndOneHealthPotionWasUsed_ThenSlotShouldBeEmpty()
    {
      // Arrange.
      Inventory inventory = Create.InventoryWithCharStatsAndItemsDb();
      InvItemData healthPotion = Create.LoadInvItem(GameDesign.Items.HealthPotion);

      // Act.
      inventory.AddItem(healthPotion);
      Assert.IsTrue(inventory.HasItemInSlotsOfQuantity(healthPotion, 1));
      inventory.Backpack.GetSlot(0).Use();
      
      // Assert.
      inventory.Backpack.GetSlot(0).IsEmpty().Should().BeTrue();
    }
    
    [Test]
    public void WhenHaving2HealthPotion_And1HealthPotionWasUsed_ThenShouldHave1Potion()
    {
      // Arrange.
      Inventory inventory = Create.InventoryWithCharStatsAndItemsDb();
      InvItemData healthPotion = Create.LoadInvItem(GameDesign.Items.HealthPotion);

      // Act.
      inventory.AddItem(healthPotion, 2);
      inventory.HasItemInSlotsOfQuantity(healthPotion, 2).Should().BeTrue();
      inventory.Backpack.GetSlot(0).Use();
      
      // Assert.
      inventory.Backpack.GetSlot(0).IsEmpty().Should().BeFalse();
      inventory.Backpack.GetSlot(0).ItemSlot.Quantity.Should().Be(1);
    }
    
    [Test]
    public void WhenHavingEmptyBackpack_AndAdd2PotionOneAfterAnother_ThenShouldHave2Potion()
    {
      // Arrange.
      Inventory inventory = Create.InventoryWithCharStatsAndItemsDb();
      InvItemData healthPotion = Create.LoadInvItem(GameDesign.Items.HealthPotion);

      // Act.
      inventory.AddItem(healthPotion);
      inventory.AddItem(healthPotion);

      // Assert.
      inventory.HasItemInSlotsOfQuantity(healthPotion, 2).Should().BeTrue();
    }
    
    [Test]
    public void WhenHavingEmptyBackpack_AndAdded1PotionAfterWhichItWasRemoved_ThenShouldHaveEmptySlot()
    {
      // Arrange.
      Inventory inventory = Create.InventoryWithCharStatsAndItemsDb();
      InvItemData healthPotion = Create.LoadInvItem(GameDesign.Items.HealthPotion);

      // Act.
      bool potionWasSuccessfullyAdded = inventory.AddItem(healthPotion);
      inventory.RemoveItems(healthPotion, 1);

      // Assert.
      potionWasSuccessfullyAdded.Should().BeTrue();
      inventory.Backpack.GetSlot(0).IsEmpty().Should().BeTrue();
      //inventory.HasItemInSlotsOfQuantity(healthPotion, 2).Should().BeTrue();
    }

    [Test]
    public void WhenHavingEmptyBackpack_AndAdd2PotionOneAfterAnother_ThenShouldHaveOnlyOneNonEmptySlot()
    {
      // Arrange.
      Inventory inventory = Create.InventoryWithCharStatsAndItemsDb();
      InvItemData healthPotion = Create.LoadInvItem(GameDesign.Items.HealthPotion);

      // Act.
      inventory.AddItem(healthPotion);
      inventory.AddItem(healthPotion);

      // Assert.
      inventory.Backpack.GetNonEmptySlots().Count.Should().Be(1);
    }

    [Test]
    public void WhenHavingEmptyBackpackWith1Slot_AndAdd2PotionOneAfterAnother_ThenShouldHave2PotionInOneSlot()
    {
      // Arrange.
      Inventory inventory = ScriptableObject.CreateInstance<Inventory>();
      AdditiveHp additiveHp = new AdditiveHp();
      InventoryBackpack backpack = new InventoryBackpack(1, inventory, additiveHp);
      IMoneyProgress moneyProgress = new TestGameProgress();
      InvItemsDb itemsDb = Create.LoadInvItemsDbFromResources();
      inventory.Construct(backpack, moneyProgress, itemsDb, Create.LoadRecipesBook());

      //Inventory inventory = Create.InventoryWithCharStatsAndItemsDb();
      //InventoryBackpack backpack = new InventoryBackpack(1, inventory, additiveHp);
      InvItemData healthPotion = Create.LoadInvItem(GameDesign.Items.HealthPotion);

      // Act.
      bool firstItemWasSuccessfullyAdded = inventory.AddItem(healthPotion);
      bool secondItemWasSuccessfullyAdded = inventory.AddItem(healthPotion);
      
      // Assert.
      firstItemWasSuccessfullyAdded.Should().BeTrue();
      secondItemWasSuccessfullyAdded.Should().BeTrue();
      inventory.GetSlotWithItem(healthPotion).ItemSlot.Quantity.Should().Be(2);
      inventory.Backpack.GetAllSlots().Count.Should().Be(1);
    }

  }
}