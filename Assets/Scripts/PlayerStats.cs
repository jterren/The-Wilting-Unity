using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    //public GameObject sleepButton;
    //private int maxCash = 1000000;
    // private int minHealth = 0;
    //private int maxHealth = 100;

    public int curHealth;
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


    private void OnEnable()
    {
        /*curHealth = 100;
        DmgLvl = 10;
        curCash = 5000;
        curKills = 0;*/
        LoadPData();
    }

    private void OnDisable()
    {
        SavePlayer();
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
        random = Random.Range(1, 9);

        healthbar.SetMaxHealth(100);

        GameObject.FindGameObjectWithTag("Player").SetActive(true);

    }

    // Update is called once per frame
    void Update()
    {
        curKills = GameObject.FindGameObjectWithTag("KillUI").GetComponent<KillUI>().KillCount;
        healthbar.SetHealth(curHealth);

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

        if (Input.GetKeyDown(KeyCode.P) == true)
        {
            SavePlayer();
        }
        if (Input.GetKeyDown(KeyCode.L) == true)
        {
            LoadPData();
        }
        if (Input.GetKeyDown(KeyCode.H) == true)
        {
            curHealth = 100;
        }


        random = Random.Range(1, 9);
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

    public void SavePlayer()
    {
        SaveSystem.SavePlayer(this);
    }

    public void LoadPData()
    {
        PlayerData pData = SaveSystem.LoadPlayer();

        curCash = pData.playerCash;
        curHealth = pData.playerHealth;
        DmgLvl = pData.playerDmg;

        Vector3 position;
        position.x = pData.location[0];
        position.y = pData.location[1];
        position.z = pData.location[2];
        transform.position = position;
        Debug.Log(position.x);

    }
}
