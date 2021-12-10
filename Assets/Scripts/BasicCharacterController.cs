using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicCharacterController : MonoBehaviour
{
    public float mvmtSpeed = 3f;
    public float sprintSpeedMultiplier = 1.5f;
    [Range(0, 1f)]
    public float skidMultiplier = 0.4f;
    public Rigidbody rb;

    private float originalSpeed; // stores the original speed before sprinting

    // Ground checking
    [SerializeField]
    private GameObject groundCheck;
    private bool isGrounded;

    private void Start()
    {
        originalSpeed = mvmtSpeed;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            mvmtSpeed = originalSpeed * sprintSpeedMultiplier;
        }
        else
        {
            mvmtSpeed = originalSpeed;
        }

        Vector3 netMovement = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
        {
            netMovement += transform.forward * mvmtSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            netMovement += -transform.forward * mvmtSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A))
        {
            netMovement += -transform.right * mvmtSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            netMovement += transform.right * mvmtSpeed * Time.deltaTime;
        }
        transform.position += netMovement;

        netMovement *= skidMultiplier * Time.deltaTime;
    }

    private void FixedUpdate()
    {
        //if (Input.GetKey(KeyCode.W))
        //{
        //    rb.AddForce(transform.forward * mvmtSpeed);
        //}
        //if (Input.GetKey(KeyCode.S))
        //{
        //    rb.AddForce(-transform.forward * mvmtSpeed);
        //}
        //if (Input.GetKey(KeyCode.A))
        //{
        //    rb.AddForce(-transform.right * mvmtSpeed);
        //}
        //if (Input.GetKey(KeyCode.D))
        //{
        //    rb.AddForce(transform.right * mvmtSpeed);
        //}
    }
}
