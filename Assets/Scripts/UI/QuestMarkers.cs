using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestMarkers : MonoBehaviour
{
    public GameObject Quest;
    private RectTransform ptrTransform;
    private GameObject Arrow;
    public GameObject Player;


    private void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        Arrow = GameObject.Find("Arrow");
    }
    private void Start()
    {
        ptrTransform = Arrow.GetComponent<RectTransform>();
        Quest = GameObject.FindGameObjectWithTag("Quest");
    }

    private void Update()
    {
        CalculateAngle(Quest.transform.position);
    }

    private void CalculateAngle(Vector3 Target)
    {
        Vector3 toPosition = Target;
        Vector3 fromPosition = Camera.main.transform.position;
        fromPosition.z = 0f;
        Vector3 dir = (toPosition - fromPosition).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
        ptrTransform.eulerAngles = Vector3.forward * angle;
    }

    public void EnableArrow()
    {
        Arrow.SetActive(true); //Fetch manually to avoid onStart error Arrow undefined.
    }

    public void DisableArrow()
    {
        Arrow.SetActive(false);
    }
}