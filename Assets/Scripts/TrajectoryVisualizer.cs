using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryVisualizer : MonoBehaviour
{
    [Header("Components")]
    public GameObject ProjectilePrefab;       // the gameobject of the projectile that is shot
    public LineRenderer LineRend;
    public GameObject Origin;                 // the gameobject that the projectile spawns on
    public GameObject Player;

    [Header("Visuals")]
    public float TimeSimulated = 3f;          // how far the arc should be drawn
    public int Segments = 20;                 // how many segments of the arc should be drawn, basically how smooth the arc should be

    [Header("Physics")]
    [Range(0f, 1f)] public float InstantVelocityAccuracy = 0.5f;

    private ProjectileBehavior _projBehav;    // the projectile behavior script of the egg to be thrown 
    private Rigidbody _projRb;                // the rigidbody of the projectile
    private Camera _mainCam;                  // the main camera, the "eyes" of the player

    private void Awake()
    {
        _mainCam = Camera.main;
        _projBehav = ProjectilePrefab.GetComponent<ProjectileBehavior>();
        _projRb = ProjectilePrefab.GetComponent<Rigidbody>();
        initRenderer();
    }

    void Update()
    {
        Vector3 camForward = calcAngledForward();

        Vector3 force = calcForce(camForward, _projBehav.launchForce);
        Vector3 vel = estimateInstantVelocity(force);

        updateRenderer(vel);
    }

    private void initRenderer() // initialize the line renderer
    {
        LineRend.useWorldSpace = true;
        LineRend.positionCount = Segments;
    }

    private void updateRenderer(Vector3 vel)
    {
        // physics: http://hyperphysics.phy-astr.gsu.edu/hbase/traj.html#tracon

        float deltaTime = TimeSimulated / Segments;
        float timePassed = 0f;

        float aY = -Physics.gravity.y; // y acceleration

        for (int segment = 0; segment < Segments; segment++)
        {
            float xPos = vel.x * timePassed;
            float yPos = vel.y * timePassed - 0.5f * aY * (timePassed * timePassed);
            float zPos = vel.z * timePassed;

            Vector3 newPos = Origin.transform.position + new Vector3(xPos, yPos, zPos);

            LineRend.SetPosition(segment, newPos);
            timePassed += deltaTime;
        }
    }

    private Vector3 calcAngledForward() // degs
    {
        Vector3 cF = _mainCam.transform.forward;
        Vector3 angled = Quaternion.AngleAxis(_mainCam.transform.localEulerAngles.x - 2f * _projBehav.angleAdd, Vector3.right) * Vector3.one;
        return new Vector3(cF.x, angled.y, cF.z).normalized;
    }

    private Vector3 estimateInstantVelocity(Vector3 force)
    {
        float eps = InstantVelocityAccuracy;
        return force * eps / _projRb.mass;
    }

    private Vector3 calcForce(Vector3 dir, float impulse) // calculate F_avg for impulse
    {
        if (dir.magnitude != 1f)
            Vector3.Normalize(dir);
        return dir * impulse; 
    }
}
    
