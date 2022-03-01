using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class DisplayGameover : MonoBehaviour
{
    [SerializeField] private BasicCharacterController _playerController;
    [SerializeField] private BabyThrowing _eggThrowBehavior;
    [SerializeField] private Text _text; 
        
    [HideInInspector] public int DinosFed
    {
        get => _dinosFed;
        set => _dinosFed = value;
    }

    private int _dinosFed;

    private void Start()
    {
        Reset();
    }
    
    public void Reset()
    {
        int distance = (int) _playerController.Distance;

        _text.text = distance.ToString() + "\n" + _dinosFed.ToString() + "\n" + _eggThrowBehavior.AccumulatedAmmo
            .ToString();
    }
}
