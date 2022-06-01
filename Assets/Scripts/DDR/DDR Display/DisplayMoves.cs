using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class DisplayMoves : DDRComponent
{
    [Header("Components")]
    public BabyThrowing ThrowBehavior;
    public EggDDR EggBehavior;
    [SerializeField] private Canvas miniDisplay;
    [SerializeField] private GameObject player;
    private BasicCharacterController _playerController;

    [Header("Settings")]
    [SerializeField] private int maxIndex;
    [SerializeField] private Color _rightColor;
    [SerializeField] private Color _wrongColor;
    [SerializeField] private float _flashTime;

    private SequenceDisplay _sequenceDisplay;

    [HideInInspector] public int CurrIndex = 0; // tracks which character the player is supposed to press
    public List<char> Seq
    {
        get { return _seq; }
        set { _seq = value; }
    }

    private List<char> _seq;

    public override void Awake()
    {
        _sequenceDisplay = miniDisplay.GetComponent<SequenceDisplay>();
        ThrowBehavior = GameObject.FindGameObjectWithTag("Player").GetComponent<BabyThrowing>();
        _playerController = player.GetComponent<BasicCharacterController>();
    }

    public override void Update()
    {
        char inputChar = getInput();
        if (inputChar == '\0') return;
        
        if (miniDisplay.gameObject.activeSelf)
        {
            if (inputChar == _seq[CurrIndex])
            {
                _sequenceDisplay.Flash(CurrIndex, _rightColor, _flashTime);
                CurrIndex++;
                if (CurrIndex >= maxIndex)
                {
                    _sequenceDisplay.Stop();
                    ThrowBehavior.ChangeAmmo(1);
                    //_playerController.GrabEgg();
                    EggBehavior.Resume();
                    Destroy(EggBehavior.gameObject);
                    reset();
                }
            }
            else
            {
                // reset currIndex
                _sequenceDisplay.Flash(CurrIndex, _wrongColor, _flashTime);
                reset();
            }
        }
    }

    private char getInput()
    {
        string fullInput = Input.inputString.ToUpper();
        if (fullInput.Length == 0)
        {
            return '\0';
        }

        return fullInput[0];
    }

    IEnumerator Flash(Text t, float rate) {
        while (t.color.g < 0.4) {
            t.color = new Color(t.color.r, t.color.g + rate * Time.deltaTime, t.color.b);
            yield return null;
        }
        while (t.color.g > 0) {
            t.color = new Color(t.color.r, t.color.g - rate * Time.deltaTime, t.color.b);
            yield return null;
        }
    }

    public void Display(Vector3 eggPos)
    {
        reset();

        if (_seq.Count <= 0)
            return;

        miniDisplay.gameObject.SetActive(true);
        _sequenceDisplay.Display(_seq.GetRange(0, maxIndex));
        miniDisplay.transform.position = eggPos + new Vector3(0, 1f, 0);
        miniDisplay.transform.LookAt(player.transform.position, Vector3.up);
    }

    private void reset()
    {
        CurrIndex = 0;
    }
}
