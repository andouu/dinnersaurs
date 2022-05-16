using System;
using System.Collections;
using UnityEngine;

// If we give individual dinosaurs cool features then we can turn this into a parent class for the different dinosaurs
public class PredatorController : MonoBehaviour
{
    [SerializeField] private Vector3 _resetPosition;
    [SerializeField] private Quaternion _resetRotation;

    [SerializeField] private float _startSpeed;
    [SerializeField] private float _slowTime = 10; // seconds
    [SerializeField] private float _slowSpeedMultipler = 0.6f;
    [SerializeField] private float _speedUpMultipler = 0.15f;
    [SerializeField] private float _speedCreep = 0.15f;
    [SerializeField] private float _speedGainPerSec = 0.02f;
    [SerializeField] private float _maxSpeed = 16f;
    [SerializeField] private Transform _player;
    private Transform target;

    public bool Active
    {
        get => _active;
        set => _active = value;
    }

    [SerializeField] private float _minProjectileDistance = 2f;
    [SerializeField] private EggEating _eggEating;
    [SerializeField] SkinnedMeshRenderer _rend;
    [SerializeField] private GameObject _eye;
    [SerializeField] private GameObject _teeth;
    private bool _active = true;
    private float _targetSpeed;
    private float _currSpeed;
    private bool _slowing = false;

    public void Reset()
    {
        target = _player;
        _targetSpeed = _startSpeed;
        transform.position = _resetPosition;
        transform.rotation = _resetRotation;
        _eggEating.Reset();
    }

    public void Slow()
    {
        _slowing = true;
        _targetSpeed *= _slowSpeedMultipler;
        _currSpeed = _targetSpeed;
        StartCoroutine(SpeedUp());
    }

    private IEnumerator SpeedUp()
    {
        yield return new WaitForSeconds(_slowTime);
        _targetSpeed /= _slowSpeedMultipler;
        _targetSpeed *= _speedUpMultipler;
        _currSpeed = _targetSpeed;
        _slowing = false;
        _eggEating.Reset();
    }

    private void Awake()
    {
        target = _player;
        _targetSpeed = _startSpeed;
    }

    private void Retarget(out bool targetIsProjectile)
    {
        targetIsProjectile = false;
        float minDist = Vector3.Distance(transform.position, _player.position);
        foreach (Transform projectile in ProjectileBehavior.projectiles)
        {
            float newDist = Vector3.Distance(transform.position, projectile.position);
            if (newDist < minDist)
            {
                minDist = newDist;
                target = projectile;
                targetIsProjectile = true;
            }
        }
    }
    
    private void Update()
    {
        if (!_active)
        {
            _rend.enabled = false;
            _eye.SetActive(false);
            _teeth.SetActive(false);
            return;
        }

        _eye.SetActive(true);
        _teeth.SetActive(true);
        _rend.enabled = true;
        
        _targetSpeed += _speedGainPerSec * Time.deltaTime; // dinosaurs speed up over time regardless
        _currSpeed = _targetSpeed; // TODO: make smooth speed change (acceleration)
        transform.position = Vector3.MoveTowards(transform.position, target.position, _currSpeed * Time.deltaTime);
        transform.LookAt(target.position, Vector3.up);
        if (!_slowing) _targetSpeed += _speedCreep * Time.deltaTime;
        _targetSpeed = Mathf.Clamp(_targetSpeed, 0f, _maxSpeed);
    }

    private void LateUpdate()
    {
        bool targetIsProjectile;
        Retarget(out targetIsProjectile);
        if (targetIsProjectile && Vector3.Distance(transform.position, target.position) < _minProjectileDistance)
        {
            Destroy(target.gameObject);
        }

        if (ProjectileBehavior.projectiles.Count == 0) target = _player;
    }

    private void OnCollisionStay(Collision collisionInfo)
    {
        if (collisionInfo.gameObject.CompareTag("Player"))
        {
            target.GetComponent<BasicCharacterController>().Die();
        }
    }

    // TODO: Add very simple obstacle avoidance
}
