using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject LoadingScreen;
    public void Start()
    {
        LoadingScreen = Tools.FindGameObjectByName("Loading");
    }
    public void Resume()
    {
        ActivateLoading();
        SaveSystem.Resume();
    }
    public void NewGame()
    {
        ActivateLoading();
        SceneManager.LoadScene("Maze");
    }


    public void Settings()
    {
        //TODO
    }

    public void ExitDesktop()
    {
        Application.Quit();
    }

    public void ActivateLoading()
    {
        LoadingScreen.SetActive(true);
        gameObject.transform.root.gameObject.SetActive(false);
    }
}
