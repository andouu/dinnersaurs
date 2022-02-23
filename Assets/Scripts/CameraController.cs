using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private BasicCharacterController _playerController;

    [Header("Camera Rotation")]
    [SerializeField] private float _xSens = 0.3f;
    [SerializeField] private float _ySens = 0.15f;
    [SerializeField] private float _smoothing = 6f;
    private Vector2 smoothVector;
    private Vector2 mouseLook;

    [Header("Bobbing Head")]
    [SerializeField] private float _sneakYOffset = -0.2f;
    [SerializeField] private float _amplitude;
    [SerializeField] private float _period;
    [SerializeField] private Vector3 _restPosition = new Vector3(0, 0.5f, 0);
    [SerializeField] private float _smoothTime;
    private Vector3 _velocity = Vector3.zero;


    private float delta = 0f; // current x value of sinusoidal function(dynamic camera) 

    void Update()
    {
        // smooth camera lerp
        if (_playerController.MovementState != MovementState.Frozen)
        {
            Vector2 movement = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
            movement = Vector2.Scale(movement, new Vector2(_xSens * _smoothing, _ySens * _smoothing));

            smoothVector.x = Mathf.Lerp(smoothVector.x, movement.x, 1f / _smoothing);
            smoothVector.y = Mathf.Lerp(smoothVector.y, movement.y, 1f / _smoothing);

            mouseLook += smoothVector;
            mouseLook.y = Mathf.Clamp(mouseLook.y, -90f, 90f);

            transform.localRotation = Quaternion.AngleAxis(-mouseLook.y, Vector3.right);
            _playerTransform.localRotation = Quaternion.AngleAxis(mouseLook.x, _playerTransform.up);
        }

        // bobbing head
        delta += Time.deltaTime;

        delta %= 2f * Mathf.PI;

        if (_playerController.MovementState == MovementState.Walking)
            transform.localPosition = new Vector3(0, calcSinusoidal(delta, _amplitude, _period) + _restPosition.y, 0);
        else if (_playerController.MovementState == MovementState.Sprinting)
            transform.localPosition = new Vector3(0, calcSinusoidal(delta, _amplitude * 1.25f, _period * 0.75f) + _restPosition.y, 0);
        else if (_playerController.MovementState == MovementState.Sneaking)
            transform.localPosition = new Vector3(0, calcSinusoidal(delta, _amplitude * 0.5f, _period * 2f) + _restPosition.y + _sneakYOffset, 0);
        else
            transform.localPosition = Vector3.SmoothDamp(transform.localPosition, _restPosition, ref _velocity, _smoothTime);
    }

    private float calcSinusoidal(float delta, float amp, float per)
    {
        float b = 2f * Mathf.PI / per;
        return amp * Mathf.Sin(b * delta);
    }
}
