using System;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.AI;

public class TarbosaurusController : DinosaurController
{
    private void Awake()
    {
        // Movement Fields
        _chaseSpeed = 2.3f;
        _wanderSpeed = 1f;
        _minPlayerDistance = 7f;
        _maxPlayerDistance = 30f;
        
        // Sensing Fields
        _halfAngleFOV = 30f;
    }
    
    private void Start()
    {
        base.stopChasing();
    }

    protected override void Wander()
    {
        // TODO wandering movement
    }
}
