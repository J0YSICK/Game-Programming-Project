using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;

public class TorchLight : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private InputActionReference toggleAction;
    [SerializeField] private Light torchLight;
    [SerializeField] private float battery = 100f;
    [SerializeField] private float batteryDepletionSpeed = 0.7f;
    [SerializeField] private float maxBattery = 100f;
    private bool on;

    [Header("UI")]
    [SerializeField] private TMP_Text batteryText;
    [SerializeField] private Image batteryBar;
    private float maxBarWidth;

    void Start(){
        maxBarWidth = batteryBar.rectTransform.sizeDelta.x;
    }

    private void OnEnable()
    {
        toggleAction.action.performed += OnToggle;
    }

    private void OnDisable()
    {
        toggleAction.action.performed -= OnToggle;
    }

    private void OnToggle (InputAction.CallbackContext ctx){
        //torchLight.enabled = !torchLight.enabled;
        // only allow flashlight to turn on when battery is still alive
        if (battery > 0f){
            on = !on;
        } else {
            on = false;
        }
    }

    private void Update(){
        if (on && battery > 0f){
            battery -= batteryDepletionSpeed * Time.deltaTime;
        }

        if (battery <= 0f){
            battery = 0f;   // prevents negative numbers (e.g. -1%)
            on = false;
        }

        if (batteryBar != null){
            // Shrinks width proportionally from full width down to 0
            float newWidth = maxBarWidth * (battery / maxBattery);
            // changes battery bars 'Width' value
            batteryBar.rectTransform.sizeDelta = new Vector2(newWidth, batteryBar.rectTransform.sizeDelta.y);
        }

        batteryText.text = $"Battery: {(int)battery}%";

        torchLight.enabled = on;
    }

}