using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ZoneData : MonoBehaviour
{
    public GameObject X;
    public GameObject World;
    public GameObject[] groundType;
    private GameObject[] grassCells = new GameObject[625];

    public int minX;
    public int minY;
    public int maxX;
    public int maxY;

    private int zoneLength;
    private int zoneHeight;

    private int random;
    private int stop;
    private bool generated = false;

    void Start()
    {
        World = GameObject.FindGameObjectWithTag("World");
        zoneLength = World.GetComponent<WorldSpace>().zoneLength;
        zoneHeight = World.GetComponent<WorldSpace>().zoneHeight;

        minX = (int)(this.transform.position.x);
        maxX = minX + zoneLength;
        minY = (int)(this.transform.position.y);
        maxY = minY + zoneHeight;
        stop = 0;

        GenerateGrass();
        DisableGrass();
        // if (-51 < this.transform.position.x && this.transform.position.x < 50)
        // {
        //     if (-51 < this.transform.position.y && this.transform.position.y < 50)
        //     {
        //         EnableGrass();
        //     }
        // }
        if (playerInZone())
        {
            EnableGrass();
        }
    }

    void Update()
    {
        if (stop == 0 && X.GetComponent<Rounds>().zoneComplete == true)
        {
            BeginSelfDestruct();
            stop = 1;
        }
        playerInZone();
    }

    void BeginSelfDestruct()
    {
        GameObject world = GameObject.FindGameObjectWithTag("World");
        world.GetComponent<WorldSpace>().ClearZone(X);
        world.GetComponent<WorldSpace>().FindNewZone();
    }

    private void GenerateGrass()
    {
        GameObject temp;
        Vector2 spawn;
        float x = minX + (float).5;
        float y = minY + (float).5;
        int count = 0;

        while (y <= maxY)
        {
            while (x <= maxX)
            {
                random = UnityEngine.Random.Range(0, groundType.Length);
                spawn = new Vector2(x, y);
                temp = Instantiate(groundType[random], spawn, Quaternion.identity) as GameObject;
                temp.transform.SetParent(this.transform);
                grassCells[count] = temp.gameObject;
                count++;
                x += 1;
            }
            x = minX + (float).5;
            y += 1;
        }
        generated = true;
        count = 0;
    }

    private void EnableGrass()
    {
        if (generated == true)
        {
            for (int i = 0; i < grassCells.Length; i++)
            {
                grassCells[i].SetActive(true);
            }
        }
    }

    private void DisableGrass()
    {
        for (int i = 0; i < grassCells.Length; i++)
        {
            grassCells[i].SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") == true)
        {
            EnableGrass();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") == true)
        {
            DisableGrass();
        }
    }

    private bool playerInZone()
    {
        Vector3 pos = GameObject.Find("Player").GetComponent<Transform>().position;

        if (minX - 51 <= pos.x && pos.x <= maxX + 51 && minY - 51 <= pos.y && pos.y <= maxY + 51) return true;
        return false;
    }
}
