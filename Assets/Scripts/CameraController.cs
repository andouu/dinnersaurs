using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform Player;
    public bool IsFrozen
    {
        get { return _isFrozen; }
        set { _isFrozen = value; }
    }
    [SerializeField]
    private bool _isFrozen;

    [SerializeField]
    private float xSens;
    [SerializeField]
    private float ySens;
    [SerializeField]
    private float lerpSpeed;

    private float xRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _isFrozen = false;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * xSens * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * ySens * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        if (!_isFrozen)
        {
            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            Player.Rotate(Vector3.up * mouseX);
        }
    }
}
