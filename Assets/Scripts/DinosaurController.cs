using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.AI;

public abstract class DinosaurController : MonoBehaviour
{
    [Header("GameObject Components")]
    [SerializeField] protected Transform _neck;
    [SerializeField] protected Transform _targetTransform;
    [SerializeField] protected LayerMask _targetRaycastLayerMask;
    [SerializeField] protected NavMeshAgent _agent;
    
    // Movement Fields
    protected float _chaseSpeed;
    protected float _wanderSpeed;
    protected float _minPlayerDistance;
    protected float _maxPlayerDistance;
    
    // Sensing Fields
    protected float _halfAngleFOV;
    
    // Movement Variables
    protected bool _isChasing = false;
    public bool IsChasing
    {
        get { return _isChasing; }
        set { _isChasing = value; }
    }
    protected float _speed = 0f;
    
    protected void startChasing()
    {
        if (_isChasing) return;
        _isChasing = true;
        _speed = _chaseSpeed;
        _agent.speed = _speed;
        _agent.isStopped = false;
    }
    
    protected abstract void Wander();
    
    protected void stopChasing() // both stops and wanders
    {
        if (!_isChasing)
        {
            Wander();
            return;
        }
        
        _agent.isStopped = true;
        _isChasing = false;
        _speed = _wanderSpeed;
        _agent.speed = _speed;
    }

    protected void Update() // can be overriden
    {
        float playerDistance = Vector3.Distance(transform.position, new Vector3(_targetTransform.position.x, transform.position.y, _targetTransform.position.z));
        if (playerDistance > _maxPlayerDistance) stopChasing();
        else if (playerDistance >= _minPlayerDistance)
        {
            Vector3 targetDir = (_targetTransform.position - _neck.position).normalized;
            Vector3 neckDir = _neck.up.normalized; // this is currently up because rotation is weird
            float angle = Vector3.Angle(new Vector3(neckDir.x, targetDir.y, neckDir.z), targetDir);
            if (angle > _halfAngleFOV)
            {
                stopChasing();
                return;
            }
            
            RaycastHit raycastHit;
            bool hit = Physics.Raycast(_neck.position, targetDir, out raycastHit, playerDistance + 10f, _targetRaycastLayerMask);
            if (hit && raycastHit.collider.CompareTag("Player"))
            {
                startChasing();
                
                _agent.SetDestination(_targetTransform.position);
            }
        }
    }
    
    void FixedUpdate()
    {
        Update();
    }
}

/*// rotation
Vector3 direction = _targetMove - transform.position;
Quaternion endRot = Quaternion.LookRotation(direction, Vector3.up);
transform.rotation = Quaternion.Slerp(transform.rotation, endRot, _rotSpeed * Time.fixedDeltaTime);

// position
transform.position = Vector3.MoveTowards(transform.position, _targetMove, _speed * Time.fixedDeltaTime);*/