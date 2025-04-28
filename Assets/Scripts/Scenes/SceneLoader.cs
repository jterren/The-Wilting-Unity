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

    public async Task LoadSceneByIndexAsync(int index)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(index, LoadSceneMode.Single);
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
}
