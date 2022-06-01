using System.Collections;
using UnityEngine;

public class EggEating : MonoBehaviour
{
    [SerializeField] int eggHunger;
    [SerializeField] GameObject dinosaur;

    //[SerializeField] float flashSpeed;
    //private SphereCollider mouth;
    [SerializeField] private DisplayGameover _gameoverDisplay;
    [SerializeField] private AudioSource chomp;

    private PredatorController _dinoBehavior;
    private int currHunger;
    
    private void Start()
    {
        currHunger = eggHunger;
        _dinoBehavior = dinosaur.GetComponent<PredatorController>();
        //mouth = gameObject.GetComponent<SphereCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject collided = other.gameObject;
        if (collided.CompareTag("Egg Projectile"))
        {
            currHunger--;
            _gameoverDisplay.DinosFed++;
            chomp.Play();
        
            if (currHunger <= 0)
            {
                if (collided.GetComponent<ProjectileBehavior>().HasTouchedGround)
                    return;
                
                _dinoBehavior.Slow(); 
            }
            
            Destroy(other.gameObject);
        }
    }

    public void Reset()
    {
        currHunger = eggHunger;
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
