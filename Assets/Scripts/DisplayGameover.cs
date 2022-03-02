using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class DisplayGameover : MonoBehaviour
{
    [SerializeField] private BasicCharacterController _playerController;
    [SerializeField] private BabyThrowing _eggThrowBehavior;
    [SerializeField] private Button cutsceneButton;
    [SerializeField] private Text _text;
    [SerializeField] private Text endMessage;
    [SerializeField] private Text highScoreText;
        
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

        if (distance > StaticVariables.highScore)
        {
            StaticVariables.highScore = distance;
            endMessage.text = "NEW HIGH SCORE!";
            highScoreText.text = "HIGH SCORE: " + StaticVariables.highScore;
        }
        else endMessage.text = "NICE JOB!";

        if (distance > 700) {
            cutsceneButton.interactable = true;
        }

        _text.text = distance.ToString() + "\n" + _dinosFed.ToString() + "\n" + _eggThrowBehavior.AccumulatedAmmo
            .ToString();        
    }
}
