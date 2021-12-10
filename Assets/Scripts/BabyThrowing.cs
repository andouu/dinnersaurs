using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabyThrowing : MonoBehaviour
{
    [Header("Shooting Settings")]
    public int shotDelay; // ms between shots
    public GameObject projectile;
    public GameObject rightHand;

    private bool canShoot = true;

    [Header("Sounds")]
    public AudioSource shootSound;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0) && canShoot)
        {
            StartCoroutine(Shoot(shotDelay, projectile));
        }
    }

    private IEnumerator Shoot(float shotDelay, GameObject proj)
    {
        canShoot = false;

        shootSound.Play(0);
        GameObject clone = Instantiate(proj, rightHand.transform);
        clone.transform.parent = null;

        yield return new WaitForSeconds(shotDelay / 1000f);
        canShoot = true;
    }
}
