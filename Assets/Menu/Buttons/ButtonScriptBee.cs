using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonScriptBee : ButtonParent
{

    [Range(0.0f, 0.1f)]
    public float highlight;

    private SpriteRenderer spriteRenderer;
    private float spriteHeight;
    private Vector3 highlightVec;

    private CapsuleCollider2D thisCollider;
    private float colliderOffset;
    private float factor;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        thisCollider = GetComponent<CapsuleCollider2D>();

        // Calculates the current sprite height, as well as the offset of the collider
        // The collider is offset to prevent jittering when the sprite moves
        spriteHeight = spriteRenderer.sprite.bounds.size.y;
        colliderOffset = thisCollider.offset.y;

        // When the sprite is highlighted, the collider will grow to cover the whole sprite.
        // This factor is the proportion by which it changes.
        factor = thisCollider.size.y / spriteHeight;

        // This is how much it moves when highlighted.
        highlightVec = new Vector3(0, highlight * spriteHeight, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (!set) {
            if (active) {
                // Recenters the collider and expands it to the size of the whole sprite.
                thisCollider.offset = new Vector2(0, 0);
                Vector2 currentSize = thisCollider.size;

                currentSize.y = currentSize.y / factor;
                thisCollider.size = currentSize;

                // Offsets the sprite to indicate it is chosen
                transform.position = transform.position + highlightVec;
            } else {
                // Replaces the offset and shrinks the collider
                thisCollider.offset = new Vector2(0, colliderOffset);
                Vector2 currentSize = thisCollider.size;

                currentSize.y = currentSize.y * factor;
                thisCollider.size = currentSize;

                // Moves the sprite back to where it was
                transform.position = transform.position - highlightVec;
            }
             
            set = true;
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        active = true;
        set = false;
    }

    void OnTriggerExit2D(Collider2D other) {
        active = false;
        set = false;
    }

    // The existence of AttemptActivate and AttemptLoad are artifacts of a poor design decision, wherein
    // the main menu and level menu used different user interfaces
    public override void AttemptLoad() {
        AttemptActivate();
    }

    public void AttemptActivate() {
        bool tutorial = false;
        if (gameObject.name == "continue_button"){
            int i = 0;
            while (i < 28){
                if (!LevelManager.levels[i]){
                    LevelInformation.levels[i+1].SetUpLevel(i+1);
                    break;
                }
                else{
                    i++;
                }
            }
            // Level 1 (Tutorial #1)
            if (i + 1 == 1){
                SceneManager.LoadScene("tutorial");
                tutorial = true;
            }
            // Level 2 (Tutorial #2)
            if (i + 1 == 2){
                // SceneManager.LoadScene("INSERT NAME OF SCENE HERE");
                // tutorial = true;
            }
        }
        if (active && !tutorial) {
            StartCoroutine(LoadYourAsyncScene());
        }
    }
}
