using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Player")]
    public Transform Player;
    public BasicCharacterController PlayerController;
    public bool IsFrozen
    {
        get { return _isFrozen; }
        set { _isFrozen = value; }
    }
    private bool _isFrozen;

    [Header("Default Settings")]
    [SerializeField]
    private float _xSens;
    [SerializeField]
    private float _ySens;

    [Header("Dynamic Camera")]
    [SerializeField]
    private float _amplitude;
    [SerializeField]
    private float _period;
    [SerializeField]
    private Vector3 _restPosition = new Vector3(0, 0.5f, 0);
    [SerializeField]
    private float _smoothTime;
    private Vector3 _velocity = Vector3.zero;

    private float _xRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _isFrozen = false;
    }

    private float delta = 0f; // current x value of sinusoidal function(dynamic camera) 

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * _xSens * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * _ySens * Time.deltaTime;

        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);
        if (!_isFrozen)
        {
            transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
            Player.Rotate(Vector3.up * mouseX);
        }

        delta += Time.deltaTime;
        if (delta >= 2f * Mathf.PI)
            delta -= 2f * Mathf.PI;

        if (PlayerController.IsWalking && !PlayerController.IsSprinting)
        {
            transform.localPosition = new Vector3(0, calcSinusoidal(delta, _amplitude, _period) + _restPosition.y, 0);
        }
        else if (PlayerController.IsSprinting)
            transform.localPosition = new Vector3(0, calcSinusoidal(delta, _amplitude, _period * 0.75f) + _restPosition.y, 0);
        else
            transform.localPosition = Vector3.SmoothDamp(transform.localPosition, _restPosition, ref _velocity, _smoothTime);
    }

    private float calcSinusoidal(float delta, float amp, float per)
    {
        float b = 2f * Mathf.PI / per;
        return amp * Mathf.Sin(b * delta);
    }
}
