using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadScene(string sceneName){
        Time.timeScale = 1f;
        AudioListener.pause = false;
        SceneManager.LoadScene(sceneName);

    }

    public void QuitGame(){
        Debug.Log("Quit Game");
        Application.Quit();
    }
    
}


