using System;
using System.Collections.Generic;
using Xunit;

public class InventoryManagerTest
{
	[Theory]
	[InlineData("Apple", 10, 5)]
	[InlineData("Banana", 20, 10)]
	public void AddItem_ShouldIncreaseQuantity(string itemName, int initialQuantity, int addedQuantity)
	{
		var inventoryManager = new InventoryManager();
		inventoryManager.AddItem(itemName, initialQuantity);
		inventoryManager.AddItem(itemName, addedQuantity);

		var outOfStockItems = inventoryManager.GetOutOfStockItems();
		Assert.DoesNotContain(itemName, outOfStockItems);
	}

	[Theory]
	[InlineData("Banana", 10, 5)]
	[InlineData("Orange", 20, 15)]
	public void RemoveItem_ShouldDecreaseQuantity(string itemName, int initialQuantity, int removedQuantity)
	{
		var inventoryManager = new InventoryManager();
		inventoryManager.AddItem(itemName, initialQuantity);
		inventoryManager.RemoveItem(itemName, removedQuantity);

		var outOfStockItems = inventoryManager.GetOutOfStockItems();
		Assert.DoesNotContain(itemName, outOfStockItems);
	}

	[Theory]
	[InlineData("Orange", 0, "Grapes", 10)]
	[InlineData("Mango", 0, "Pineapple", 5)]
	public void GetOutOfStockItems_ShouldReturnCorrectItems(string outOfStockItem, int outOfStockQuantity, string inStockItem, int inStockQuantity)
	{
		var inventoryManager = new InventoryManager();
		inventoryManager.AddItem(outOfStockItem, outOfStockQuantity);
		inventoryManager.AddItem(inStockItem, inStockQuantity);

		var outOfStockItems = inventoryManager.GetOutOfStockItems();
		Assert.Contains(outOfStockItem, outOfStockItems);
		Assert.DoesNotContain(inStockItem, outOfStockItems);
	}

	[Theory]
	[InlineData("Pineapple", 5, 10)]
	public void RemoveItem_ShouldThrowException_WhenQuantityIsInsufficient(string itemName, int initialQuantity, int removedQuantity)
	{
		var inventoryManager = new InventoryManager();
		inventoryManager.AddItem(itemName, initialQuantity);

		Assert.Throws<InvalidOperationException>(() => inventoryManager.RemoveItem(itemName, removedQuantity));
	}

	[Theory]
	[InlineData("NonExistentItem", 1)]
	public void RemoveItem_ShouldThrowException_WhenItemDoesNotExist(string itemName, int removedQuantity)
	{
		var inventoryManager = new InventoryManager();

		Assert.Throws<InvalidOperationException>(() => inventoryManager.RemoveItem(itemName, removedQuantity));
	}

	[Theory]
	[InlineData(null, 10)]
	public void AddItem_ShouldHandleNullItemName(string itemName, int quantity)
	{
		var inventoryManager = new InventoryManager();

		Assert.Throws<ArgumentNullException>(() => inventoryManager.AddItem(itemName, quantity));
	}

	[Theory]
	[InlineData(null, 10)]
	public void RemoveItem_ShouldHandleNullItemName(string itemName, int quantity)
	{
		var inventoryManager = new InventoryManager();

		Assert.Throws<ArgumentNullException>(() => inventoryManager.RemoveItem(itemName, quantity));
	}

	[Theory]
	[InlineData("", 10)]
	public void AddItem_ShouldHandleEmptyItemName(string itemName, int quantity)
	{
		var inventoryManager = new InventoryManager();

		Assert.Throws<ArgumentException>(() => inventoryManager.AddItem(itemName, quantity));
	}

	[Theory]
	[InlineData("", 10)]
	public void RemoveItem_ShouldHandleEmptyItemName(string itemName, int quantity)
	{
		var inventoryManager = new InventoryManager();

		Assert.Throws<ArgumentException>(() => inventoryManager.RemoveItem(itemName, quantity));
	}

	[Theory]
	[InlineData("NegativeItem", -5)]
	public void AddItem_ShouldHandleNegativeQuantity(string itemName, int quantity)
	{
		var inventoryManager = new InventoryManager();

		Assert.Throws<ArgumentOutOfRangeException>(() => inventoryManager.AddItem(itemName, quantity));
	}

	[Theory]
	[InlineData("InStockItem1", 5, "InStockItem2", 10)]
	public void GetOutOfStockItems_ShouldReturnEmptyList_WhenAllItemsInStock(string itemName1, int quantity1, string itemName2, int quantity2)
	{
		var inventoryManager = new InventoryManager();
		inventoryManager.AddItem(itemName1, quantity1);
		inventoryManager.AddItem(itemName2, quantity2);

		var outOfStockItems = inventoryManager.GetOutOfStockItems();
		Assert.Empty(outOfStockItems);
	}

	[Theory]
	[InlineData("Item", 1, 2)]
	public void AddItem_ShouldInitializeAndIncreaseQuantity(string itemName, int initialQuantity, int addedQuantity)
	{
		var inventoryManager = new InventoryManager();
		inventoryManager.AddItem(itemName, initialQuantity);
		inventoryManager.AddItem(itemName, addedQuantity);

		var outOfStockItems = inventoryManager.GetOutOfStockItems();
		Assert.DoesNotContain(itemName, outOfStockItems);
	}
}