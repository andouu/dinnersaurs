using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.AI;

public class TarbosaurusController : MonoBehaviour
{
    [SerializeField] private Transform _eyes;
    [SerializeField] private Transform _targetTransform;
    [SerializeField] private LayerMask _targetRaycastLayerMask;
    private Vector3 _targetMove;
    private bool _isChasing = false;
    public bool IsChasing
    {
        get { return _isChasing; }
        set { _isChasing = value; }
    }
    private float _speed = 0f;

    private NavMeshAgent _agent;
    
    [Header("Movement Fields")]
    [SerializeField] private float _rotSpeed = 2f;
    [SerializeField] private float _chaseSpeed = 3.5f;
    [SerializeField] private float _wanderSpeed = 1.2f;
    [SerializeField] private float _minPlayerDistance = 7f;
    [SerializeField] private float _maxPlayerDistance = 30f;
    [SerializeField] private float _maxWanderDistance = 15f;

    private const float MinWanderDist = 0.5f;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        stopChasing();
    }

    private void startChasing()
    {
        print("START CHASING");
        _isChasing = true;
        _speed = _chaseSpeed;
        _agent.speed = _speed;
        _agent.isStopped = false;
    }
    
    private void stopChasing()
    {
        print("STOP CHASING");
        _agent.isStopped = true;
        _isChasing = false;
        _speed = _wanderSpeed;
        _agent.speed = _speed;
    }

    void FixedUpdate()
    {
        float playerDistance = Vector3.Distance(transform.position, new Vector3(_targetTransform.position.x, transform.position.y, _targetTransform.position.z));
        if (playerDistance > _maxPlayerDistance)
        {
            if (_isChasing)
            {
                stopChasing();
            }
            
            // TODO wandering movement
        }
        else if (playerDistance >= _minPlayerDistance)
        {
            Vector3 dir = (_targetTransform.position - _eyes.position).normalized;
            RaycastHit raycastHit;
            bool hit = Physics.Raycast(_eyes.position, dir, out raycastHit, playerDistance, _targetRaycastLayerMask);
            if (hit) print("Raycast hit " + raycastHit.collider.gameObject.name);
            if (hit && raycastHit.collider.CompareTag("Player"))
            {
                if (_isChasing)
                {
                    _agent.SetDestination(_targetTransform.position);
                }
                else
                {
                    startChasing();
                }
            }
        }
        
        /*// rotation
        Vector3 direction = _targetMove - transform.position;
        Quaternion endRot = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, endRot, _rotSpeed * Time.fixedDeltaTime);
        
        // position
        transform.position = Vector3.MoveTowards(transform.position, _targetMove, _speed * Time.fixedDeltaTime);*/
    }
}
