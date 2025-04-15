using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;

public class LoadMenu : MonoBehaviour
{
    private List<FoundFile> foundFiles = new();
    public GameObject saveTile;
    public GameObject loadingScreen;
    void Awake()
    {
        Transform container = GameObject.Find("SaveFiles").GetComponent<RectTransform>().transform;
        foreach (string file in Directory.GetFiles(SaveSystem.SaveFolder).OrderByDescending(x => Directory.GetLastWriteTime(x)))
        {
            foundFiles.Add(new(file, Directory.GetLastWriteTime(file).ToString()));
            Instantiate(saveTile, container, false).GetComponentInChildren<TextMeshProUGUI>().text = foundFiles[^1].Name;
        }
    }
    public void Start()
    {
        loadingScreen = Tools.FindGameObjectByName("Loading");
    }
    public void LoadGame()
    {
        GameObject player = Tools.FindGameObjectByName("Player");
        GameObject playerUI = Tools.FindGameObjectByName("PlayerUI");
        if (GameManager.Instance.SelectedSave != null)
        {
            if (player != null) player.SetActive(false);
            if (playerUI != null) playerUI.SetActive(false);
            ActivateLoading();
            SaveSystem.LoadAsync(foundFiles.FirstOrDefault(x => x.Name.Contains(GameManager.Instance.SelectedSave)).Path);
        }
    }
    public void ActivateLoading()
    {
        gameObject.transform.parent.gameObject.SetActive(false);
        loadingScreen.SetActive(true); ;
    }
}

public class FoundFile
{
    public string Path;
    public string Name;

    public FoundFile(string path, string name)
    {
        Path = path;
        Name = name;
    }
}
