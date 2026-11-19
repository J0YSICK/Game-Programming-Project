using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] public ItemType itemType;
    [SerializeField] public string displayName;
    [SerializeField] private int amount = 1;

    public string PromptText => $"[E] Pick up {displayName}";

    // TODO: add a sound when picking up an item.
    public void Pickup(Inventory inventory)
    {
        if (inventory.AddItem(itemType, amount))
        {
            Destroy(gameObject);
        }
    }
}