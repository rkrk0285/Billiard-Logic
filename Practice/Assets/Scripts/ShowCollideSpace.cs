using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class ShowCollideSpace : MonoBehaviour
{
    [SerializeField] private Vector3 offset;
    [SerializeField] private float radius;
    
    private void Update()
    {
        DrawCircle(radius);
    }
    private void DrawCircle(float radius)
    {
        int segments = 60;
        float angle = 0f;
        float angleStep = 360f / segments;

        for (int i = 0; i < segments; i++)
        {
            float x = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;
            float y = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;

            LineRenderer lr = transform.GetComponent<LineRenderer>();
            lr.positionCount = segments;
            lr.SetPosition(i, transform.position + new Vector3(x, y, 0f) + offset);
            lr.enabled = true;
            angle += angleStep;
        }
    }
}
