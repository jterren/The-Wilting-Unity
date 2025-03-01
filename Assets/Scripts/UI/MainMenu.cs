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

    public void Settings()
    {
        //TODO
    }

    public void ExitDesktop()
    {
        Application.Quit();
    }
}
