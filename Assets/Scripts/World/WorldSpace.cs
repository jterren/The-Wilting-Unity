using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class WorldSpace : MonoBehaviour
{
    public PlayerStats Player;
    public WorldData world = new();
    private List<GameObject> prefabs;

    private void Awake()
    {
        Player = GameManager.Instance.Player;
        if (GameManager.Instance.WorldSpace == null) GameManager.Instance.WorldSpace = this;
    }

    private async void Start()
    {
        Dictionary<string, GameObject> prefabsDict = await Tools.LoadPrefabsAsync(world.gameObjects.Select(go => go.addressableKey).Distinct().ToList());

        if (GameManager.Instance.WorldSpace.world.gameObjects.Count != 0)
        {
            foreach (GameObjectData obj in world.gameObjects)
            {
                if (prefabsDict.TryGetValue(obj.prefabName, out GameObject prefab))
                {
                    if (!Tools.GetAllObjectsInScene().Any(p => p.name == obj.prefabName))
                    {
                        GameObject temp = Instantiate(prefab, new Vector2(obj.x, obj.y), Quaternion.identity);
                        temp.SetActive(obj.active);
                        temp.transform.parent = GameObject.Find(obj.parent).transform;
                    }
                }
                else
                {
                    Debug.LogWarning($"Prefab not found for: {obj.prefabName}");
                }
            }
        }
    }


    private WorldData GetWorld()
    {

        foreach (GameObject obj in Tools.GetAllChildren(GameObject.Find("Maze").transform))
        {
            GameObjectData objData = new()
            {
                prefabName = obj.name.Replace("(Clone)", ""),
                x = obj.transform.position.x,
                y = obj.transform.position.y,
            };
            world.gameObjects.Add(objData);
        }
        return world;
    }

    public void Save(ref WorldSaveData data)
    {
        data.world = world;
        Debug.Log("World Saved!");
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