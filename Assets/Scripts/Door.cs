using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private ItemType requiredItem = ItemType.Keycard;
    [SerializeField] private bool consumeKey = false;   // true = key is used up
    [SerializeField] private string lockedMessage = "You need a key to open this door.";

    [Header("Audio")]
    [SerializeField] private AudioClip openSound;
    [SerializeField, Range(0f, 1f)] private float volume = 1f;

    public string PromptText => "[E] Open door";

    public void Interact(Inventory inventory)
    {
        if (inventory != null && inventory.GetCount(requiredItem) > 0)
        {
            if (consumeKey) inventory.UseItem(requiredItem);

            if (openSound != null)
                AudioSource.PlayClipAtPoint(openSound, transform.position, volume);

            Destroy(gameObject);
        }
        else
        {
            HUDManager.Instance?.ShowTip(lockedMessage);
        }
    }
}