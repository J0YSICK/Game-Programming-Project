using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public enum ItemType
{
    Flashlight = 0,
    Apple = 1,
    OxygenTank = 2,
    Keycard = 3,
}

public enum DetectionState{ Hidden, Suspicious, Spotted }

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance { get; private set; }

    [System.Serializable]
    public class SlotUI{
        public Image icon;
        public Image highlight;
        public TMP_Text count;
        public bool alwaysOwned;
    }

    [Header("Health")]
    [SerializeField] private Image[] hearts;
    [SerializeField] private Sprite heartFull;
    [SerializeField] private Sprite heartEmpty;
    [SerializeField] private Image damageFlash;
    [SerializeField] private float flashDuration = 0.5f;
    [SerializeField] private float flashAlpha = 0.35f;

    [Header("Meters")]
    [SerializeField] private MeterBar oxygenBar;
    [SerializeField] private MeterBar staminaBar;
    [SerializeField] private MeterBar batteryBar;

    [Header("Objective")]
    [SerializeField] private TMP_Text floorText;
    [SerializeField] private TMP_Text objectiveText;

    [Header("Interaction prompt")]
    [SerializeField] private GameObject promptRoot;
    [SerializeField] private TMP_Text promptText;

    [Header("First-time tips")]
    [SerializeField] private CanvasGroup tipGroup;
    [SerializeField] private TMP_Text tipText;
    [SerializeField] private float tipDuration = 4f;
    [SerializeField] private float tipFadeTime = 0.5f;

    [Header("Detection eye")]
    [SerializeField] private Image detectionEye;
    [SerializeField] private Color suspiciousColor = Color.white;
    [SerializeField] private Color spottedColor = new Color(0.85f, 0.15f, 0.15f);

    [Header("Inventory")]
    [SerializeField] private SlotUI[] slots;
    [SerializeField, Range(0f, 1f)] private float emptyIconAlpha = 0.35f;
    [SerializeField] private Color highlightIdle = new Color32(0x80, 0x80, 0x80, 0xFF);
    [SerializeField] private Color highlightEquipped = new Color32(0xE0, 0x8C, 0x21, 0xFF);

    

    private HashSet<string> shownTips = new HashSet<string>();

    private Coroutine flashRoutine;
    private Coroutine tipRoutine;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;

        HidePrompt();
        SetDetection(DetectionState.Hidden);
        SetAlpha(damageFlash, 0f);
        SetEquipped(ItemType.Flashlight);
        if (tipGroup != null)
        {
            tipGroup.alpha = 0f;
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    public void SetHearts(int count){
        for(int i = 0; i < hearts.Length; i++)
        {
            hearts[i].sprite = (i < count) ? heartFull : heartEmpty;
        }
    }

    public void FlashDamage()
    {
        if (flashRoutine != null)
        {
            StopCoroutine(flashRoutine);
        }
        flashRoutine = StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        float t = 0f;
        while (t < flashDuration)
        {
            t += Time.deltaTime;
            SetAlpha(damageFlash, Mathf.Lerp(flashAlpha, 0f, t / flashDuration));
            yield return null;
        }
        SetAlpha(damageFlash, 0f);
    }

    // Meters
    public void SetOxygen(float current, float max)  => oxygenBar.SetValue(current, max);
    public void SetStamina(float current, float max) => staminaBar.SetValue(current, max);
    public void SetBattery(float current, float max) => batteryBar.SetValue(current, max);

    // Objective
    public void SetFloor(int floor) => floorText.text = $"Floor {floor}";
    public void SetObjective(string objective) => objectiveText.text = objective;

    // Interaction prompt
    public void ShowPrompt(string prompt)
    {
        if (promptText.text != prompt)
        {
            promptText.text = prompt;
        }

        if (!promptRoot.activeSelf)
        {
            promptRoot.SetActive(true);
        }
    }

    public void HidePrompt()
    {
        if (promptRoot.activeSelf)
        {
            promptRoot.SetActive(false);
        }
    }

    // Tips
    public void ShowTip(string message)
    {
        if (tipRoutine != null)
        {
            StopCoroutine(tipRoutine);
        }
        tipRoutine = StartCoroutine(ShowTipRoutine(message));
    }

    public void ShowTipOnce(string id, string message)
    {
        if (!shownTips.Add(id)) return;
        ShowTip(message);
    }

    private IEnumerator ShowTipRoutine(string message){
        tipText.text = message;
        yield return Fade(tipGroup, 1f);
        yield return new WaitForSeconds(tipDuration);
        yield return Fade(tipGroup, 0f);
    }

    private IEnumerator Fade(CanvasGroup group, float targetAlpha){
        float startAlpha = group.alpha;
        float t = 0f;
        while(t < tipFadeTime){
            t += Time.deltaTime;
            group.alpha = Mathf.Lerp(startAlpha, targetAlpha, t / tipFadeTime);
            yield return null;
        }
        group.alpha = targetAlpha;
    }

    // Detection eye
    public void SetDetection(DetectionState state){
        detectionEye.enabled = state != DetectionState.Hidden;
        if(state == DetectionState.Suspicious){
            detectionEye.color = suspiciousColor;
        }
        if(state == DetectionState.Spotted){
            detectionEye.color = spottedColor;
        }
    }

    // Inventory
    public void SetItemCount(ItemType item, int count){
    int index = (int)item;
    if (index < 0 || index >= slots.Length)
    {
        Debug.LogError($"No HUD slot for {item} (index {index}). Slots size is {slots.Length}");
        return;
    }

    SlotUI slot = slots[index];

    if (slot.count == null)
        Debug.LogWarning($"Slot {index} ({item}) has no Count text assigned");
    else
    {
        slot.count.text = count.ToString();
        Debug.Log($"HUD: set {item} count text to {count} on object '{slot.count.gameObject.name}' (parent '{slot.count.transform.parent.name}')");
    }

    SetAlpha(slot.icon, count > 0 ? 1f : emptyIconAlpha);
}

    public void SetEquipped(ItemType item)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            bool isEquipped = i == (int)item;

            if (slots[i].highlight != null)
            {
                slots[i].highlight.enabled = true;  
                slots[i].highlight.color = isEquipped ? highlightEquipped : highlightIdle;
            }

        }
    }


    //Helper
    private void SetAlpha(Image image, float alpha)
    {
        if (image == null) return;
        Color c = image.color;
        c.a = alpha;
        image.color = c;
    }



}
