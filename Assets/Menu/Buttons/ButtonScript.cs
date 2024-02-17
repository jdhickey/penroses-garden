using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{

    [Range(0.0f, 0.1f)]
    public float highlight;
    private SpriteRenderer spriteRenderer;
    private float spriteHeight;
    private Vector3 highlightVec;

    private CapsuleCollider2D thisCollider;
    private float colliderOffset;
    private float factor;
    private bool active;

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
        
    }

    void OnMouseEnter() {
        // Recenters the collider and expands it to the size of the whole sprite.
        thisCollider.offset = new Vector2(0, 0);
        Vector2 currentSize = thisCollider.size;

        currentSize.y = currentSize.y / factor;
        thisCollider.size = currentSize;

        // Offsets the sprite to indicate it is chosen
        transform.position = transform.position + highlightVec;
        active = true;
    }

    void OnMouseExit() {
        // Replaces the offset and shrinks the collider
        thisCollider.offset = new Vector2(0, colliderOffset);
        Vector2 currentSize = thisCollider.size;

        currentSize.y = currentSize.y * factor;
        thisCollider.size = currentSize;

        // Moves the sprite back to where it was
        transform.position = transform.position - highlightVec;
        active = false;
    }

    public void AttemptActivate() {
        if (active) {
            // Replace with scene to load TODO
            Debug.Log(name);
        }
    }
}
