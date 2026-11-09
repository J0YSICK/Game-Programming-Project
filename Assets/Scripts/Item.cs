using UnityEngine;

public class Item : MonoBehaviour
{
    public string itemName;

    private bool playerInRange = false;
    private Inventory playerInventory;

    private void Update()
    {
        // player item pickup
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (playerInventory != null)
            {
                bool successfullyAdded = playerInventory.AddItem(itemName);

                if (successfullyAdded)
                {
                    Destroy(gameObject);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInventory = other.GetComponent<Inventory>();
            if (playerInventory != null)
            {
                playerInRange = true;
                Debug.Log($"Press 'E' to pick up {itemName}");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            playerInventory = null;
        }
    }
}