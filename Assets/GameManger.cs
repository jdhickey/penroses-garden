using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManger : MonoBehaviour
{
    public ThinRhombusTile ThinRhombusPrefab;
    public float tileSize = 1.0f;
    public float offsetDistance = 0.5f;

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
                int connection = tile.CanConnectWith(adjacentTile);
                if (connection != 4)
                {
                    float rotationAngle = CalculateRotation(adjacentTile, connection);
                    tile.transform.rotation = Quaternion.Euler(0, 0, rotationAngle);

                    Vector2 offset = CalculatePositionOffset(adjacentTile, rotationAngle, connection);
                    Vector3 newPosition = (Vector3)adjacentTile.transform.position + (Vector3)offset;
                    tile.transform.position = newPosition;

                    Debug.Log("Setting rotation to: " + rotationAngle);
                    return;
                }
            }
        }
    }
    float CalculateRotation(PenroseTile adjacentTile, int connection)
    {
        float adjacentRotation = adjacentTile.transform.rotation.eulerAngles.z;
        float rotationAngle = 0;
        if(adjacentTile.tileType == PenroseTile.TileType.ThinRhombus)
        {
            if(connection == 0 || connection == 2){
                rotationAngle = adjacentRotation + 216;
            }
            else if(connection == 1 || connection == 3){
                rotationAngle = adjacentRotation + 144;
            }
        }
        return rotationAngle;
    }
    Vector2 CalculatePositionOffset(PenroseTile adjacentTile, float rotationAngle, int connection)
    {
        float radians = Mathf.Deg2Rad * rotationAngle;
        float offsetX = 0.0f;
        float offsetY = 0.0f;
        if(adjacentTile.tileType == PenroseTile.TileType.ThinRhombus)
        {
            offsetX = offsetDistance;
            offsetY = offsetDistance * (Mathf.Sin(36)/Mathf.Sin(54));

            if(connection == 0){
                offsetX *= -1;
            }
            else if(connection == 1){
                offsetX *= 1;
            }
            else if(connection == 2){
                offsetY *= -1;
            }
            else if(connection == 3){
                offsetX *= -1;
                offsetY *= -1;
            }
        }

        float rotatedX = offsetX * Mathf.Cos(radians) - offsetY * Mathf.Sin(radians);
        float rotatedY = offsetX * Mathf.Sin(radians) + offsetY * Mathf.Cos(radians);

        return new Vector2(offsetX, offsetY);
    }
}
