using UnityEngine;

public class ActivateSpawn : MonoBehaviour
{
    private Collider2D Player;
    private Collider2D Trigger;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player").GetComponentsInChildren<CircleCollider2D>()[1];
        Trigger = GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Player != null && Trigger.IsTouching(Player))
        {
            transform.GetChild(0).gameObject.SetActive(false);
            foreach (GameObject enemy in Tools.GetAllChildrenByTag(transform, "Enemy"))
            {
                enemy.SetActive(true);
            }
            GetComponent<ActivateSpawn>().enabled = false;
        }
    }
}
