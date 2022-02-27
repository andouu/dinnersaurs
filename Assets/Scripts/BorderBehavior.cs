using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorderBehavior : MonoBehaviour
{
    [SerializeField] private float _moveSpeed; // dz/s

    private Vector3 _step;

    private void Awake()
    {
        _step = new Vector3(0, 0, _moveSpeed);
    }

    private void Update()
    {
        transform.position += _step * Time.deltaTime;
    }
}
