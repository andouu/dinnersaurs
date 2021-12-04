using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairCollide : MonoBehaviour
{
    public float maxRange = 10f;

    private GameObject queriedObj;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        RaycastHit hit;

        if (Physics.Raycast(transform.position, fwd, out hit, maxRange) && hit.collider.tag == "Egg")
        {
            if (queriedObj != null)
            {
                queriedObj.GetComponent<EggDDR>().setState("idle");
            }
            queriedObj = hit.collider.gameObject;
            queriedObj.GetComponent<EggDDR>().setState("hovering");
            // print(hit.collider);
        }
        else if (queriedObj != null)
        {
            queriedObj.GetComponent<EggDDR>().setState("idle");
            queriedObj = null;
        }
    }
}
