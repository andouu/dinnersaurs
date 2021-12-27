using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

public class CrosshairIndicate : MonoBehaviour
{
    [Header("Components")]
    public Image Indicator;

    [Header("Settings")]
    public float LoadTime; // time to go from 0% to 100%

    [Header("Visuals")]
    public Color DefaultColor;
    public Color SelectedColor;

    [HideInInspector]
    public bool Full = false; // whether the indicator is at 100% or not

    [HideInInspector]
    public bool Loading
    {
        get { return _loading; }
    }

    [HideInInspector]
    public float PctPerTime; // time it should take to load one percent, calculated at awake

    [HideInInspector]
    public float LoadPct
    { 
        get { return _pct; }
        set { _pct = value; }
    }

    [Range(0, 1f)]
    private float _pct; // how much the indicator has loaded
    private bool _loading = false; // whether the indicator is loading or not
    private float _vel; // velocity variable for smoothdamp

    private void Awake()
    {
        PctPerTime = 100f / LoadTime;
    }

    void Update()
    {
        _loading = Indicator.fillAmount == _pct;
        Full = Indicator.fillAmount == 1f;

        float smoothTime = calcSmoothTime(Indicator.fillAmount, _pct);
        float newPct = Mathf.SmoothDamp(Indicator.fillAmount, _pct, ref _vel, smoothTime);
        Indicator.fillAmount = Mathf.Clamp01(newPct);
    }

    private float calcSmoothTime(float originalPct, float targetPct)
    {
        float diff = Mathf.Abs(targetPct - originalPct);
        return LoadTime * diff;
    }

    public void InstantReset() // instantly resets the percentage of the indicator
    {
        Indicator.fillAmount = 0f;
        _pct = 0f;
    }
}
