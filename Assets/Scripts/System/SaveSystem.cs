using UnityEngine;
using System.IO;
using System;
using System.Linq;
using System.Threading.Tasks;

public static class SaveSystem
{
    private static SaveData _saveData = new();
    public static readonly string SaveFolder = $"{Application.persistentDataPath}/saves/";

    [Serializable]

    public struct SaveData
    {
        public PlayerSaveData PlayerData;
        public SceneSaveData SceneData;
        public WorldSaveData WorldData;
    }
    public static string SaveFileName()
    {
        Directory.CreateDirectory(SaveFolder);
        return $"{SaveFolder}{DateTime.UtcNow:yyyy-dd-M--HH-mm-ss}.sav";
    }

    private static void HandleSaveData()
    {
        GameManager.Instance.Scene.Save(ref _saveData.SceneData);
        GameManager.Instance.WorldSpace.Save(ref _saveData.WorldData);
        GameManager.Instance.Player.Save(ref _saveData.PlayerData);
    }

    public static void Save()
    {
        HandleSaveData();
        if (!Directory.Exists(SaveFolder)) Directory.CreateDirectory(SaveFolder);
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

    private static async Task HandleLoadDataAsync()
    {
        await GameManager.Instance.Scene.LoadAsync(_saveData.SceneData);
        await GameManager.Instance.Scene.WaitForSceneLoad();
        GameManager.Instance.Player.Load(_saveData.PlayerData);
        GameManager.Instance.WorldSpace.Load(_saveData.WorldData);
        await GameManager.Instance.WorldSpace.LoadWorldObjectsAsync();
        Time.timeScale = 1;
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
        try
        {
            return new DirectoryInfo(SaveFolder)
                .GetFiles("*.sav") // Get all .sav files
                .OrderByDescending(f => f.LastWriteTime) // Sort by last modified time (most recent first)
                .FirstOrDefault();
        }
        catch (Exception err)
        {
            return null;
        }
    }

}
