using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelButton : ButtonParent
{
    [Range(0, 99)]
    public int LevelNumber;
    public Sprite lockedSprite;
    public bool locked = true;

    private Sprite[] numberSprites;
    private SpriteRenderer _rend;
    private float spriteHeight;
    private Vector3 highlightVec;
    private PolygonCollider2D thisCollider;

    // Start is called before the first frame update
    void Start()
    {
        _rend = GetComponent<SpriteRenderer>();
        Sprite[] numberSprites = Resources.LoadAll<Sprite>("LevelButtons1");

        // Determines the left and right sides of the level button by the number assigned to the button
        char[] nameChars = levelName(LevelNumber);
        string left = "l" + int.Parse(nameChars[0].ToString());
        string right = "r" + int.Parse(nameChars[1].ToString());
        int breakLoop = 0;

        // Assigns the proper sprites to the left and rights sides of the button.
        foreach (Sprite number in numberSprites) {
            if (number.name == left) {
                transform.Find("left").GetComponent<SpriteRenderer>().sprite = number;
                breakLoop++;
            } else if (number.name == right) {
                transform.Find("right").GetComponent<SpriteRenderer>().sprite = number;
                breakLoop++;
            }

            if (breakLoop == 2) break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (locked) {
            _rend.sprite = lockedSprite;
        } else {
            _rend.sprite = null;
        }
    }

    char[] levelName(int val) {
        string name = val.ToString("00");
        return name.ToCharArray();
    }

    public void AttemptLoad() {
        if (!locked) {
            StartCoroutine(LoadYourAsyncScene());
        }
    }
}
