using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject LoadingScreen;
    public void Start()
    {
        LoadingScreen = Tools.FindGameObjectByName("Loading");
        if (SaveSystem.GetRecentSave() == null) Tools.GetChildByName(transform, "ResumeLast").GetComponent<Button>().interactable = false;
    }
    public void Resume()
    {
        SaveSystem.Resume();
    }
    public void NewGame()
    {
        SceneManager.LoadScene("Maze", LoadSceneMode.Single);
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
