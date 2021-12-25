using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabyThrowing : MonoBehaviour
{
    [Header("Shooting Settings")]
    public int ShotDelay; // ms between shots
    public GameObject Projectile;
    public GameObject RightHand;
    public bool UnlimitedAmmo = true;
    public int StartingAmmo = 10;
    public int AmmoCount
    {
        get { return _numAmmo; }
        set { _numAmmo = value; }
    }

    [SerializeField]
    private int _numAmmo = 0;
    private bool _notShooting = true;

    [Header("Sounds")]
    public AudioSource ShootSound;

    void Start()
    {
        _numAmmo = StartingAmmo;
    }

    void Update()
    {
        if (Input.GetMouseButton(0) && _notShooting)
        {
            if (!UnlimitedAmmo && _numAmmo <= 0)
                return;
            StartCoroutine(Shoot(ShotDelay, Projectile));
        }
    }

    private IEnumerator Shoot(float shotDelay, GameObject proj)
    {
        _notShooting = false;

        ShootSound.Play(0);
        GameObject clone = Instantiate(proj, RightHand.transform);
        clone.transform.parent = null;

        yield return new WaitForSeconds(shotDelay / 1000f);
        _notShooting = true;
    }
}
