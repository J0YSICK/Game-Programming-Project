using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    // A list to store the names of items collected
    public List<string> items = new List<string>();

    // Maximum capacity of the inventory
    public int capacity = 5;

    // adding item to the inventory if space is available
    public bool AddItem(string itemName)
    {
        if (items.Count < capacity)
        {
            items.Add(itemName);
            Debug.Log($"Picked up: {itemName}. Inventory count: {items.Count}/{capacity}");
            return true;
        }
        else
        {
            Debug.Log("Inventory is full!");
            return false;
        }
    }
}