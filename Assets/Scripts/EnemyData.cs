using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyData : MonoBehaviour
{
    public int health;
    public Transform Player;
    private Rigidbody2D rb;
    private Vector2 movement;
    public float speed;
    public int MinDist;
    public bool aggro;
    private float dazedTime;
    public float startDazedTime;
    public GameObject x;

    public GameObject UI;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        aggro = false;
    }

    // Update is called once per frame
    void Update()
    {
        //  transform.LookAt(Player.transform);

        if (health <= 0)
        {
            aggro = false;
            x.GetComponent<Rounds>().enemyCounter -= 1;
            GameObject.FindGameObjectWithTag("KillUI").GetComponent<KillUI>().AddKills(1);
            Destroy(gameObject);
        }

        if (dazedTime <= 0)
        {
            speed = 1;
        }
        else
        {
            speed = 0;
            dazedTime -= Time.deltaTime;
        }

        Vector3 direction = Player.position - transform.position;
        direction.Normalize();
        movement = direction;
    }

    private void FixedUpdate()
    {
        if (aggro == true)
        {
            moveEnemy(movement);
        }
    }


    void moveEnemy(Vector2 direction)
    {
        rb.MovePosition((Vector2)transform.position + (direction * speed * Time.deltaTime));
    }

    public void TakeDamage(int x)
    {
        dazedTime = startDazedTime;
        health -= x;
        speed = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player") == true)
        {
            aggro = true;
        }
    }

}
