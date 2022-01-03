using UnityEngine;

public class BasicCharacterController : MonoBehaviour
{
    [Header("Movement Speeds")]
    [SerializeField] private float _walkSpeed = 9f;
    [SerializeField] private float _sprintSpeedMultiplier = 1.5f;
    [SerializeField] private float _sneakSpeedMultiplier = 0.6f;
    
    [Header("Jump Settings")]
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private float _groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask _groundCheckLayerMask;
    [SerializeField] private float _jumpForceMagnitude = 70f;

    private float _movementSpeed;
    private MovementState _movementState = MovementState.Idle;
    public MovementState MovementState
    {
        get => _movementState;
        set => _movementState = value;
    }
    
    void Update()
    {
        if (_movementState == MovementState.Frozen) return;
            
        if (Input.GetKey(KeyCode.LeftShift))
        {
            _movementState = MovementState.Sprinting;
            _movementSpeed = _walkSpeed * _sprintSpeedMultiplier;
        }
        else if (Input.GetKey(KeyCode.C))
        {
            _movementState = MovementState.Sneaking;
            _movementSpeed = _walkSpeed * _sneakSpeedMultiplier;
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
            _movementState = MovementState.Idle;

        _rb.velocity = new Vector3(netMovement.x, _rb.velocity.y, netMovement.z);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Physics.OverlapSphere(_groundCheck.position, _groundCheckRadius, _groundCheckLayerMask).Length > 0)
            {
                print("Jump");
                _rb.AddForce(Vector3.up * _jumpForceMagnitude, ForceMode.Impulse);
            }
        }
    }
}

public enum MovementState
{
    Idle,
    Walking,
    Sneaking,
    Sprinting,
    Frozen
}