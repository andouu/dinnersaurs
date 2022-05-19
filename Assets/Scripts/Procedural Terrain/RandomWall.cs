using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomWall : MonoBehaviour
{
    public bool isLeftSide = false;
    public string Type;
    
    [SerializeField] private List<GameObject> _go;
    
    private void Start()
    {
        int randomIndex = Random.Range(0, _go.Count);
        int randomDirection = Random.Range(0, 2) == 0 ? -1 : 1;

        GameObject selectedGo = _go[randomIndex];
        Vector3 localScale = selectedGo.transform.localScale;
        localScale = new Vector3(localScale.x, localScale.y * randomDirection, localScale.z);
        selectedGo.transform.localScale = localScale;
        
        Vector3 selectedLocalPos = transform.localPosition;
        if (Type == "left" && isLeftSide)
        {
            print("left");
            selectedLocalPos = new Vector3(
                -selectedLocalPos.y, selectedLocalPos.x, selectedLocalPos.z);
            selectedGo.transform.localRotation *= Quaternion.Euler(new Vector3(0, 0, -90f));
        }

        if (Type == "right" && !isLeftSide)
        {
            print("right");
            selectedLocalPos = new Vector3(selectedLocalPos.y,
                -selectedLocalPos.x, selectedLocalPos.z);
            selectedGo.transform.localRotation *= Quaternion.Euler(new Vector3(0, 0, 90f));
        }

        transform.localPosition = selectedLocalPos;
        _go[randomIndex].SetActive(true);
    }
}
