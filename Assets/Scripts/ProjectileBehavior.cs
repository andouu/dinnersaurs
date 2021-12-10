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
        angleAdd = -angleAdd;
        mainCam = Camera.main;
    }

    // Start is called before the first frame update
    void Start()
    {
        Vector3 targetVec = ForwardWithAddedAngle(angleAdd);
        rb.AddForce(targetVec * launchForce);

        Vector3 torque;
        torque.x = Random.Range(-torqueRange, torqueRange);
        torque.y = Random.Range(-torqueRange, torqueRange);
        torque.z = Random.Range(-torqueRange, torqueRange);
        rb.AddTorque(torque);
    }

    Vector3 ForwardWithAddedAngle(float angleAdd) // deg
    {
        Vector3 forward = mainCam.transform.forward;
        float forwardAngle = mainCam.transform.rotation.eulerAngles.x; // angle of forward vector on the ZY plane

        float targetAngle = (forwardAngle + angleAdd) * Mathf.Deg2Rad;
        return new Vector3(forward.x, Mathf.Sin(-targetAngle), forward.z); // return rotated forward vector
    }
}
