using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManger : MonoBehaviour
{
    public ThinRhombusTile ThinRhombusPrefab;
    public float tileSize = 1.0f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            PlaceTile(mousePos);
        }
    }

    void PlaceTile(Vector2 position)
    {
        ThinRhombusTile newTile = Instantiate(ThinRhombusPrefab, position, Quaternion.identity);
        newTile.InitializeTile();
        OrientTile(newTile);
    }

    void OrientTile(ThinRhombusTile tile)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(tile.transform.position, tileSize);

        foreach (Collider2D collider in colliders)
        {
            PenroseTile adjacentTile = collider.GetComponent<PenroseTile>();

            if (adjacentTile != null && adjacentTile != tile)
            {
                int connectionDirection = tile.CanConnectWith(adjacentTile);
                if (connectionDirection != 4)
                {
                    float rotationAngle = connectionDirection * 90f;
                    tile.transform.rotation = Quaternion.Euler(0, 0, rotationAngle);

                    Debug.Log("Setting rotation to: " + rotationAngle);
                    return;
                }
            }
        }
    }
}
