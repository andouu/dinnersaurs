using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DDRDisplay : MonoBehaviour
{
    public bool Visible
    {
        get { return _visible; }
    }

    [SerializeField]
    private DisplayMoves moveDisplay;
    private bool _visible = false;

    public void Show(List<char> seq, EggDDR egg)
    {
        moveDisplay.Seq = seq;
        moveDisplay.EggBehavior = egg;
        moveDisplay.Display();

        gameObject.SetActive(true);
        _visible = true;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        _visible = false;
    }
}
