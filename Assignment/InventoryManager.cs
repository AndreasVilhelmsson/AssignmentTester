using System;
using System.Collections.Generic;

public class InventoryManager
{
	private Dictionary<string, int> inventory;

	public InventoryManager()
	{
		inventory = new Dictionary<string, int>();
	}

	// Metod för att lägga till en vara i lagret
	public void AddItem(string itemName, int quantity)
	{
		if (inventory.ContainsKey(itemName))
		{
			inventory[itemName] += quantity;
		}
		else
		{
			inventory[itemName] = quantity;
		}
	}

	// Metod för att ta bort en vara från lagret
	public void RemoveItem(string itemName, int quantity)
	{
		if (!inventory.ContainsKey(itemName))
		{
			throw new InvalidOperationException($"Item '{itemName}' does not exist in the inventory.");
		}

		if (inventory[itemName] < quantity)
		{
			throw new InvalidOperationException($"Not enough quantity of item '{itemName}' in the inventory.");
		}

		inventory[itemName] -= quantity;

		// Ta bort varan från lagret om kvantiteten är noll
		if (inventory[itemName] == 0)
		{
			inventory.Remove(itemName);
		}
	}

	// Metod för att få en lista på varor som inte finns på lager
	public List<string> GetOutOfStockItems()
	{
		List<string> outOfStockItems = new List<string>();

		foreach (var item in inventory)
		{
			if (item.Value == 0)
			{
				outOfStockItems.Add(item.Key);
			}
		}

		return outOfStockItems;
	}
}