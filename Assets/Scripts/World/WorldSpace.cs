using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            if (world.gameObjects.Count != 0)
            {
                foreach (Prefab addressable in world.gameObjects)
                {
                    if (prefabsDict.TryGetValue(addressable.addressableKey, out GameObject prefab))
                    {
                        foreach (Vector2 obj in addressable.instances)
                        {
                            if (!Tools.GetAllObjectsInScene().Any(p => p.name == addressable.addressableKey))
                            {
                                GameObject temp = Instantiate(prefab, new Vector2(obj.x, obj.y), Quaternion.identity);
                                temp.SetActive(addressable.active);
                                GameObject parent = GameObject.Find(addressable.parent);
                                if (parent) temp.transform.parent = parent.transform;
                            }

                        }
                    }
                    else
                    {
                        Debug.LogWarning($"Prefab not found for key: {addressable.addressableKey}");
                    }
                }

            }
            Tools.FinishLoading();
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
    public List<Prefab> gameObjects = new();
}


[Serializable]
public class Prefab
{
    public string addressableKey;
    public string parent;
    public bool active;
    public List<Vector2> instances = new();
}