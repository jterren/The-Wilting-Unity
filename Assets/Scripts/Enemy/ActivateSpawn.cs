using System.Collections.Generic;
using UnityEngine;

public class ActivateSpawn : MonoBehaviour
{
    private Collider2D playerAOE;
    private Collider2D Trigger;
    public List<GameObject> enemyType;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerAOE = Tools.GetChildByName(GameObject.FindGameObjectWithTag("Player").transform, "AOE").GetComponent<CircleCollider2D>();
        Trigger = GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerAOE && Trigger.IsTouching(playerAOE))
        {
            CreateEnemies(transform);
            transform.GetChild(0).gameObject.SetActive(false);
            foreach (GameObject enemy in Tools.GetAllChildrenByTag(transform, "Enemy"))
            {
                enemy.SetActive(true);
            }
            GetComponent<ActivateSpawn>().enabled = false;
        }
        else if (!playerAOE)
        {
            playerAOE = Tools.GetChildByName(GameObject.FindGameObjectWithTag("Player").transform, "AOE").GetComponent<CircleCollider2D>();
        }
    }

    void CreateEnemies(Transform targetSpawn)
    {
        for (int i = 0; i < 5; i++)
        {
            GameObject prefab = enemyType[0];
            GameObject temp = Instantiate(prefab, targetSpawn.position, Quaternion.identity);
            temp.SetActive(false);
            temp.transform.parent = targetSpawn;
            temp.transform.position += new Vector3((float)0.01 * i, 0, 0);
            temp.GetComponent<EnemyData>().x = gameObject;
            temp.GetComponent<EnemyData>().Player = GameManager.Instance.Player.transform;
            Tools.CaptureObject(temp, prefab.name);
        }
    }
}
