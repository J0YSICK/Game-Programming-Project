using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class TorchLight : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private InputActionReference toggleAction;
    [SerializeField] private Light torchLight;
    [SerializeField] private float battery = 100f;
    [SerializeField] private float batteryDepletionSpeed = 0.7f;
    private bool on;

    [Header("UI")]
    [SerializeField] private TMP_Text batteryText;


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
            on = false;
        }

        batteryText.text = $"Battery: {(int)battery}%";

        torchLight.enabled = on;
    }

}