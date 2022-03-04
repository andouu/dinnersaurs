using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomWall : MonoBehaviour
{
    [SerializeField] private List<GameObject> _go;

    private void Awake()
    {
        int randomIndex = Random.Range(0, _go.Count);
        int randomDirection = Random.Range(0, 2) == 0 ? -1 : 1;
        Vector3 localScale = _go[randomIndex].transform.localScale;
        localScale = new Vector3(localScale.x, localScale.y * randomDirection, localScale.z);
        _go[randomIndex].transform.localScale = localScale;
        _go[randomIndex].SetActive(true);
    }
}
