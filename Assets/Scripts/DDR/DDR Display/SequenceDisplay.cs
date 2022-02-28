using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SequenceDisplay : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private DisplayMoves _moveDisplay;
    [SerializeField] private GameObject _charPrefab;

    [Header("Misc")]
    [SerializeField] private AnimationCurve _opacityCurve;
    [SerializeField] private float _smoothTime;
    [SerializeField] private float _fadeTime;

    // cache
    private bool canPress = true;
    private List<char> _seq = new List<char>();
    private List<DDRChar> _charData = new List<DDRChar>();
    private float _charWidth;
    private Vector3 _charStep;
    private Color _originalColor;
    
    private class DDRChar
    {
        public GameObject gameObject;
        public Vector3 velocity;

        public DDRChar(GameObject go)
        {
            gameObject = go;
            velocity = Vector3.zero;
        }
    }
    
    private void Awake()
    {
        _charWidth = _charPrefab.GetComponent<RectTransform>().rect.width;
        _charStep = new Vector3(1, 0, 0) * _charWidth / 2f;
        _originalColor = _charPrefab.GetComponent<Text>().color;
    }

    private void Update()
    {
        if (!canPress) return;
        
        int currIndex = _moveDisplay.CurrIndex;
        Vector3 localStart = calculateLocalStart(currIndex);
        for (int ch = 0; ch < _seq.Count; ch++)
        {
            DDRChar ddrChar = _charData[ch];
            GameObject clone = ddrChar.gameObject;
            Vector3 targetPosition = (currIndex - ch) * _charStep;
            clone.transform.localPosition = Vector3.SmoothDamp(clone.transform.localPosition, targetPosition,
                ref ddrChar.velocity, _smoothTime);

            Text text = clone.GetComponent<Text>();
            text.text = _seq[ch].ToString();
            
            Color textCol = text.color;
            float mappedDist = map(clone.transform.localPosition.x);
            textCol.a = _opacityCurve.Evaluate(mappedDist);
            text.color = textCol;
            
            clone.SetActive(true);
        }
    }

    public void Display(List<char> sequence)
    {
        if (sequence.Count <= 0)
        {
            return;
        }

        _seq = sequence;
        initCharData(sequence.Count);
        canPress = true;
    }

    public void Flash(int index, Color color, float time)
    {
        if (index < 0 || index > _seq.Count - 1)
        {
            return;
        }
        StopAllCoroutines();
        StartCoroutine(flashChar(index, color, time));
    }

    public void Stop()
    {
        StopAllCoroutines();
        StartCoroutine(fade(_fadeTime));
    }

    private IEnumerator flashChar(int index, Color flashColor, float flashTime)
    {
        DDRChar ddrChar = _charData[index];
        Text text = ddrChar.gameObject.GetComponent<Text>();
        text.color = _originalColor;
        Color targetColor = text.color;
        text.color = flashColor;
        float elapsedTime = 0f;
        while (elapsedTime < flashTime)
        {
            if (_moveDisplay.CurrIndex != index)
            {
                float mappedDist = map(ddrChar.gameObject.transform.localPosition.x);
                targetColor.a = _opacityCurve.Evaluate(mappedDist);
            }
            text.color = Color.Lerp(text.color, targetColor, elapsedTime / flashTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
    
    private IEnumerator fade(float fadeTime)
    {
        canPress = false;
        float elapsedTime = 0f;
        while (elapsedTime < fadeTime)
        {
            foreach (DDRChar c in _charData)
            {
                Text text = c.gameObject.GetComponent<Text>();
                Color targetCol = text.color;
                targetCol.a = 0;
                text.color = Color.Lerp(text.color, targetCol, elapsedTime / fadeTime);
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        gameObject.SetActive(false);
    }

    private void initCharData(int len)
    {
        foreach (DDRChar c in _charData)
        {
            Destroy(c.gameObject);
        }
        _charData.Clear();
        for (int i = 0; i < len; i++)
        {
            GameObject go = Instantiate(_charPrefab, gameObject.transform, false);
            go.transform.localPosition = -_charStep * i;
            go.SetActive(false);

            DDRChar clone = new DDRChar(go);
            _charData.Add(clone);
        }
    }

    private float map(float x)
    {
        return Mathf.Abs(x / (_charStep.x * 2f));
    }
    
    private Vector3 calculateLocalStart(int index)
    {
        return Vector3.zero + _charStep * index;
    }
}