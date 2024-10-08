using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraMove : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;

    private const float POWER = 0.02f;
    private void Update()
    {
        if (Input.GetKey(KeyCode.W))
            MoveCamera(new Vector2(0, 1).normalized);
        if (Input.GetKey(KeyCode.A))
            MoveCamera(new Vector2(-1, 0).normalized);
        if (Input.GetKey(KeyCode.S))
            MoveCamera(new Vector2(0, -1).normalized);
        if (Input.GetKey(KeyCode.D))
            MoveCamera(new Vector2(1, 0).normalized);
    }

    private void MoveCamera(Vector3 dir)
    {
        Vector3 pos = transform.position + dir * POWER;

        if (tilemap.cellBounds.xMin > pos.x || tilemap.cellBounds.xMax < pos.x
            || tilemap.cellBounds.yMin > pos.y || tilemap.cellBounds.yMax < pos.y)
            return;

        transform.position = pos;
    }
}
