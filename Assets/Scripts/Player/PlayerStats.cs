using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    //public GameObject sleepButton;
    //private int maxCash = 1000000;
    private readonly int maxHealth = 100;
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

    public KillUI killCounter;

    private void Awake()
    {
        GameManager.Instance.Player = this;
    }

    // Use this for initialization
    void Start()
    {
        minDmg = DmgLvl / 2;
        avgDmg = DmgLvl;
        maxDmg = DmgLvl * 2;
        random = UnityEngine.Random.Range(1, 9);
        healthbar.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        healthbar.SetHealth((int)(curHealth / maxHealth * 100));

        if (curHealth <= 0)
        {
            PlayerDie();
        }

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

    public void TakeDamage(int damage)
    {
        curHealth -= damage;
    }

    void PlayerDie()
    {
        gameObject.SetActive(false);
        Tools.CompleteGame();
    }

    public void Save(ref PlayerSaveData data)
    {
        data.location = transform.position;
        data.playerCash = curCash;
        data.playerDmg = DmgLvl;
        data.playerHealth = curHealth;
        data.playerKills = curKills;
    }

    public void Load(PlayerSaveData data)
    {
        transform.position = data.location;
        curCash = data.playerCash;
        curDmg = data.playerDmg;
        curHealth = data.playerHealth;
        curKills = data.playerKills;
    }

}

[Serializable]
public struct PlayerSaveData
{
    public Vector3 location;
    public int playerCash;
    public int playerDmg;
    public double playerHealth;
    public int playerKills;
}
