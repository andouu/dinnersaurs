using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayMoves : DDRComponent
{
    public BabyThrowing ThrowBehavior;
    //public List<Sprite> KeySprites;
    public EggDDR EggBehavior;
    //public GameObject LetterGO; // prefab of letter to be cloned
    public int currIndex = 0; // tracks which character the player is supposed to press
    [SerializeField] private int maxIndex;

    [SerializeField] private Canvas miniDisplay;
    [SerializeField] private GameObject player;
    [SerializeField] private float flashRate;
    private Text text;

    public List<char> Seq
    {
        get { return _seq; }
        set { _seq = value; }
    }

    private List<char> _seq;
    //private bool dictHasBuilt = false; // tracks whether the dictionary has been built;
    //private IDictionary<char, Sprite> _keyToSprite = new Dictionary<char, Sprite>(); // a dictionary that stores the gameobject a key is associated with

    public override void Awake()
    {
        //if (!dictHasBuilt)
        //    buildDictionary();
        text = miniDisplay.GetComponentInChildren<Text>();

        ThrowBehavior = GameObject.FindGameObjectWithTag("Player").GetComponent<BabyThrowing>();
    }

    public override void Update()
    {
        string input = Input.inputString.ToUpper();
        if (miniDisplay.gameObject.activeSelf)
        {
            foreach (char c in input)
            {
                if (c == _seq[currIndex])
                {
                    StartCoroutine(Flash(text, flashRate));

                    currIndex++;
                    if (currIndex >= maxIndex)
                    {
                        miniDisplay.gameObject.SetActive(false);
                        ThrowBehavior.AmmoCount++;
                        EggBehavior.Resume();
                        Destroy(EggBehavior.gameObject);
                        reset();
                    }
                    else
                        text.text = _seq[currIndex].ToString();
                }
            }
        }
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
        /*if (!dictHasBuilt)
            buildDictionary();*/

        reset();

        if (_seq.Count <= 0)
            return;

        miniDisplay.gameObject.SetActive(true);
        text.text = _seq[currIndex].ToString();
        miniDisplay.transform.position = eggPos + new Vector3(0, 1f, 0);
        miniDisplay.transform.LookAt(player.transform.position, Vector3.up);

        /*
        for (int move = 0; move < _seq.Count; move++)
        {
            GameObject clone = Instantiate(LetterGO);
            clone.transform.SetParent(gameObject.transform);

            LetterBehavior behavior = clone.GetComponent<LetterBehavior>();
            behavior.LetterSprite = _keyToSprite[_seq[move]];
            behavior.Index = move;
            behavior.Display = GetComponent<DisplayMoves>();
        }*/
    }

    /*
    private void buildDictionary() // builds _keyToGO (key to gameobject)
    {
        if (KeySprites.Count != DDRChars.Count) // check if building a dictionary is even possible
            throw new Exception("Length of sprite list is not equal to length of char list");
        for (int letter = 0; letter < DDRChars.Count; letter++)
        {
            _keyToSprite.Add(new KeyValuePair<char, Sprite>(DDRChars[letter], KeySprites[letter]));
        }
        print("Dictionary successfully built");
        dictHasBuilt = true;
    }

    private void destroyChildren()
    {
        int numChildren = gameObject.transform.childCount;
        foreach (Transform child in gameObject.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void logDict()
    {
        foreach (KeyValuePair<char, Sprite> kvp in _keyToSprite)
        {
            string str = "Key: " + kvp.Key + ", Value: " + kvp.Value;
            print(str);
        }
    }*/

    private void reset()
    {
        currIndex = 0;
        //destroyChildren();
    }
}
