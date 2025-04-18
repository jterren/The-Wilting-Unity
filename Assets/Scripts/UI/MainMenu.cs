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
        SaveSystem.Resume();
        ActivateLoading();
    }
    public void NewGame()
    {
        SceneManager.LoadScene("Maze", LoadSceneMode.Single);
        ActivateLoading();
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
