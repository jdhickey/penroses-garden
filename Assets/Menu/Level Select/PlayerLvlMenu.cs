using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLvlMenu : MonoBehaviour
{
    private float radius;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody2D>().gravityScale = 0f;
        radius = GetComponent<SpriteRenderer>().bounds.size.x / 4;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnSelect() 
    {
        // 1 here signifies it is only searching the default layer -- layer mask = 2^layer#
        Collider2D overlapping = Physics2D.OverlapCircle(transform.position, radius, 1);

        if (overlapping) {
            overlapping.GetComponent<LevelButton>().AttemptLoad();
        }
    }
}
