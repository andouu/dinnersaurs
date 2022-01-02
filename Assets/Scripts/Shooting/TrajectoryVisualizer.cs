using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

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
    [Range(0f, 1f)] public float InstantVelocityAccuracy = 1f;

    [Header("Animations")]
    [Range(0f, 1f)] public float SegmentLength;
    public Color SegmentColor;
    [Min(0f)] public float SegmentEndDropoff;     // How sharply the opacity should cutoff at the end of the segment
    public float TimeBetween;                     // the time between each animation cycle (starts counting after fade) in seconds
    public float AnimateTime;                     // time to complete the animation (not including the fade)
    public float FadeTime;                        // how long it should take the line to fade after the animation
    public AnimationCurve AnimationTiming;
    public float AnimationCurveEpsilon = 0.01f;       // how accurately to get the rate of change from AnimationTiming (limit variable) 

    [HideInInspector]
    public bool Show
    {
        get { return _show; }
        set { _show = value; }
    }

    // private component fields
    private ProjectileBehavior _projBehav;    // the projectile behavior script of the egg to be thrown 
    private Rigidbody _projRb;                // the rigidbody of the projectile
    private Camera _mainCam;                  // the main camera, the "eyes" of the player

    // flags
    private bool _show = false;               // whether the line should render
    private bool _animating = false;          // whether the line is animating or not

    private float _curveLength = 0f;               // the length of the trajectory arc (approximated)

    private void Awake()
    {
        _mainCam = Camera.main;
        _projBehav = ProjectilePrefab.GetComponent<ProjectileBehavior>();
        _projRb = ProjectilePrefab.GetComponent<Rigidbody>();
        initRenderer();
    }

    void Update()
    {
        if (_show)
        {
            LineRend.enabled = true;
            if (!_animating)
            {
                StartCoroutine(animateArc());
            }

            Vector3 camForward = calcAngledForward();

            Vector3 force = calcForce(camForward, _projBehav.launchForce);
            Vector3 vel = estimateInstantVelocity(force);

            updateRenderer(vel);
        }
        else
        {
            LineRend.enabled = false;
        }
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
        _curveLength = 0f;

        float aY = -Physics.gravity.y; // y acceleration

        LineRend.SetPosition(0, Origin.transform.position);

        for (int segment = 1; segment < Segments; segment++)
        {
            float xPos = vel.x * timePassed;
            float yPos = vel.y * timePassed - 0.5f * aY * (timePassed * timePassed);
            float zPos = vel.z * timePassed;

            Vector3 newPos = Origin.transform.position + new Vector3(xPos, yPos, zPos);

            _curveLength += (newPos - LineRend.GetPosition(segment - 1)).magnitude;

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

    private IEnumerator animateArc()
    {
        _animating = true;

        bool useAnimationCurve = AnimationTiming.length >= 2;

        float timePassed = 0f;

        // TODO: cache the start gradient variables as a class field (lines 140 - 166) since they don't change at runtime, so there's no need to make new ones every call
        Gradient lineGradient;
        GradientColorKey[] colorKey;
        GradientAlphaKey[] alphaKey;

        lineGradient = new Gradient();

        // populate color keys
        colorKey = new GradientColorKey[3];
        colorKey[0].color = SegmentColor;
        colorKey[0].time = 0.01f;
        colorKey[1].color = SegmentColor;
        colorKey[1].time = SegmentLength;
        colorKey[2].color = SegmentColor;
        colorKey[2].time = 1f;

        // populate alpha keys
        Assert.IsTrue(SegmentLength + SegmentEndDropoff <= 1f); // ensure that the last alpha key is in range
        
        alphaKey = new GradientAlphaKey[4];
        alphaKey[0].alpha = 0f;
        alphaKey[0].time = 0f;
        alphaKey[1].alpha = 1f;
        alphaKey[1].time = 0.01f;
        alphaKey[2].alpha = 1f;
        alphaKey[2].time = SegmentLength;
        alphaKey[3].alpha = 0f;
        alphaKey[3].time = SegmentLength + SegmentEndDropoff;

        float deltaPct = 0f;

        if (!useAnimationCurve)
            deltaPct = (1f - SegmentLength) / AnimateTime;

        // animate one cycle
        while (timePassed < AnimateTime)
        {
            lineGradient.SetKeys(colorKey, alphaKey);
            LineRend.colorGradient = lineGradient;

            if (useAnimationCurve)
            {
                float pctDone = timePassed / AnimateTime;
                deltaPct = differentiate(AnimationTiming, pctDone);
            }

            float pctOverTime = deltaPct * Time.deltaTime;

            // update color key times
            for (int cKey = 0; cKey < 2; cKey++)
            {
                colorKey[cKey].time += pctOverTime;
            }

            // update alpha key times
            for (int aKey = 1; aKey < 4; aKey++)
            {
                alphaKey[aKey].time += pctOverTime;
            }

            timePassed += Time.deltaTime;

            yield return null;
        }

        // animate the fading
        timePassed = 0f; // reset time
        float deltaOpacity = 1 / FadeTime;

        while(timePassed < FadeTime)
        {
            lineGradient.SetKeys(colorKey, alphaKey);
            LineRend.colorGradient = lineGradient;

            float opacityOverTime = deltaOpacity * Time.deltaTime;

            for (int aKey = 1; aKey < 3; aKey++) // no need to change first and last key since they're already transparent
            {
                alphaKey[aKey].alpha -= opacityOverTime;
            }

            timePassed += Time.deltaTime;

            yield return null;
        }

        yield return new WaitForSeconds(TimeBetween);
        _animating = false;
    }

    private float differentiate(AnimationCurve curve, float x) // estimate slope of animation curve (differentiation by first principles)
    {
        float h = AnimationCurveEpsilon;
        return (curve.Evaluate(x + h) - curve.Evaluate(x)) / h; 
    }
}  
