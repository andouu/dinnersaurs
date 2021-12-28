using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TarbosaurusController : MonoBehaviour
{
    [SerializeField, Tooltip("Targets by priority: 1st should be player, 2nd is nest")] private List<Transform> _targets;
    private Transform _targetTransform;
    private Vector3 _targetMove;
    private bool _isChasing = false;
    private float _speed = 0f;

    private Animator _animator;
    
    [Header("Movement Fields")]
    [SerializeField] private float _rotSpeed = 2f;
    [SerializeField] private float _chaseSpeed = 3.5f;
    [SerializeField] private float _wanderSpeed = 1.2f;
    [SerializeField] private float _minPlayerDistance = 7f;
    [SerializeField] private float _maxPlayerDistance = 30f;
    [SerializeField] private float _maxWanderDistance = 15f;

    private const float MinWanderDist = 2f;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        stopChasing();
    }

    private void setAnimatorSpeed()
    {
        float animSpeed = _speed / 2.5f; // TODO: calibrate so dinosaur movement matches footstep animation
        _animator.SetFloat("animSpeed", animSpeed);
    }

    private void startChasing()
    {
        _targetTransform = _targets[0];
        _isChasing = true;
        _speed = _chaseSpeed;
        setAnimatorSpeed();
    }
    
    private void stopChasing()
    {
        _targetTransform = _targets[1];
        _isChasing = false;
        _speed = _wanderSpeed;
        setAnimatorSpeed();
    }

    private void setNewWanderTarget()
    {
        Vector2 displacement = Random.insideUnitCircle * _maxWanderDistance;
        float x = _targetTransform.position.x + displacement.x;
        float z = _targetTransform.position.z + displacement.y;
        _targetMove = new Vector3(x, transform.position.y, z);
    }

    void FixedUpdate()
    {
        // choosing target TODO later: cycle through targets[1...] when too far from primary target
        float playerDistance = Vector3.Distance(transform.position, new Vector3(_targets[0].position.x, transform.position.y, _targets[0].position.z));
        if (playerDistance > _maxPlayerDistance)
        {
            if (_isChasing)
            {
                stopChasing();
                _targetMove = new Vector3(_targetTransform.position.x, transform.position.y, _targetTransform.position.z);
            }
            
            // wandering movement
            float dist = Vector3.Distance(transform.position, _targetMove);
            if (dist < MinWanderDist)
            {
                setNewWanderTarget();
            }
        }
        else
        {
            if (playerDistance < _minPlayerDistance)
            {
                return;
            }

            if (_isChasing)
            {
                _targetMove = new Vector3(_targetTransform.position.x, transform.position.y, _targetTransform.position.z);
            }
            else
            {
                startChasing();
            }
        }
        
        // rotation
        Vector3 direction = _targetMove - transform.position;
        Quaternion endRot = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, endRot, _rotSpeed * Time.fixedDeltaTime);
        //transform.LookAt(_targetMove, Vector3.up);
        
        // position
        transform.position = Vector3.MoveTowards(transform.position, _targetMove, _speed * Time.fixedDeltaTime);
    }
}
