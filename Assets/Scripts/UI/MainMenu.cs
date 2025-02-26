using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Resume()
    {
        SaveSystem.Resume();
    }
    public void NewGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadGame()
    {
        SaveSystem.Resume();
    }

    public void Settings()
    {
        SceneManager.LoadScene("World");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
