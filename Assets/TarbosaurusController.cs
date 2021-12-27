using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// THIS IS TEMPORARY CODE LOL
public class TarbosaurusController : MonoBehaviour
{
    public GameObject target;
    public float speed;

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z), speed * Time.deltaTime);
        transform.LookAt(target.transform.position, Vector3.up);
    }
}
