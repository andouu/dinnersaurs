using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadTracking : MonoBehaviour
{
    [Header("Settings")]
    public float horzMaxRot; // max z rotation in degrees
    public float vertMaxRot; // max y rotation in degrees

    [Header("Components")]
    [SerializeField] private Transform _neck;
    private PredatorController _controller;

    // offset angles since the head is a different rotation from the bone
    private float _yAngleOffset = 45f;   
    private float _xAngleOffset = 0.01f;

    private void Awake()
    {
        _controller = GetComponent<PredatorController>();
    }

    private void LateUpdate()
    {
        Transform target = _controller.Target;
        
        // find the rotation of the forward vector on the z axis to get the unity forward vector
        Vector2 yzForward = new Vector2(_neck.forward.z, _neck.forward.y);
        Vector2 flippedYzForward = new Vector2(-_neck.forward.z, _neck.forward.y);
        float fwdAngWPlane = Mathf.Abs(Vector2.Angle(yzForward, Vector2.right) - Vector2.Angle(flippedYzForward, Vector2.right) - 90f) * Mathf.Deg2Rad;
        Vector3 trueForward = new Vector3(-_neck.forward.x + _xAngleOffset, _neck.forward.y - Mathf.Sin(fwdAngWPlane + _yAngleOffset), -_neck.forward.z);

        Vector3 toPlayerDir = target.position - _neck.position;

        // xz rotation (horizontal rotation)
        Vector2 horzForward2d = new Vector2(trueForward.x, trueForward.z);
        Vector2 horzDir2d = new Vector2(toPlayerDir.x, toPlayerDir.z);
        float xzAngle = Vector2.SignedAngle(horzForward2d, horzDir2d);
        float clampedXzAngle = Mathf.Clamp(xzAngle, -horzMaxRot, horzMaxRot);
        Quaternion horzRotation = Quaternion.AngleAxis(-clampedXzAngle, Vector3.forward);

        // zy rotation (vertical rotation)
        Vector2 vertForward2d = new Vector2(trueForward.z, trueForward.y);
        Vector2 vertDir2d = new Vector2(toPlayerDir.z, toPlayerDir.y);
        float zyAngle = -Vector2.Angle(vertForward2d, vertDir2d);
        float clampedZyAngle = Mathf.Clamp(zyAngle, -vertMaxRot, vertMaxRot);
        Quaternion vertRotation = Quaternion.AngleAxis(-clampedZyAngle, -Vector3.right);

        Quaternion netRotation = horzRotation * vertRotation;
        _neck.localRotation = netRotation;
    }
}
