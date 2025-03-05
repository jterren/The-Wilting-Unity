using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ZoneManager : MonoBehaviour
{
    public GameObject OverSeer;
    public GameObject World;
    public GameObject[] groundType;
    public int minX;
    public int minY;
    public int maxX;
    public int maxY;

    private int zoneLength;
    private int zoneHeight;

    private int random;
    private int stop;
    private bool generated = false;
    private Transform parentTransform;

    void Start()
    {
        World = GameObject.FindGameObjectWithTag("World");
        zoneLength = World.GetComponent<WorldSpace>().zoneLength;
        zoneHeight = World.GetComponent<WorldSpace>().zoneHeight;
        parentTransform = Tools.GetAllChildrenByTag(transform, "Terrain")[0].transform;

        minX = (int)transform.position.x;
        maxX = minX + zoneLength;
        minY = (int)transform.position.y;
        maxY = minY + zoneHeight;
        stop = 0;

        GenerateTerrain();
        DisableTerrain();
        if (PlayerInZone())
        {
            EnableTerrain();
        }
    }

    void Update()
    {
        if (stop == 0 && OverSeer.GetComponent<Rounds>().zoneComplete == true)
        {
            BeginSelfDestruct();
            stop = 1;
        }
    }

    void BeginSelfDestruct()
    {
        GameObject world = GameObject.FindGameObjectWithTag("World");
        world.GetComponent<WorldSpace>().FindNewZone();
    }

    private void GenerateTerrain()
    {
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
                Instantiate(groundType[random], spawn, Quaternion.identity).transform.SetParent(parentTransform);
                count++;
                x += 1;
            }
            x = minX + (float).5;
            y += 1;
        }
        generated = true;
    }

    private void EnableTerrain()
    {
        if (generated == true)
        {
            parentTransform.gameObject.SetActive(true);
        }
    }

    private void DisableTerrain()
    {
        parentTransform.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") == true)
        {
            EnableTerrain();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") == true)
        {
            DisableTerrain();
        }
    }

    private bool PlayerInZone()
    {
        Vector3 pos = GameObject.Find("Player").GetComponent<Transform>().position;

        if (minX - 51 <= pos.x && pos.x <= maxX + 51 && minY - 51 <= pos.y && pos.y <= maxY + 51) return true;
        return false;
    }
}
