using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyGeneration:MonoBehaviour
{
    public BoxCollider2D triggerArea;
    public GameObject[] EnemyType;
    private GameObject[] Enemies;
    public GameObject[] GenType;
    private GameObject[] Generators;
    public GameObject Player;
    public int genCount = 1;
    public int maxGen;
    public int enemyPerGen;
    private int enemyMax;
    public int enemyCounter;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        // QuestUI = GameObject.FindGameObjectWithTag("QuestUI");


        Generators = new GameObject[maxGen];

        enemyMax = Generators.Length * enemyPerGen;
        Enemies = new GameObject[enemyMax];
        enemyCounter = 0;
        CreateGenerators();
        GenerateEnemies();

        for (int i = 0; i < genCount; i++)
        {
            Generators[i].transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Player.GetComponentInChildren<Collider2D>().IsTouching(triggerArea))
        {
            if (Input.GetKey(KeyCode.E))
            {
                // if (arrowNeed == false)
                // {
                //     QuestUI.GetComponent<QuestMarkers>().disableArrow();
                //     arrowNeed = true;
                // }
                Start();
            }
        }

        if (enemyCounter > 0 && Input.GetKey(KeyCode.K))
        {
            KillActiveEnemies();
        }

    }
    void CreateEnemies(Transform targetSpawn)
    {
        for (int i = 0; i < enemyPerGen; i++)
        {
            Enemies[enemyCounter] = Instantiate(EnemyType[0], targetSpawn.position, Quaternion.identity) as GameObject;
            Enemies[enemyCounter].transform.position += (new Vector3(((float)0.01 * i), 0, 0));
            Enemies[enemyCounter].GetComponent<EnemyData>().x = this.gameObject;
            Enemies[enemyCounter].GetComponent<EnemyData>().Player = Player.transform;
            Enemies[enemyCounter].SetActive(false);
            enemyCounter++;
        }
    }

    void KillActiveEnemies()
    {
        GameObject[] currentEnemies = GameObject.FindGameObjectsWithTag("Enemy");

        //Debug.Log("You prayed to God to kill all your enemies.");
        for (int i = 0; i < enemyCounter; i++)
        {
            currentEnemies[i].GetComponent<EnemyData>().health = 0;
        }
        //Debug.Log("God killed all your enemies.");
    }

    void CreateGenerators()
    {
        float x;
        float y;
        float spacerX = (float)1;
        float spacerY = (float)1;
        bool FindNew = false;
        int curGen = 0;

        Vector3[] UsedSpawn = new Vector3[genCount + 1];
        UsedSpawn[0] = new Vector3(0, 0, 0);

        while (curGen < genCount)
        {
            FindNew = false;
            x = Player.transform.position.x + 10; //add player coordinates with random position within range
            y = Player.transform.position.y + 10; //add player coordinates with random position within range

            foreach (Vector3 Used in UsedSpawn)
            {
                if (Used.x >= (x - spacerX) && Used.x <= (x + spacerX))
                {
                    FindNew = true;
                }

                if (Used.y >= (y - spacerY) && Used.y <= (y + spacerY))
                {
                    FindNew = true;
                }
            }

            if (FindNew == false)
            {
                Generators[curGen] = Instantiate(GenType[0], new Vector3(x, y, 0), Quaternion.identity) as GameObject;
                curGen += 1;
                UsedSpawn[curGen] = new Vector3(x, y, 0);
            }
        }

        UsedSpawn = new Vector3[genCount + 1];
    }

    void GenerateEnemies()
    {
        Transform[] genChild;
        Transform targetSpawn;
        for (int x = 0; x < genCount; x++)
        {
            genChild = Generators[x].GetComponentsInChildren<Transform>();
            targetSpawn = genChild[1];
            CreateEnemies(targetSpawn);

            genChild = null;
        }
    }

}
