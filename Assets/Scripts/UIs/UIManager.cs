using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Panels")]
    [SerializeField] private GameObject pausePanel;

    [Header("Player scripts to switch off while a menu is open")]
    [SerializeField] private MonoBehaviour[] disableWhileInMenu;

    public bool IsPaused { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            pausePanel.SetActive(false);
        }
    }

    private void Start()
    {
        EnterGameplayMode();
    }

    private void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (IsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Pause(){
        if (IsPaused) return;
        IsPaused = true;
        pausePanel.SetActive(true);
        EnterMenuMode(true);
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    public void Resume(){
        IsPaused = false;
        pausePanel.SetActive(false);
        EnterGameplayMode();
    }

    private void EnterMenuMode(bool pauseAudio){
        
        Time.timeScale = 0f;
        AudioListener.pause = pauseAudio;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SetPlayerScripts(false);
        HUDManager.Instance?.HidePrompt();
    }

    private void EnterGameplayMode(){
        Time.timeScale = 1f;
        AudioListener.pause = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        SetPlayerScripts(true);
    }

    private void SetPlayerScripts(bool enabled){
        foreach (var script in disableWhileInMenu)
        {
            if(script != null){
                script.enabled = enabled;
            }
            
        }
    }



}
