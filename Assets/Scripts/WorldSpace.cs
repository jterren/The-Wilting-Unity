using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public GameObject[] OverSeers;
    public GameObject RoundCounter;
    public GameObject Player;
    //public GameObject Landscape;
    public GameObject QuestUI;

    public int count = 0;
    private int random;


    // Start is called before the first frame update
    void Start()
    {
        RoundCounter = GameObject.FindGameObjectWithTag("RoundCounter");
        QuestUI = GameObject.FindGameObjectWithTag("QuestUI");
        Player = GameObject.FindGameObjectWithTag("Player");

        xRange = maxX - minX;
        yRange = maxY - minY;

        zoneSpawns = new Vector2[zoneCount];
        zones = new GameObject[zoneCount];
        OverSeers = new GameObject[zoneCount];

        createZones();

        OverSeers = GameObject.FindGameObjectsWithTag("OverSeer");
        for (int i = 0; i < OverSeers.Length; i++)
        {
            OverSeers[i].SetActive(false);
        }

        FindNewZone();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.T))
        {
            Player.gameObject.transform.position = OverSeers[random].transform.position;
            
        }
        /*if (Input.GetKey(KeyCode.L))
        {
            LoadWorld(); //Will result in error can't convert game objects to binary
        }
        if (Input.GetKey(KeyCode.P))
        {
            SaveWorld(); //Will result in error can't convert game objects to binary
        }*/
    }

    void createZones()
    {
        int x = minX;
        int y = minY;
        Vector2 spawnPoint;

        for (int j = 0; j < Mathf.Sqrt(zoneCount); j++)
        {
            for (int i = 0; i < Mathf.Sqrt(zoneCount); i++)
            {
                spawnPoint = new Vector2(x, y);
                zoneSpawns[count] = spawnPoint;
                zones[count] = Instantiate(zoneTypes[0], spawnPoint, Quaternion.identity) as GameObject;
                zones[count].transform.SetParent(this.transform);
                //zones[count].transform.SetParent(Landscape);
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
    }

    public void ClearZone(GameObject currentZone)
    {
        RoundCounter.GetComponent<RoundUI>().Disable();
        Destroy(currentZone);
        Debug.Log("Zone cleared, destroying enemy source.");
        random = Random.Range(0, 63);
    }

    public void FindNewZone()
    {
        random = Random.Range(0, 63);
        Zone = zones[random];
        OverSeers[random].SetActive(true);
        curOverSeer = OverSeers[random];
        QuestUI.GetComponent<QuestMarkers>().enableArrow();
        /*Debug.Log("Random is " + random);
        Debug.Log("Zone is " + Zone.transform.position);
        Debug.Log("Zone is " + OverSeers[random].transform.position);*/
    }

    public void SaveWorld()
    {
        SaveSystem.SaveWorld(this);
    }

    public void LoadWorld()
    {
        WorldData data = SaveSystem.LoadWorld();

        zones = data.Zones;

    }
}
