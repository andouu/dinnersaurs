using System;
using UnityEngine;

public class BasicCharacterController : MonoBehaviour
{
    [Header("Reset Settings")]
    [SerializeField] private Vector3 _resetPosition;

    [Header("Movement Speeds")]
    [SerializeField] private float _walkSpeed = 7.5f;
    [SerializeField] private float _sprintSpeedMultiplier = 1.5f;
    
    [Header("Jump Settings")]
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private float _groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask _groundCheckLayerMask;
    [SerializeField] private float _jumpForceMagnitude = 70f;

    [Header("Audio")]
    [SerializeField] private AudioSource _footsteps;
    [SerializeField] private AudioSource chomp;

    [Header("Misc")]
    [SerializeField] private MenuController menu;
    [SerializeField] private ChunkLoader _chunkLoader;

    [HideInInspector] public float Distance => _distanceRan;

    // cache
    private float _distanceRan = 0f;
    
    private float _movementSpeed;
    private MovementState _movementState = MovementState.Idle;

    public MovementState MovementState
    {
        get => _movementState;
        set => _movementState = value;
    }

    public void Reset()
    {
        _movementState = MovementState.Idle;
        _distanceRan = 0f;
        transform.position = _resetPosition;
    }

    public void Freeze() {
        _movementState = MovementState.Frozen;
    }

    public void Unfreeze() {
        _movementState = MovementState.Idle;
    }

    public void Die() {
        chomp.Play();
        menu.PauseGame();
        menu.EndResults();
    }

    private void Start()
    {
        _footsteps.Play();
        _footsteps.Pause();
    }

    void Update()
    {
        if (_movementState == MovementState.Frozen)
        {
            _footsteps.Pause();
            return;
        }

        bool grounded = Physics.OverlapSphere(_groundCheck.position, _groundCheckRadius, _groundCheckLayerMask).Length >
                        0;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            _movementState = MovementState.Sprinting;
            _movementSpeed = _walkSpeed * _sprintSpeedMultiplier;
        }
        else
        {
            _movementState = MovementState.Walking;
            _movementSpeed = _walkSpeed;
        }
        
        float forwardSpeed = Input.GetAxisRaw("Vertical") * _movementSpeed;
        float sideSpeed = Input.GetAxisRaw("Horizontal") * _movementSpeed;
        Vector3 netMovement = transform.forward * forwardSpeed + transform.right * sideSpeed;

        if (netMovement == Vector3.zero)
        {
            _movementState = MovementState.Idle;
            _footsteps.Pause();
        }
        else _footsteps.UnPause();

        _rb.velocity = new Vector3(netMovement.x, _rb.velocity.y, netMovement.z);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (grounded)
            {
                _movementState = MovementState.Jumping;
                _rb.AddForce(Vector3.up * _jumpForceMagnitude, ForceMode.Impulse);
            }
        }

        if (!grounded) _movementState = MovementState.Jumping;

            // we are running straight in the z direction, so the distance is just the difference in z values
        _distanceRan = transform.position.z - _chunkLoader.InitialPosition.z;
    }
}

public enum MovementState
{
    Idle,
    Walking,
    Sprinting,
    Frozen,
    Jumping,
}