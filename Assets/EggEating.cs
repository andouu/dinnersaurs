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
        MeshRenderer renderer = gameObject.GetComponent<MeshRenderer>();
        renderer.material.color = new Color(1, 0, 0, renderer.material.color.a);
        yield return new WaitForSeconds(0.2f);
        gameObject.GetComponent<MeshRenderer>().material.color = new Color(1, 1, 1, renderer.material.color.a);
    }
}
