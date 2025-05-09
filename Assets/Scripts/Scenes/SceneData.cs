using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneData : MonoBehaviour
{
    public SceneDataSO Data;

    private void Awake()
    {
        GameManager.Instance.Scene = this;
    }

    public void Save(ref SceneSaveData data)
    {
        data.SceneId = Data.UniqueName;
    }

    public void Load(SceneSaveData data)
    {
        GameManager.Instance.SceneLoader.LoadSceneByIndex(data.SceneId);
    }

    public async Task LoadAsync(SceneSaveData data)
    {
        await GameManager.Instance.SceneLoader.LoadSceneByIndexAsync(data.SceneId);
    }

    public Task WaitForSceneLoad()
    {
        TaskCompletionSource<bool> taskCompletion = new TaskCompletionSource<bool>();
        UnityEngine.Events.UnityAction<Scene, LoadSceneMode> sceneLoaderHandler = null;

        sceneLoaderHandler = (scene, mode) =>
        {
            taskCompletion.SetResult(true);
            SceneManager.sceneLoaded -= sceneLoaderHandler;
        };

        SceneManager.sceneLoaded += sceneLoaderHandler;

        return taskCompletion.Task;
    }
}
[System.Serializable]
public struct SceneSaveData
{
    public string SceneId;
}