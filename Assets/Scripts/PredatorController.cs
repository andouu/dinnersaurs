using System;
using UnityEngine;

// If we give individual dinosaurs cool features then we can turn this into a parent class for the different dinosaurs
public class PredatorController : MonoBehaviour
{
    [SerializeField] private float _startSpeed;
    [SerializeField] private float _speedChange = 3;
    [SerializeField] private float _changeTime = 60; // seconds
    [SerializeField] private Transform target;

    public bool Active
    {
        get => _active;
        set => _active = value;
    }

    [SerializeField] SkinnedMeshRenderer _rend;
    [SerializeField] private GameObject _eye;
    [SerializeField] private GameObject _teeth;
    private bool _active = true;
    private float _targetSpeed;
    private float _currSpeed;
    private float _elapsedTime = 0f;    
    
    private void Awake()
    {
        _targetSpeed = _startSpeed + _speedChange;
    }

    void Update()
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
        
        if (_elapsedTime < _changeTime)
            _elapsedTime += Time.deltaTime;
        _currSpeed = Mathf.Lerp(_startSpeed, _targetSpeed, _elapsedTime / _changeTime);
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, _currSpeed * Time.deltaTime);
        transform.LookAt(target.transform.position, Vector3.up);
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
