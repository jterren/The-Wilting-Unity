using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatSys : MonoBehaviour
{
    private float timeBtwAtk;
    public float startTimeBtwAtk;

    public Transform attackPosL;
    public Transform attackPosUpperL;
    public Transform attackPosR;
    public Transform attackPosUpperR;
    public LayerMask whatIsEnemies;
    public float attackRange;
    private int damage;
    private int playerHealth;

    void Start()
    {
        damage = this.GetComponent<PlayerStats>().curDmg;
    }

    void Update()
    {
        if(timeBtwAtk <= 0)
        {
            FindAttack();
        }
        else
        {
            timeBtwAtk -= Time.deltaTime;
        }

    }

    void FindAttack()
    {
        Collider2D[] enemiesToDamageL = Physics2D.OverlapCircleAll(attackPosL.position, attackRange, whatIsEnemies);
        Collider2D[] enemiesToDamageUpperL = Physics2D.OverlapCircleAll(attackPosUpperL.position, attackRange, whatIsEnemies);
        Collider2D[] enemiesToDamageR = Physics2D.OverlapCircleAll(attackPosR.position, attackRange, whatIsEnemies);
        Collider2D[] enemiesToDamageUpperR = Physics2D.OverlapCircleAll(attackPosUpperR.position, attackRange, whatIsEnemies);
    
        if (this.GetComponent<Movement>().attackD == "Left")
        {
            if (this.GetComponent<Animator>().GetInteger("DirY") == -1)
            {
                for (int i = 0; i < enemiesToDamageL.Length; i++)
                {
                    damage = this.GetComponent<PlayerStats>().curDmg;
                    enemiesToDamageL[i].GetComponent<EnemyData>().TakeDamage(damage);
                }
            }
            else if (this.GetComponent<Animator>().GetInteger("DirY") == 1)
            {
                for (int i = 0; i < enemiesToDamageUpperL.Length; i++)
                {
                    damage = this.GetComponent<PlayerStats>().curDmg;
                    enemiesToDamageUpperL[i].GetComponent<EnemyData>().TakeDamage(damage);
                }
            }

            timeBtwAtk = startTimeBtwAtk;
        }
        else if (this.GetComponent<Movement>().attackD == "Right")
        {
            if (this.GetComponent<Animator>().GetInteger("DirY") == -1)
            {
                for (int i = 0; i < enemiesToDamageR.Length; i++)
                {
                    damage = this.GetComponent<PlayerStats>().curDmg;
                    enemiesToDamageR[i].GetComponent<EnemyData>().TakeDamage(damage);
                }
            }
            else if (this.GetComponent<Animator>().GetInteger("DirY") == 1)
            {
                for (int i = 0; i < enemiesToDamageUpperR.Length; i++)
                {
                    damage = this.GetComponent<PlayerStats>().curDmg;
                    enemiesToDamageUpperR[i].GetComponent<EnemyData>().TakeDamage(damage);
                }
            }

            timeBtwAtk = startTimeBtwAtk;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPosL.position, attackRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPosUpperL.position, attackRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPosR.position, attackRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPosUpperR.position, attackRange);
    }
}
