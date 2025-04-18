using System.Collections.Generic;
using UnityEngine;

public class ActivateSpawn : MonoBehaviour
{
    private Collider2D playerAOE;
    private Collider2D Trigger;
    public List<GameObject> enemyType;
    public List<GameObject> enemies = new();
    private GameObject activator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerAOE = Tools.GetChildByName(Tools.FindGameObjectByName("Player").transform, "AOE").GetComponent<CircleCollider2D>();
        Trigger = GetComponent<CircleCollider2D>();
        activator = transform.GetChild(0).gameObject;
        CreateEnemies(transform);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Trigger.IsTouching(playerAOE) && activator.activeInHierarchy)
        {
            activator.SetActive(false);
            foreach (GameObject enemy in enemies)
            {
                enemy.SetActive(true);
            }
        }

        if (!activator.activeInHierarchy && enemies.Count == 0)
        {
            Destroy(gameObject);
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
            enemies.Add(temp);
        }
    }
}
