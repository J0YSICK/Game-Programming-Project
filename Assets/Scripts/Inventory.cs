using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private int capacity = 5;   // total items carried

    private readonly Dictionary<ItemType, int> counts = new Dictionary<ItemType, int>();
    private int total;

    private void Start()
    {
        // Initialise every HUD slot (dims the empty ones)
        foreach (ItemType type in System.Enum.GetValues(typeof(ItemType)))
        {
            counts[type] = 0;
            HUDManager.Instance?.SetItemCount(type, 0);
        }
    }

    public bool AddItem(ItemType type, int amount = 1)
{
    if (total + amount > capacity)
    {
        HUDManager.Instance?.ShowTipOnce("inv_full", "Inventory is full!");
        return false;
    }

    counts[type] += amount;
    total += amount;
    Debug.Log($"Added {type}. Count is now {counts[type]}, total {total}");
    HUDManager.Instance?.SetItemCount(type, counts[type]);
    ObjectiveManager.Instance?.OnItemCollected(type, amount);
    return true;
}

    public bool UseItem(ItemType type)
    {
        if (!counts.TryGetValue(type, out int c) || c <= 0) return false;
        counts[type]--;
        total--;
        HUDManager.Instance?.SetItemCount(type, counts[type]);
        return true;
    }

    public int GetCount(ItemType type) => counts.TryGetValue(type, out int c) ? c : 0;
}