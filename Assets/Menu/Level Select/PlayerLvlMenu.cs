using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLvlMenu : MonoBehaviour
{
    private float radius;

    // Start is called before the first frame update
    void Start()
    {
        radius = GetComponent<SpriteRenderer>().bounds.size.x / 4;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnSelect() 
    {
        Collider2D overlapping = Physics2D.OverlapCircle(transform.position, radius);

        if (overlapping) {
            overlapping.GetComponent<LevelButton>().AttemptLoad();
        }
    }
}
