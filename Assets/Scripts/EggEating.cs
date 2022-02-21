using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EggEating : MonoBehaviour
{
    [SerializeField] int eggHunger;
    [SerializeField] Transform player;
    [SerializeField] GameObject dinosaur;
    [SerializeField] GameObject counterPrefab;
    [SerializeField] Vector3 counterDisplacement;

    private SphereCollider mouth;
    private Text hungerCounter;

    private void Start()
    {
        mouth = gameObject.GetComponent<SphereCollider>();
        hungerCounter = Instantiate(counterPrefab, FindObjectOfType<Canvas>().transform).GetComponent<Text>();
        //hungerCounter.text = eggHunger.ToString();
    }

    private void Update()
    {
        hungerCounter.transform.position = Camera.main.WorldToScreenPoint(transform.position + counterDisplacement);
        float distance = Vector3.Distance(player.position, dinosaur.transform.position);
        hungerCounter.fontSize = (int) (500f * (1 / distance));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Egg Projectile") {
            StartCoroutine("flashRed");
            Destroy(other.gameObject);
            eggHunger--;
            //hungerCounter.text = eggHunger.ToString();
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
