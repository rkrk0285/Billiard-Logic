using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_BallState
{
    Default,
    Ready,
    Attacking,
}
[RequireComponent(typeof(Rigidbody2D)), RequireComponent(typeof(LineRenderer))]
public class BallController : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private float Power;
    [SerializeField] private float Mass;
    [SerializeField] private float DefaultDrag;
    [SerializeField] private float BeginDecelerateMagnitude;
    [SerializeField] private float DecelerateDrag;
    [SerializeField] private float StopMagnitude;
    
    protected Rigidbody2D rb;    
    protected float epsilon;
    private LineRenderer lr;
    private Vector2 currVelocity;    
    private E_BallState ballState;

    protected const float BALLRAD = 0.375f;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        lr = GetComponent<LineRenderer>();
        
        rb.mass = Mass;
        rb.drag = DefaultDrag;        
        epsilon = rb.sharedMaterial.bounciness;

        currVelocity = rb.velocity;
        ballState = E_BallState.Default;
    }
    private void Update()
    {
        DecelerateBall();
        currVelocity = rb.velocity;
                
        if (ballState == E_BallState.Ready)
        {
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 dir = CalculateDirection(worldPos);
            if (Input.GetMouseButtonDown(0))            
                ShootBall(dir);            
            else            
                DrawLine(dir);            
        }
        else if (ballState == E_BallState.Attacking)
        {
            if (rb.velocity.magnitude == 0)
            {
                ballState = E_BallState.Default;
                GameManager.Instance.GoNextTurn();
            }
        }
    }
    public void ChangeState(E_BallState state)
    {
        ballState = state;
    }
    public void OnClickTestButton()
    {
        ShootBall(new Vector2(1, 1));
    }
    private Vector2 CalculateDirection(Vector2 mousePos)
    {
        return mousePos - new Vector2(transform.position.x, transform.position.y);
    }
    protected void ShootBall(Vector2 dir)
    {
        rb.velocity = dir.normalized * Power;
        rb.drag = DefaultDrag;

        ballState = E_BallState.Attacking;
        lr.enabled = false;
    }
    private void DrawLine(Vector2 dir)
    {                
        RaycastHit2D firstHit = GetCircleCastHit(new Vector2(transform.position.x, transform.position.y), dir, gameObject);
        if (firstHit.collider != null)
        {
            if (firstHit.collider.CompareTag("Player") || firstHit.collider.CompareTag("Enemy")) // Conflict another ball
            {
                Vector2 n = firstHit.normal;

                float m1 = rb.mass;
                float m2 = firstHit.collider.gameObject.GetComponent<Rigidbody2D>().mass;
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
                u1 = ((m1 - epsilon * m2) * v1.magnitude * cos1 + m2 * (1 + epsilon) * v2.magnitude * cos2) / (m1 + m2) * basisX - v1.magnitude * sin1 * basisY;
                u2 = (m1 * (1 + epsilon) * v1.magnitude * cos1 + (m2 - epsilon * m1) * v2.magnitude * cos2) / (m1 + m2) * basisX - v2.magnitude * sin2 * basisY;

                RaycastHit2D secondHit = GetCircleCastHit(firstHit.point + n * BALLRAD, u1, gameObject, firstHit.collider.gameObject);
                if (secondHit.collider != null)
                {
                    lr.positionCount = 3;                    
                    lr.SetPosition(0, transform.position);
                    lr.SetPosition(1, firstHit.point + n * BALLRAD);
                    lr.SetPosition(2, secondHit.point + secondHit.normal * BALLRAD);
                    lr.enabled = true;
                }
            }
            else // Conflict Wall
            {
                Vector2 n = firstHit.normal;
                Vector2 modifyDir = firstHit.point + n * BALLRAD - new Vector2(transform.position.x, transform.position.y);
                Vector2 p = -Vector2.Dot(modifyDir, n) / n.magnitude * n / n.magnitude;
                Vector2 b = modifyDir + 2 * p;                
                
                RaycastHit2D secondHit = GetCircleCastHit(firstHit.point + n * BALLRAD, b, gameObject);
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
    protected RaycastHit2D GetCircleCastHit(Vector2 startPos, Vector2 dir, GameObject startObj)
    {
        RaycastHit2D[] hit = Physics2D.CircleCastAll(startPos, BALLRAD, dir, 100f);

        for(int i = 0; i < hit.Length; i++)
        {
            if (hit[i].collider.gameObject != startObj)
                return hit[i];
        }
        return new RaycastHit2D();
    }
    protected RaycastHit2D GetCircleCastHit(Vector2 startPos, Vector2 dir, GameObject startObj, GameObject startObj2)
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
        if (rb.velocity.magnitude < StopMagnitude)
        {
            rb.velocity = Vector2.zero;            
        }
        else if (rb.velocity.magnitude < BeginDecelerateMagnitude)
        {
            rb.drag = DecelerateDrag;            
        }                
    }
    public Vector2 GetCurrVelocity()
    {
        return currVelocity;
    }
    public E_BallState GetBallState()
    {
        return ballState;
    }
}
