using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundTile : MonoBehaviour
{

    public static Sprite[] options;
    public GameObject tile;
    private Renderer _rend;
    private bool initialized = false;

    // Start is called before the first frame update
    void Start()
    {
        // Loads sprites from background spritemap and randomly assigns one to this tile
        if (!initialized) {
            options = Resources.LoadAll<Sprite>("Background");
            initialized = true;
        }
        _rend = GetComponent<Renderer>();
        GetComponent<SpriteRenderer>().sprite = options[Random.Range(0,options.Length)];
    }

    // Update is called once per frame
    void Update()
    {
        // If the renderer is visible, will spawn in the neighbours with an offset and cancel
        // any ongoing destroy action
        if (_rend.isVisible) {
            spawnNeighbours();
            CancelInvoke("DestroyMe");

            // TODO: add condition that if a neighbour is visible, also cancel invoke
        } else {
            // If the renderer is not visible, it will be destroyed in 2 seconds
            Invoke("DestroyMe", 1);
        }
    }

    void DestroyMe() {
        Destroy(gameObject);
    }

    void spawnNeighbours() {
        var bounds = _rend.bounds;
        Vector3 centre = bounds.center;
        float width = bounds.size.x;

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
