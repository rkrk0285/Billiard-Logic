using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D)), RequireComponent(typeof(LineRenderer))]
public class BallController : MonoBehaviour
{            
    private Rigidbody2D rb;
    private LineRenderer lr;
    private Vector2 currVelocity;    

    private bool isReady;
    private const float POWER = 40f;
    private const float BALLRAD = 0.375f;
    private const float epsilon = 1;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        lr = GetComponent<LineRenderer>();
        currVelocity = rb.velocity;
        isReady = false;        
    }
    private void Update()
    {
        DecelerateBall();
        currVelocity = rb.velocity;
                
        if (isReady)
        {
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 dir = CalculateDirection(worldPos);
            if (Input.GetMouseButtonDown(0))            
                ShootBall(dir);            
            else            
                DrawLine(dir);            
        }        
    }    
    public void OnClickReadyButton()
    {
        isReady = true;       
    }
    private Vector2 CalculateDirection(Vector2 mousePos)
    {
        return mousePos - new Vector2(transform.position.x, transform.position.y);
    }
    private void ShootBall(Vector2 dir)
    {
        rb.velocity = dir.normalized * POWER;
        rb.drag = 0.4f;

        isReady = false;
        lr.enabled = false;
    }
    private void DrawLine(Vector2 dir)
    {                
        RaycastHit2D firstHit = GetCircleCastHit(new Vector2(transform.position.x, transform.position.y), dir, gameObject);
        if (firstHit.collider != null)
        {
            if (firstHit.collider.CompareTag("Player")) // Conflict another ball
            {
                Vector2 n = firstHit.normal;

                Vector2 v1 = dir;
                Vector2 v2 = Vector2.zero;
                Vector2 p1 = firstHit.point + n * BALLRAD;
                Vector2 p2 = firstHit.point - n * BALLRAD;

                Vector2 basisX = (p2 - p1).normalized;
                Vector2 basisY = Vector2.Perpendicular(basisX);
                float sin1, sin2, cos1, cos2;

                if (v1.magnitude < 0.001f)
                {
                    sin1 = 0;
                    cos1 = 1;
                }
                else
                {
                    cos1 = Vector2.Dot(v1, basisX) / v1.magnitude;
                    Vector3 cross = Vector3.Cross(v1, basisX);

                    if (cross.z > 0)
                        sin1 = cross.magnitude / v1.magnitude;
                    else
                        sin1 = -cross.magnitude / v1.magnitude;
                }

                if (v2.magnitude < 0.0001f)
                {
                    sin2 = 0;
                    cos2 = 1;
                }
                else
                {
                    cos2 = Vector2.Dot(v2, basisX) / v2.magnitude;
                    Vector3 cross = Vector3.Cross(v2, basisX);
                    if (cross.z > 0)
                        sin2 = cross.magnitude / v2.magnitude;
                    else
                        sin2 = -cross.magnitude / v2.magnitude;
                }

                Vector2 u1, u2;
                u1 = ((1 - epsilon) * v1.magnitude * cos1 + (1 + epsilon) * v2.magnitude * cos2) / 2 * basisX - v1.magnitude * sin1 * basisY;
                u2 = ((1 + epsilon) * v1.magnitude * cos1 + (1 - epsilon) * v2.magnitude * cos2) / 2 * basisX - v2.magnitude * sin2 * basisY;

                RaycastHit2D secondHit = GetCircleCastHit(firstHit.point + n * BALLRAD, u1, gameObject, firstHit.collider.gameObject);
                if (secondHit.collider != null)
                {
                    lr.positionCount = 3;                    
                    lr.SetPosition(0, transform.position);
                    lr.SetPosition(1, firstHit.point + n * 0.375f);
                    lr.SetPosition(2, secondHit.point + secondHit.normal * 0.375f);
                    lr.enabled = true;
                }
            }
            else // Conflict Wall
            {
                Vector2 n = firstHit.normal;
                Vector2 modifyDir = firstHit.point + n * BALLRAD - new Vector2(transform.position.x, transform.position.y);
                Vector2 p = -Vector2.Dot(modifyDir, n) / n.magnitude * n / n.magnitude;
                Vector2 b = modifyDir + 2 * p;                

                RaycastHit2D secondHit = Physics2D.CircleCast(firstHit.point + n * BALLRAD, BALLRAD, b, 100f);

                if (secondHit.collider != null)
                {
                    lr.positionCount = 3;
                    lr.SetPosition(0, transform.position);
                    lr.SetPosition(1, firstHit.point + n * BALLRAD);
                    lr.SetPosition(2, secondHit.point);
                    lr.enabled = true;
                }
            }
        }
    }
    private RaycastHit2D GetCircleCastHit(Vector2 startPos, Vector2 dir, GameObject startObj)
    {
        RaycastHit2D[] hit = Physics2D.CircleCastAll(startPos, BALLRAD, dir, 100f);

        for(int i = 0; i < hit.Length; i++)
        {
            if (hit[i].collider.gameObject != startObj)
                return hit[i];
        }
        return new RaycastHit2D();
    }
    private RaycastHit2D GetCircleCastHit(Vector2 startPos, Vector2 dir, GameObject startObj, GameObject startObj2)
    {
        RaycastHit2D[] hit = Physics2D.CircleCastAll(startPos, BALLRAD, dir, 100f);

        for (int i = 0; i < hit.Length; i++)
        {
            if (hit[i].collider.gameObject != startObj && hit[i].collider.gameObject != startObj2)
                return hit[i];
        }
        return new RaycastHit2D();
    }
    private void DecelerateBall()
    {        
        if (rb.velocity.magnitude < 1f)
        {
            rb.drag = 1f;            
        }
        else if (rb.velocity.magnitude < 0.5f)
        {
            rb.velocity = Vector2.zero;
        }
    }
    public Vector2 GetCurrVelocity()
    {
        return currVelocity;
    }
}
