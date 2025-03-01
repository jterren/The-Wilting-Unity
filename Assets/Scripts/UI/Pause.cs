using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;

    bool isPaused;

    public bool GetIsPaused() { return isPaused; }

    public void PauseGame()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0 : 1;
        pauseMenu.SetActive(isPaused);
    }

    public void SaveGame()
    {
        SaveSystem.Save();
        PauseGame();
    }

    public void ExitGame()
    {
        SaveSystem.Save();
        SceneManager.LoadScene(0);
    }

    public void ExitDesktop()
    {
        SaveSystem.Save();
        Application.Quit();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
    }

}
