using System;
using System.ComponentModel;
using UnityEngine;

[ExecuteInEditMode]
public class JointRestraint : MonoBehaviour
{
    public Transform joint;
    [SerializeField]
    private float defaultRotation;
    [SerializeField]
    private float minRotationOffset = 15;
    [SerializeField]
    private float maxRotationOffset = 90;
    [SerializeField]
    private float minRotation;
    [SerializeField]
    private float maxRotation;
    [SerializeField]
    private float setDefaultRotation;


    void Awake()
    {
        if (joint == null) joint = transform;
        if (defaultRotation == 0) defaultRotation = NormalizeAngle(joint.localEulerAngles.z);
        if (minRotation == 0) minRotation = defaultRotation - minRotationOffset;
        if (maxRotation == 0) maxRotation = defaultRotation + maxRotationOffset;
        joint.localEulerAngles = new(joint.position.x, joint.position.y, defaultRotation);
        setDefaultRotation = defaultRotation;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        float angle = NormalizeAngle(joint.localEulerAngles.z);
#if UNITY_EDITOR
        {
            if (defaultRotation != setDefaultRotation)
            {
                defaultRotation = setDefaultRotation;
            }
            minRotation = defaultRotation - minRotationOffset;
            maxRotation = defaultRotation + maxRotationOffset;
        }
#endif

        if (angle < minRotation || angle > maxRotation) joint.localEulerAngles = new(0, 0, Mathf.Clamp(angle, minRotation, maxRotation));
    }

    private float NormalizeAngle(float angle)
    {
        angle %= 360f;
        return angle > 180f ? angle - 360f : angle;
    }
}