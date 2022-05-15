using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoofBehavior : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private float _y;
    
    // Start is called before the first frame update
    void Start()
    {
        Vector3 playerPos = _player.transform.position;
        transform.position = new Vector3(playerPos.x, _y, playerPos.z);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerPos = _player.transform.position;
        transform.position = new Vector3(playerPos.x, _y, playerPos.z);
    }
}
