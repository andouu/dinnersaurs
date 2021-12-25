using UnityEngine;
using UnityEngine.UI;

public class LetterBehavior : MonoBehaviour
{
    public Sprite LetterSprite
    {
        get { return _letterSprite; }
        set { _letterSprite = value; }
    }
    public DisplayMoves Display;
    public int Index; // which index this letter is in the sequence;
    public Color ActiveColor; // start color
    public Color InactiveColor; // color the letter should be after it's pressed
    public float FadeTime;

    private Sprite _letterSprite;
    private Image _imageRend;
    private Color matColor;

    void Awake()
    {
        _imageRend = GetComponent<Image>();
    }
    
    void Start()
    {
        _imageRend.sprite = _letterSprite;
        matColor = ActiveColor;
    }

    private float transition = 0f; // tracks time passed since the key has been pressed
    void Update()
    {
        if (Display.currIndex > Index) // TODO: insane visual effects
        {
            float lerpPct = transition / FadeTime;
            matColor = Color.Lerp(ActiveColor, InactiveColor, lerpPct);
            _imageRend.color = matColor;
            transition += Time.deltaTime;
        }
    }
}
