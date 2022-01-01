using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    public float launchForce;
    public float angleAdd;
    public float torqueRange;
    public Rigidbody rb;

    private Camera mainCam;

    private void Awake()
    {
        mainCam = Camera.main;
    }

    void Start()
    {
        angleAdd = -angleAdd;
        Vector3 targetVec = calcAngledForward();
        rb.AddForce(targetVec * launchForce, ForceMode.Impulse);

        Vector3 torque;
        torque.x = Random.Range(-torqueRange, torqueRange);
        torque.y = Random.Range(-torqueRange, torqueRange);
        torque.z = Random.Range(-torqueRange, torqueRange);
        rb.AddTorque(torque);
    }

    private Vector3 calcAngledForward() // degs
    {
        Vector3 cF = mainCam.transform.forward;
        Vector3 angled = Quaternion.AngleAxis(mainCam.transform.localEulerAngles.x + 2f * angleAdd, Vector3.right) * Vector3.one;
        return new Vector3(cF.x, angled.y, cF.z).normalized;
    }
}
