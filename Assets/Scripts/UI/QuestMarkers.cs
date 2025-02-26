using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestMarkers : MonoBehaviour
{
    private GameObject world;
    public GameObject X;
    private Vector3 targetPosition;
    private RectTransform ptrTransform;
    private GameObject Arrow;
    public GameObject Player;

    private void Start()
    {
        Arrow = GameObject.Find("Arrow");
        world = GameObject.FindGameObjectWithTag("World");
        ptrTransform = transform.Find("Arrow").GetComponent<RectTransform>();
    }

    private void Update()
    {
        X = world.GetComponent<WorldSpace>().curOverSeer;
        targetPosition = X.transform.position;
        CalculateAngle(targetPosition);
    }

    private void CalculateAngle(Vector3 Target)
    {
        Vector3 toPosition = targetPosition;
        Vector3 fromPosition = Camera.main.transform.position;
        fromPosition.z = 0f;
        Vector3 dir = (toPosition - fromPosition).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
        ptrTransform.eulerAngles = Vector3.forward * angle;
    }

    public void EnableArrow()
    {
        GameObject.Find("Arrow").SetActive(true); //Fetch manually to avoid onStart error Arrow undefined.
    }

    public void DisableArrow()
    {
       Arrow.SetActive(false);
    }
}