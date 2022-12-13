using System.Collections.Generic;
using NUnit.Framework;
using FluentAssertions;
using LiteRPG.PlayerInventory;
using LiteRPG.PlayerInventory.InvItem;
using LiteRPG.PlayerInventory.SubMenus.Craft.Recipes;

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
      bool itemWasSoldSuccessfully = inventory.SellItem(0, 1,1);

      // Assert.
      itemWasSoldSuccessfully.Should().BeTrue();
      inventory.MoneyStats.Money.Should().Be(1);
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
      inventory.RecipesBook.AddRecipe(hammerRecipe.Id);
      
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
      inventory.RecipesBook.AddRecipe(hammerRecipe.Id);
      
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
      inventory.RecipesBook.DataExists(hammerRecipeId).Should().BeTrue();
      //new {recipeItemToCraft = resRecipeInBook.ItemToCraft.Id}.Should().Be(new {recipeItemToCraft = 1});
    }
  }
}