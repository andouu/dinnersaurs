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

        if (Physics.Raycast(transform.position, fwd, out hit, maxRange) && hit.collider.tag == "Nest Egg")
        {
            bool sameObj = ReferenceEquals(queriedObj, hit.collider.gameObject); // check if the ray is hitting the same object as before
            if (!sameObj)
            {
                if (queriedObj != null)
                {
                    queriedObj.GetComponent<EggDDR>().State = "idle";
                }
                queriedObj = hit.collider.gameObject;
            }
            EggDDR behavior = queriedObj.GetComponent<EggDDR>();
            if (behavior.State == "idle")
            {
                behavior.State = "hovering";
            }
        }
        else if (queriedObj != null)
        {
            queriedObj.GetComponent<EggDDR>().State = "idle";
            queriedObj = null;
        }
    }
}
