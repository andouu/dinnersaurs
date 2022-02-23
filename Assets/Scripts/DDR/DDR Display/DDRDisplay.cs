using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: This script can be merged with DisplayMoved
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
        moveDisplay.Display(egg.gameObject.transform.position);

        gameObject.SetActive(true);
        _visible = true;
    }

    public void Hide()
    {
        //gameObject.SetActive(false);
        _visible = false;
    }
}
