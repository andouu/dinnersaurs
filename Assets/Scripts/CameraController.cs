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

    [Header("Camera Rotation")]
    [SerializeField]
    private float _xSens;
    [SerializeField]
    private float _ySens;
    public float smoothing;
    private Vector2 smoothVector;
    private Vector2 mouseLook;

    [Header("Bobbing Head")]
    [SerializeField]
    private float _amplitude;
    [SerializeField]
    private float _period;
    [SerializeField]
    private Vector3 _restPosition = new Vector3(0, 0.5f, 0);
    [SerializeField]
    private float _smoothTime;
    private Vector3 _velocity = Vector3.zero;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _isFrozen = false;
    }

    private float delta = 0f; // current x value of sinusoidal function(dynamic camera) 

    void Update()
    {
        // smooth camera lerp
        if (!_isFrozen)
        {
            Vector2 movement = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
            movement = Vector2.Scale(movement, new Vector2(_xSens * smoothing, _ySens * smoothing));

            smoothVector.x = Mathf.Lerp(smoothVector.x, movement.x, 1f / smoothing);
            smoothVector.y = Mathf.Lerp(smoothVector.y, movement.y, 1f / smoothing);

            mouseLook += smoothVector;
            mouseLook.y = Mathf.Clamp(mouseLook.y, -90f, 90f);

            transform.localRotation = Quaternion.AngleAxis(-mouseLook.y, Vector3.right);
            Player.localRotation = Quaternion.AngleAxis(mouseLook.x, Player.up);
        }

        // bobbing head
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
