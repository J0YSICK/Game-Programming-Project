using System.Collections;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{
    public static ObjectiveManager Instance { get; private set; }

    [SerializeField] private int applesRequired = 3;
    [SerializeField] private float completeDelay = 2f;   // how long the checked objective stays up

    private int applesCollected;

    public bool ApplesDone => applesCollected >= applesRequired;
    public bool Escaped { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    private void Start()
    {
        HUDManager.Instance?.SetObjective(ApplesText());
    }

    // Called by Inventory whenever an item is successfully picked up
    public void OnItemCollected(ItemType type, int amount)
    {
        if (type != ItemType.Apple || ApplesDone) return;

        applesCollected = Mathf.Min(applesCollected + amount, applesRequired);

        if (ApplesDone)
            StartCoroutine(ApplesCompleteRoutine());
        else
            HUDManager.Instance?.SetObjective(ApplesText());
    }

    private IEnumerator ApplesCompleteRoutine()
    {
        // Show the finished objective with a check, then move on
        HUDManager.Instance?.SetObjective($"<color=#4CFF4C>[x] {ApplesText()}</color>");
        yield return new WaitForSeconds(completeDelay);
        HUDManager.Instance?.SetObjective("Escape through the elevator");
    }

    // Called by the elevator trigger
    public void TryEscape()
    {
        if (Escaped) return;

        if (!ApplesDone)
        {
            HUDManager.Instance?.ShowTip($"Find all {applesRequired} apples first.");
            return;
        }

        Escaped = true;
        HUDManager.Instance?.SetObjective("<color=#4CFF4C>[x] Escape through the elevator</color>");
        HUDManager.Instance?.ShowTip("You escaped!");
        Debug.Log("Player escaped - level complete!");
        // TODO: load next scene / win screen, e.g. SceneManager.LoadScene("WinScene");
    }

    private string ApplesText() => $"Find apples: {applesCollected}/{applesRequired}";
}