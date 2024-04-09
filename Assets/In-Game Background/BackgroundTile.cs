using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundTile : MonoBehaviour
{

    public static Sprite[] options;
    public static Sprite[] decorations;
    public static int sceneSeed;
    private GameObject tile;
    [Range(0f, 1f)]
    public float decoChance = 0.15f;
    private Renderer _rend;
    private static bool initialized = false;
    private bool decorated = false;

    private float width;
    private Vector3 centre;

    // Start is called before the first frame update
    void Awake()
    {
        tile = Resources.Load<GameObject>("BackgroundTile2");
        // Loads sprites from background spritemap and randomly assigns one to this tile
        if (!initialized) {
            options = Resources.LoadAll<Sprite>("Background");
            decorations = Resources.LoadAll<Sprite>("FlowerStone");
            sceneSeed = (int) Time.time % (17 * 13);
            initialized = true;
        }
        _rend = GetComponent<Renderer>();

        // Procedurally generates background using arbitrary prime numbers
        int x = (int)transform.position.x;
        int y = (int)transform.position.y;
        int options_index = ((x + y) * (x + y + 1) / 2) + y;

        System.Random thisRand = new System.Random(options_index);

        GetComponent<SpriteRenderer>().sprite = options[thisRand.Next(0, options.Length)];
        width = _rend.bounds.size.x;
        centre = _rend.bounds.center;
        if (thisRand.NextDouble() <= decoChance && !decorated) {
            GameObject deco = new GameObject("decoration");
            deco.layer = LayerMask.NameToLayer("Background");
            SpriteRenderer decoRend = deco.AddComponent<SpriteRenderer>();
            decoRend.sortingLayerName = "Background";
            decoRend.sortingOrder = 1;

            Vector3 scale = gameObject.transform.localScale;

            decoRend.sprite = decorations[thisRand.Next(0, decorations.Length)];
            deco.transform.parent = gameObject.transform;
            deco.transform.localPosition = Quaternion.Euler(0, 0, thisRand.Next(0, 360) * (x + y * sceneSeed)) * new Vector3((float) thisRand.NextDouble() * width, 0, 0) * 2/scale.x;
            decorated = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // the below code checks to see if any neighbouring tile is visible
        // if so, also cancel the invoke (retains border tiles)
        bool neighbourVis = false;
        Collider[] nearTiles = Physics.OverlapSphere(centre, width);
        foreach (Collider neighbour in nearTiles) {
            try {
                BackgroundTile neighbourScript = neighbour.gameObject.GetComponent<BackgroundTile>();
                neighbourVis = neighbourVis || neighbourScript._rend.isVisible;
            } catch {}
        }

        // If the renderer is visible, will spawn in the neighbours with an offset and cancel
        // any ongoing destroy action
        if (_rend.isVisible || neighbourVis) {
            spawnNeighbours();
            CancelInvoke("DestroyMe");
        } else {
            // If the renderer is not visible, it will be destroyed in 2 seconds
            Invoke("DestroyMe", 1);
        }
    }

    void DestroyMe() {
        Destroy(gameObject);
    }

    void spawnNeighbours() {
        Collider[] nearTiles = Physics.OverlapSphere(centre, width);
        Vector3[] directions = new Vector3[nearTiles.Length];

        for (int i = 0; i < nearTiles.Length; i++) {
            directions[i] = nearTiles[i].bounds.center - centre;
        }

        for (int i = 0; i < 360; i += 90) {
            Vector3 test_dir = new Vector3(width, 0, 0);
            test_dir = Quaternion.Euler(0, 0, i) * test_dir;
            bool is_open = true;

            foreach (Vector3 dir in directions) {
                if (Vector3.Distance(dir, test_dir) < 0.01) {
                    is_open = false;
                }
            }

            if (is_open) {
                Instantiate(tile, transform.position + test_dir, Quaternion.identity);
            }
        }
    }
}
