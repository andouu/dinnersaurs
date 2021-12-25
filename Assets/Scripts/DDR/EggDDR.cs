using System.Collections.Generic;
using UnityEngine;

public class EggDDR : DDRComponent
{
    public int StartLength; // starting length for the DDR 
    public DDRDisplay Display; // script that Displays the sequence
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

    public override void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    // Start is called before the first frame update
    public override void Start()
    {
        _playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<BasicCharacterController>();
        _camController = Camera.main.GetComponent<CameraController>();
        _state = "idle";
        _seq = GenerateSequence(StartLength);
    }

    // Update is called once per frame
    public override void Update()
    {
        if (_state == "hovering")
        {
            unfreezeMovement();

            if (Display.Visible)
                Display.Hide();

            _meshRenderer.material.SetFloat("_OutlineWidth", 4f);
            if (Input.GetMouseButtonUp(0))
            {
                _state = "active";
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
        _camController.IsFrozen = false;
        Display.Hide();
    }
}