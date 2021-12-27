using System.Collections.Generic;
using UnityEngine;

public class EggDDR : DDRComponent
{
    [Header("DDR Settings")]
    public int StartLength; // starting length for the DDR 
    [Space(10)]
    [Header("Indicator Settings")]
    public DDRDisplay Display; // script that Displays the sequence

    [HideInInspector]
    public string State
    {
        get { return _state; }
        set { _state = value; }
    }

    private BasicCharacterController _playerController;
    private CameraController _camController;
    private string _state; // state of the egg (changes on hover): idle, hovering, or active (pressed)
    private MeshRenderer _meshRenderer;
    private List<char> _seq; // stores the DDR Sequence for the egg
    private CrosshairIndicate _crosshairIndicatorBehavior;

    public override void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _crosshairIndicatorBehavior = GameObject.FindGameObjectWithTag("Crosshair").GetComponentInChildren<CrosshairIndicate>();
        _playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<BasicCharacterController>();
        _camController = Camera.main.GetComponent<CameraController>();
        _state = "idle";
        _seq = GenerateSequence(StartLength);
    }

    public override void Update()
    {
        if (_state == "hovering")
        {
            unfreezeMovement();

            if (Display.Visible)
                Display.Hide();

            _meshRenderer.material.SetFloat("_OutlineWidth", 4f);

            if (_crosshairIndicatorBehavior.Full)
            {
                _state = "active";
                _crosshairIndicatorBehavior.InstantReset();
            }

            if (Input.GetMouseButton(0) && _state == "hovering")
            {
                _crosshairIndicatorBehavior.LoadPct += _crosshairIndicatorBehavior.PctPerTime / 100f * Time.deltaTime;
            }
            if (Input.GetMouseButtonUp(0))
            {
                _crosshairIndicatorBehavior.LoadPct = 0f;
            }
        }
        else if (_state == "active")
        {
            freezeMovement();

            if (!Display.Visible)
                Display.Show(_seq, gameObject.GetComponent<EggDDR>());

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Resume();
            }
        }
        else
        {
            _meshRenderer.material.SetFloat("_OutlineWidth", 0f);
        }
    }

    private List<char> GenerateSequence(int len)
    {
        List<char> seq = new List<char>();
        for (int i = 0; i < len; i++)
        {
            int randIndex = Random.Range(0, DDRChars.Count); // get the index of a random char in the DDRChars list
            seq.Add(DDRChars[randIndex]);
        }
        return seq;
    }

    private void logSeq() // logs the DDR Sequence of the egg
    {
        string statement = ">> [ ";
        foreach (char c in _seq)
        {
            statement += c + ", ";
        }
        statement += "]";

        print(statement);
    }

    private void freezeMovement()
    {
        if (!_camController.IsFrozen)
            _camController.IsFrozen = true;
        if (!_playerController.IsFrozen)
            _playerController.IsFrozen = true;
    }

    private void unfreezeMovement()
    {
        if (_camController.IsFrozen)
            _camController.IsFrozen = false;
        if (_playerController.IsFrozen)
            _playerController.IsFrozen = false;
    }

    public void Resume()
    {
        _state = "hovering";
        unfreezeMovement();
        Display.Hide();
    }
}
