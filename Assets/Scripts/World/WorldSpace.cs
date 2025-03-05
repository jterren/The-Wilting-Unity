using System;
using System.Collections.Generic;
using UnityEngine;

public class WorldSpace : MonoBehaviour
{
    public int minX;
    public int minY;
    public int maxX;
    public int maxY;
    private int xRange;
    private int yRange;

    public int zoneCount;
    public int zoneLength = 25;
    public int zoneHeight = 25;

    public Vector2[] zoneSpawns;
    public GameObject Zone;
    public GameObject curOverSeer;
    public GameObject[] zones;
    public GameObject[] zoneTypes;
    public List<GameObject> OverSeers;
    public GameObject RoundCounter;
    public PlayerStats Player;
    public GameObject QuestUI;

    public int count = 0;
    private int random;
    public WorldData world;
    private List<GameObject> prefabs;

    private void Awake()
    {
        GameManager.Instance.WorldSpace = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        RoundCounter = GameObject.FindGameObjectWithTag("RoundCounter");
        QuestUI = GameObject.FindGameObjectWithTag("QuestUI");
        Player = GameManager.Instance.Player;

        xRange = maxX - minX;
        yRange = maxY - minY;

        zoneSpawns = new Vector2[zoneCount];
        zones = new GameObject[zoneCount];
        OverSeers = new List<GameObject>();

        if (world.gameObjects.Count == 0)
        {
            Debug.Log("New Game detected");
            CreateZones();
            world = GetWorld();
            FindNewZone();
        }
        else
        {
            foreach (GameObjectData data in world.gameObjects)
            {
                GameObject prefab = prefabs.Find(p => p.name == data.prefabName);
                if (prefab)
                {
                    GameObject obj = Instantiate(prefab, new Vector3(data.x, data.y, data.z), Quaternion.Euler(data.rotX, data.rotY, data.rotZ));
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.T))
        {
            Player.transform.position = OverSeers[random].transform.position;

        }
    }

    void CreateZones()
    {
        int x = minX;
        int y = minY;
        Vector2 spawnPoint;
        Transform parentTransform = GameObject.Find("Zones").transform;

        for (int j = 0; j < Mathf.Sqrt(zoneCount); j++)
        {
            for (int i = 0; i < Mathf.Sqrt(zoneCount); i++)
            {
                spawnPoint = new Vector2(x, y);
                zoneSpawns[count] = spawnPoint;
                zones[count] = Instantiate(zoneTypes[0], spawnPoint, Quaternion.identity);
                zones[count].transform.SetParent(parentTransform);
                if (x < maxX)
                {
                    x += zoneLength;
                }
                count++;
            }
            if (y < maxY)
            {
                y += zoneHeight;
                x = minX;
            }
        }
        count = 0;
        OverSeers = Tools.FindInactiveGameObjectsByTag("OverSeer");
    }

    public void FindNewZone()
    {
        random = UnityEngine.Random.Range(0, zones.Length - 1);
        Debug.Log("Finding new zone: " + random);
        Debug.Log(zones.Length - 1);
        Zone = zones[random];
        OverSeers[random].SetActive(true);
        curOverSeer = OverSeers[random];
        QuestUI.GetComponent<QuestMarkers>().EnableArrow();
    }


    private WorldData GetWorld()
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Zone"))
        {
            GameObjectData objData = new()
            {
                prefabName = obj.name.Replace("(Clone)", ""),
                x = obj.transform.position.x,
                y = obj.transform.position.y,
                z = obj.transform.position.z,
                rotX = obj.transform.rotation.eulerAngles.x,
                rotY = obj.transform.rotation.eulerAngles.y,
                rotZ = obj.transform.rotation.eulerAngles.z
            };
            world.gameObjects.Add(objData);
        }
        return world;
    }

    public void Save(ref WorldSaveData data)
    {
        data.world = GetWorld();
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
    public Vector2[] zoneSpawns;
    public GameObject Zone;
    public GameObject curOverSeer;
    public GameObject[] zones;
    public GameObject[] zoneTypes;
    public GameObject[] OverSeers;
    public GameObject RoundCounter;
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
    public string prefabName;  // Name or ID of the prefab
    public float x, y, z;      // Position
    public float rotX, rotY, rotZ; // Rotation
}