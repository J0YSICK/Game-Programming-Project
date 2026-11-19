using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractor : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;      // leave empty to use Camera.main
    [SerializeField] private float interactRange = 3f;
    [SerializeField] private LayerMask interactMask = ~0;

    private Inventory inventory;
    private Item currentItem;

    private void Awake()
    {
        inventory = GetComponent<Inventory>();
        if (playerCamera == null) playerCamera = Camera.main;
    }

    private void Update()
    {
        LookForItem();

        if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            TryPickup();
        }
    }

    private void LookForItem()
    {
        Item looked = null;

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, interactRange, interactMask,
                            QueryTriggerInteraction.Ignore))
        {
            looked = hit.collider.GetComponentInParent<Item>();
        }

        if (looked == currentItem) return;   // nothing changed

        currentItem = looked;
        if (currentItem != null) HUDManager.Instance?.ShowPrompt(currentItem.PromptText);
        else                     HUDManager.Instance?.HidePrompt();
    }

    private void TryPickup()
    {
        Debug.Log("E pressed, currentItem = " + currentItem);
        if (currentItem == null) return;

        if (inventory == null)
        {
            Debug.LogError("Player has no Inventory component!");
            return;
        }

        currentItem.Pickup(inventory);
        currentItem = null;                  // item may be destroyed
        HUDManager.Instance?.HidePrompt();
    }
}