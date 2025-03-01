using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadMenu : MonoBehaviour
{
    private string[] FoundFiles;
    public GameObject SaveTile;
    public void LoadGame()
    {
        if (GameManager.Instance.SelectedSave != null) SaveSystem.LoadAsync(FoundFiles.FirstOrDefault(x => x.Contains(GameManager.Instance.SelectedSave)));
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
}
