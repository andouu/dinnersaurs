using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadTracking : MonoBehaviour
{
    [SerializeField] private Transform neck;
    [SerializeField] private Transform target;

    // Start is called before the first frame update
    private void LateUpdate()
    {

        neck.rotation = Quaternion.LookRotation((neck.position - target.position), Vector3.up);
        neck.Rotate(new Vector3(-60, 0, 0));

        // WTF IS THIS ILL FIX THIS LATER

        /*
        // get angle to target on the XZ plane
        Vector2 XZVector = new Vector2(neck.position.z, neck.position.x) - new Vector2(target.position.z, target.position.x);
        float XZAngle = Vector2.SignedAngle(neck.parent.transform.up, XZVector);
        Debug.Log(XZAngle);

        // clamp the absolute value of the angle between 0 and 180
        //XZAngle = Mathf.Clamp(Mathf.Abs(XZAngle), 90, 180);

        // rotate around the world Y axis (X is -90 because the bone is weird)
        //neck.rotation = Quaternion.Slerp(neck.rotation, Quaternion.Euler(-90, XZAngle * sign, 0), headRotSpeed * Time.deltaTime);
        neck.localRotation = Quaternion.Euler(-45, 0, XZAngle + 180);*/
    }
}
