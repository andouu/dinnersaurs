using System.Collections;
using UnityEngine;

public class EggEating : MonoBehaviour
{
    [SerializeField] int eggHunger;
    [SerializeField] GameObject dinosaur;
    [SerializeField] GameObject number;
    [SerializeField] Material[] nums;

    //[SerializeField] float flashSpeed;
    //private SphereCollider mouth;
    [SerializeField] private DisplayGameover _gameoverDisplay;

    private PredatorController _dinoBehavior;
    private int currHunger;
    
    private void Start()
    {
        currHunger = eggHunger;
        _dinoBehavior = dinosaur.GetComponent<PredatorController>();
        number.GetComponent<MeshRenderer>().material = nums[eggHunger - 1];
        //mouth = gameObject.GetComponent<SphereCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Egg Projectile"))
        {
            currHunger--;
        
            if (currHunger <= 0) {
                _gameoverDisplay.DinosFed++;
                _dinoBehavior.Slow(); 
                number.SetActive(false);
            }
            else number.GetComponent<MeshRenderer>().material = nums[currHunger - 1];
            
            Destroy(other.gameObject);
        }
    }

    public void Reset()
    {
        currHunger = eggHunger;
        number.GetComponent<MeshRenderer>().material = nums[eggHunger - 1];
        number.SetActive(true);
    }

    /*private IEnumerator respawn()
    {
        yield return new WaitForSeconds(_respawnTime);
        Vector3 currTransform = transform.position;
        dinosaur.transform.position = new Vector3(currTransform.x, currTransform.y, _border.transform.position.z) + new Vector3(0, 0, 10);
        currHunger = eggHunger;
        number.SetActive(true);
        number.GetComponent<MeshRenderer>().material = nums[currHunger - 1];
        dinosaur.GetComponent<PredatorController>().Active = true;
    }*/
}
