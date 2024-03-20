using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition : MonoBehaviour
{

    private int rotation = 0; 
    private SpriteRenderer _rend;
    private float aspect;
    private float screenHeight;
    private float screenWidth;
    private Vector3 spriteSize;
    private Vector3 direction;
    private int step = 10;
    private float ct = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ct = ct + 0.5f * Time.deltaTime;
        transform.Translate(direction * Mathf.Pow(step, ct) * Time.deltaTime, Space.World);
    }

    public void updateTransform(int i) {
        rotation = i;
        _rend = GetComponent<SpriteRenderer>();
        aspect = Screen.width / Screen.height;
        
        screenHeight = Camera.main.orthographicSize * 2;
        screenWidth = screenHeight * aspect;

        Vector3 spriteSize = _rend.bounds.size;

        if (rotation % 180 == 0) {
            _rend.size = new Vector2(spriteSize.x * screenWidth + 1, screenHeight + 1);
        } else {
            _rend.size = new Vector2(spriteSize.y * screenHeight + 1, screenWidth + 1);
        }

        direction =  new Vector3(Mathf.Cos((i - 90) * Mathf.PI / 180), Mathf.Sin((i - 90) * Mathf.PI / 180), 0);
        Debug.Log(direction);
    }
}
