using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TutorialManagerParent : MonoBehaviour
{
    // Prefab for the tutorial box
    public GameObject tutorialPrefab;
    // The art for each tutorial box
    public Texture2D[] textures;
    // The instantiated prefabs;
    protected GameObject[] tutorialBoxes;

    protected int stateFlag = 0;
    public GameObject GoalCanvas;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected void StateCheck() {
        if (!(stateFlag == -1)){
            if (stateFlag >= tutorialBoxes.Length) {
                TextUpdate[] boxes = FindObjectsOfType<TextUpdate>();

                foreach (TextUpdate box in boxes){
                    box.gameObject.SetActive(false);
                }
                if (!LevelManager.won){
                    TutorialOver();
                }
                stateFlag = -1;
            }
            else {
                for (int i = 0; i <= stateFlag; i++) {
                    tutorialBoxes[i].SetActive(true);
                }
            }
        }
    }

    protected void TutorialOver() {
        FindObjectOfType<LevelManagerActing>().Invoke("Win", 2f);
    }

    protected void GenerateTutorialBoxes() {
        tutorialBoxes = new GameObject[textures.Length];
        float angle = Mathf.PI / (textures.Length - 1);

        // Generates the objects for each tutorial box, with the right sprite.
        for (int i = 0; i < textures.Length; i++) {
            // Instantiates the boxes relative to the camera
            tutorialBoxes[i] = Instantiate(tutorialPrefab, new Vector3(0, 0, 0), Quaternion.identity, Camera.main.transform);
            SpriteRenderer _rend = tutorialBoxes[i].GetComponent<SpriteRenderer>();
            Vector3 boxSize = _rend.sprite.bounds.size;

            // Places the box away from the centre
            // Each box is progressively further around a circle surrounding the bee
            tutorialBoxes[i].transform.position = 1.5f * (1 - 1/PlayerPreferences.FOV) * new Vector3(-Mathf.Cos(i * angle) * boxSize.x, Mathf.Sin(i * angle) * boxSize.y,0);

            // Updates each box and hides them
            Sprite newSprite = Sprite.Create(textures[i], new Rect(0, 0, textures[1].width, textures[1].height), new Vector2(0.5f, 0.5f), 64);
            _rend.sprite = newSprite;
            tutorialBoxes[i].SetActive(false);
            tutorialBoxes[i].transform.localScale *= (1 - 1/PlayerPreferences.FOV);
        }
    }
}
