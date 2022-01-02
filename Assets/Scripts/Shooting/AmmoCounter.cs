using UnityEngine;
using TMPro;

public class AmmoCounter : MonoBehaviour
{
    public BabyThrowing ThrowBehavior;
    
    [SerializeField]
    private TextMeshProUGUI _textCounter;

    // Start is called before the first frame update
    void Start()
    {
        _textCounter.text = ThrowBehavior.StartingAmmo.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        _textCounter.text = ThrowBehavior.AmmoCount.ToString();
    }
}
