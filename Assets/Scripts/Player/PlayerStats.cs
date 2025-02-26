using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    //public GameObject sleepButton;
    //private int maxCash = 1000000;
    private int minHealth = 0;
    private double maxHealth = 100;

    public double curHealth;
    public int DmgLvl;
    public int curCash;
    public int curDmg;
    public int curKills;
    private int minDmg;
    private int avgDmg;
    private int maxDmg;
    private int random;


    public HealthBar healthbar;


    //public bool menuOpen;
    //public bool canMove;

    private void Awake()
    {
        GameManager.Instance.Player = this;
    }

    private void OnEnable()
    {
        curHealth = 100;
        DmgLvl = 10;
        curCash = 5000;
        curKills = 0;
        // LoadPData();
    }

    private void OnDisable()
    {
        // SavePlayer();
    }


    // Use this for initialization
    void Start()
    {
        //Player.transform.position = pData.Location;

        /*
        canMove = true;
        menuOpen = false;
        animator.SetBool("nearBed", false);
        sleepButton.SetActive(false);*/

        minDmg = DmgLvl / 2;
        avgDmg = DmgLvl;
        maxDmg = DmgLvl * 2;
        random = UnityEngine.Random.Range(1, 9);

        healthbar.SetMaxHealth(100);

        GameObject.FindGameObjectWithTag("Player").SetActive(true);

    }

    // Update is called once per frame
    void Update()
    {
        curKills = GameObject.FindGameObjectWithTag("KillUI").GetComponent<KillUI>().KillCount;
        healthbar.SetHealth((int)(curHealth / maxHealth * 100));

        // if (curHealth <= 0)
        // {
        //     PlayerDie();
        // }

        if (random < 3)
        {
            curDmg = minDmg + Mathf.ClosestPowerOfTwo(random);
        }
        else if (random > 3 && random < 7)
        {
            curDmg = avgDmg;
        }
        else if (random > 6 && random < 9)
        {
            curDmg = maxDmg;
        }
        if (Input.GetKeyDown(KeyCode.H) == true)
        {
            curHealth = 100;
        }


        random = UnityEngine.Random.Range(1, 9);
    }

    /*


       void OnTriggerExit2D(Collider2D other)
       {

           if (other.gameObject.CompareTag("BedroomSleep"))
           {
               sleepAnimator.SetBool("nearBed", false);
           }
       }*/

    public void takeDamage(int damage)
    {
        curHealth -= damage;
    }

    void PlayerDie()
    {
        //Debug.Log(pData.PlayerKills.ToString());
        this.gameObject.SetActive(false);
    }

    public void SavePlayerData(ref PlayerSaveData data)
    {
        data.location = transform.position;
        data.playerCash = curCash;
        data.playerDmg = DmgLvl;
        data.playerHealth = curHealth;
    }

    public void LoadPlayerData(PlayerSaveData data)
    {
        transform.position = data.location;
        curCash = data.playerCash;
        curDmg = data.playerDmg;
        curHealth = data.playerHealth;
    }

}

[System.Serializable]
public struct PlayerSaveData
{
    public Vector3 location;
    public int playerCash;
    public int playerDmg;
    public double playerHealth;

}
