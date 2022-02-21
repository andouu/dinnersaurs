using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// If we give individual dinosaurs cool features then we can turn this into a parent class for the different dinosaurs
public class PredatorController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Transform target;

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
        transform.LookAt(target.transform.position, Vector3.up);
    }

    // TODO: Add very simple obstacle avoidance
}
