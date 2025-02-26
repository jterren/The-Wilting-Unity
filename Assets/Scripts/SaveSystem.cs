using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;
using System;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor;

public static class SaveSystem
{
    private static SaveData _saveData = new();
    private static readonly string SaveFolder = $"{Application.persistentDataPath}/saves/";

    [System.Serializable]

    public struct SaveData
    {
        public PlayerSaveData PlayerData;
        public SceneSaveData SceneSaveData;
    }
    public static string SaveFileName()
    {
        return $"{SaveFolder}{DateTime.UtcNow.ToString("yyyy-dd-M--HH-mm-ss")}.sav";

    }

    private static void HandleSaveData()
    {
        GameManager.Instance.Player.SavePlayerData(ref _saveData.PlayerData);
        GameManager.Instance.Scene.Save(ref _saveData.SceneSaveData);
    }

    public static void Save()
    {
        HandleSaveData();
        File.WriteAllText(SaveFileName(), JsonUtility.ToJson(_saveData, true));
    }

    public static async Task AsynchronouslySave()
    {
        await SaveAsync();
    }

    private static async Task SaveAsync()
    {
        HandleSaveData();
        await File.WriteAllTextAsync(SaveFileName(), JsonUtility.ToJson(_saveData, true));
    }

    private static void HandleLoadData()
    {
        GameManager.Instance.Scene.Load(_saveData.SceneSaveData);
        GameManager.Instance.Player.LoadPlayerData(_saveData.PlayerData);
    }


    private static async Task HandleLoadDataAsync()
    {
        await GameManager.Instance.Scene.LoadAsync(_saveData.SceneSaveData);
        await GameManager.Instance.Scene.WaitForSceneLoad();
        GameManager.Instance.Player.LoadPlayerData(_saveData.PlayerData);
    }

    public static void Load(string filePath)
    {
        string saveData = File.ReadAllText(filePath);
        _saveData = JsonUtility.FromJson<SaveData>(saveData);

        HandleLoadData();
    }

    public static async Task LoadAsync(string filePath)
    {
        string saveData = File.ReadAllText(filePath);
        _saveData = JsonUtility.FromJson<SaveData>(saveData);

        await HandleLoadDataAsync();
    }

    public static async Task Resume()
    {
        string saveData = File.ReadAllText(GetRecentSave().FullName);
        _saveData = JsonUtility.FromJson<SaveData>(saveData);

        await HandleLoadDataAsync();
    }

    public static FileInfo GetRecentSave()
    {
        return new DirectoryInfo(SaveFolder)
            .GetFiles("*.sav") // Get all .sav files
            .OrderByDescending(f => f.LastWriteTime) // Sort by last modified time (most recent first)
            .FirstOrDefault();
    }

}
