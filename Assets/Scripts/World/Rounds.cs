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

        transform.localPosition = Tools.RandomizeChildPosition(0, 0, zoneLength, zoneHeight);
    }

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
        int curGen = 0;
        while (curGen < genCount)
        {
            Generators[curGen] = Instantiate(GenType[0], Tools.RandomizeChildWithRadius(transform, 15), Quaternion.identity);
            curGen += 1;
        }
    }

    void CreateEnemies(Transform targetSpawn)
    {
        for (int i = 0; i < enemyPerGen; i++)
        {
            Enemies[enemyCounter] = Instantiate(EnemyType[0], targetSpawn.position, Quaternion.identity);
            Enemies[enemyCounter].transform.position += new Vector3((float)0.01 * i, 0, 0);
            Enemies[enemyCounter].GetComponent<EnemyData>().x = gameObject;
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
        for (int i = 0; i < enemyCounter; i++)
        {
            currentEnemies[i].GetComponent<EnemyData>().health = 0;
        }
    }
}
