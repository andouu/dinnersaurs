using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggEating : MonoBehaviour
{
    [SerializeField] int eggHunger;
    [SerializeField] GameObject dinosaur;

    private SphereCollider mouth;

    private void Start()
    {
        mouth = gameObject.GetComponent<SphereCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Egg Projectile") {
            StartCoroutine("flashRed");
            eggHunger--;
            if (eggHunger <= 0) {
                // make the dinosaur go away later
                dinosaur.SetActive(false);
            }
        }
    }

    IEnumerator flashRed() {
        gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        gameObject.GetComponent<MeshRenderer>().material.color = Color.white;
    }
}
