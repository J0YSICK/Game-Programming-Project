using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractor : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float interactRange = 3f;
    [SerializeField] private LayerMask interactMask = ~0;

    private Inventory inventory;
    private Item currentItem;
    private Door currentDoor;

    private void Awake()
    {
        inventory = GetComponent<Inventory>();
        if (playerCamera == null) playerCamera = Camera.main;
    }

    private void Update()
    {
        LookForTarget();

        if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            TryInteract();
        }
    }

    private void LookForTarget()
    {
        currentItem = null;
        currentDoor = null;

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, interactRange, interactMask,
                            QueryTriggerInteraction.Ignore))
        {
            currentItem = hit.collider.GetComponentInParent<Item>();
            if (currentItem == null)
                currentDoor = hit.collider.GetComponentInParent<Door>();
        }

        // ShowPrompt/HidePrompt already skip redundant updates,
        // so this also refreshes "Open"/"Close" text when the door state changes.
        if (currentItem != null)      HUDManager.Instance?.ShowPrompt(currentItem.PromptText);
        else if (currentDoor != null) HUDManager.Instance?.ShowPrompt(currentDoor.PromptText);
        else                          HUDManager.Instance?.HidePrompt();
    }

    private void TryInteract()
    {
        if (currentItem != null)
        {
            if (inventory == null) { Debug.LogError("Player has no Inventory component!"); return; }
            currentItem.Pickup(inventory);
            currentItem = null;
            HUDManager.Instance?.HidePrompt();
        }
        else if (currentDoor != null)
        {
            currentDoor.Interact(inventory);
        }
    }
}