using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Resume()
    {
        SaveSystem.Resume();
        this.gameObject.SetActive(false);
    }
    public void NewGame()
    {
        SceneManager.LoadScene(2);
        this.gameObject.SetActive(false);
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
