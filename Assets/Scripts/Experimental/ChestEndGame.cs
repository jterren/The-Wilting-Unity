using UnityEngine;

public class ChestEndGame : MonoBehaviour
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (
             // GetComponent<CircleCollider2D>().IsTouchingLayers(9) &&
             Input.GetKeyDown(KeyCode.E))
        {
            Tools.CompleteGame();
        }
    }
}
