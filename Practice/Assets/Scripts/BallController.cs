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

    [Header("Current Parameters")]
    [SerializeField] private float currentPower;

    protected float epsilon;
    private Vector2 currVelocity;
    [SerializeField] private E_BallState ballState;

    protected const float BALLRAD = 0.375f;

    [Header("Components")]
    protected Rigidbody2D rb;
    private BallStat ballStat;
    private LineRenderer lr;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        lr = GetComponent<LineRenderer>();
        ballStat = GetComponent<BallStat>();

        rb.mass = Mass;
        rb.drag = DefaultDrag;
        epsilon = rb.sharedMaterial.bounciness;

        currVelocity = rb.velocity;
        currentPower = Power;
        ballState = E_BallState.Default;
    }
    private void Update()
    {
        DecelerateBall();
        currVelocity = rb.velocity;

        switch (ballState)
        {
            case E_BallState.Ready:
                Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 dir = worldPos - new Vector2(transform.position.x, transform.position.y);

                if (Input.GetMouseButtonDown(0))
                    ShootBall(dir);
                else
                    DrawLine(dir);
                break;
            case E_BallState.Attacking:
                if (rb.velocity.magnitude == 0)
                {
                    ChangeState(E_BallState.Default);
                    transform.GetComponent<BallStat>().ResetEndCondition();
                    GameManager.Instance.TurnEnd();
                }
                break;
        }
    }
    public void ResetPhysicsParameter()
    {
        currentPower = Power;
    }
    private void DrawLine(Vector2 dir)
    {
        Vector2 nextDir = dir;
        Vector2 nextPos = transform.position;

        RaycastHit2D firstHit = GetCircleCastHit(nextPos, nextDir, gameObject);
        if (firstHit.collider != null)
        {
            nextDir = CalculateNextDirection(gameObject, firstHit.collider.gameObject, firstHit, nextDir);
            nextPos = firstHit.point + firstHit.normal * BALLRAD;
        }

        RaycastHit2D secondHit = GetCircleCastHit(nextPos, nextDir, gameObject, firstHit.collider.gameObject);
        if (secondHit.collider != null)
        {
            lr.positionCount = 3;
            lr.SetPosition(0, transform.position);
            lr.SetPosition(1, firstHit.point + firstHit.normal * BALLRAD);
            lr.SetPosition(2, secondHit.point);
            lr.enabled = true;
        }
    }
    protected void ShootBall(Vector2 dir)
    {
        rb.velocity = dir.normalized * currentPower;
        rb.drag = DefaultDrag;

        ballState = E_BallState.Attacking;
        lr.enabled = false;

        GameManager.Instance.ResetHandsUpAlly();
    }
    public void StopBall()
    {
        rb.velocity = Vector2.zero;
    }
    public IEnumerator DelayedStopBall()
    {
        yield return new WaitForFixedUpdate();
        transform.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        StopCoroutine(DelayedStopBall());
    }
    protected Vector2 CalculateNextDirection(GameObject firstObj, GameObject secondObj, RaycastHit2D hit, Vector2 dir)
    {
        Vector2 result = Vector2.zero;
        if (secondObj.CompareTag("Player") || secondObj.CompareTag("Enemy"))
        {
            Vector2 n = hit.normal;

            Rigidbody2D rb1 = firstObj.GetComponent<Rigidbody2D>();
            Rigidbody2D rb2 = secondObj.GetComponent<Rigidbody2D>();
            float m1 = rb1.mass;
            float m2 = rb2.mass;
            Vector2 v1 = dir;
            Vector2 v2 = Vector2.zero;
            Vector2 p1 = hit.point + n * BALLRAD;
            Vector2 p2 = hit.point - n * BALLRAD;

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
            result = u1;
        }
        else
        {
            Vector2 n = hit.normal;
            Vector2 modifyDir = hit.point + n * BALLRAD - new Vector2(transform.position.x, transform.position.y);
            Vector2 p = -Vector2.Dot(modifyDir, n) / n.magnitude * n / n.magnitude;
            Vector2 b = modifyDir + 2 * p;
            result = b;
        }
        return result;
    }
    protected RaycastHit2D GetCircleCastHit(Vector2 startPos, Vector2 dir, GameObject startObj)
    {
        RaycastHit2D[] hit = Physics2D.CircleCastAll(startPos, BALLRAD, dir, 100f);

        for (int i = 0; i < hit.Length; i++)
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
    public void ChangeState(E_BallState state)
    {
        ballState = state;
    }
    public Vector2 GetCurrVelocity()
    {
        return currVelocity;
    }
    public E_BallState GetBallState()
    {
        return ballState;
    }
    
    public void IncreasePowerMultiplier(float multiplier)
    {        
        currentPower *= multiplier;
    }
}