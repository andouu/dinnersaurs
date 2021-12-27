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
    private CrosshairCollide _collisionController;

    [Header("Sounds")]
    public AudioSource ShootSound;

    void Awake()
    {
        _collisionController = GetComponentInChildren<CrosshairCollide>();
        _numAmmo = StartingAmmo;
    }

    void Update()
    {
        bool collectingEgg = false;
        if (_collisionController.QueriedObj && _collisionController.QueriedObj.tag == "Nest Egg")
        {
            collectingEgg = true;
        }
        if (Input.GetMouseButton(0) && _notShooting && !collectingEgg)
        {
            if (!UnlimitedAmmo && _numAmmo <= 0)
                return;
            StartCoroutine(Shoot(ShotDelay, Projectile));
        }
    }

    private IEnumerator Shoot(float shotDelay, GameObject proj)
    {
        _notShooting = false;

        _numAmmo--;
        ShootSound.Play(0);
        GameObject clone = Instantiate(proj, RightHand.transform);
        clone.transform.parent = null;

        yield return new WaitForSeconds(shotDelay / 1000f);
        _notShooting = true;
    }
}
