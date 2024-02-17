using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudSpawner : MonoBehaviour
{
    public GameObject cloudPrefab;
    public float maxInterval;
    public float minInterval;

    private static Camera cam;
    private static bool camera_def = false;
    private bool spawn_cloud = true;
    private Sprite[] cloud_options;

    private float[] spawn_bounds;

    // Start is called before the first frame update
    void Start()
    {
        if (!camera_def) {
            cam = (Camera)FindObjectOfType(typeof(Camera));
            spawn_bounds = new float[2];

            spawn_bounds[0] = cam.orthographicSize;
            spawn_bounds[1] = -cam.orthographicSize;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float seconds = Random.Range(minInterval, maxInterval);
        if (spawn_cloud) {
            Invoke("SpawnCloud", seconds);
            spawn_cloud = false;
        }
    }

    void SpawnCloud() {
        float yCoord = Random.Range(spawn_bounds[0], spawn_bounds[1]);

        Vector3 position = new Vector3(transform.position.x, yCoord, 0);
        Instantiate(cloudPrefab, position, Quaternion.identity);
        spawn_cloud = true;
    }
}
