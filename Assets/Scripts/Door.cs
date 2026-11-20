using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private ItemType requiredItem = ItemType.Keycard;
    [SerializeField] private bool consumeKey = false;   // true = key is used up
    [SerializeField] private string lockedMessage = "You need a key to open this door.";

    public string PromptText => "[E] Open door";

    public void Interact(Inventory inventory)
    {
        if (inventory != null && inventory.GetCount(requiredItem) > 0)
        {
            if (consumeKey) inventory.UseItem(requiredItem);
            Destroy(gameObject);
        }
        else
        {
            HUDManager.Instance?.ShowTip(lockedMessage);
        }
    }
}