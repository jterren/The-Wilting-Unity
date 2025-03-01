using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rounds : MonoBehaviour
{
    public CircleCollider2D triggerArea;
    public CircleCollider2D PlayerAOE;

    public int roundCounter;
    public int maxRounds = 5;
    public bool RoundStart;
    public float waitTime = 5;
    public bool zoneComplete;

    public GameObject[] EnemyType;
    private GameObject[] Enemies;
    public GameObject[] GenType;
    private GameObject[] Generators;
    public GameObject Player;
    public GameObject QuestUI;
    public GameObject RoundUI;

    public int genCount = 1;
    public int maxGen;
    public int enemyPerGen;
    private int enemyMax;
    public int enemyCounter;

    public float MinX;
    public float MaxX;
    public float MinY;
    public float MaxY;

    public int zoneLength;
    public int zoneHeight;
    public bool arrowNeed = false;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        QuestUI = GameObject.FindGameObjectWithTag("QuestUI");
        RoundUI = GameObject.FindGameObjectWithTag("RoundCounter");
        PlayerAOE = Player.GetComponentsInChildren<CircleCollider2D>()[1];

        RoundStart = false;
        Generators = new GameObject[maxGen];
        triggerArea = GetComponent<CircleCollider2D>();

        enemyMax = Generators.Length * enemyPerGen;
        Enemies = new GameObject[enemyMax];
        enemyCounter = 0;
        roundCounter = 1;

        zoneLength = GameObject.FindGameObjectWithTag("World").GetComponent<WorldSpace>().zoneLength;
        zoneHeight = GameObject.FindGameObjectWithTag("World").GetComponent<WorldSpace>().zoneHeight;

        MinX = (float)(this.transform.position.x - 50);
        MaxX = (float)(this.transform.position.x + 50);
        MinY = (float)(this.transform.position.y - 50);
        MaxY = (float)(this.transform.position.y + 50);
    }

    // Update is called once per frame
    void Update()
    {
        if (!zoneComplete)
        {
            Player = GameObject.FindGameObjectWithTag("Player");
            if (Player.GetComponentInChildren<Collider2D>().IsTouching(triggerArea))
            {
                if (RoundStart == false && triggerArea.IsTouching(PlayerAOE) == true)
                {
                    if (arrowNeed == false)
                    {
                        QuestUI.GetComponent<QuestMarkers>().DisableArrow();
                        arrowNeed = true;
                    }
                    StartRound();
                }
            }

            if (enemyCounter > 0 && Input.GetKey(KeyCode.K) && RoundStart == true)
            {
                if (zoneComplete == false)
                {
                    KillActiveEnemies();
                }
            }

            CheckStart();
        }
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
            x = Random.Range(MinX + 5, MaxX - 5);
            y = Random.Range(MinY + 5, MaxY - 5);

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

    void CheckStart()
    {
        if (RoundStart == true && waitTime > 0)
        {
            waitTime -= Time.deltaTime;
        }

        if (waitTime <= 0 && enemyCounter == 0)
        {
            RoundStart = false;
            waitTime = 5;
            ResetRound();
        }
    }

    void StartRound()
    {
        CreateGenerators();
        GenerateEnemies();
        RoundStart = true;

        for (int i = 0; i < genCount; i++)
        {
            Generators[i].transform.GetChild(0).gameObject.SetActive(false);
        }

        for (int i = 0; i < enemyCounter; i++)
        {
            Enemies[i].SetActive(true);
        }
    }

    void ResetRound()
    {
        for (int i = 0; i < genCount; i++)
        {
            Destroy(Generators[i]);
        }

        if (roundCounter == maxRounds)
        {
            Debug.Log("Area complete move on to the next round");
            zoneComplete = true;
        }

        Generators = new GameObject[maxGen];
        Enemies = new GameObject[enemyMax];
        enemyCounter = 0;

        if (genCount < maxGen)
        {
            genCount += 1;
        }

        GameManager.Instance.RoundCounter += 1;
        roundCounter += 1;
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
}
