using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public enum E_BallState
{
    Default,
    Ready,
    Attacking,
}

[RequireComponent(typeof(Rigidbody2D)), RequireComponent(typeof(LineRenderer))]
public class MonsterController : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private float Power;
    [SerializeField] private float Mass;
    [SerializeField] private float DefaultDrag;
    [SerializeField] private float BeginDecelerateMagnitude;
    [SerializeField] private float DecelerateDrag;
    [SerializeField] private float StopMagnitude;
    [SerializeField] private float BreakDrag;

    [Header("Current Parameters")]
    private float ballRadius;
    private float currentPower;
    private float epsilon;
    private Vector2 currVelocity;
    private E_BallState ballState;

    [Header("Components")]
    [SerializeField] private MonsterStat monsterStat;

    private Rigidbody2D rb;
    private BallStat ballStat;
    private LineRenderer lr;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        lr = GetComponent<LineRenderer>();
        ballStat = GetComponent<BallStat>();

        rb.mass = Mass;
        rb.drag = Mathf.Max(DefaultDrag + PlayerPrefs.GetFloat("Drag", 0), 0);
        epsilon = rb.sharedMaterial.bounciness;

        ballRadius = transform.localScale.x / 2;
        currVelocity = rb.velocity;
        currentPower = Power + PlayerPrefs.GetFloat("Power", 0);
        ballState = E_BallState.Default;
    }

    private void Update()
    {
        DecelerateBall();
        CheckBallState();

        currVelocity = rb.velocity;
    }

    private void DecelerateBall()
    {
        if (rb.velocity.magnitude < StopMagnitude)
            rb.velocity = Vector2.zero;
        else if (rb.velocity.magnitude < BeginDecelerateMagnitude)
            rb.drag = DecelerateDrag;
    }
    public void ChangeState(E_BallState state)
    {
        ballState = state;
    }
    private void CheckBallState()
    {
        switch (ballState)
        {
            case E_BallState.Ready:
                Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 dir = worldPos - new Vector2(transform.position.x, transform.position.y);
                if (Input.GetMouseButtonDown(0))
                    MoveMonster(dir);                    
                else
                    DrawLine(dir);
                break;
            case E_BallState.Attacking:
                if (rb.velocity.magnitude == 0)
                {
                    ResetEndPhysicsParameter();
                    ChangeState(E_BallState.Default);                    
                }
                break;
        }
    }
    protected void MoveMonster(Vector2 dir)
    {
        rb.velocity = dir.normalized * currentPower;
        rb.drag = DefaultDrag;

        ballState = E_BallState.Attacking;
        lr.enabled = false;
    }    
    private void DrawLine(Vector2 dir)
    {        
        RaycastHit2D hit = GetCircleCastHit(transform.position, dir, gameObject);        
        
        lr.positionCount = 2;
        lr.SetPosition(0, transform.position);
        lr.SetPosition(1, hit.point + hit.normal * ballRadius);

        lr.startColor = Color.white;
        lr.endColor = Color.white;
        lr.enabled = true;        
    }
    protected RaycastHit2D GetCircleCastHit(Vector2 pos, Vector2 dir, GameObject obj)
    {
        RaycastHit2D[] hit = Physics2D.CircleCastAll(pos, ballRadius, dir, 100f);
        for (int i = 0; i < hit.Length; i++)
        {
            if (hit[i].collider.gameObject != obj)
                return hit[i];
        }
        return new RaycastHit2D();
    }
    public void ResetEndPhysicsParameter()
    {
        currentPower = Power;
        rb.mass = Mass;
        rb.drag = Mathf.Max(DefaultDrag + PlayerPrefs.GetFloat("Drag", 0), 0);
    }
    public Vector2 GetCurrVelocity()
    {
        return currVelocity;
    }
    public void BreakMonster()
    {
        rb.drag = BreakDrag;
    }
}
