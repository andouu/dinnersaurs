using System;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.AI;

public class DEPRECATEDTarbosaurusController : DEPRECATEDDinosaurController
{
    private void Awake()
    {
        // Movement Fields
        _chaseSpeed = 3f;
        _wanderSpeed = 1.8f;
        
        _maxPlayerDistance = 40f;
        
        _
            
            Max = 20f;
        _staminaRecovery = 0.75f;
        _staminaStartChase = 10f;
        
        // Sensing Fields
        _halfAngleFOV = 30f;
    }

    protected override void Chase(Vector3 targetPos)
    {
        _agent.SetDestination(targetPos);
    }

    protected override void Wander()
    {
        // TODO wandering movement
    }
}
