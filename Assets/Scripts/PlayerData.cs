using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public float[] location;
    public int playerCash;
    public int playerDmg;
    public int playerHealth;

    public PlayerData(PlayerStats player)
    {
        playerCash = player.curCash;
        playerDmg = player.DmgLvl;
        playerHealth = player.curHealth;

        location = new float[3];
        location[0] = player.GetComponent<Transform>().transform.position.x;
        location[1] = player.GetComponent<Transform>().transform.position.y;
        location[2] = player.GetComponent<Transform>().transform.position.z;
    }

}