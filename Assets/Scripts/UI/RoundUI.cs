// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;

// public class RoundUI : MonoBehaviour
// {
//     public GameObject X;
//     public int curRound;

//     void Start()
//     {

//     }

//     // Update is called once per frame
//     void Update()
//     {
//         if (X != null)
//         {
//             curRound = GameManager.Instance.RoundCounter;
//             this.GetComponent<Text>().text = curRound.ToString();
//         }

//         X = GameObject.FindGameObjectWithTag("OverSeer");
//     }

//     public void Disable()
//     {
//         X = null;
//         this.GetComponent<Text>().text = "";
//     }
// }
