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
    public bool IsFrozen
    {
        get { return _isFrozen; }
        set { _isFrozen = value; }
    }
    public bool IsWalking
    {
        get { return _isWalking; }
    }
    public bool IsSprinting
    {
        get { return _isSprinting; }
    }

    private bool _isWalking = false;
    private bool _isSprinting = false;
    private float originalSpeed; // stores the original speed before sprinting
    private bool _isFrozen = false; // TODO: extend a base movement class or smth
    // TODO: Ground checking
    [SerializeField]
    private GameObject groundCheck;
    private bool isGrounded;

    private void Start()
    {
        originalSpeed = mvmtSpeed;
    }

    void Update()
    {
        if (!_isFrozen)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                _isSprinting = true;
                mvmtSpeed = originalSpeed * sprintSpeedMultiplier;
            }
            else
            {
                _isSprinting = false;
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
            if (netMovement != Vector3.zero)
                _isWalking = true;
            else
                _isWalking = false;
            transform.position += netMovement;

            netMovement *= skidMultiplier * Time.deltaTime;
        }
        else
        {
            _isWalking = false;
            _isSprinting = false;
        }
    }
}
