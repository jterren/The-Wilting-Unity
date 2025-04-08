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
    private Vector3 direction;
    public int baseLayer;
    private int defaultLayer;
    public bool layerChange;
    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        aggro = false;
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultLayer = spriteRenderer.sortingOrder;
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            aggro = false;
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
        direction = Player.position - transform.position;
        direction.Normalize();
    }

    private void FixedUpdate()
    {
        if (transform.position.y < Player.position.y)
        {
            if (layerChange) spriteRenderer.sortingOrder = defaultLayer + baseLayer;
        }
        else
        {
            if (layerChange) spriteRenderer.sortingOrder = defaultLayer;
        }
        MoveEnemy(direction);
    }


    void MoveEnemy(Vector2 direction)
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
        if (collision.gameObject.CompareTag("Player") == true)
        {
            aggro = true;
        }
    }

}
