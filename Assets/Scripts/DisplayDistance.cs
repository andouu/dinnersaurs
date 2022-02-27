using UnityEngine;
using TMPro;

public class DisplayDistance : MonoBehaviour
{
    [SerializeField] private BasicCharacterController _characterController;
    [SerializeField] private TextMeshProUGUI _textCounter;
    
    void Update()
    {
        _textCounter.text = ((int) _characterController.Distance).ToString();
    }
}
