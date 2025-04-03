using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CombatSys : MonoBehaviour
{
    private float timeBtwAtk = 0;
    public float startTimeBtwAtk;
    public ContactFilter2D enemyFilter = new();
    private int damage;
    public Transform attackRight;
    public Animator animator;
    public CircleCollider2D effectorRight;
    public CircleCollider2D effectorLeft;
    private IEnumerable<GameObject> targetsLeft;
    private IEnumerable<GameObject> targetsRight;

    void Start()
    {
        damage = GetComponent<PlayerStats>().curDmg;
        enemyFilter.SetLayerMask(LayerMask.GetMask("Enemy"));
        enemyFilter.useLayerMask = true;
        enemyFilter.useTriggers = true;
    }

    void Update()
    {
        if (timeBtwAtk <= 0)
        {
            FindMouseClick();
            timeBtwAtk = startTimeBtwAtk;
        }
        else
        {
            timeBtwAtk -= Time.deltaTime;
        }
    }

    void FindMouseClick()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            animator.SetBool("AttackingLeft", true);
            targetsLeft = Physics2D.OverlapCircleAll(effectorRight.transform.position, effectorRight.radius, enemyFilter.layerMask).Select(x => x.gameObject).Distinct();
            if (targetsLeft.Count() > 0) DamageEnemies(targetsLeft);
        }
        else
        {
            animator.SetBool("AttackingLeft", false);
        }

        if (Input.GetKey(KeyCode.Mouse1))
        {
            animator.SetBool("AttackingRight", true);
            targetsRight = Physics2D.OverlapCircleAll(effectorRight.transform.position, effectorRight.radius, enemyFilter.layerMask).Select(x => x.gameObject).Distinct();
            if (targetsRight.Count() > 0) DamageEnemies(targetsRight);
        }
        else
        {
            animator.SetBool("AttackingRight", false);
        }


    }

    void DamageEnemies(IEnumerable<GameObject> targets)
    {
        damage = GetComponent<PlayerStats>().curDmg;
        foreach (GameObject target in targets)
        {
            target.GetComponent<EnemyData>().TakeDamage(damage / targets.Count());
        }
    }
}
