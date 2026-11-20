using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] public ItemType itemType;
    [SerializeField] public string displayName;
    [SerializeField] private int amount = 1;

    [Header("Audio")]
    [SerializeField] private AudioClip pickupSound;
    [SerializeField, Range(0f, 1f)] private float pickupVolume = 1f;

    public string PromptText => $"[E] Pick up {displayName}";

    public void Pickup(Inventory inventory)
    {
        if (inventory.AddItem(itemType, amount))
        {
            if (pickupSound != null)
                AudioSource.PlayClipAtPoint(pickupSound, transform.position, pickupVolume);

            Destroy(gameObject);
        }
    }
}