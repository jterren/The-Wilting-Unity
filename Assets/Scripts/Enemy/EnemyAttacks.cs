using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttacks : MonoBehaviour
{
    public float timeBtwAtk;
    public float startTimeBtwAtk;
    public bool AttackReady;

    public int DmgLvl;
    public int curDmg;
    private int minDmg;
    private int avgDmg;
    private int maxDmg;
    private int random;
    public CircleCollider2D PlayerAOE;
    public CircleCollider2D AttackArea;

    public GameObject Player;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        PlayerAOE = Tools.GetChildByName(Player.transform, "AOE").GetComponent<CircleCollider2D>();

        DmgLvl = 2;

        minDmg = DmgLvl / 2;
        avgDmg = DmgLvl;
        maxDmg = DmgLvl * 2;
        random = Random.Range(1, 9);

        timeBtwAtk = startTimeBtwAtk;
        AttackReady = true;
    }

    // Update is called once per frame
    void Update()
    {
        curDmg = calculateDmg();
        AttackReady = CheckWait();

        if (AttackArea.IsTouching(PlayerAOE) == true)
        {
            if (AttackReady == true)
            {
                Player.GetComponent<PlayerStats>().takeDamage(curDmg);
            }
        }
    }

    public int calculateDmg()
    {
        int dmg = 0;

        if (random < 3)
        {
            dmg = minDmg + Mathf.ClosestPowerOfTwo(random);
        }
        else if (random > 3 && random < 7)
        {
            dmg = avgDmg;
        }
        else if (random > 6 && random < 9)
        {
            dmg = maxDmg;
        }

        random = Random.Range(1, 9);

        return dmg;
    }

    bool CheckWait()
    {
        if (timeBtwAtk <= 0)
        {
            timeBtwAtk = startTimeBtwAtk;
            return true;
        }
        else if (AttackReady == false)
        {
            timeBtwAtk -= Time.deltaTime;
        }

        return false;
    }
}
