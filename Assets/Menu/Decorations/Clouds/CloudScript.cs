using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudScript : MonoBehaviour
{
    [Range(0f, 5f)]
    public float maxSpeed;
    [Range(0f, 5f)]
    public float minSpeed;
    [Range(0f, 15)]
    public float maxAngle;

    private float speed;
    private float vertical;
    private Quaternion rotation;

    // Start is called before the first frame update
    void Start()
    {
        rotation = Quaternion.Euler(Random.Range(0, maxAngle), Random.Range(0, maxAngle), Random.Range(0, maxAngle));
        transform.rotation = rotation;

        Sprite[] cloud_options = Resources.LoadAll<Sprite>("Clouds");
        GetComponent<SpriteRenderer>().sprite = cloud_options[Random.Range(0, cloud_options.Length)];

        speed = Random.Range(minSpeed, maxSpeed);
        vertical = Random.Range(-(maxSpeed - minSpeed), (maxSpeed - minSpeed));
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 velocity = new Vector3(speed, vertical * Mathf.Sin(Time.frameCount * Time.deltaTime + vertical), 0);
        velocity = Vector3.ClampMagnitude(velocity, maxSpeed*maxSpeed);
        transform.position = transform.position + velocity * Time.deltaTime;
    }

    public void OnBecameInvisible() {
        UnityEngine.Object.Destroy(gameObject);
    }
}
