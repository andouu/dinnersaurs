using System.Collections;
using UnityEngine;

public class EggEating : MonoBehaviour
{
    [SerializeField] int eggHunger;
    [SerializeField] GameObject dinosaur;
    [SerializeField] GameObject number;
    [SerializeField] Material[] nums;

    [SerializeField] private float _respawnTime; // seconds
    [SerializeField] private GameObject _border;
    [SerializeField] float flashSpeed;
    private SphereCollider mouth;
    [SerializeField] private DisplayGameover _gameoverDisplay;

    private PredatorController _dinoBehavior;
    private int currHunger;
    
    private void Start()
    {
        currHunger = eggHunger;
        _dinoBehavior = dinosaur.GetComponent<PredatorController>();
        number.GetComponent<MeshRenderer>().material = nums[eggHunger - 1];
        mouth = gameObject.GetComponent<SphereCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Egg Projectile"))
        {
            currHunger--;
        
            if (currHunger <= 0) {
                // TODO: make the dinosaur go away
                _gameoverDisplay.DinosFed++;
                StartCoroutine(respawn(dinosaur));
                number.SetActive(false);
                _dinoBehavior.Active = false;
            }
            else number.GetComponent<MeshRenderer>().material = nums[currHunger - 1];
            
            Destroy(other.gameObject);
        }
    }

    private IEnumerator respawn(GameObject dinosaur)
    {
        yield return new WaitForSeconds(_respawnTime);
        Vector3 currTransform = transform.position;
        dinosaur.transform.position = new Vector3(currTransform.x, currTransform.y, _border.transform.position.z) + new Vector3(0, 0, 10);
        currHunger = eggHunger;
        number.SetActive(true);
        number.GetComponent<MeshRenderer>().material = nums[currHunger - 1];
        dinosaur.GetComponent<PredatorController>().Active = true;
    }
}
