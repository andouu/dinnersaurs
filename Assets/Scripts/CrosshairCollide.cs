using UnityEngine;
using UnityEngine.UI;

public class CrosshairCollide : MonoBehaviour
{
    public float maxRange = 10f;
    public GameObject QueriedObj // the object that the raycast has hit, or null if no object was hit
    {
        get { return _queriedObj; }
    }

    private GameObject _queriedObj;
    private GameObject _crosshair; // crosshair UI element, which should have the crosshair image and crosshair indicator gameobject
    private Image _crosshairImage;
    private CrosshairIndicate _crosshairIndicatorBehavior;

    private void Awake()
    {
        _crosshair = GameObject.FindGameObjectWithTag("Crosshair");
        _crosshairImage = _crosshair.transform.GetChild(0).GetComponent<Image>();
        _crosshairIndicatorBehavior = _crosshair.GetComponentInChildren<CrosshairIndicate>();
        _crosshairImage.color = _crosshairIndicatorBehavior.DefaultColor;
    }

    private void FixedUpdate()
    {
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        RaycastHit hit;

        if (Physics.Raycast(transform.position, fwd, out hit, maxRange) && hit.collider.tag == "Next Egg")
        {
            bool sameObj = ReferenceEquals(_queriedObj, hit.collider.gameObject); // check if the ray is hitting the same object as before
            if (!sameObj)
            {
                if (_queriedObj != null)
                {
                    _queriedObj.GetComponent<EggDDR>().State = "idle";
                }
                _queriedObj = hit.collider.gameObject;
            }
            EggDDR behavior = _queriedObj.GetComponent<EggDDR>();
            if (behavior.State == "idle")
            {
                behavior.State = "hovering";
            }
            _crosshairImage.color = _crosshairIndicatorBehavior.SelectedColor;
        }
        else if (_queriedObj != null)
        {
            _queriedObj.GetComponent<EggDDR>().State = "idle";
            _queriedObj = null;
        }
        else
        {
            _crosshairImage.color = _crosshairIndicatorBehavior.DefaultColor;
            _crosshairIndicatorBehavior.LoadPct = 0f;
        }
    }
}
