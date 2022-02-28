using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabyThrowing : MonoBehaviour
{
    [Header("Shooting Settings")]
    public int ShotDelay; // ms between shots
    public bool UnlimitedAmmo = true;
    public int StartingAmmo = 10;
    [Space(10)]
    private const float StartingAngleAdd = -17.5f;
    public float AngleVariation = 1f;               // the angle (rads) that the trajectory changes when the player holds down the fire button
    public float AimInterpolateTime = 1f;           // time it takes to go from lowest angle to highest angle

    [Header("Components")]
    public GameObject Projectile;
    public GameObject RightHand;
    public TrajectoryVisualizer TrajectoryBehavior;
    public ProjectileBehavior ProjBehav;
    
    public int AmmoCount
    {
        get { return _numAmmo; }
        set { _numAmmo = value; }
    }

    [Header("Sounds")]
    public AudioSource ShootSound;

    [SerializeField]
    private int _numAmmo = 0;
    public int AccumulatedAmmo => _accumulatedAmmo;
    private int _accumulatedAmmo = 0;
    private bool _notShooting = true;
    private CrosshairCollide _collisionController;

    private float _maxAngle;
    private float _deltaTheta;

    void Awake()
    {
        _collisionController = GetComponentInChildren<CrosshairCollide>();
        _numAmmo = StartingAmmo;
        _maxAngle = StartingAngleAdd + AngleVariation;
        _deltaTheta = AngleVariation / AimInterpolateTime;
    }

    private void Start()
    {
        ProjBehav.angleAdd = StartingAngleAdd;
    }

    void Update()
    {    
        bool collectingEgg = false;
        if (_collisionController.QueriedObj && _collisionController.QueriedObj.tag == "Nest Egg")
        {
            collectingEgg = true;
        }

        bool canShoot = _numAmmo > 0 || UnlimitedAmmo;

        if (Input.GetMouseButton(0) && !collectingEgg && canShoot)
        {
            TrajectoryBehavior.Show = true;

            float clampedAngle = Mathf.Clamp(ProjBehav.angleAdd + _deltaTheta * Time.deltaTime, StartingAngleAdd, _maxAngle);

            ProjBehav.angleAdd = clampedAngle;
            
            if (clampedAngle == _maxAngle)
            {
                _deltaTheta = -Mathf.Abs(_deltaTheta);
            }
            else if (clampedAngle == StartingAngleAdd)
            {
                _deltaTheta = Mathf.Abs(_deltaTheta);
            }
        }
        if (Input.GetMouseButtonUp(0) && !collectingEgg && canShoot)
        {
            if (!UnlimitedAmmo && _numAmmo <= 0)
                return;

            if (_notShooting)
                StartCoroutine(shoot(ShotDelay, Projectile));

            resetTrajectory();
        }
    }

    public void ChangeAmmo(int amt)
    {
        _numAmmo += amt;
        _accumulatedAmmo += amt;
    }
    
    private IEnumerator shoot(float shotDelay, GameObject proj)
    {
        _notShooting = false;

        _numAmmo--;
        ShootSound.Play(0);
        GameObject clone = Instantiate(proj, RightHand.transform);
        clone.transform.parent = null;

        yield return new WaitForSeconds(shotDelay / 1000f); // divide by 1000 since shotDelay is in milliseconds
        _notShooting = true;
    }
    
    private void resetTrajectory()
    {
        ProjBehav.angleAdd = StartingAngleAdd;
        TrajectoryBehavior.Show = false;
    }
}
