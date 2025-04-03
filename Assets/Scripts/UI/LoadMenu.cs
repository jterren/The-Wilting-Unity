using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;

public class LoadMenu : MonoBehaviour
{
    private string[] FoundFiles;
    public GameObject SaveTile;
    public GameObject LoadingScreen;
    public void Start()
    {
        LoadingScreen = Tools.FindGameObjectByName("Loading");
    }
    public void LoadGame()
    {
        GameObject player = Tools.FindGameObjectByName("Player");
        if (GameManager.Instance.SelectedSave != null)
        {
            if (player != null) ; player.SetActive(false);
            ActivateLoading();
            SaveSystem.LoadAsync(FoundFiles.FirstOrDefault(x => x.Contains(GameManager.Instance.SelectedSave)));
        }
    }

    void Awake()
    {
        FoundFiles = Directory.GetFiles(SaveSystem.SaveFolder);
        Transform container = GameObject.Find("SaveFiles").GetComponent<RectTransform>().transform;
        foreach (string file in FoundFiles)
        {
            Instantiate(SaveTile, container, false).GetComponentInChildren<TextMeshProUGUI>().text = file.Split('/')[^1].Replace(".sav", "");
        }
    }

    public void ActivateLoading()
    {
        gameObject.transform.parent.gameObject.SetActive(false);
        LoadingScreen.SetActive(true); ;
    }
}
