using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public class WorldSpace : MonoBehaviour
{
    public PlayerStats Player;
    public WorldData world = new();
    public List<Task<GameObject>> instantiationTasks = new();

    private void Awake()
    {
        Player = GameManager.Instance.Player;
        if (GameManager.Instance.WorldSpace == null) GameManager.Instance.WorldSpace = this;
    }

    public async Task LoadWorldObjectsAsync()
    {
        try
        {
            var keys = world.gameObjects.Select(go => go.addressableKey).Distinct().ToList();
            Dictionary<string, GameObject> prefabsDict = await Tools.LoadPrefabsAsync(keys);
            Tools.FinishLoading();
            if (world.gameObjects.Count != 0)
            {
                foreach (GameObjectData obj in world.gameObjects)
                {
                    if (prefabsDict.TryGetValue(obj.addressableKey, out GameObject prefab))
                    {
                        if (!Tools.GetAllObjectsInScene().Any(p => p.name == obj.prefabName))
                        {
                            GameObject temp = Instantiate(prefab, new Vector2(obj.x, obj.y), Quaternion.identity);
                            temp.SetActive(obj.active);
                            GameObject parent = GameObject.Find(obj.parent);
                            if (parent) temp.transform.parent = parent.transform;
                        }
                    }
                    else
                    {
                        Debug.LogWarning($"Prefab not found for key: {obj.addressableKey}");
                    }
                }

            }
        }
        catch (Exception err)
        {
            Debug.LogError(err.ToString());
        }
    }

    public void Save(ref WorldSaveData data)
    {
        data.world = world;
    }

    public void Load(WorldSaveData data)
    {
        world = data.world;
    }
}
[Serializable]
public struct WorldSaveData
{
    public WorldData world;
}

[Serializable]
public class WorldData
{
    public List<GameObjectData> gameObjects = new();
}

[Serializable]
public class GameObjectData
{
    public string prefabName;
    public float x, y;
    public string parent;
    public bool active;
    public string addressableKey;
}