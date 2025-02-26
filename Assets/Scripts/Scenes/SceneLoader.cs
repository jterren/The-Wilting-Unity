using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private SceneDataSO[] _sceneDataSOs;
    private Dictionary<string, int> _sceneDataSOsIndexMap = new();

    private void Awake()
    {
        GameManager.Instance.SceneLoader = this;

        PopulateSceneMappings();
    }

    private void PopulateSceneMappings()
    {
        foreach (SceneDataSO sceneDataSO in _sceneDataSOs)
        {
            _sceneDataSOsIndexMap[sceneDataSO.UniqueName] = sceneDataSO.SceneIndex;
        }
    }

    public void LoadSceneByIndex(string sceneId)
    {
        if (_sceneDataSOsIndexMap.TryGetValue(sceneId, out int index))
        {
            SceneManager.LoadScene(index);
        }
        else
        {
            Debug.LogError("No scene found: " + sceneId);
        }
    }

    public async Task LoadSceneByIndexAsync(string sceneId)
    {
        if (_sceneDataSOsIndexMap.TryGetValue(sceneId, out int index))
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(index);
            asyncLoad.allowSceneActivation = false;

            while (!asyncLoad.isDone)
            {
                if (asyncLoad.progress >= 0.9f)
                {
                    asyncLoad.allowSceneActivation = true;
                    break;
                }
                await Task.Yield();
            }

        }
        else
        {
            Debug.LogError("No scene found: " + sceneId);
        }
    }
}
