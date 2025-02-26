using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class KillUI : MonoBehaviour
{
    public GameObject Player;
    public int KillCount;
    
    // Start is called before the first frame update
    void Start()
    {
       this.GetComponent<Text>().text = KillCount.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        this.GetComponent<Text>().text = KillCount.ToString();
    }

    public void AddKills(int x)
    {
      KillCount += x;
    }
}
