using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonScript : MonoBehaviour
{

    [Range(0.0f, 0.1f)]
    public float highlight;

    public string loadScene;

    private SpriteRenderer spriteRenderer;
    private float spriteHeight;
    private Vector3 highlightVec;

    private CapsuleCollider2D thisCollider;
    private float colliderOffset;
    private float factor;
    private bool active = false;
    private bool set = true;

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

    void OnMouseEnter() {
        active = true;
        set = false;
    }

    void OnMouseExit() {
        active = false;
        set = false;
    }

    public void AttemptActivate() {
        if (active) {
            StartCoroutine(LoadYourAsyncScene());
        }
    }

    IEnumerator LoadYourAsyncScene()
    {
        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.
        // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
        // a sceneBuildIndex of 1 as shown in Build Settings.
        if (!(string.IsNullOrEmpty(loadScene))) {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(loadScene);

            // Wait until the asynchronous scene fully loads
            while (!asyncLoad.isDone)
            {
                yield return null;
            }
        }
        
    }
}
