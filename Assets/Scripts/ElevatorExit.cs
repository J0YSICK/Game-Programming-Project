using UnityEngine;

public class ElevatorExit : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<FpsMovement>() == null) return;
        ObjectiveManager.Instance?.TryEscape();
    }
}