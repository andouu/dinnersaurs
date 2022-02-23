using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EggEating : MonoBehaviour
{
    [SerializeField] int eggHunger;
    [SerializeField] GameObject dinosaur;
    [SerializeField] GameObject number;
    [SerializeField] Material[] nums;

    [SerializeField] float flashSpeed;
    private SphereCollider mouth;

    private void Start()
    {
        number.GetComponent<MeshRenderer>().material = nums[eggHunger - 1];
        mouth = gameObject.GetComponent<SphereCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Egg Projectile") {

            eggHunger--;

            if (eggHunger <= 0) {
                // TODO: make the dinosaur go away
                dinosaur.SetActive(false);
            }
            else number.GetComponent<MeshRenderer>().material = nums[eggHunger - 1];
            
            Destroy(other.gameObject);
        }
    }

}
