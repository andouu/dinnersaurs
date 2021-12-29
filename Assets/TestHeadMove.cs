using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestHeadMove : MonoBehaviour
{
    public GameObject neck;
    public GameObject target;

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = (target.transform.position - transform.position);
    }
}
