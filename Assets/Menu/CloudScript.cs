using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudScript : MonoBehaviour
{
    [Range(0f, 5f)]
    public float maxSpeed;
    [Range(0f, 5f)]
    public float minSpeed;
    private float speed;
    private float vertical;

    // Start is called before the first frame update
    void Start()
    {
        Sprite[] cloud_options = Resources.LoadAll<Sprite>("Clouds");
        GetComponent<SpriteRenderer>().sprite = cloud_options[Random.Range(0, cloud_options.Length)];

        speed = Random.Range(minSpeed, maxSpeed);
        vertical = Random.Range(-(maxSpeed - minSpeed), (maxSpeed - minSpeed));
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 velocity = new Vector3(speed, vertical * Mathf.Sin(Time.frameCount * Time.deltaTime), 0);
        velocity = Vector3.ClampMagnitude(velocity, maxSpeed*maxSpeed);
        transform.position = transform.position + velocity * Time.deltaTime;
    }

    public void OnBecameInvisible() {
        UnityEngine.Object.Destroy(gameObject);
    }
}
