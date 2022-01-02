using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.AI;

public abstract class DinosaurController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] protected Transform _neck;
    [SerializeField] protected Transform _targetTransform;
    [SerializeField] protected LayerMask _targetRaycastLayerMask;
    [SerializeField] protected NavMeshAgent _agent;
    [SerializeField] protected SliderBar _staminaBar;
    
    // Movement Fields
    protected float _chaseSpeed;
    protected float _wanderSpeed;
    
    protected float _maxPlayerDistance;
    
    protected float _staminaMax; // maximum stamina time in seconds dinosaur has to chase
    protected float _staminaRecovery; // how many seconds of stamina gained per second of slow movement
    protected float _staminaStartChase; // amount of stamina required for dinosaur to begin chasing
    
    // Sensing Fields
    protected float _halfAngleFOV;
    
    // Movement Variables
    protected Vector3 _targetPos;
    protected float _stamina;
    protected bool _isChasing = false;
    public bool IsChasing
    {
        get { return _isChasing; }
        protected set { _isChasing = value; }
    }
    protected float _speed = 0f;

    private void regainStamina(float max)
    {
        _stamina += Mathf.Min(_staminaRecovery * Time.fixedDeltaTime, max - _stamina); // caps _stamina at max
    }
    
    protected abstract void Chase(Vector3 targetPos);
    
    protected void startChasing()
    {
        if (_isChasing)
        {
            if (_stamina <= 0)
            {
                _speed = _wanderSpeed;
                _agent.speed = _speed;
            }
            
            if (_speed == _wanderSpeed)
            {
                if (_stamina < _staminaStartChase)
                    regainStamina(_staminaStartChase);
                else
                {
                    _speed = _chaseSpeed;
                    _agent.speed = _speed;
                }
            }
            else
            {
                print(_stamina);
                _stamina -= Time.fixedDeltaTime;
            }
            
            Chase(_targetPos);
            return;
        }
        
        _isChasing = true;
        _agent.isStopped = false;
    }
    
    protected abstract void Wander();
    
    protected void stopChasing() // both stops and wanders
    {
        if (!_isChasing)
        {
            Wander();
            regainStamina(_staminaMax);
            return;
        }
        
        _agent.isStopped = true;
        _isChasing = false;
        _speed = _wanderSpeed;
        _agent.speed = _speed;
    }

    protected void Start()
    {
        _stamina = _staminaMax;
        _staminaBar.SetMax(_staminaMax);
        stopChasing();
    }

    protected void UpdateDinosaur() // can be overriden, NOTE: DO NOT NAME THIS FUNCTION "Update"
    {
        float playerDistance = Vector3.Distance(transform.position, new Vector3(_targetTransform.position.x, transform.position.y, _targetTransform.position.z));
        if (playerDistance > _maxPlayerDistance)
            stopChasing();
        else
        {
            Vector3 targetDir = (_targetTransform.position - _neck.position).normalized;
            Vector3 neckDir = _neck.up.normalized; // this is currently up because rotation is weird
            float angle = Vector3.Angle(new Vector3(neckDir.x, targetDir.y, neckDir.z), targetDir);
            if (angle > _halfAngleFOV)
                stopChasing();
            else
            {
                RaycastHit raycastHit;
                bool hit = Physics.Raycast(_neck.position, targetDir, out raycastHit, playerDistance + 10f, _targetRaycastLayerMask);
                if (hit && raycastHit.transform == _targetTransform)
                    _targetPos = _targetTransform.position; // reset target position because dinosaur can see where
                
                startChasing();
            }
        }
        
        _staminaBar.SetVal(_stamina);
    }
    
    void FixedUpdate()
    {
        UpdateDinosaur();
    }
}

/*// rotation
Vector3 direction = _targetMove - transform.position;
Quaternion endRot = Quaternion.LookRotation(direction, Vector3.up);
transform.rotation = Quaternion.Slerp(transform.rotation, endRot, _rotSpeed * Time.fixedDeltaTime);

// position
transform.position = Vector3.MoveTowards(transform.position, _targetMove, _speed * Time.fixedDeltaTime);*/